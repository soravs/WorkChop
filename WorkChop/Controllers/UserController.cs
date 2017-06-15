using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkChop.App_Helper;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.Common.Utils;
using WorkChop.Common.ViewModel;
using WorkChop.DataModel.Models;
using WorkChop.Filters;

namespace WorkChop.Controllers
{
    [HandleApiException]
    [ValidateModel]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Method to get all users
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getallusers")]
        public HttpResponseMessage GetAllUsers()
        {
            var users = _userService.GetAll();
            if (users.Any()) return Request.CreateResponse(HttpStatusCode.OK, users);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No users found.");
        }

        /// <summary>
        /// Method to get user by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getparticularuser")]
        public HttpResponseMessage GetParticularUser(Guid userId)
        {
            var users = _userService.Get(userId);
            if (users != null) return Request.CreateResponse(HttpStatusCode.OK, users);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No users found.");
        }

        /// <summary>
        /// Method to register user detail by user viewmodel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signup")]
        public HttpResponseMessage SignUp(User userVM)
        {
            _userService.Insert(userVM);
            return Request.CreateResponse(HttpStatusCode.Created, userVM);
        }

        /// <summary>
        /// Method to Sign In user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signin")]
        public HttpResponseMessage SignIn(LoginViewModel objLoginViewModel)
        {
            var user = _userService.GetUserByRole(objLoginViewModel);
            if (user.HasError) return Request.CreateErrorResponse(HttpStatusCode.NotFound, user.ErrorMessage);



            var decryptedPassword = Security.Decrypt(user.Password, user.PasswordSalt);

            if (!string.IsNullOrEmpty(decryptedPassword))
            {
                if (!decryptedPassword.Equals(objLoginViewModel.Password))
                {
                    user.ErrorMessage = "Password didn't match";
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, user.ErrorMessage);
                }
            }

            var token = AuthToken.GetLoingInfo(objLoginViewModel);

            if (token == null)
            {
                user.ErrorMessage = "Token Unavailable";
                return Request.CreateErrorResponse(HttpStatusCode.NotModified, user.ErrorMessage);
            }

            user.TokenModel = token;

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [Authorize]
        [HttpGet]
        [Route("getAllTeachers")]
        public HttpResponseMessage GetAllTeachers(string userId)
        {
            var users = _userService.GetAllTeachers(userId);
            if (users.Any()) return Request.CreateResponse(HttpStatusCode.OK, users);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No users found.");
        }

    }
}
