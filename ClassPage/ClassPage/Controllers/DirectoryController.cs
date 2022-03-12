using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Controllers
{
    [Authorize(Policy = "IsMemberOfSchool")]
    public class DirectoryController : Controller
    {
        private readonly SchooldbContext _context;

        public DirectoryController(SchooldbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.ClassList = _context.Classes.ToList();
            return View();
        }

        public IActionResult ListTeachers()
        {
            List<Teacher> teachers = _context.Teachers.Include(c => c.Class).Include(s => s.TeachersSubjects).ThenInclude(s => s.Subject).ToList();

            return View(teachers);
        }

        public IActionResult ListStudents()
        {
            List<Student> students = _context.Students.Include(c => c.Class).ToList();

            return View(students);
        }

        public IActionResult TeacherDetails(int teacherId)
        {
            Teacher teacher = _context.Teachers.Include(c => c.Class).Include(c => c.ClassesTeachers).ThenInclude(c => c.Class).Include(s => s.TeachersSubjects).ThenInclude(s => s.Subject).FirstOrDefault(t => t.Id == teacherId);

            return View(teacher);
        }

        public IActionResult StudentDetails(int studentId)
        {
            Student student = _context.Students.Include(c => c.Class).FirstOrDefault(s => s.Id == studentId);

            return View(student);
        }

        public IActionResult ClassDetails(int classId)
        {
            Class schoolClass = _context.Classes.Include(c => c.MainTeacher).Include(s => s.Students).Include(t => t.ClassesTeachers).ThenInclude(t => t.Teacher).ThenInclude(s => s.TeachersSubjects).ThenInclude(s => s.Subject).FirstOrDefault(c => c.Id == classId);

            return View(schoolClass);
        }
    }
}
