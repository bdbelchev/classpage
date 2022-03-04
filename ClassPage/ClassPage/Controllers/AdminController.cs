using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Models;

namespace ClassPage.Controllers
{
    public class AdminController : Controller
    {
        private readonly schooldbContext _context;

        public AdminController(schooldbContext context)
        {
            _context = context;
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
        public IActionResult AddTeacher(string firstName, string middleName, string lastName, string email,
            string phone, List<int> subjectIds, List<int> classIds)
        {
            Teacher teacher = new Teacher();
            teacher.FirstName = firstName;
            teacher.MiddleName = middleName;
            teacher.LastName = lastName;
            teacher.Email = email;
            teacher.Phone = phone;

            foreach (int subjectId in subjectIds)
            {
                teacher.TeachersSubjects.Add(new TeachersSubject { SubjectId = subjectId, Teacher = teacher });
            }

            foreach (int classId in classIds)
            {
                teacher.ClassesTeachers.Add(new ClassesTeacher { ClassId = classId, Teacher = teacher });
            }

            using (_context)
            {
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
