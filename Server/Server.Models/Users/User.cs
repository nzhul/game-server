using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Server.Models.Users
{
    public class User : IdentityUser<int>, IAuditedEntity
    {
        public string Discriminator { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime LastActive { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<Message> MessagesSent { get; set; }

        public virtual ICollection<Message> MessagesRecieved { get; set; }

        public virtual ICollection<Avatar> Avatars { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<Friendship> SendFriendRequests { get; set; }

        public virtual ICollection<Friendship> RecievedFriendRequests { get; set; }

        public int CurrentRealmId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public int ActiveConnection { get; set; }

        public byte OnlineStatus { get; set; }

        public User()
        {
            this.Photos = new Collection<Photo>();
            this.MessagesSent = new Collection<Message>();
            this.MessagesRecieved = new Collection<Message>();
            this.Avatars = new Collection<Avatar>();
            this.SendFriendRequests = new Collection<Friendship>();
            this.RecievedFriendRequests = new Collection<Friendship>();
        }
    }
}
