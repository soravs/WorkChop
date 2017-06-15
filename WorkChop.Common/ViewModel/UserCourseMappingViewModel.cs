using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChop.Common.ViewModel
{
    public class UserCourseMappingViewModel
    {
        public Guid UserCourseMappingId { get; set; }
        public Guid Fk_UserId { get; set; }
        public Guid Fk_CourseId { get; set; }
        public bool IsAssignee { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdateOn { get; set; }
        public string CourseName { get; set; }
        public string UserType { get; set; }
        public double CourseCreatedDays { get; set; }
        public string ImageSrc { get; set; }
    }
}
