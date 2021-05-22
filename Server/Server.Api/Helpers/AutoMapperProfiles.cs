using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Server.Api.Controllers;
using Server.Api.Models.Input;
using Server.Api.Models.View;
using Server.Api.Models.View.Avatars;
using Server.Api.Models.View.Games;
using Server.Api.Models.View.Realms;
using Server.Api.Models.View.UnitConfigurations;
using Server.Models.Armies;
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

            // GAMES
            CreateMap<Game, GameDetailedDto>()
                .ForMember(x => x.Avatars, opt => opt.MapFrom(u => u.Users));

            // REGIONS
            //CreateMap<Game, RegionDetailedDto>();
                //.ForMember(x => x.Heroes, opt => opt.MapFrom(u => u.Heroes.Where(h => !h.IsNPC)))
                //.ForMember(x => x.NpcHeroes, opt => opt.MapFrom(u => u.Heroes.Where(h => h.IsNPC)));
            CreateMap<Room, RoomDetailedDto>();

            // DWELLINGS
            CreateMap<Dwelling, DwellingDetailedDto>()
                .ForMember(x => x.VisitorsString, opt => opt.Condition(src => src.Team == Team.Neutral));
            //CreateMap<Dwelling, WaypointDto>()
            //    .ForMember( x => x.RegionId, opt => opt.MapFrom(u => u.Game.Id))
            //    .ForMember( x => x.RegionName, opt => opt.MapFrom(u => u.Game.Name));

            // AVATARS
            CreateMap<Unit, UnitDetailedDto>();

            //CreateMap<Avatar, AvatarDetailedDto>()
            //    .ForMember(x => x.Heroes, opt => opt.Ignore())
            //    .ForMember(x => x.Dwellings, opt => opt.Ignore())
            //    .AfterMap((r, rm) => rm.Heroes = new List<int>(r.Heroes.Select(c => c.Id)))
            //    .AfterMap((r, rm) => rm.Dwellings = new List<int>(r.Dwellings.Select(c => c.Id)));

            CreateMap<User, AvatarDetailedDto>() // TODO: refactor this to use ConvertUsing
                .ForMember(x => x.UserId, opt => opt.MapFrom(u => u.Id))
                .ForMember(x => x.Wood, opt => opt.MapFrom(u => u.Avatar.Wood))
                .ForMember(x => x.Ore, opt => opt.MapFrom(u => u.Avatar.Ore))
                .ForMember(x => x.Gold, opt => opt.MapFrom(u => u.Avatar.Gold))
                .ForMember(x => x.Gems, opt => opt.MapFrom(u => u.Avatar.Gems))
                .ForMember(x => x.Team, opt => opt.MapFrom(u => u.Avatar.Team))
                .ForMember(x => x.VisitedString, opt => opt.MapFrom(u => u.Avatar.VisitedString));

            // UNIT CONFIGURATIONS
            CreateMap<UnitConfiguration, UnitConfigurationView>();


            // Army
            CreateMap<Army, ArmyDetailedDto>()
                .ForMember(x => x.NPCData, opt => opt.Condition(src => src.IsNPC));
        }
    }
}
