using ClassPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClassPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClassPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ITeacherService teacherService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(IStudentService studentService, ITeacherService teacherService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.studentService = studentService;
            this.teacherService = teacherService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Welcome");
            }

            return View();
        }

        public IActionResult Welcome()
        {
            ViewBag.StudentList = studentService.GetAll();
            ViewBag.TeacherList = teacherService.GetAll();

            return View();
        }

        public IActionResult Verify()
        {
            //TODO: Use DTOs from master

            if (User.Claims.Any(c => c.Type == "EntityID"))
            {
                return RedirectToAction("Index");
            }

            ViewBag.StudentList = studentService.GetAll();
            ViewBag.TeacherList = teacherService.GetAll();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Verify(string role, int entityId)
        {
            Task<IdentityUser> userTask = userManager.GetUserAsync(User);
            IdentityUser user = await userTask;

            Claim roleClaim = new Claim("Role", role);
            Claim entityIdClaim = new Claim("EntityID", entityId.ToString());

            Task<IdentityResult> addClaimsTask = userManager.AddClaimsAsync(user, new[] { roleClaim, entityIdClaim });
            await addClaimsTask;

            Task signoutTask = signInManager.SignOutAsync();
            await signoutTask;

            return RedirectToAction("Welcome");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
