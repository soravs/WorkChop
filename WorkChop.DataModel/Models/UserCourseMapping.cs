using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WorkChop.DataModel.Models
{
    public class UserCourseMapping
    {
        public Guid Fk_UserId { get; set; }
        public bool IsAssignee { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
