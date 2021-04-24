using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Shared
{
    public static class PacketRegistry
    {
        private static Dictionary<PacketType, Type> _packetTypes = new Dictionary<PacketType, Type>();

        public static Dictionary<PacketType, Type> PacketTypes
        {
            get
            {
                if (_packetTypes.Count > 0)
                {
                    return _packetTypes;
                }

                throw new Exception("PacketRegistry is not initialized! Please invoke PacketRegistry.Initialize() on StartUp");
            }
        }

        public static void Initialize()
        {
            if (_packetTypes.Count > 0)
            {
                return;
            }

            var packetType = typeof(INetPacket);
            var packets = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => packetType.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var packet in packets)
            {
                var instance = (INetPacket)Activator.CreateInstance(packet);
                _packetTypes.Add(instance.Type, packet);
            }
        }
    };
}