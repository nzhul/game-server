﻿using Assets.Scripts.Network.Services.TCP.Interfaces;
using NetworkingShared.Packets.World.ServerClient;

namespace Assets.Scripts.Network.Services.TCP
{
    public class GameService : IGameService
    {
        public void StartGame(int gameId, int[] connectionIds)
        {
            foreach (var connectionId in connectionIds)
            {
                Net_OnStartGame msg = new Net_OnStartGame
                {
                    GameId = gameId
                };

                // TODO: Invoke server.
                //NetworkServer.Instance.SendClient(0, connectionId, msg);
            }
        }
    }
}
