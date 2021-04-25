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

            // Login the API Admin
            try
            {
                var adminData = UsersService.LoginAdmin();
                UpdateHeaders(adminData.tokenString);
                Console.WriteLine("Admin authenticated!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FATAL] Admin user was not authenticated!" + ex.ToString());
                throw;
            }

        }


        // TODO: Refactor this
        // send headers per request. See: https://stackoverflow.com/a/43780538/3937407
        public void UpdateHeaders(string token)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            Client.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}
