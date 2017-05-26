using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WorkChop.DataModel.Models
{
    public class UserRoleMapping
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid UserRoleMappingId { get; set; }
        public int Fk_RoleId { get; set; }
        public Guid Fk_UserId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
