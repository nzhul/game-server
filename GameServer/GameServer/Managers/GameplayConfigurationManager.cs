using System;
using System.Collections.Generic;
using Assets.Scripts.Network.Services;
using GameServer.Models;
using NetworkingShared.Enums;

namespace GameServer.Managers
{
    public class GameplayConfigurationManager
    {
        private static GameplayConfigurationManager _instance;

        public static GameplayConfigurationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameplayConfigurationManager();
                }

                return _instance;
            }
        }

        public Dictionary<CreatureType, UnitConfiguration> UnitConfigurations { get; private set; }

        public void Initialize()
        {
            UnitConfigurations = RequestManagerHttp.GameService.GetUnitConfigurations();
            Console.WriteLine("Unit configurations loaded successfully!");
        }
    }
}
