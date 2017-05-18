using System;
using System.Collections.Generic;
using WorkChop.DataModel.Models;

namespace WorkChop.BusinessService.IBusinessService
{
    public interface ICourseService
    {
        Course AddNewCourse(Course courseVM);
        List<Course> GetCoursesByFilter(Guid userId, int assigneeRoleId);
    }
}
