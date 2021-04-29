using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using NetworkingShared;

namespace GameServer
{
    public class Server : INetEventListener
    {
        private const int MAX_USERS_COUNT = 100;

        private static Server _instance;

        public static Server Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Server();
                }

                return _instance;
            }
        }

        // private constructor prevents all instantiations of this class other than the Singleton.
        private Server() { }

        private NetManager _netManager;
        private Dictionary<int, ServerConnection> _connections;
        private readonly NetDataWriter _cachedWriter = new NetDataWriter();

        public int ConnectionsCount { get { return _connections.Count; } }

        public Dictionary<int, ServerConnection> Connections { get => _connections; }

        public void Start()
        {
            _connections = new Dictionary<int, ServerConnection>();
            _netManager = new NetManager(this);
            _netManager.DisconnectTimeout = 100000; // TODO: use config for this. Default is 5000

            _netManager.Start(9050);
            Console.WriteLine("Server listening on port 9050");
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var packetType = (PacketType)reader.GetByte();
            var packet = NetworkUtils.ResolvePacket(packetType, reader);
            HandlerRegistry.Handlers[packet.Type].Handle(packet, peer.Id);
            reader.Recycle();
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
            Connections.Remove(peer.Id);
            Console.WriteLine($"Peer disconnected: {peer.EndPoint}");
        }

        // This method is manually invoked when user press logout button.
        public void DisconnectUser(NetPeer peer)
        {
            _connections.Remove(peer.Id);
            _netManager.DisconnectPeer(peer);
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
            throw new NotImplementedException();
        }
    }
}
