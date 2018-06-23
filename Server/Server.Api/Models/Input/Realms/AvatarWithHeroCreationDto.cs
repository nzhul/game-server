using Server.Models.Heroes;

namespace Server.Api.Models.Input.Realms
{
    public class AvatarWithHeroCreationDto
    {
        public int UserId { get; set; }

        public int RealmId { get; set; }

        public string HeroClassName { get; set; }

        public HeroFaction Faction { get; set; }
    }
}