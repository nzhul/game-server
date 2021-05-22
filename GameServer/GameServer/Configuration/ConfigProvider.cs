using System.IO;
using Newtonsoft.Json;

namespace GameServer.Configuration
{
    public class ConfigProvider
    {
        private static ConfigProvider _instance;

        static ConfigProvider()
        {
        }

        public static ConfigProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConfigProvider();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        public ServerConfiguration ServerConfigraution { get; private set; }

        private void Initialize()
        {
            if (ServerConfigraution == null)
            {
                var configJson = File.ReadAllText("Configuration/server-config.json");
                ServerConfigraution = JsonConvert.DeserializeObject<ServerConfiguration>(configJson);
            }
        }
    }
}
