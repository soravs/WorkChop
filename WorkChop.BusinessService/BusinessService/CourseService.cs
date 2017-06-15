using System;
using System.Collections.Generic;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.DataModel.Models;
using WorkChop.DataModel.Repository;
using System.Linq;
using WorkChop.Common.ViewModel;
using AutoMapper;
using WorkChop.Common;
using WorkChop.Common.Utils;

namespace WorkChop.BusinessService.BusinessService
{
    public class CourseService : ICourseService
    {
        public const string CourseImage = "../../assets/images/Asset 4.png";
        private readonly UnitOfWork _unitOfwork;
        ErrorLogHandler ErrorLogHandler = null;

        public CourseService()
        {
            _unitOfwork = new UnitOfWork();
            ErrorLogHandler = new ErrorLogHandler();
        }

        #region Basic Course related Methods
        public Course AddNewCourse(Course courseVM)
        {
            try
            {
                courseVM.CourseId = new Guid();
                // courseVM.IsActive = true;
                courseVM.CreatedOn = DateTime.UtcNow;
                courseVM.UpdatedOn = DateTime.UtcNow;

                // Set IsAssignee=true in UserCourseMapping table when add course 
                //by teacher/someone
                _unitOfwork.CourseRepository.Add(courseVM);
                var userCourseMappingVM = new UserCourseMapping()
                {
                    Fk_CourseId = courseVM.CourseId,
                    Fk_UserId = courseVM.CreatedBy,
                    IsAssignee = true
                };
                AddUserCourseMapping(userCourseMappingVM);
                return courseVM;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);
                return null;
            }
        }

        /// <summary>
        /// Method to Add new user course mapping
        /// </summary>
        /// <param name="userCourseMappingVM"></param>
        public UserCourseMapping AddUserCourseMapping(UserCourseMapping userCourseMappingVM)
        {
            userCourseMappingVM.UserCourseMappingId = new Guid();
            userCourseMappingVM.IsActive = true;
            userCourseMappingVM.CreatedOn = DateTime.UtcNow;
            userCourseMappingVM.UpdateOn = DateTime.UtcNow;

            _unitOfwork.UserCourseMappingRepository.Add(userCourseMappingVM);

            return userCourseMappingVM;
        }

        public Course UpdateCourse(Course courseVM)
        {
            try
            {
                var courseDetail = _unitOfwork.CourseRepository.Get(courseVM.CourseId);
                if (courseDetail != null)
                {
                    courseDetail.UpdatedOn = DateTime.UtcNow;
                    courseDetail.CourseName = courseVM.CourseName;
                    courseDetail.Description = courseVM.Description;
                    courseDetail.ImageSrc = courseVM.ImageSrc;
                    courseDetail.Latitude = courseVM.Latitude;
                    courseDetail.Longitude = courseVM.Longitude;
                    courseDetail.Location = courseVM.Location;
                    _unitOfwork.CourseRepository.Update(courseDetail);
                }
                return courseVM;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);

                return null;
            }
        }

        public CourseViewModel DeleteCourse(Guid courseId)
        {
            CourseViewModel objCourseViewModel = new CourseViewModel();
            objCourseViewModel.HasError = true;
            try
            {
                var getCourse = _unitOfwork.CourseRepository.Get(courseId);

                if (getCourse == null)
                {
                    objCourseViewModel.ErrorMessage = "Course not found";
                    return objCourseViewModel;
                }
                  
                getCourse.DeletedOn = DateTime.UtcNow;
                getCourse.UpdatedOn = DateTime.UtcNow;
                _unitOfwork.CourseRepository.Update(getCourse);


                AutoMapper.Mapper.CreateMap<Course, CourseViewModel>();
                objCourseViewModel = AutoMapper.Mapper.Map<Course, CourseViewModel>(getCourse);

                objCourseViewModel.HasError = false;

                return objCourseViewModel;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);

                objCourseViewModel.ErrorMessage = ex.Message;
                return objCourseViewModel;
            }
        }

        public CourseViewModel LeaveCourse(Guid UserCourseMappingId)
        {
            CourseViewModel objCourseViewModel = new CourseViewModel();
            objCourseViewModel.HasError = true;
            try
            {
                var getCourseMapping = _unitOfwork.UserCourseMappingRepository.Get(UserCourseMappingId);

                if (getCourseMapping == null)
                {
                    objCourseViewModel.ErrorMessage = "Course Mapping not found";
                    return objCourseViewModel;
                }

                getCourseMapping.IsActive = false;
                getCourseMapping.UpdateOn = DateTime.UtcNow;
                _unitOfwork.UserCourseMappingRepository.Update(getCourseMapping);

                AutoMapper.Mapper.CreateMap<UserCourseMapping, CourseViewModel>();
                objCourseViewModel = AutoMapper.Mapper.Map<UserCourseMapping, CourseViewModel>(getCourseMapping);

                objCourseViewModel.HasError = false;
                return objCourseViewModel;
            }
            catch (Exception ex)
            {
                //To save Exception in database
                string methodName = ErrorLogHandler.GetCurrentMethod();
                ErrorLogHandler.SaveException(ex, methodName);

                objCourseViewModel.ErrorMessage = ex.Message;
                return objCourseViewModel;
            }
        }

        #endregion

        #region Get Courses by filters and validations/authorization methods

        /// <summary>
        /// Method to get course detail by assign role id to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assigneeRoleId"></param>
        /// <returns></returns>
        public List<UserCourseMappingViewModel> GetCoursesByFilter(Guid userId, int assignRoleId)
        {
            bool isAssignee = true;
            switch (assignRoleId)
            {
                case 2:
                    isAssignee = false;
                    break;
                case 3:
                    isAssignee = true;
                    break;
            }

            var userCourseMappingList = (from course in _unitOfwork.CourseRepository.GetDbSet(x => x.DeletedOn == null)
                                         join userCourseMapping in _unitOfwork.UserCourseMappingRepository.GetDbSet(x => x.IsActive && x.Fk_UserId == userId)
                                         on course.CourseId equals userCourseMapping.Fk_CourseId
                                         where assignRoleId == 1 ? 1 == 1 : userCourseMapping.IsAssignee == isAssignee
                                         select new UserCourseMappingViewModel
                                         {
                                             UserCourseMappingId = userCourseMapping.UserCourseMappingId,
                                             Fk_UserId = userCourseMapping.Fk_UserId,
                                             Fk_CourseId = userCourseMapping.Fk_CourseId,
                                             CourseName = course.CourseName,
                                             CourseCreatedDays = (int)(DateTime.UtcNow - course.CreatedOn).TotalDays,
                                             IsActive = userCourseMapping.IsActive,
                                             IsAssignee = userCourseMapping.IsAssignee,
                                             ImageSrc = !string.IsNullOrEmpty(course.ImageSrc) ? course.ImageSrc : CourseImage,
                                             UserType = userCourseMapping.IsAssignee ? "Owner" : "Enrolled",
                                             CreatedOn=course.CreatedOn
                                         }).OrderByDescending(a=>a.CreatedOn).ToList();


            return userCourseMappingList;
        }

        public Course GetCourseById(string courseId)
        {
            if (courseId == "undefined" || string.IsNullOrEmpty(courseId)) return null;

            var getCourse = _unitOfwork.CourseRepository.Get(new Guid(courseId));

            if (getCourse == null)
                return null;

            return getCourse;

        }

        /// <summary>
        /// To check if Logged in user is owner of course or not
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool IsCourseOwner(Guid userId, Guid courseId)
        {
            if (userId== Guid.Empty || courseId== Guid.Empty)
                return false;

            return _unitOfwork.UserCourseMappingRepository.GetDbSet(a => a.Fk_UserId == userId && a.Fk_CourseId == courseId).Select(a => a.IsAssignee).FirstOrDefault();
        }
        public bool IsCourseExist(Course courseDetail)
        {
            try
            {
                var name = (from course in _unitOfwork.CourseRepository.GetDbSet(x => x.DeletedOn == null)
                            join userCourseMapping in _unitOfwork.UserCourseMappingRepository.GetDbSet(
                                x => x.IsActive 
                                && x.Fk_UserId == courseDetail.CreatedBy 
                                & x.IsAssignee
                                && (courseDetail.CourseId==Guid.Empty || x.Fk_CourseId!=courseDetail.CourseId)
                                )
                            on course.CourseId equals userCourseMapping.Fk_CourseId
                            where course.CourseName.Trim().ToUpper() == courseDetail.CourseName.Trim().ToUpper()
                            select course.CourseName).FirstOrDefault();

                if (!string.IsNullOrEmpty(name))
                    return true;

                return false;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

    }
}
