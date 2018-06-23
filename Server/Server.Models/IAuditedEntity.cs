using System;

namespace Server.Models
{
    public interface IAuditedEntity
    {
        string CreatedBy { get; set; }

        DateTime CreatedAt { get; set; }

        string ModifiedBy { get; set; }

        DateTime ModifiedAt { get; set; }
    }
}
