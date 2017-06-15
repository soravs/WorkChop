using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChop.DataModel.Models;

namespace WorkChop.Common.ViewModel
{
    public class CategoryViewModel : BaseViewModel
    {
        public string CategoryId { get; set; }
        public string CourseId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        List<ContentViewModel> Content { get; set; }
        public bool IsDeleted { get; set; }
    }
}
