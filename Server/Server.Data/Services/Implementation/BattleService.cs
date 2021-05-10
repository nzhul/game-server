using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data.Services.Abstraction;

namespace Server.Data.Services.Implementation
{
    public class BattleService : BaseService, IBattleService
    {
        private readonly IMapper _mapper; // TODO: put mapper in BaseService.

        public BattleService(
            DataContext context,
            IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> RegisterBattle(Guid battleId, int userId)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (dbUser == null)
            {
                return false;
            }

            dbUser.BattleId = battleId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnRegisterBattle(int userId)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (dbUser == null)
            {
                return false;
            }

            dbUser.BattleId = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
