using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Network.Services;
using GameServer.Managers;
using GameServer.Models;
using GameServer.Utilities;
using NetworkingShared.Enums;
using NetworkingShared.Packets.World.ServerClient;

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

        private List<MMRequest> Pool = new List<MMRequest>();

        public void RegisterPlayer(ServerConnection connection, CreatureType @class)
        {
            var request = new MMRequest()
            {
                Connection = connection,
                SearchStart = DateTime.UtcNow,
                StartingClass = @class
            };

            this.Pool.Add(request);

            Console.WriteLine($"{request.Connection.Username} registered in matchmaking pool " +
                $"with {request.Connection.MMR} MMR and {@class} class.");
        }

        public void UnRegisterPlayer(ServerConnection connection)
        {
            var request = this.Pool.FirstOrDefault(x => x.Connection.ConnectionId == connection.ConnectionId);

            if (request == null)
            {
                return;
            }

            this.Pool.Remove(request);
            Console.WriteLine($"{request.Connection.Username} canceled his MM request.");
        }

        public void DoMatchmaking()
        {
            var matchedRequests = new List<MMRequest>();
            foreach (var request in this.Pool)
            {
                var match = this.Pool.FirstOrDefault(
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

                    var @params = new GameParams
                    {
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

                    var game = RequestManagerHttp.GameService.CreateGame(@params);
                    GameManager.Instance.RegisterGame(game);

                    var connectionIds = new int[] { request.Connection.ConnectionId, match.Connection.ConnectionId };

                    foreach (var connectionId in connectionIds)
                    {
                        Net_OnStartGame msg = new Net_OnStartGame
                        {
                            GameId = game.Id
                        };

                        NetworkServer.Instance.Send(connectionId, msg);
                    }
                }
            }

            foreach (var request in matchedRequests)
            {
                this.Pool.Remove(request);
            }
        }
    }
}
