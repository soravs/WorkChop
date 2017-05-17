using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WorkChop.Filters
{
    public class HandleApiExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        ///  Api exception filter
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var request = actionExecutedContext.ActionContext.Request;
            actionExecutedContext.Response = request.CreateResponse(HttpStatusCode.InternalServerError, actionExecutedContext.Exception.Message);
        }
    }
}