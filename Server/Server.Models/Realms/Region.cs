using System.Collections.Generic;

namespace Server.Models.Realms
{
    public class Region : Entity
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public int? RealmId { get; set; }

        public Realm Realm { get; set; }


        #region MapData

        public string MapMatrix { get; set; }

        public List<Room> Rooms { get; set; }

        #endregion
    }
}