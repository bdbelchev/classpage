﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Controllers
{
    public class GradesController : Controller
    {
        private readonly SchooldbContext _context;

        public GradesController(SchooldbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            using (_context)
            {
                ViewBag.ClassList = _context.Classes.ToList();
                ViewBag.StudentList = _context.Students.ToList();
                ViewBag.TeacherList = _context.Teachers.ToList();
            }

            return View();
        }

        public JsonResult GetStudentsDropdown(int classId)
        {
            List<SelectListItem> students = _context.Students.Where(s => s.ClassId == classId).Select(s => new SelectListItem(s.FirstName + " " + s.LastName, s.Id.ToString())).ToList();

            return Json(new { students });
        }

        public IActionResult List(int studentId)
        {
            List<Grade> grades = _context.Grades.Include(g => g.Student).Include(g => g.Subject).Include(g => g.Teacher).Where(s => s.Student.Id == studentId).OrderByDescending(g => g.DateAdded).ToList();

            return View(grades);
        }

        public IActionResult Add(int teacherId)
        {
            Teacher teacher = _context.Teachers.Include(s => s.TeachersSubjects).ThenInclude(s => s.Subject)
                .Include(s => s.ClassesTeachers).ThenInclude(s => s.Class).First(s => s.Id == teacherId);
            return View(teacher);
        }

        [HttpPost]
        public IActionResult Add(int studentId, int subjectId, int teacherId, double gradeValue, DateTime date,
            string description)
        {
            Grade grade = new Grade();
            grade.TeacherId = teacherId;
            grade.StudentId = studentId;
            grade.SubjectId = subjectId;
            grade.Value = gradeValue;
            grade.DateAdded = date;
            grade.Description = description;

            _context.Grades.Add(grade);
            _context.SaveChanges();

            return RedirectToAction("List", new { studentId });
        }

        public IActionResult Edit(int gradeId)
        {
            Grade grade = _context.Grades.Include(s=>s.Student).Include(t => t.Teacher).ThenInclude(s => s.TeachersSubjects).ThenInclude(s => s.Subject)
                .Include(s => s.Teacher.ClassesTeachers).ThenInclude(s => s.Class).First(s => s.Id == gradeId);
            return View(grade);
        }

        [HttpPost]
        public IActionResult Edit(int id, int studentId, int subjectId, int teacherId, double gradeValue, DateTime date,
            string description)
        {
            Grade grade = _context.Grades.First(g => g.Id == id);
            grade.TeacherId = teacherId;
            grade.StudentId = studentId;
            grade.SubjectId = subjectId;
            grade.Value = gradeValue;
            grade.DateAdded = date;
            grade.Description = description;

            _context.SaveChanges();

            return RedirectToAction("List", new { studentId });
        }

        public IActionResult Delete(int gradeId)
        {
            Grade grade = _context.Grades.First(g => g.Id == gradeId);
            _context.Grades.Remove(grade);
            _context.SaveChanges();

            return RedirectToAction("List", new { grade.StudentId });
        }
    }
}
