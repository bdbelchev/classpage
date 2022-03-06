using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Models;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult EditTeacher(int id, string firstName, string middleName, string lastName, string email,
            string phone, List<int> subjectIds, List<int> classIds)
        {
            Teacher teacher = _context.Teachers.First(t => t.Id == id);

            _context.TeachersSubjects.RemoveRange(_context.TeachersSubjects.Where(t => t.TeacherId == id));
            _context.ClassesTeachers.RemoveRange(_context.ClassesTeachers.Where(t => t.TeacherId == id));

            _context.SaveChanges();

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

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
