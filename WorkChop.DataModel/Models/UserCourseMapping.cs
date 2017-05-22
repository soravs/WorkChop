using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WorkChop.DataModel.Models
{
    public class UserCourseMapping
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid UserCourseMappingId { get; set; }
        public Guid Fk_UserId { get; set; }
        public Guid Fk_CourseId { get; set; }
        public bool IsAssignee { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdateOn { get; set; }

        //public Guid Fk_UserId { get; set; }
        //public bool IsAssignee { get; set; }
        //public bool IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public DateTime UpdatedOn { get; set; }

        //public Course Course { get; set; }
    }
}
