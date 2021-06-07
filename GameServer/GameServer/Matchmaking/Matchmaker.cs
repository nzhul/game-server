using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Managers;
using GameServer.Models;
using GameServer.Models.View;
using GameServer.Utilities;
using NetworkingShared.Enums;
using NetworkingShared.Packets.World.ServerClient;
using Newtonsoft.Json;

namespace GameServer.Matchmaking
{
    public class Matchmaker
    {
        private static Matchmaker _instance;

        public static Matchmaker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Matchmaker();
                }

                return _instance;
            }
        }

        private List<MMRequest> _pool = new List<MMRequest>();

        public int PoolSize { get { return _pool.Count; } }

        public void RegisterPlayer(ServerConnection connection, CreatureType @class)
        {
            var request = new MMRequest()
            {
                Connection = connection,
                SearchStart = DateTime.UtcNow,
                StartingClass = @class
            };

            this._pool.Add(request);

            Console.WriteLine($"{request.Connection.Username} registered in matchmaking pool " +
                $"with {request.Connection.MMR} MMR and {@class} class.");
        }

        public void UnRegisterPlayer(ServerConnection connection)
        {
            var request = this._pool.FirstOrDefault(x => x.Connection.ConnectionId == connection.ConnectionId);

            if (request == null)
            {
                return;
            }

            this._pool.Remove(request);
            Console.WriteLine($"{request.Connection.Username} canceled his MM request.");
        }

        public void DoMatchmaking()
        {
            var matchedRequests = new List<MMRequest>();
            foreach (var request in this._pool)
            {
                // TODO: Use custom Range class, because the default one doesn't allow negative numbers
                // https://stackoverflow.com/a/5343033/3937407
                var match = this._pool.FirstOrDefault(
                    x => !x.MatchFound &&
                    x.SearchRange.Overlap(request.SearchRange)
                    && x.Connection.ConnectionId != request.Connection.ConnectionId);

                if (match != null)
                {
                    Console.WriteLine($"Match found for players: {request.Connection.Username} and {match.Connection.Username}");

                    request.MatchFound = true;
                    match.MatchFound = true;
                    matchedRequests.Add(request);
                    matchedRequests.Add(match);

                    // TODO: Run this in new thread.
                    var @params = new GameParams
                    {
                        MapTemplate = MapTemplate.Small,
                        Players = new List<Player>
                        {
                            new Player
                            {
                                UserId = request.Connection.UserId,
                                Team = Team.Team1,
                                StartingClass = request.StartingClass
                            },
                            new Player
                            {
                                UserId = match.Connection.UserId,
                                Team = Team.Team2,
                                StartingClass = match.StartingClass
                            }
                        }
                    };

                    //var game = RequestManagerHttp.GameService.CreateGame(@params);
                    var game = GameManager.Instance.CreateGame(@params);
                    GameManager.Instance.RegisterGame(game);

                    var simpleGame = AM.Instance.Mapper.Map<GameDetailedDto>(game);
                    var gameString = JsonConvert.SerializeObject(simpleGame);
                    var connectionIds = new int[] { request.Connection.ConnectionId, match.Connection.ConnectionId };

                    foreach (var connectionId in connectionIds)
                    {
                        Net_OnStartGame msg = new Net_OnStartGame
                        {
                            GameId = game.Id,
                            GameString = gameString
                        };

                        NetworkServer.Instance.Send(connectionId, msg);
                    }

                    // ^^ TODO: Run this in new thread.
                }
            }

            foreach (var request in matchedRequests)
            {
                this._pool.Remove(request);
            }
        }
    }
}
