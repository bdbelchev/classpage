using System.Collections.Generic;
using System.Linq;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClassPage.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchooldbContext _context;

        public StudentService(SchooldbContext _context)
        {
            this._context = _context;
        }

        public void Add(StudentDTO studentDTO)
        {
            Student student = toEntity(studentDTO);

            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void Edit(int id, StudentDTO studentDTO)
        {
            Student student = _context.Students.First(s => s.Id == id);
            student.FirstName = studentDTO.FirstName;
            student.MiddleName = studentDTO.MiddleName;
            student.LastName = studentDTO.LastName;
            student.ClassId = studentDTO.ClassId;
            student.Email = studentDTO.Email;
            student.Phone = studentDTO.Phone;

            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Student student = _context.Students.FirstOrDefault(s => s.Id == id);
            _context.Grades.RemoveRange(_context.Grades.Where(t => t.StudentId == id));

            _context.Students.Remove(student);
            _context.SaveChanges();
        }

        public StudentDTO GetById(int id)
        {
            Student student = _context.Students.FirstOrDefault(s => s.Id == id);

            StudentDTO studentDTO = new StudentDTO();
            studentDTO.Id = student.Id;
            studentDTO.FirstName = student.FirstName;
            studentDTO.MiddleName = student.MiddleName;
            studentDTO.LastName = student.LastName;
            studentDTO.ClassId = student.ClassId;
            studentDTO.Email = student.Email;
            studentDTO.Phone = student.Phone;

            return studentDTO;
        }

        public List<StudentDTO> GetAll()
        {
            List<Student> dbStudents = _context.Students.Include(c => c.Class).ToList();

            List<StudentDTO> studentDTOs = dbStudents.Select(t => GetById(t.Id)).ToList();

            return studentDTOs;
        }

        private Student toEntity(StudentDTO studentDTO)
        {
            Student student = new Student();
            student.FirstName = studentDTO.FirstName;
            student.MiddleName = studentDTO.MiddleName;
            student.LastName = studentDTO.LastName;
            student.ClassId = studentDTO.ClassId;
            student.Email = studentDTO.Email;
            student.Phone = studentDTO.Phone;

            return student;
        }
    }
}
