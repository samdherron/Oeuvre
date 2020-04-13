using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oeuvre.Helpers;
using Oeuvre.Models;

namespace Oeuvre.Controllers
{
    public class GalleryRequestController : Controller
    {
        public string canadaRegex = @"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy]{1}\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstv‌​xy]{1} *\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvxy]{1}\d{1}$)";
        public string phoneNumberRegex = @"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$";
        public string emailRegex = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

        private SearchGallery userSearch;
        public IActionResult Index()
        {
            return View("GalleryRequest");
        }

       

        public ActionResult requestGalleryMail(GalleryRequestModel userRequest)
        {
            userSearch = new SearchGallery();

            bool dataValid = true;

            string email = userSearch.removeSqlInjectionParams(userRequest.galleryEmail);
            string name = userSearch.removeSqlInjectionParams(userRequest.galleryName);
            string type = userSearch.removeSqlInjectionParams(userRequest.galleryType);
            string phoneNumber = userSearch.removeSqlInjectionParams(userRequest.phoneNumber);
            string address = userSearch.removeSqlInjectionParams(userRequest.address);
            string province = userSearch.removeSqlInjectionParams(userRequest.province);
            string city = userSearch.removeSqlInjectionParams(userRequest.city);
            string postalCode = userSearch.removeSqlInjectionParams(userRequest.postalCode);
            string galleryDesc = userSearch.removeSqlInjectionParams(userRequest.galleryDesc);

            //galleryDesc = galleryDesc.Replace("/r/n", " ");
            galleryDesc = Regex.Replace(galleryDesc, @"[^0-9a-zA-Z]+", " ");


            if (checkRegex(email, emailRegex) == false)
            {
                ViewData["errorEmail"] = "Please fill in a valid email address";
                dataValid = false;
            }

            if (name == "" || name == null)
            {
                ViewData["errorName"] = "Please fill in a Name";
                dataValid = false;
            }

            if (type == "" || type == null || (type.ToLower() != "public" && type.ToLower() != "private"))
            {
                ViewData["errorType"] = "Please fill in a type of Gallery (Either Private or Public)";
                dataValid = false;
            }

            if (address == "" || address == null)
            {
                ViewData["errorAddress"] = "Please fill in an address";
                dataValid = false;

            }

            if (province == "" || province == null)
            {
                ViewData["errorProvince"] = "Please fill in a Province";
                dataValid = false;
            }

            if (city == "" || city == null)
            {
                ViewData["errorCity"] = "Please fill in a City";
                dataValid = false;
            }

            //if (galleryDesc == "" || galleryDesc == null)
            //{
            //    ViewData["errorPostal"] = "Please fill in a valid Postal Code";
            //    dataValid = false;
            //}


            //check for postal code
            if (checkRegex(postalCode, canadaRegex) == false)
            {
                ViewData["errorPostal"] = "Please fill in a valid Postal Code";
                dataValid = false;
            }



            if (checkRegex(phoneNumber, phoneNumberRegex) == false)
            {
                ViewData["errorPhone"] = "Please fill in a valid Phone Number";
                dataValid = false;
            }
               

            if(dataValid == true)
            {
                sendGalleryRequest(email, name, type, phoneNumber, address, province, city, postalCode, galleryDesc);

                ViewData["Success"] = "Email SuccessFully Sent!";
                return View("../Home/Index");
            }
            else
            {
                return View("GalleryRequest");
            }

            
        }

        public bool sendGalleryRequest(string email, string name, string type, string phoneNumber, string address, string province, string city,string postal, string galleryDescription )
        {

            try
            {
                //these will be used to access the SMTP
                var mailCredentails = new NetworkCredential("oeuvreinf@gmail.com", "Oa9!@e6_$u");

                string update = "";
                string latestExhibition = "";

                DateTime todaysDate = DateTime.Now;

                var newMail = new MailMessage()
                {
                    From = new MailAddress(email),
                    Subject = "Oeuvre Gallery Request",
                    Body = "<h2>Gallery Request<h2> <h3> Gallery Information </h3> <p>Email: " + email + " </p> <p>Gallery Name: " + name + "</p> <p> Gallery Type: " + type +
                    "</p><p> Gallery Phone Number: " + phoneNumber + "</p>  <p>Gallery Description: " + galleryDescription + "</p> <p>Date and Time Submitted: " + todaysDate + "</p> <h3>Gallery Address Information</h3> <p>Address: " + address + "</p> <p>Province: " + province +
                    "</p> City: " + city + "</p> <p> Postal Code: " + postal.ToUpper() + "</p>"

                };

                newMail.IsBodyHtml = true;
                newMail.To.Add(new MailAddress("OeuvreInf@gmail.com"));

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



                newMail = new MailMessage()
                {
                    From = new MailAddress("OeuvreInf@gmail.com"),
                    Subject = "Oeuvre Newsletter",
                    Body = "<h5>Good day, thank you for reaching out. We will look at your form and get back to you. Thank you!</h5>"
                };

                newMail.IsBodyHtml = true;
                newMail.To.Add(new MailAddress(email));

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
                return true;

            }
            catch (System.Exception e)
            {
                return false;
            }

        }

        public bool checkRegex(string value, string regex)
        {
            //var _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
            //var canadaRegex = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";
            Regex reg = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            bool doesMatch = reg.Match(value.Replace(" ", "")).Success;

            if(Regex.IsMatch(value, regex))
            {
                return true;
            }
            else
            {
                return false;
            }


            //if (Regex.Match(value.Replace(" ", ""), regex).Success)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

    }
}