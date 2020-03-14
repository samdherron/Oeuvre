using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oeuvre.Models;

namespace Oeuvre.Controllers
{
    public class VisitUsController : Controller
    {
        private dbo_OeuvreContext _context;
        public VisitUsController(dbo_OeuvreContext context)
        {
            _context = context;
        }

        public IActionResult VisitUs()
        {
            return View();
        }



    }
}