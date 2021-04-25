using System;
using Assets.Scripts.Network.Shared.NetMessages.Users;
using GameServer.Shared;
using GameServer.Shared.Attributes;

namespace GameServer.PacketHandlers
{
    [HandlerRegister(PacketType.AuthRequest)]
    public class AuthRequestHandler : IPacketHandler
    {
        public void Handle(INetPacket packet)
        {
            var msg = (Net_AuthRequest)packet;
            Console.WriteLine($"[{nameof(Net_AuthRequest)}] received!");

            //Net_OnAuthRequest rmsg = new Net_OnAuthRequest();

            //if (msg.IsValid())
            //{
            //    rmsg.Success = 1;
            //    rmsg.ConnectionId = connectionId;

            //    ServerConnection connection = new ServerConnection
            //    {
            //        ConnectionId = connectionId,
            //        UserId = msg.UserId,
            //        Token = msg.Token,
            //        Username = msg.Username,
            //        MMR = msg.MMR,
            //        GameId = msg.GameId,
            //        BattleId = msg.BattleId
            //    };

            //    NetworkServer.Instance.Connections.Add(connectionId, connection);

            //    string endpoint = "users/{0}/setonline/{1}";
            //    string[] @params = new string[] { msg.UserId.ToString(), connectionId.ToString() };

            //    RequestManager.Instance.Put(endpoint, @params, msg.Token, OnSetOnline);

            //    Debug.Log(string.Format("{0} logged in to the server!", msg.Username));
            //    OnAuth?.Invoke(connection);
            //}
            //else
            //{
            //    rmsg.Success = 0;
            //    rmsg.ErrorMessage = "Invalid connection request!";
            //}

            //NetworkServer.Instance.SendClient(recievingHostId, connectionId, rmsg);
        }
    }
}
