using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;

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

            using (_context)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }
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
