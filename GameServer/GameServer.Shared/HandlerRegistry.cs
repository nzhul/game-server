using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using GameServer.Shared.Attributes;

namespace GameServer.Shared
{
    public static class HandlerRegistry
    {
        private static Dictionary<PacketType, IPacketHandler> _handlers = new Dictionary<PacketType, IPacketHandler>();

        public static Dictionary<PacketType, IPacketHandler> Handlers
        {
            get
            {
                if (_handlers.Count > 0)
                {
                    return _handlers;
                }

                throw new Exception("HandlerRegistry is not initialized! Please invoke HandlerRegistry.Initialize() on StartUp");
            }
        }

        public static void Initialize()
        {
            if (_handlers.Count > 0)
            {
                return;
            }

            var handlers = AssemblyLoadContext.Default
                .Assemblies.SelectMany(x => x.DefinedTypes)
                // Find concrete classes only
                .Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition)
                // Find only types implementing the interface
                .Where(x => typeof(IPacketHandler).IsAssignableFrom(x))
                // Find those with our attribute
                .Select(t => (type: t, attr: t.GetCustomAttribute<HandlerRegisterAttribute>()))
                .Where(x => x.attr != null)
                // Find those that have a parameterless constructor of ANY visibility
                .Where(x => x.type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null) != null);

            foreach (var (type, attr) in handlers)
            {
                if (!_handlers.ContainsKey(attr.PacketType))
                {
                    _handlers[attr.PacketType] = (IPacketHandler)Activator.CreateInstance(type, nonPublic: true);
                }
                else
                {
                    throw new Exception($"Multiple handlers for `{attr.PacketType}` packet type detected! Only one handler per packet type is supported!");
                }
            }

            Console.WriteLine($"{handlers.Count()} handlers where registered!");
        }
    }
}
