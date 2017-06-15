using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WorkChop.BusinessService.IBusinessService;
using WorkChop.Common.ViewModel;
using WorkChop.Filters;
using System.IO;
using System.Net.Http.Headers;

namespace WorkChop.Controllers
{
    [HandleApiException]
    [ValidateModel]
    [RoutePrefix("api/content")]
    public class ContentController : ApiController
    {
        private readonly IContentService _contentService;


        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        #region Category Section

        /// <summary>
        /// Method to add new category coorosponding to course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        ///
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("addupdatecategory")]
        public HttpResponseMessage AddUpdateCategory(CategoryViewModel category)
        {
            var res = _contentService.AddUpdateCategory(category);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }

        [HttpPost]
        [Route("iscategoryexist")]
        public HttpResponseMessage IsCategoryExist(CategoryViewModel category)
        {
            var res = _contentService.IsCategoryExist(category);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }

        //Function to get all the categories and content of categories
        [HttpGet]
        [Route("getcategories")]
        public HttpResponseMessage GetCategories(string courseId,string userId)
        {
            var res = _contentService.GetCategories(new Guid(courseId),new Guid(userId));
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }


        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("deletecategory")]
        public HttpResponseMessage DeleteCategory(CategoryViewModel category)
        {
            var res = _contentService.DeleteCategory(category);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }
        #endregion

        #region Content Section

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("addupdatecontent")]
        public HttpResponseMessage AddUpdateContent()
        {
            var content = UploadFiles();

            var res = _contentService.AddUpdateContent(content);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }




        public ContentViewModel UploadFiles()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Form == null)
                return null;

            var contentDetail = JsonConvert.DeserializeObject<ContentViewModel>(httpRequest.Form[0]);

            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];


                string extension = Path.GetExtension(postedFile.FileName);
                string fileBase = Path.GetFileNameWithoutExtension(postedFile.FileName);

                string fileName = fileBase + "_" + contentDetail.CategoryId + "_" + contentDetail.ContentName + extension;
                var filePath = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + fileName);
                postedFile.SaveAs(filePath);
                contentDetail.FileUrl = filePath;
                contentDetail.FileName = postedFile.FileName;
                contentDetail.FileType = postedFile.ContentType;
            }
            else if (string.IsNullOrEmpty(contentDetail.FileUrl))
            {
                contentDetail = null;
            }


            return contentDetail;
        }






        [HttpGet]
        [Route("downloadfilefromserver")]
        public HttpResponseMessage DownloadFileFromServer(string fileName,string filePath)
        {
            filePath = HttpUtility.UrlDecode(filePath);
             HttpResponseMessage result = null;

            if (!File.Exists(filePath))
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
                
            }
            else
            {
                byte[] fileContents = null;

                using (var streamReader = new StreamReader(filePath))
                using (var memoryStream = new MemoryStream())
                {
                    streamReader.BaseStream.CopyTo(memoryStream);
                    fileContents = memoryStream.ToArray();
                }

                 result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileContents)
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
               
            }

            return result;
        }





        [HttpPost]
        [Route("iscontentexist")]
        public HttpResponseMessage IsContentExist(ContentViewModel content)
        {
            var res = _contentService.IsContentExist(content);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [Route("deletecontent")]
        public HttpResponseMessage DeleteContent(ContentViewModel content)
        {
            var res = _contentService.DeleteContent(content);
            return Request.CreateResponse(HttpStatusCode.Created, res);
        }

        #endregion





    }

}