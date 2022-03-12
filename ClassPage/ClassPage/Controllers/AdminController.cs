using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly SchooldbContext _context;
        private readonly ITeacherService teacherService;
        private readonly IStudentService studentService;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(SchooldbContext context, ITeacherService teacherService, IStudentService studentService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListUsers()
        {
            List<IdentityUser> users = userManager.Users.ToList();
            return View(users);
        }

        public IActionResult EditClaims(string id)
        {
            IdentityUser user = userManager.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditClaims(string id, string role)
        {
            IdentityUser user = userManager.Users.FirstOrDefault(u => u.Id == id);
            Claim newClaim = new Claim("Role", role);

            Task<IList<Claim>> getClaimsTask = userManager.GetClaimsAsync(user);
            IList<Claim> userClaims = await getClaimsTask;

            Task<IdentityResult> removeClaimsTask = userManager.RemoveClaimsAsync(user, userClaims.Where(c => c.Type == "Role"));
            await removeClaimsTask;

            Task<IdentityResult> addClaimTask = userManager.AddClaimAsync(user, newClaim);
            await addClaimTask;

            return RedirectToAction("ListUsers");
        }

        public IActionResult AddStudent()
        {
            using (_context)
            {
                ViewBag.ClassList = _context.Classes.ToList();
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(StudentDTO student)
        {
            studentService.Add(student);

            return RedirectToAction("Index");
        }

        public IActionResult AddTeacher()
        {
            using (_context)
            {
                ViewBag.ClassList = _context.Classes.ToList();
                ViewBag.SubjectList = _context.Subjects.ToList();
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddTeacher(TeacherDTO teacher)
        {
            teacherService.Add(teacher);

            return RedirectToAction("Index");
        }

        public IActionResult EditStudent(int studentId)
        {
            Student student = _context.Students.Include(c => c.Class).First(s => s.Id == studentId);

            ViewBag.ClassList = _context.Classes.ToList();

            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent(int id, StudentDTO student)
        {
            studentService.Edit(id, student);

            return RedirectToAction("Index");
        }

        public IActionResult EditTeacher(int teacherId)
        {
            Teacher teacher = _context.Teachers.Include(s => s.TeachersSubjects).ThenInclude(s => s.Subject).Include(c => c.ClassesTeachers).ThenInclude(c => c.Class).First(t => t.Id == teacherId);

            ViewBag.ClassList = _context.Classes.ToList();
            ViewBag.SubjectList = _context.Subjects.ToList();

            return View(teacher);
        }

        [HttpPost]
        public IActionResult EditTeacher(int id, TeacherDTO teacherDTO)
        {
            teacherService.Edit(id, teacherDTO);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteStudent(int studentId)
        {
            studentService.Delete(studentId);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteTeacher(int teacherId)
        {
            teacherService.Delete(teacherId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UnlinkEntity(string id)
        {
            IdentityUser user = userManager.Users.FirstOrDefault(u => u.Id == id);

            Task<IList<Claim>> getClaimsTask = userManager.GetClaimsAsync(user);
            IList<Claim> userClaims = await getClaimsTask;

            Task<IdentityResult> removeClaimsTask = userManager.RemoveClaimsAsync(user, userClaims);
            await removeClaimsTask;

            return RedirectToAction("ListUsers");
        }
    }
}
