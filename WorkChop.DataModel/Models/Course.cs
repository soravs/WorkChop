using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WorkChop.DataModel.Models
{
    public class Course
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        public UserCourseMapping UserCourseMapping { get; set; }
    }
}
