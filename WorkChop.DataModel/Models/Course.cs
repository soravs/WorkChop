using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkChop.DataModel.Models
{
    public class Course
    {
        public Course()
        {
            this.Categories = new List<Models.Category>();
        }
        [BsonId]
        [BsonElement("_id")]
        public Guid CourseId { get; set; }
        [Required]
        public string CourseName { get; set; }
       // public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        public string Description { get; set; }
        public List<Guid> Co_Teacher { get; set; }

        public string ImageSrc { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Location { get; set; }
        public List<Category> Categories { get; set; }

    }

    public class Category
    {
        public Category()
        {
            this.Contents = new List<Models.Content>();
        }
        [BsonId]
        [BsonElement("_id")]
        public Guid CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
      
        public List<Content> Contents{get;set;}
    }

    public class Content
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid ContentId { get; set; }
        [Required]
        public string ContentName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsVisibleToAttendees { get; set; }
        public bool SendEmailToAttendees { get; set; }
        public bool IsDeleted { get; set; }
	    public string FileName { get; set; }
	    public string FileType { get; set; }
	    public string FileUrl{get;set;}
    }
    
}
