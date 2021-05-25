using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Users
{
    public class Friendship
    {
        public int SenderId { get; set; }

        public int RecieverId { get; set; }

        public virtual User Sender { get; set; }

        public virtual User Reciever { get; set; }

        public DateTime? RequestTime { get; set; }

        public DateTime? BecameFriendsTime { get; set; }

        public FriendshipState State { get; set; }

        [NotMapped]
        public bool IsApproved => State == FriendshipState.Approved;
    }

    public enum FriendshipState
    {
        Pending,
        Approved,
        Rejected,
        Blocked,
        Spam
    }
}


//var clubs = dbContext.People
//    .Where(p => p.PersonId == id)
//    .SelectMany(p => p.PersonClubs);
//    .Select(pc => pc.Club);