using System;
using System.Collections.Generic;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;

namespace WorkChop.BusinessService.IBusinessService
{
    public interface ICourseService
    {
        Course AddNewCourse(Course courseVM);
        Course UpdateCourse(Course courseVM);
        List<UserCourseMappingViewModel> GetCoursesByFilter(Guid userId, int assigneeRoleId);
        Course GetCourseById(string courseVM);
        UserCourseMapping AddUserCourseMapping(UserCourseMapping userCourseMappingVM);
        BaseViewModel DeleteCourse(Guid CourseId);
        BaseViewModel LeaveCourse(Guid UsercourseMappingId);
    }
}
