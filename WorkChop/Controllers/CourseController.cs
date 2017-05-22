using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.DataModel.Models;
using WorkChop.Filters;

namespace WorkChop.Controllers
{
    [Authorize]
    [HandleApiException]
    [ValidateModel]
    [RoutePrefix("api/course")]
    public class CourseController : ApiController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        
        /// <summary>
        /// Method to add new course detail
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addnewcourse")]
        public HttpResponseMessage AddNewCourse(Course course)
        {
            var res = _courseService.AddNewCourse(course);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }

        /// <summary>
        /// Method to get course detail by assign role to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="objAssigneeRoleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcourses")]
        public HttpResponseMessage GetCoursesByFilter(Guid userId, int assigneeRoleId)
        {
            var res = _courseService.GetCoursesByFilter(userId, assigneeRoleId);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }

        [HttpPost]
        [Route("addusercoursemapping")]
        public HttpResponseMessage AddUserCourseMapping(UserCourseMapping userCourseMappingVM)
        {
            var res = _courseService.AddUserCourseMapping(userCourseMappingVM);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }
    }
}
