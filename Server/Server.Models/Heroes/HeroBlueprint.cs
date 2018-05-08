namespace Server.Models.Heroes
{
    public class HeroBlueprint
    {

        #region Meta

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PortraitImgUrl { get; set; }

        public int HeroClassId { get; set; }

        public HeroBlueprintClass HeroClass { get; set; }

        #endregion

        #region Army

        /// <summary>
        /// Army attack modifier
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// Army defense modifier
        /// </summary>
        public int Defense { get; set; }

        #endregion

        #region Combat

        /// <summary>
        /// Starting attack of the hero unit in combat.
        /// </summary>
        public int PersonalAttack { get; set; }

        /// <summary>
        /// % of the incomming physical damage that is reduced
        /// Example: Defense=20; Damage=100; FinalDamage=80;
        /// </summary>
        public int PersonalDefense { get; set; }

        /// <summary>
        /// The mana of the hero. Magic == Mana
        /// </summary>
        public int Magic { get; set; }

        /// <summary>
        /// Spell power. Defines how powerfull are the spell the hero casts
        /// </summary>
        public int MagicPower { get; set; }

        /// <summary>
        /// % chance to dodge incomming physical or magic attack
        /// </summary>
        public int Dodge { get; set; }

        public int Health { get; set; }

        public int MinDamage { get; set; }

        public int MaxDamage { get; set; }

        /// <summary>
        /// % of the incomming magic damage that is reduced
        /// Example: MagicResistance=20; Damage=100; FinalDamage=80;
        /// </summary>
        public int MagicResistance { get; set; }

        //Possible attributes: Earth, Water, Lightning, Fire resistance %
        //Possible attributes: Earth, Water, Lightning, Fire spell power %

        #endregion
    }
}