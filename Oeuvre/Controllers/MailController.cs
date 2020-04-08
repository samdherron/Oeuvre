using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oeuvre.Helpers;

namespace Oeuvre.Controllers
{
    public class MailController : Controller
    {
        private SearchGallery userSearch;

        public IActionResult Index()
        {
            return View();
        }


        public ActionResult SendEmail(string Email, string firstname, string lastname, bool subscribeCheckbox = false)
        {

            userSearch = new SearchGallery();
            try
            {
                Email = userSearch.removeSqlInjectionParams(Email);
                firstname = userSearch.removeSqlInjectionParams(firstname);
                lastname = userSearch.removeSqlInjectionParams(lastname);


                //these will be used to access the SMTP
                var mailCredentails = new NetworkCredential("oeuvreinf@gmail.com", "Oa9!@e6_$u");

                string update = "";
                string latestExhibition = "";

                if(subscribeCheckbox == true)
                {
                    update = "This is a reminder that you have asked to be subscribed to our monthly newsletter. In additon you have also asked to be kept up with the most up to date exhibitions and we would like to thank you!";
                    latestExhibition = "True";
                }
                else
                {
                    update = "This is a reminder that you have asked to be subscribed to our monthly newsletter.";
                    latestExhibition = "False";
                }


                var newMail = new MailMessage()
                {
                    From = new MailAddress("OeuvreInf@gmail.com"),
                    Subject = "Oeuvre Newsletter"  ,
                    Body = "<h5>Good day " + firstname + " " + lastname + "!</h5>  <p>Thank you for reaching out to the team at Oeuvre.</p> <p>" +
                    update + "</p> <p>We thank you for your interest and look forward to hearing from you again!</p>"
                };

                newMail.IsBodyHtml = true;
                newMail.To.Add(new MailAddress(Email));

                var smtpClient = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = mailCredentails
                };

                smtpClient.Send(newMail);


                DateTime todaysDate = DateTime.Now;

                newMail = new MailMessage()
                {
                    From = new MailAddress("OeuvreInf@gmail.com"),
                    Subject = "Oeuvre Newsletter Receipt",
                    Body = "<h2>Receipt for " + todaysDate + ".</h2><h3>User Information</h3> <p> Email: " + Email + "</p> <p> First Name: " + firstname + "</p> <p> Last Name: " + lastname +
                   "</p> <p> Wants to updated for most current exhibitions: " + latestExhibition + "</p>"
                };

                newMail.IsBodyHtml = true;
                newMail.To.Add(new MailAddress("OeuvreInf@gmail.com"));

                smtpClient = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = mailCredentails
                };

                smtpClient.Send(newMail);

                ViewData["Success"] = "Email SuccessFully Sent!";
                return View("../Home/Index");

            }
            catch (System.Exception e)
            {
                Trace.WriteLine(e.ToString());
                return View("../Home/Index");
            }
            
            
        }
    }
}