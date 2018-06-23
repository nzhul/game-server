using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Realms;

namespace Server.Models.Users
{
    public class User : Entity
    {
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSald { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime LastActive { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<Message> MessagesSent { get; set; }

        public ICollection<Message> MessagesRecieved { get; set; }

        public ICollection<Avatar> Avatars { get; set; }

        public int CurrentRealmId { get; set; }

        public User()
        {
            this.Photos = new Collection<Photo>();
            this.MessagesSent = new Collection<Message>();
            this.MessagesRecieved = new Collection<Message>();
            this.Avatars = new Collection<Avatar>();
        }
    }
}
