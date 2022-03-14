using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ClassPage.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ITeacherService teacherService;
        private readonly IStudentService studentService;
        private readonly ISubjectService subjectService;
        private readonly IClassService classService;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(ITeacherService teacherService, IStudentService studentService, ISubjectService subjectService, IClassService classService, UserManager<IdentityUser> userManager)
        {
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.classService = classService;
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
            ViewBag.ClassList = classService.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(StudentDTO student)
        {
            studentService.Add(student);
            return RedirectToAction("ListStudents", "Directory");
        }

        public IActionResult AddTeacher()
        {
            ViewBag.ClassList = classService.GetAll();
            ViewBag.SubjectList = subjectService.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult AddTeacher(TeacherDTO teacher)
        {
            teacherService.Add(teacher);
            return RedirectToAction("ListTeachers", "Directory");
        }

        public IActionResult EditStudent(int id)
        {
            ViewBag.ClassList = classService.GetAll();

            StudentDTO studentDTO = studentService.GetById(id);

            return View(studentDTO);
        }

        [HttpPost]
        public IActionResult EditStudent(int id, StudentDTO student)
        {
            studentService.Edit(id, student);
            return RedirectToAction("StudentDetails", "Directory", new { id });
        }

        public IActionResult EditTeacher(int id)
        {
            ViewBag.ClassList = classService.GetAll();
            ViewBag.SubjectList = subjectService.GetAll();

            TeacherDTO teacherDTO = teacherService.GetById(id);

            return View(teacherDTO);
        }

        [HttpPost]
        public IActionResult EditTeacher(int id, TeacherDTO teacherDTO)
        {
            teacherService.Edit(id, teacherDTO);
            return RedirectToAction("TeacherDetails", "Directory", new { id });
        }

        public IActionResult DeleteStudent(int id)
        {
            studentService.Delete(id);
            return RedirectToAction("ListStudents", "Directory");
        }

        public IActionResult DeleteTeacher(int id)
        {
            teacherService.Delete(id);
            return RedirectToAction("ListTeachers", "Directory");
        }

        public IActionResult ManageSubjects()
        {
            List<SubjectDTO> subjects = subjectService.GetAll();
            return View(subjects);
        }

        public IActionResult AddSubject()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSubject(SubjectDTO subject)
        {
            subjectService.Add(subject);
            return RedirectToAction("ManageSubjects");
        }

        public IActionResult EditSubject(int id)
        {
            SubjectDTO subject = subjectService.GetById(id);
            return View(subject);
        }

        [HttpPost]
        public IActionResult EditSubject(int id, SubjectDTO subject)
        {
            subjectService.Edit(id, subject);
            return RedirectToAction("ManageSubjects");
        }

        public IActionResult DeleteSubject(int id)
        {
            subjectService.Delete(id);
            return RedirectToAction("ManageSubjects");
        }

        public IActionResult ManageClasses()
        {
            List<ClassDTO> classes = classService.GetAll();
            return View(classes);
        }

        public IActionResult AddClass()
        {
            ViewBag.TeacherList = teacherService.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult AddClass(ClassDTO classDTO)
        {
            classService.Add(classDTO);
            return RedirectToAction("ManageClasses");
        }

        public IActionResult EditClass(int id)
        {
            ClassDTO classDTO = classService.GetById(id);

            ViewBag.TeacherList = teacherService.GetAll();

            return View(classDTO);
        }

        [HttpPost]
        public IActionResult EditClass(int id, ClassDTO classDTO)
        {
            classService.Edit(id, classDTO);
            return RedirectToAction("ManageClasses");
        }

        public IActionResult DeleteClass(int id)
        {
            classService.Delete(id);
            return RedirectToAction("ManageClasses");
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
