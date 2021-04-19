using System;
using System.Collections.Generic;
using System.Net.Http;
using Assets.Scripts.Network.Services.HTTP;
using Assets.Scripts.Network.Services.HTTP.Interfaces;

namespace Assets.Scripts.Network.Services
{
    public class RequestManagerHttp
    {
        public static string SERVER_ROOT = "http://localhost:5000/api/";

        private static RequestManagerHttp _instance;

        public static RequestManagerHttp Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RequestManagerHttp();
                }

                return _instance;
            }
        }

        public static HttpClient Client { get; private set; }

        public static IBattleService BattleService { get; private set; }

        public static IUsersService UsersService { get; private set; }

        public static IGameService GameService { get; private set; }

        public static IArmiesService ArmiesService { get; private set; }

        public void Initialize()
        {
            if (_instance == null)
            {
                _instance = new RequestManagerHttp();
            }

            if (Client == null)
            {
                Client = new HttpClient();
                Client.BaseAddress = new Uri(SERVER_ROOT);
                UsersService = new UsersService();
                GameService = new GameService();
                ArmiesService = new ArmiesService();
                BattleService = new BattleService();
            }
        }

        // TODO: Invoke on server start, after admin login
        // var headers = new Dictionary<string, string>();
        // headers.Add("Authorization", "Bearer " + NetworkServer.Instance.Admin.tokenString);
        public void UpdateHeaders(IDictionary<string, string> headers)
        {
            Client.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}
