namespace Server.Models.Items
{
    /// <summary>
    /// Item blueprints will work as "Blueprints" for generating new items.
    /// Some of the item properties will vary. And the player has to have some luck to get better weapon!
    /// Also this will enable us to list all possible items in a web page later on.
    /// For example:
    /// One-Hand Damage: (62-87) To (164-474) (113-280.5 Avg)
    // Two-Hand Damage: (145-203) To(289-649) (217-426 Avg)
    // Required Level: 81
    // Required Strength: 189
    // Required Dexterity: 110
    // Base Weapon Speed: [5]
    // +150-250% Enhanced Damage(varies)
    // + (2.5 Per Character Level) 2-247 To Maximum Damage(Based On Character Level)
    // +50% Bonus To Attack Rating
    // +80 To Life
    // +20 To All Attributes
    // Indestructible
    /// </summary>
    public class ItemBlueprint : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ItemSlotType ItemSlotType { get; set; }

        // LevelRequirement ?

        // Minimum Strength required ??

        // Minimum Dexterity required ??
    }
}
