using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.IO;
using Entity;

namespace WebApi1.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    [RoutePrefix("api/Upload")]
    public class UploadController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {

            EmployeeContext db = new EmployeeContext();
            var data = from i in db.ImageTable
                       where i.id == id
                       select i;
            ProductImage img = (ProductImage)data.SingleOrDefault();
            byte[] imgData = img.Image;
            MemoryStream ms = new MemoryStream(imgData);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }


        //[FromUri]
        //UploadFile uploadfile

        [Route("user/PostUserImage")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> PostUserImage()
        {
            var filePath = "";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpeg", ".gif", ".png","jpg" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpeg,.gif,.png,.jpg");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                             filePath = HttpContext.Current.Server.MapPath("~/images/" + postedFile.FileName);

                            postedFile.SaveAs(filePath);
                            EmployeeContext db = new EmployeeContext();
                            ProductImage img = new ProductImage();
                            img.Image = File.ReadAllBytes(filePath);
                            db.ImageTable.Add(img);
                            db.SaveChanges();
                        }
                    }

                    //var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, filePath); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);

                // return "File uploaded successfully!";
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);

            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        public HttpResponseMessage UploadImage()
        {
            var exMessage = string.Empty;
            try
            {
                string uploadPath = "~/content/upload";
                HttpPostedFile file = null;
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    file = HttpContext.Current.Request.Files.Get("file");
                }
                // Check if we have a file
                if (null == file)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        error = true,
                        message = "Image file not found"
                    });

                // Make sure the file has content
                if (!(file.ContentLength > 0))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        error = true,
                        message = "Image file not found"
                    });

                if (!Directory.Exists(HttpContext.Current.Server.MapPath(uploadPath)))
                {
                    // If it doesn't exist, create the directory
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(uploadPath));
                }

                //Upload File
                file.SaveAs(HttpContext.Current.Server.MapPath($"{uploadPath}/{file.FileName}"));

            }
            catch (Exception ex)
            {
                exMessage = ex.Message;
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = true, message = exMessage == string.Empty ? "An unknown error occured" : exMessage });
        }
    }
}
