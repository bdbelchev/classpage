using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Controllers
{
    public class DirectoryController : Controller
    {
        private readonly SchooldbContext _context;
        private readonly ITeacherService teacherService;
        private readonly IStudentService studentService;

        public DirectoryController(SchooldbContext context, ITeacherService teacherService, IStudentService studentService)
        {
            _context = context;
            this.teacherService = teacherService;
            this.studentService = studentService;
        }

        public IActionResult Index()
        {
            ViewBag.ClassList = _context.Classes.ToList();
            return View();
        }

        public IActionResult ListTeachers()
        {
            List<TeacherDTO> teachers = teacherService.GetAll();
            ViewBag.SubjectList = _context.Subjects.ToList();

            return View(teachers);
        }

        public IActionResult ListStudents()
        {
            List<StudentDTO> students = studentService.GetAll();
            ViewBag.ClassList = _context.Classes.ToList();

            return View(students);
        }

        public IActionResult TeacherDetails(int id)
        {
            TeacherDTO teacher = teacherService.GetById(id);

            ViewBag.ClassList = _context.Classes.ToList();
            ViewBag.SubjectList = _context.Subjects.ToList();

            return View(teacher);
        }

        public IActionResult StudentDetails(int id)
        {
            StudentDTO student = studentService.GetById(id);
            ViewBag.ClassList = _context.Classes.ToList();

            return View(student);
        }

        public IActionResult ClassDetails(int id)
        {
            Class schoolClass = _context.Classes.Include(c => c.MainTeacher).Include(s => s.Students).Include(t => t.ClassesTeachers).ThenInclude(t => t.Teacher).ThenInclude(s => s.TeachersSubjects).ThenInclude(s => s.Subject).FirstOrDefault(c => c.Id == id);

            return View(schoolClass);
        }
    }
}
