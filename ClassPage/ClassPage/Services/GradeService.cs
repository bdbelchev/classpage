using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public class GradeService : IGradeService
    {
        private readonly SchooldbContext _context;

        public GradeService(SchooldbContext context)
        {
            this._context = context;
        }

        public void Add(GradeDTO gradeDto)
        {
            Grade grade = toEntity(gradeDto);

            _context.Grades.Add(grade);
            _context.SaveChanges();
        }

        public void Edit(int id, GradeDTO gradeDto)
        {
            Grade grade = _context.Grades.FirstOrDefault(g => g.Id == id);
            grade.TeacherId = gradeDto.TeacherId;
            grade.SubjectId = gradeDto.SubjectId;
            grade.StudentId = gradeDto.StudentId;
            grade.Value = gradeDto.Value;
            grade.DateAdded = gradeDto.DateAdded;
            grade.Description = gradeDto.Description;

            _context.Grades.Update(grade);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Grade grade = _context.Grades.FirstOrDefault(g => g.Id == id);
            _context.Grades.Remove(grade);
            _context.SaveChanges();
        }

        private Grade toEntity(GradeDTO gradeDto)
        {
            Grade grade = new Grade();
            grade.TeacherId = gradeDto.TeacherId;
            grade.SubjectId = gradeDto.SubjectId;
            grade.StudentId = gradeDto.StudentId;
            grade.Value = gradeDto.Value;
            grade.DateAdded = gradeDto.DateAdded;
            grade.Description = gradeDto.Description;

            return grade;
        }
    }
}
