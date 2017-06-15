using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkChop.DataModel.Models;

namespace WorkChop.Common.ViewModel
{
    public class ContentViewModel : BaseViewModel
    {
        public Guid CourseId { get; set; }
        public Guid CategoryId { get; set; }
        public string ContentId { get; set; }
        public string ContentName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsVisibleToAttendees { get; set; }
        public bool SendEmailToAttendees { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public int? CreatedDaysAgo { get; set; }
        public dynamic ContentFile { get; set; }
    }
}
