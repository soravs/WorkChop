using System;
using System.Collections.Generic;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.DataModel.Models;
using WorkChop.DataModel.Repository;
using System.Linq;
using WorkChop.Common.ViewModel;
using AutoMapper;
using static WorkChop.Common.EnumUtil;

namespace WorkChop.BusinessService.BusinessService
{
    public class CourseService : ICourseService
    {
        private readonly UnitOfWork _unitOfwork;

        public CourseService()
        {
            _unitOfwork = new UnitOfWork();
        }

        /// <summary>
        /// Method to Add New Course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
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
                return null;
            }
        }

        /// <summary>
        /// Update Course detail
        /// </summary>
        /// <param name="courseVM"></param>
        /// <returns></returns>
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

            var userCourseMappingList = (from course in _unitOfwork.CourseRepository.GetAll().Where(x => x.DeletedOn == null)
                                         join userCourseMapping in _unitOfwork.UserCourseMappingRepository.GetAll().Where(x => x.IsActive)
                                         on course.CourseId equals userCourseMapping.Fk_CourseId
                                         where userCourseMapping.Fk_UserId == userId && assignRoleId == 1 ? 1 == 1 : userCourseMapping.IsAssignee == isAssignee
                                         select new UserCourseMappingViewModel
                                         {
                                             UserCourseMappingId = userCourseMapping.UserCourseMappingId,
                                             Fk_UserId = userCourseMapping.Fk_UserId,
                                             Fk_CourseId = userCourseMapping.Fk_CourseId,
                                             CourseName = course.CourseName,
                                             CourseCreatedDays = (int)(DateTime.UtcNow - course.CreatedOn).TotalDays,
                                             IsActive = userCourseMapping.IsActive,
                                             IsAssignee = userCourseMapping.IsAssignee,
                                             UserType = userCourseMapping.IsAssignee ? "Owner" : "Enrolled"
                                         }).ToList();


            return userCourseMappingList;
        }

        /// <summary>
        /// To Get the Course Detail
        /// </summary>
        /// <param name="courseVM"></param>
        /// <returns></returns>
        public Course GetCourseById(string courseId)
        {
            if (courseId == "undefined" || string.IsNullOrEmpty(courseId)) return null;

            var getCourse = _unitOfwork.CourseRepository.Get(new Guid(courseId));

            if (getCourse == null)
                return null;

            return getCourse;

        }


        /// <summary>
        /// Delete Course
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public BaseViewModel DeleteCourse(Guid courseId)
        {
            try
            {
                var getCourse = _unitOfwork.CourseRepository.Get(courseId);

                if (getCourse != null)
                {
                    getCourse.DeletedOn = DateTime.UtcNow;
                    getCourse.UpdatedOn = DateTime.UtcNow;
                    _unitOfwork.CourseRepository.Update(getCourse);

                    return new BaseViewModel { HasError = false, ErrorMessage = string.Empty };
                }
                return new BaseViewModel { HasError = true, ErrorMessage = "Course not found" };
            }
            catch (Exception ex)
            {
                return new BaseViewModel { HasError = true, ErrorMessage = ex.Message };
            }
        }
        /// <summary>
        /// Leave Course
        /// </summary>
        /// <param name="userCourseMappingId"></param>
        /// <returns></returns>
        public BaseViewModel LeaveCourse(Guid UserCourseMappingId)
        {
            try
            {
                var getCourseMapping = _unitOfwork.UserCourseMappingRepository.Get(UserCourseMappingId);

                if (getCourseMapping != null)
                {
                    getCourseMapping.IsActive = false;
                    getCourseMapping.UpdateOn = DateTime.UtcNow;
                    _unitOfwork.UserCourseMappingRepository.Update(getCourseMapping);

                    return new BaseViewModel { HasError = false, ErrorMessage = string.Empty };
                }
                return new BaseViewModel { HasError = true, ErrorMessage = "Course Mapping not found" };
            }
            catch (Exception ex)
            {
                return new BaseViewModel { HasError = true, ErrorMessage = ex.Message };
            }
        }
    }
}
