using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WorkChop.DataModel.Models
{
    public class UserRoleMapping
    {
        public int Fk_RoleId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
