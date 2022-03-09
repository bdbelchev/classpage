using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Models;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Controllers
{
    public class AdminController : Controller
    {
        private readonly SchooldbContext _context;
        private readonly TeacherService teacherService;

        public AdminController(SchooldbContext context, TeacherService teacherService)
        {
            _context = context;
            this.teacherService = teacherService;
        }

        public IActionResult Index()
        {
            return View();
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
        public IActionResult AddStudent(string firstName, string middleName, string lastName, int classId, string email, string phone)
        {
            Student student = new Student();
            student.FirstName = firstName;
            student.MiddleName = middleName;
            student.LastName = lastName;
            student.ClassId = classId;
            student.Email = email;
            student.Phone = phone;

            using (_context)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }

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
        public IActionResult EditStudent(int id, string firstName, string middleName, string lastName, int classId,
            string email, string phone)
        {
            Student student = _context.Students.First(s => s.Id == id);
            student.FirstName = firstName;
            student.MiddleName = middleName;
            student.LastName = lastName;
            student.ClassId = classId;
            student.Email = email;
            student.Phone = phone;

            _context.SaveChanges();

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
            Student student = _context.Students.First(s => s.Id == studentId);
            _context.Grades.RemoveRange(_context.Grades.Where(t => t.StudentId == studentId));

            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteTeacher(int teacherId)
        {
            teacherService.Delete(teacherId);

            return RedirectToAction("Index");
        }
    }
}
