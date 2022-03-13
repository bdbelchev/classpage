﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;
using ClassPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Controllers
{
    [Authorize(Policy = "IsMemberOfSchool")]
    public class GradesController : Controller
    {
        private readonly SchooldbContext _context;
        private readonly IGradeService gradeService;
        private readonly ITeacherService teacherService;
        private readonly IStudentService studentService;

        public GradesController(SchooldbContext context, IGradeService gradeService, ITeacherService teacherService, IStudentService studentService)
        {
            _context = context;
            this.gradeService = gradeService;
            this.teacherService = teacherService;
            this.studentService = studentService;
        }

        public IActionResult Index()
        {
            if (User.HasClaim("Role", "Student"))
            {
                return RedirectToAction("List", new { studentId = int.Parse(User.Claims.Single(c => c.Type == "EntityID").Value) });
            }

            ViewBag.ClassList = _context.Classes.ToList();
            ViewBag.StudentList = _context.Students.ToList();
            ViewBag.TeacherList = _context.Teachers.ToList();

            return View();
        }

        public JsonResult GetStudentsDropdown(int classId)
        {
            List<SelectListItem> students = _context.Students.Where(s => s.ClassId == classId).Select(s => new SelectListItem(s.FirstName + " " + s.LastName, s.Id.ToString())).ToList();

            return Json(new { students });
        }

        public IActionResult List(int studentId)
        {
            if (User.HasClaim("Role", "Student") && studentId != int.Parse(User.Claims.Single(c => c.Type == "EntityID").Value))
            {
                return Redirect("/Identity/Account/AccessDenied");
            }

            List<GradeDTO> grades = gradeService.GetAll().Where(s => s.StudentId == studentId).OrderByDescending(g => g.DateAdded).ToList();

            ViewBag.StudentList = studentService.GetAll();
            ViewBag.TeacherList = teacherService.GetAll();
            ViewBag.SubjectList = _context.Subjects.ToList();

            return View(grades);
        }

        [Authorize(Policy = "StaffOnly")]
        public IActionResult Add(int teacherId)
        {
            if (!User.HasClaim("Role", "Admin") && teacherId != int.Parse(User.Claims.Single(c => c.Type == "EntityID").Value))
            {
                return Redirect("/Identity/Account/AccessDenied");
            }

            TeacherDTO teacher = teacherService.GetById(teacherId);

            ViewBag.TeacherList = teacherService.GetAll();
            ViewBag.StudentList = studentService.GetAll();
            ViewBag.ClassList = _context.Classes.ToList();
            ViewBag.SubjectList = _context.Subjects.ToList();

            return View(teacher);
        }

        [HttpPost]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult Add(GradeDTO gradeDto)
        {
            if (!User.HasClaim("Role", "Admin"))
            {
                gradeDto.TeacherId = int.Parse(User.Claims.Single(c => c.Type == "EntityID").Value);
            }

            gradeService.Add(gradeDto);

            return RedirectToAction("List", new { gradeDto.StudentId });
        }

        [Authorize(Policy = "StaffOnly")]
        public IActionResult Edit(int gradeId)
        {
            GradeDTO grade = gradeService.GetById(gradeId);

            if (!User.HasClaim("Role", "Admin") && grade.TeacherId != int.Parse(User.Claims.Single(c => c.Type == "EntityID").Value))
            {
                return Redirect("/Identity/Account/AccessDenied");
            }

            ViewBag.StudentList = studentService.GetAll();
            ViewBag.TeacherList = teacherService.GetAll();
            ViewBag.ClassList = _context.Classes.ToList();
            ViewBag.SubjectList = _context.Subjects.ToList();

            return View(grade);
        }

        [HttpPost]
        [Authorize(Policy = "StaffOnly")]
        public IActionResult Edit(int id, GradeDTO gradeDto)
        {
            if (!User.HasClaim("Role", "Admin"))
            {
                gradeDto.TeacherId = int.Parse(User.Claims.Single(c => c.Type == "EntityID").Value);
            }

            gradeService.Edit(id, gradeDto);

            return RedirectToAction("List", new { gradeDto.StudentId });
        }

        [Authorize(Policy = "StaffOnly")]
        public IActionResult Delete(int gradeId)
        {
            if (!User.HasClaim("Role", "Admin") && _context.Grades.FirstOrDefault(g => g.Id == gradeId).TeacherId != int.Parse(User.Claims.Single(c => c.Type == "EntityID").Value))
            {
                return Redirect("/Identity/Account/AccessDenied");
            }

            int studentId = gradeService.GetById(gradeId).StudentId;
            gradeService.Delete(gradeId);

            return RedirectToAction("List", new { studentId });
        }
    }
}
