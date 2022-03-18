using System.Collections.Generic;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassPage.Controllers
{
    [Authorize(Policy = "IsMemberOfSchool")]
    public class DirectoryController : Controller
    {
        private readonly ITeacherService teacherService;
        private readonly IStudentService studentService;
        private readonly ISubjectService subjectService;
        private readonly IClassService classService;

        public DirectoryController(ITeacherService teacherService, IStudentService studentService, ISubjectService subjectService, IClassService classService)
        {
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.classService = classService;
        }

        public IActionResult Index()
        {
            ViewBag.ClassList = classService.GetAll();
            return View();
        }

        public IActionResult ListTeachers()
        {
            List<TeacherDTO> teachers = teacherService.GetAll();

            ViewBag.SubjectList = subjectService.GetAll();

            return View(teachers);
        }

        public IActionResult ListStudents()
        {
            List<StudentDTO> students = studentService.GetAll();

            ViewBag.ClassList = classService.GetAll();

            return View(students);
        }

        public IActionResult TeacherDetails(int id)
        {
            TeacherDTO teacher = teacherService.GetById(id);

            ViewBag.ClassList = classService.GetAll();
            ViewBag.SubjectList = subjectService.GetAll();

            return View(teacher);
        }

        public IActionResult StudentDetails(int id)
        {
            StudentDTO student = studentService.GetById(id);

            ViewBag.ClassList = classService.GetAll();

            return View(student);
        }

        public IActionResult ClassDetails(int id)
        {
            ClassDTO schoolClass = classService.GetById(id);

            ViewBag.TeacherList = teacherService.GetAll();
            ViewBag.StudentList = studentService.GetAll();
            ViewBag.SubjectList = subjectService.GetAll();

            return View(schoolClass);
        }
    }
}
