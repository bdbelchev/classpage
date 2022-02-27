using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Models;

namespace ClassPage.Controllers
{
    public class DirectoryController : Controller
    {
        private readonly schooldbContext _context;

        public DirectoryController(schooldbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
