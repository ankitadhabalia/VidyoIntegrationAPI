using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;
using Entity;
using System.Web.Http.Cors;

namespace WebApi1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmailController : ApiController
    {
        [System.Web.Http.HttpPost]
        public IHttpActionResult sendEmailViaWebApi(Email data)
        {
            var Body = data.Body;
            var Subject = data.Subject;
            var toEmail = data.toEmail;

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            // setup Smtp authentication
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("ankitadhabalia@gmail.com", "avtaar_anki@3");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            //can be obtained from your model
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("ankitadhabalia@gmail.com");
            msg.To.Add(new MailAddress(toEmail));

            msg.Subject = Subject;
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<html><head></head><body>" + Body + "</body>");
            try
            {
                client.Send(msg);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
    }
}
