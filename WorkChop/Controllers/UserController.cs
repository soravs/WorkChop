using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkChop.App_Helper;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.Common.ViewModel;
using WorkChop.Filters;

namespace WorkChop.Controllers
{
    [HandleApiException]
    [ValidateModel]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public HttpResponseMessage GetAllUsers()
        { 
            var users = _userService.GetAll();
            if (users.Any()) return Request.CreateResponse(HttpStatusCode.OK, users);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No users found.");
        }
        //[Authorize]
        [HttpGet]
        [Route("GetParticularUser")]
        public HttpResponseMessage GetParticularUser(Guid userId)
        {
            var users = _userService.Get(userId);
            if (users!=null) return Request.CreateResponse(HttpStatusCode.OK, users);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No users found.");
        }
        [HttpPost]
        [Route("SignUp")]
        public HttpResponseMessage SignUp(UserViewModel user)
        {
            _userService.Insert(user); 

            // Send Email

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        /// <summary>
        /// Sign In user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignIn")]
        public HttpResponseMessage SignIn(LoginViewModel objLoginViewModel)
        {
            var user =  _userService.GetUserByRole(objLoginViewModel);
            if (user == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User Not Found");

            if(user.Password != objLoginViewModel.Password) return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Password didn't match");

            var tkn = AuthToken.GetLoingInfo(objLoginViewModel);

            if(tkn==null) return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Token Unavialable");

            user.TokenModel = tkn;

            return Request.CreateResponse(HttpStatusCode.OK,user);
        }

       
    }
}
