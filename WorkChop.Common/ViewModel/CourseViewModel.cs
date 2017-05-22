using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChop.DataModel.Models;

namespace WorkChop.Common.ViewModel
{
    public class CourseViewModel : BaseViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
