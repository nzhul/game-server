using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Models.Items
{
    public class ItemBlueprint : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ItemSlotType ItemSlotType { get; set; }
    }
}
