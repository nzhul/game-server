﻿using System.Threading.Tasks;
using Server.Models.Realms;
using Server.Models.Realms.Input;

namespace Server.Data.Services.Abstraction
{
    public interface IGameService : IService
    {
        Task<Game> CreateGameAsync(GameParams gameParams);

        Task<Game> GetGameAsync(int id);
    }
}