using System.Linq;
using AutoMapper;
using Server.Api.Controllers;
using Server.Api.Models.Input;
using Server.Api.Models.View;
using Server.Api.Models.View.Avatars;
using Server.Api.Models.View.Realms;
using Server.Models.Heroes;
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
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<User, UserForDetailedDto>()
            .ForMember(dest => dest.PhotoUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            })
            .ForMember(dest => dest.Age, opt =>
            {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
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
            CreateMap<Realm, RealmListItemDto>()
                .ForMember(x => x.AvatarsCount, opt =>
                {
                    opt.ResolveUsing(d => d.Avatars.Count);
                })
                .ForMember(x => x.RealmType, opt => opt.MapFrom(u => u.Type.ToString()))
                .ForMember(x => x.ResetDate, opt => opt.MapFrom(u => u.ResetDate.ToString("dd MMMM yyyy")));

            // REGIONS
            CreateMap<Region, RegionDetailedDto>();
            CreateMap<Room, RoomDetailedDto>();

            // DWELLINGS
            CreateMap<Dwelling, DwellingDetailedDto>();
            CreateMap<Dwelling, WaypointDto>()
                .ForMember( x => x.RegionId, opt => opt.MapFrom(u => u.Region.Id))
                .ForMember( x => x.RegionLevel, opt => opt.MapFrom(u => u.Region.Level))
                .ForMember( x => x.RegionName, opt => opt.MapFrom(u => u.Region.Name));

            // AVATARS
            CreateMap<Hero, HeroDetailedDto>()
                .ForMember(x => x.Class, opt => opt.MapFrom(u => u.Blueprint.Class))
                .ForMember(x => x.Faction, opt => opt.MapFrom(u => u.Blueprint.Faction));
            CreateMap<Avatar, AvatarDetailedDto>()
                .ForMember(x => x.Heroes, opt =>
                {
                    opt.MapFrom(u => u.Heroes);
                });

            // BLUEPRINTS
            CreateMap<HeroBlueprint, Hero>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
