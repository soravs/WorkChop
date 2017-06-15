using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.Common.Utils;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;
using WorkChop.DataModel.Repository;

namespace WorkChop.BusinessService.BusinessService
{
    public class ContentService : IContentService
    {
        private readonly UnitOfWork _unitOfwork;
        ErrorLogHandler ErrorLogHandler = null;

        public ContentService()
        {
            _unitOfwork = new UnitOfWork();
            ErrorLogHandler = new ErrorLogHandler();
        }

        #region Course Category
        /// <summary>
        /// TO add new category corrosponding to course
        /// </summary>
        /// <param name="categoryVM"></param>
        /// <returns></returns>
        public Category AddUpdateCategory(CategoryViewModel categoryVM)
        {
            try
            {
                var courseDetail = _unitOfwork.CourseRepository.Get(new Guid(categoryVM.CourseId));
                if (courseDetail == null)
                    return null;

                Category category = new Category();
                if (!string.IsNullOrEmpty(categoryVM.CategoryId))
                {
                    var categoryToUpdate = courseDetail.Categories.Where(a => a.CategoryId == new Guid(categoryVM.CategoryId)).FirstOrDefault();
                    if (categoryToUpdate == null)
                        return null;
                    categoryToUpdate.CategoryName = categoryVM.CategoryName;
                    category.UpdatedOn = DateTime.Now;
                }
                else
                {
                    category.CategoryName = categoryVM.CategoryName;
                    category.CreatedOn = DateTime.Now;
                    category.UpdatedOn = DateTime.Now;
                    category.DeletedOn = null;
                    category.CategoryId = Guid.NewGuid();
                    category.IsDeleted = false;
                    courseDetail.Categories.Add(category);
                }

                _unitOfwork.CourseRepository.Update(courseDetail);

                return category;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);
                return null;
            }

        }

        //Check if Category already exists in database
        public bool IsCategoryExist(CategoryViewModel categoryVM)
        {
            try
            {
                var categoryName = _unitOfwork.CourseRepository.GetAll().Where(a =>
                a.CourseId == new Guid(categoryVM.CourseId)
                    && a.Categories.Any(x => !x.IsDeleted
                    && x.CategoryName.Trim().ToLower().Equals(categoryVM.CategoryName.Trim().ToLower())
                    && (categoryVM.CategoryId == string.Empty || x.CategoryId != new Guid(categoryVM.CategoryId))
                    ))
                    .Select(a => a.CourseName).FirstOrDefault();

                if (!string.IsNullOrEmpty(categoryName))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);

                return false;
            }
        }

        public List<Category> GetCategories(Guid courseId,Guid userId)
        {
            var course = _unitOfwork.CourseRepository.Get(courseId);
            if (course == null)
                return null;

            var coursesCategories = course.Categories;
            return coursesCategories.Where(a => !a.IsDeleted).OrderByDescending(a => a.CreatedOn)
                .Select(i => new Category
                {
                    CategoryId = i.CategoryId,
                    CategoryName = i.CategoryName,
                    CreatedOn = i.CreatedOn,
                    UpdatedOn = i.UpdatedOn,
                    DeletedOn = i.DeletedOn,
                    IsDeleted = i.IsDeleted,
                    Contents = i.Contents.Where(a => !a.IsDeleted && (userId== course.CreatedBy || (userId != course.CreatedBy && a.IsVisibleToAttendees))).OrderByDescending(a => a.CreatedOn).ToList()
                }).ToList();

        }

        public CategoryViewModel DeleteCategory(CategoryViewModel categoryVM)
        {
            categoryVM.HasError = true;
            try
            {

                var getCourse = _unitOfwork.CourseRepository.Get(new Guid(categoryVM.CourseId));
                if (getCourse == null)
                {
                    categoryVM.ErrorMessage = "Course not found";
                    return categoryVM;
                }

                var getCategory = getCourse.Categories.Where(a => a.CategoryId == new Guid(categoryVM.CategoryId)).FirstOrDefault();
                if (getCategory == null)
                {
                    categoryVM.ErrorMessage = "Category not found";
                    return categoryVM;
                }

                getCategory.DeletedOn = DateTime.Now;
                getCategory.IsDeleted = true;
                getCategory.UpdatedOn = DateTime.Now;
                _unitOfwork.CourseRepository.Update(getCourse);

                categoryVM.HasError = false;
                categoryVM.ErrorMessage = string.Empty;
                return categoryVM;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);

                categoryVM.ErrorMessage = ex.Message;
                return categoryVM;
            }
        }

        #endregion

        #region Category Content section

        public Content AddUpdateContent(ContentViewModel contentVM)
        {
            try
            {
                if (contentVM == null)
                    return null;

                var courseDetail = _unitOfwork.CourseRepository.Get(contentVM.CourseId);
                if (courseDetail == null)
                    return null;

                var categoryDetail = courseDetail.Categories.Where(a => a.CategoryId == contentVM.CategoryId).FirstOrDefault();

                if (categoryDetail == null)
                    return null;

                var contentDetail = new Content();



                if (!string.IsNullOrEmpty(contentVM.ContentId))
                {
                    contentDetail = courseDetail.Categories.Where(a => a.CategoryId == contentVM.CategoryId).FirstOrDefault().Contents.Where(a => a.ContentId == new Guid(contentVM.ContentId)).FirstOrDefault();
                    if (contentDetail == null)
                        return null;

                    contentDetail.CreatedOn = contentDetail.CreatedOn;
                }
                contentDetail.ContentName = contentVM.ContentName;
                contentDetail.FileName = contentVM.FileName;
                contentDetail.FileType = contentVM.FileType;
                contentDetail.FileUrl = IsUrlEncoded(contentVM.FileUrl)? contentVM.FileUrl :HttpUtility.UrlEncode(contentVM.FileUrl);
                contentDetail.UpdatedOn = DateTime.Now;
                contentDetail.DeletedOn = null;
                contentDetail.IsDeleted = false;
                contentDetail.IsVisibleToAttendees = contentVM.IsVisibleToAttendees;
                contentDetail.SendEmailToAttendees = contentVM.SendEmailToAttendees;

                if (string.IsNullOrEmpty(contentVM.ContentId))
                {
                    contentDetail.ContentId = Guid.NewGuid();
                    contentDetail.CreatedOn = DateTime.Now;
                    categoryDetail.Contents.Add(contentDetail);
                }


                _unitOfwork.CourseRepository.Update(courseDetail);
                return contentDetail;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);
                return null;
            }

        }

        private bool IsUrlEncoded(string source)
        {
            return source != HttpUtility.UrlDecode(source);
        }

        //Check if Content already exists in database
        public bool IsContentExist(ContentViewModel contentVM)
        {
            try
            {
                var courseWithSameContent = _unitOfwork.CourseRepository.GetAll().Where(a =>
                    a.CourseId == contentVM.CourseId
                    && a.Categories.Any(x => !x.IsDeleted
                    && x.CategoryId == contentVM.CategoryId
                    && x.Contents.Any(z => !z.IsDeleted
                    && z.ContentName.Trim().ToLower().Equals(contentVM.ContentName.Trim().ToLower())
                    &&((contentVM.ContentId == string.Empty) ||(z.ContentId  != new Guid(contentVM.ContentId)))
                    ))).Select(a => a.CourseName).FirstOrDefault();

                if (!string.IsNullOrEmpty(courseWithSameContent))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);

                return false;
            }
        }

        

        public ContentViewModel DeleteContent(ContentViewModel contentVM)
        {

            contentVM.HasError = true;
            try
            {
                var getCourse = _unitOfwork.CourseRepository.Get(contentVM.CourseId);

                if (getCourse == null)
                {
                    contentVM.ErrorMessage = "Course not found";
                    return contentVM;
                }


                var getCategory = getCourse.Categories.Where(a => a.CategoryId == contentVM.CategoryId).Select(a => new Category
                {
                    CategoryId = a.CategoryId,
                    CategoryName = a.CategoryName,
                    Contents = a.Contents.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedOn).ToList(),
                    CreatedOn = a.CreatedOn,
                    DeletedOn = a.DeletedOn,
                    IsDeleted = a.IsDeleted,
                    UpdatedOn = a.UpdatedOn
                }).FirstOrDefault();

                if (getCategory == null)
                {
                    contentVM.ErrorMessage = "Category not found";
                    return contentVM;
                }

                var getContent = getCourse.Categories.Where(a => a.CategoryId == contentVM.CategoryId).FirstOrDefault().Contents.Where(a => a.ContentId == new Guid(contentVM.ContentId)).FirstOrDefault();

                if (getContent == null)
                {
                    contentVM.ErrorMessage = "Content not found";
                    return contentVM;
                }


                getContent.DeletedOn = DateTime.Now;
                getContent.IsDeleted = true;
                getContent.UpdatedOn = DateTime.Now;

                _unitOfwork.CourseRepository.Update(getCourse);


                contentVM.HasError = false;
                return contentVM;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);

                contentVM.ErrorMessage = ex.Message;
                return contentVM;
            }
        }
        #endregion

    }
}
