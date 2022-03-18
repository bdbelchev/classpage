using System.Collections.Generic;
using System.Linq;
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
            _context = context;
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

        public List<GradeDTO> GetAll()
        {
            List<Grade> dbGrades = _context.Grades.ToList();

            List<GradeDTO> gradeDTOs = dbGrades.Select(t => GetById(t.Id)).ToList();

            return gradeDTOs;
        }

        public GradeDTO GetById(int id)
        {
            Grade grade = _context.Grades.FirstOrDefault(s => s.Id == id);

            GradeDTO gradeDTO = new GradeDTO();
            gradeDTO.Id = grade.Id;
            gradeDTO.StudentId = grade.StudentId;
            gradeDTO.SubjectId = grade.SubjectId;
            gradeDTO.TeacherId = grade.TeacherId;
            gradeDTO.DateAdded = grade.DateAdded;
            gradeDTO.Value = grade.Value;
            gradeDTO.Description = grade.Description;
            return gradeDTO;
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
