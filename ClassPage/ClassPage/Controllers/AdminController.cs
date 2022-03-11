﻿using Microsoft.AspNetCore.Mvc;
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
    public class AdminController : Controller
    {
        private readonly SchooldbContext _context;
        private readonly ITeacherService teacherService;
        private readonly IStudentService studentService;

        public AdminController(SchooldbContext context, ITeacherService teacherService, IStudentService studentService)
        {
            _context = context;
            this.teacherService = teacherService;
            this.studentService = studentService;
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
        public IActionResult AddStudent(StudentDTO student)
        {
            studentService.Add(student);

            return RedirectToAction("ListStudents", "Directory");
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

            return RedirectToAction("ListTeachers", "Directory");
        }

        public IActionResult EditStudent(int id)
        {
            Student student = _context.Students.Include(c => c.Class).First(s => s.Id == id);

            ViewBag.ClassList = _context.Classes.ToList();

            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent(int id, StudentDTO student)
        {
            studentService.Edit(id, student);

            return RedirectToAction("StudentDetails", "Directory", new { studentId = id });
        }

        public IActionResult EditTeacher(int id)
        {
            Teacher teacher = _context.Teachers.Include(s => s.TeachersSubjects).ThenInclude(s => s.Subject).Include(c => c.ClassesTeachers).ThenInclude(c => c.Class).First(t => t.Id == id);

            ViewBag.ClassList = _context.Classes.ToList();
            ViewBag.SubjectList = _context.Subjects.ToList();

            return View(teacher);
        }

        [HttpPost]
        public IActionResult EditTeacher(int id, TeacherDTO teacherDTO)
        {
            teacherService.Edit(id, teacherDTO);

            return RedirectToAction("TeacherDetails", "Directory", new { teacherId = id });
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

        //TODO: Make and redirect to a "success" page instead.
    }
}
