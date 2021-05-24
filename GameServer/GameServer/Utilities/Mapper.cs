using AutoMapper;
using GameServer.Models;
using GameServer.Models.Units;
using GameServer.Models.View;

namespace GameServer.Utilities
{
    public class AM
    {
        private static AM _instance;

        public static AM Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AM();
                }
                return _instance;
            }
        }

        public void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Game, GameDetailedDto>();
                cfg.CreateMap<Army, ArmyDetailedDto>()
                    .ForMember(x => x.NPCData, opt => opt.Condition(src => src.IsNPC));
                cfg.CreateMap<Avatar, AvatarDetailedDto>();
                cfg.CreateMap<Unit, UnitDetailedDto>();
                cfg.CreateMap<Dwelling, DwellingDetailedDto>()
                .ForMember(x => x.VisitorsString, opt => opt.Condition(src => src.Team == Team.Neutral));
            });

            Mapper = new Mapper(config);
        }

        public Mapper Mapper { get; private set; }
    }
}
