using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;

namespace WorkChop.BusinessService.IBusinessService
{
    public interface IContentService
    {
        Category AddUpdateCategory(CategoryViewModel categoryVM);
        bool IsCategoryExist(CategoryViewModel categoryVM);
        CategoryViewModel DeleteCategory(CategoryViewModel categoryVM);
        List<Category> GetCategories(Guid courseId, Guid userId);
        Content AddUpdateContent(ContentViewModel contentVM);
        
        bool IsContentExist(ContentViewModel contentVM);
        ContentViewModel DeleteContent(ContentViewModel contentVM);
    }
}
