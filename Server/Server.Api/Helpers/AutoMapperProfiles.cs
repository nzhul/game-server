using System.Linq;
using AutoMapper;
using Server.Api.Controllers;
using Server.Api.Models.Input;
using Server.Api.Models.View;
using Server.Api.Models.View.Avatars;
using Server.Api.Models.View.Realms;
using Server.Api.Models.View.UnitConfigurations;
using Server.Models.Heroes;
using Server.Models.Heroes.Units;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            })
            .ForMember(dest => dest.Age, opt =>
            {
                opt.MapFrom(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<User, UserForDetailedDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            })
            .ForMember(dest => dest.Age, opt =>
            {
                opt.MapFrom(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<Photo, PhotosForDetailedDto>();

            CreateMap<UserForUpdateDto, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));


            // REALMS
            //CreateMap<Realm, RealmListItemDto>()
            //    .ForMember(x => x.AvatarsCount, opt =>
            //    {
            //        opt.MapFrom(d => d.Avatars.Count);
            //    })
            //    .ForMember(x => x.RealmType, opt => opt.MapFrom(u => u.Type.ToString()))
            //    .ForMember(x => x.ResetDate, opt => opt.MapFrom(u => u.ResetDate.ToString("dd MMMM yyyy")));

            // REGIONS
            CreateMap<Game, RegionDetailedDto>();
                //.ForMember(x => x.Heroes, opt => opt.MapFrom(u => u.Heroes.Where(h => !h.IsNPC)))
                //.ForMember(x => x.NpcHeroes, opt => opt.MapFrom(u => u.Heroes.Where(h => h.IsNPC)));
            CreateMap<Room, RoomDetailedDto>();

            // DWELLINGS
            CreateMap<Dwelling, DwellingDetailedDto>();
            CreateMap<Dwelling, WaypointDto>()
                .ForMember( x => x.RegionId, opt => opt.MapFrom(u => u.Game.Id))
                .ForMember( x => x.RegionName, opt => opt.MapFrom(u => u.Game.Name));

            // AVATARS
            CreateMap<Hero, HeroDetailedDto>()
                .ForMember(x => x.Class, opt => opt.MapFrom(u => u.Blueprint.Class))
                .ForMember(x => x.Faction, opt => opt.MapFrom(u => u.Blueprint.Faction))
                .ForMember(x => x.OwnerId, opt => opt.MapFrom(u => u.AvatarId))
                .ForMember(x => x.HeroType, opt => opt.MapFrom(u => u.Type));
            CreateMap<Avatar, AvatarDetailedDto>()
                .ForMember(x => x.Heroes, opt =>
                {
                    opt.MapFrom(u => u.Heroes);
                });

            // UNITS
            CreateMap<Unit, UnitDetailedDto>()
                .ForMember(x => x.OwnerId, opt => opt.MapFrom(u => u.Hero.AvatarId))
                .ForMember(x => x.CreatureType, opt => opt.MapFrom(u => u.Type))
                .ForMember(x => x.RegionId, opt => opt.MapFrom(u => u.Hero.GameId));

            // BLUEPRINTS
            CreateMap<HeroBlueprint, Hero>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            // UNIT CONFIGURATIONS
            CreateMap<UnitConfiguration, UnitConfigurationView>();
        }
    }
}
