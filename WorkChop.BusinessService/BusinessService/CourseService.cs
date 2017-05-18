using System;
using System.Collections.Generic;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.DataModel.Models;
using WorkChop.DataModel.Repository;
using static WorkChop.Common.EnumUtil;
using System.Linq;

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
        public Course AddNewCourse(Course course)
        {
            course.CourseId = new Guid();
            course.IsActive = true;
            course.CreatedOn = DateTime.UtcNow;
            course.UpdatedOn = DateTime.UtcNow;

            // Set IsAssignee=true in UserCourseMapping table when add course 
            //by teacher/someone
            course.UserCourseMapping = new UserCourseMapping
            {
                Fk_UserId = course.CreatedBy,
                IsAssignee = true,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
            _unitOfwork.CourseRepository.Add(course);
            return course;
        }

        /// <summary>
        /// Method to get course detail by assign role id to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assigneeRoleId"></param>
        /// <returns></returns>
        public List<Course> GetCoursesByFilter(Guid userId, int assigneeRoleId)
        {
            var courseVM = _unitOfwork.CourseRepository.GetDbSet(x => x.CreatedBy == userId && x.IsActive);
            switch (assigneeRoleId)
            {
                case (int)AssigneeRole.Enrolled:
                    return courseVM.Where(x=>x.UserCourseMapping.IsAssignee==false).ToList();

                case (int)AssigneeRole.Self:
                    return courseVM.Where(x => x.UserCourseMapping.IsAssignee == true).ToList();

                default:
                    return courseVM.ToList();
            }
        }
    }
}
