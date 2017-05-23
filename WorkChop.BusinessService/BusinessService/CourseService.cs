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
                courseVM.IsActive = true;
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
            var userCourseMappingList = (from course in _unitOfwork.CourseRepository.GetAll()
                                         join userCourseMapping in _unitOfwork.UserCourseMappingRepository.GetAll()
                                         on course.CourseId equals userCourseMapping.Fk_CourseId
                                         where userCourseMapping.Fk_UserId == userId &&  assignRoleId == 1 ? 1 == 1 : userCourseMapping.IsAssignee == isAssignee
                                         select new UserCourseMappingViewModel
                                         {
                                             UserCourseMappingId = userCourseMapping.UserCourseMappingId,
                                             Fk_UserId = userCourseMapping.Fk_UserId,
                                             Fk_CourseId = userCourseMapping.Fk_CourseId,
                                             CourseName = course.CourseName,
                                             CourseCreatedDays = (DateTime.UtcNow - course.CreatedOn).TotalDays,
                                             IsActive = course.IsActive,
                                             IsAssignee = userCourseMapping.IsAssignee,
                                             UserType = userCourseMapping.IsAssignee ? "Owner" : "Enrolled"
                                         }).ToList();

            //foreach (var userCourseMapping in userCourseMappingList)
            //{
            //    //switch (assigneeRoleId)
            //    //{
            //    //    case (int)AssigneeRole.Enrolled:
            //    //        userCourseMapping.UserType = "Enrolled";
            //    //        break;


            //    //    case (int)AssigneeRole.Self:
            //    //        userCourseMapping.UserType = "Owner";
            //    //        break;

            //    //    default:
            //    //        userCourseMapping.UserType = userCourseMapping.IsAssignee ? "Owner" : "Enrolled";
            //    //        break;
            //    //}
            //}
            return userCourseMappingList;
        }
    }
}
