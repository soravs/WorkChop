using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkChop.DataModel.Models
{
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid UserID { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Nullable<DateTime> DeletedOn { get; set; }

        public List<UserCourseMapping> UserCourseMapping { get; set; }
       // public UserRoleMapping UserRoleMapping { get; set; }

    }
}
