using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Oeuvre.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Oeuvre.Services;
using Oeuvre.Helpers;

namespace Oeuvre.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private dbo_OeuvreContext _context;
        Account _account;
        Cloudinary _cloudinary;
        PageOn page;

        public HomeController(ILogger<HomeController> logger, dbo_OeuvreContext context)
        {
            _account = new Account(
                "oeuvre",
                "591857667739764",
                "tVwlzrfSYIFs8gxwIKnMW_OfRd0");

            _cloudinary = new Cloudinary(_account);
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {

            return View();
        }
   
    }
}
