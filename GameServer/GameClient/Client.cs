using System;
using System.Net;
using System.Net.Sockets;
using GameServer.Shared;
using LiteNetLib;
using LiteNetLib.Utils;

namespace GameClient
{
    public class Client : INetEventListener
    {
        private NetManager _client;
        private NetPeer _server;
        private NetDataWriter _writer;

        public void Connect()
        {
            _writer = new NetDataWriter();
            _client = new NetManager(this);
            _client.DisconnectTimeout = 100000; // TODO: use config for this. Default is 5000
            _client.Start();
            Console.WriteLine("Connecting to server on port 9050");
            _client.Connect("localhost", 9050, "");
        }

        public void PollEvents()
        {
            if (_client == null)
            {
                return;
            }

            _client.PollEvents();
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            // TODO: implement if needed
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var packetType = (PacketType)reader.GetByte();
            var packet = NetworkUtils.ResolvePacket(packetType, reader);
            HandlerRegistry.Handlers[packetType].Handle(packet, peer.Id);
            reader.Recycle();
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine("Connected to server");
            _server = peer;
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine("Disconnected from server!");
        }

        public void SendPacketSerializable<T>(T packet, DeliveryMethod deliveryMethod) where T : INetSerializable
        {
            if (_server == null)
                return;
            _writer.Reset();
            //_writer.Put((byte)type);
            packet.Serialize(_writer);
            _server.Send(_writer, deliveryMethod);
        }
    }
}
