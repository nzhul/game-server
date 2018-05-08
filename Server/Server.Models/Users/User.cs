﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Server.Models.Heroes;

namespace Server.Models.Users
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSald { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<Message> MessagesSent { get; set; }

        public ICollection<Message> MessagesRecieved { get; set; }

        public ICollection<Hero> Heroes { get; set; }

        public User()
        {
            this.Photos = new Collection<Photo>();
            this.Heroes = new Collection<Hero>();
        }
    }
}
