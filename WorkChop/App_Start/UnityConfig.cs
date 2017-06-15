using Microsoft.Practices.Unity;
using System.Web.Http;
using WorkChop.App_Helper;
using WorkChop.BusinessService.BusinessService;
using WorkChop.BusinessService.IBusinessService;

namespace WorkChop
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ICourseService, CourseService>();
            container.RegisterType<IContentService, ContentService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);
        }
    }
}