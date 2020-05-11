using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public abstract class Entity : IAuditedEntity
    {
        [Key]
        public int Id { get; set; }

        //public string CreatedBy { get; set; }

        //public DateTime CreatedAt { get; set; }

        //public string ModifiedBy { get; set; }

        //public DateTime ModifiedAt { get; set; }
    }
}