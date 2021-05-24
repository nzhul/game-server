using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Assets.Scripts.Network.Services;
using GameServer.Managers;
using LiteNetLib;
using LiteNetLib.Utils;
using NetworkingShared;

namespace GameServer
{
    public class NetworkServer : INetEventListener
    {
        private const int MAX_USERS_COUNT = 100;

        private static NetworkServer _instance;

        public static NetworkServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NetworkServer();
                }

                return _instance;
            }
        }

        // private constructor prevents all instantiations of this class other than the Singleton.
        private NetworkServer() { }

        private NetManager _netManager;
        private Dictionary<int, ServerConnection> _connections;
        private readonly NetDataWriter _cachedWriter = new NetDataWriter();

        public int ConnectionsCount { get { return _connections.Count; } }

        public Dictionary<int, ServerConnection> Connections { get => _connections; }

        public void Start()
        {
            _connections = new Dictionary<int, ServerConnection>();
            _netManager = new NetManager(this);
            _netManager.DisconnectTimeout = 5000; // TODO: use config for this. Default is 5000

            _netManager.Start(9050);
            Console.WriteLine("Server listening on port 9050");
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var packetType = (PacketType)reader.GetByte();

            try
            {
                var packet = NetworkUtils.ResolvePacket(packetType, reader);
                HandlerRegistry.Handlers[packet.Type].Handle(packet, peer.Id);
                reader.Recycle();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error processing {packetType} packet. Ex: {ex}");
            }
        }

        public void PollEvents()
        {
            _netManager.PollEvents();
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            Console.WriteLine($"Incomming connection from {request.RemoteEndPoint}");

            if (_connections.Count < MAX_USERS_COUNT)
            {
                request.Accept();
                return;
            }

            Console.WriteLine("Connection rejected! Server is FULL!");
            request.Reject();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine($"Client connected to server: {peer.EndPoint}. Id: {peer.Id}");
            Connections.Add(peer.Id, new ServerConnection { ConnectionId = peer.Id, Peer = peer });
        }

        // This method is automatically invoked when there is no response from the user for X amount of time.
        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            var connection = _connections[peer.Id];

            BattleManager.Instance.DisconnectFromBattle(peer.Id);
            _netManager.DisconnectPeer(peer);
            Console.WriteLine($"{connection.Username} disconnected: {peer.EndPoint}");

            var userId = connection.UserId;

            // TODO: Extract into FireAndForget() utility method.
            Task.Run(() =>
            {
                try
                {
                    RequestManagerHttp.UsersService.SetOffline(userId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting user offline. UserId: {userId}. Ex: {ex}");
                }
            });

            _connections.Remove(peer.Id);
        }

        public void Send(int peerId, INetPacket packet, DeliveryMethod method = DeliveryMethod.ReliableOrdered)
        {
            var peer = Connections[peerId].Peer;
            this.Send(peer, packet, method);
        }

        public void Send(NetPeer peer, INetPacket packet, DeliveryMethod method)
        {
            peer.Send(WriteSerializable(packet), method);
        }

        private NetDataWriter WriteSerializable(INetPacket packet)
        {
            _cachedWriter.Reset();
            //_cachedWriter.Put((byte)packet.Type);
            packet.Serialize(_cachedWriter);
            return _cachedWriter;
        }

        // NOT IMPLEMENTED:
        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            // TODO: implement if needed
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            // USE THIS TO CALCULATE PING !
            // _netManager.UnconnectedMessagesEnabled = true;
            // 1. Client sends _netManager.SendUnconnectedMessage()
            // 2. Server calls back the client with _netManager.SendUnconnectedMessage() and info about players count and server status
            // 3. Client calculate ping based on time passed after client message send and server message received.
            // NOTE: _netManager.SendUnconnectedMessage() --> Those messages are unreliable! Should send multiple times from client!

            throw new NotImplementedException();
        }
    }
}
