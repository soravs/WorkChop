using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WorkChop.DataModel.Models
{
    public class UserRoleRelation
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid UserRoleRelationId { get; set; }
        public int Fk_RoleId { get; set; }
        public Guid Fk_UserId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
