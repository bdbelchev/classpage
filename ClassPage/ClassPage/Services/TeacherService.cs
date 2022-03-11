using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public class TeacherService
    {
        private readonly SchooldbContext _context;

        public TeacherService(SchooldbContext _context)
        {
            this._context = _context;
        }

        public void Add(TeacherDTO teacherDTO)
        {
            using (_context)
            {
                Teacher teacher = toEntity(teacherDTO);

                foreach (int subjectId in teacherDTO.SubjectIds)
                {
                    teacher.TeachersSubjects.Add(new TeachersSubject { SubjectId = subjectId, Teacher = teacher });
                }

                foreach (int classId in teacherDTO.ClassIds)
                {
                    teacher.ClassesTeachers.Add(new ClassesTeacher { ClassId = classId, Teacher = teacher });
                }
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
            }
        }

        public void Edit(int id, TeacherDTO teacher)
        {
            using (_context)
            {
                Teacher dbTeacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
                dbTeacher.FirstName = teacher.FirstName;
                dbTeacher.MiddleName = teacher.MiddleName;
                dbTeacher.LastName = teacher.LastName;
                dbTeacher.Email = teacher.Email;
                dbTeacher.Phone = teacher.Phone;

                _context.TeachersSubjects.RemoveRange(_context.TeachersSubjects.Where(t => t.TeacherId == id));
                _context.ClassesTeachers.RemoveRange(_context.ClassesTeachers.Where(t => t.TeacherId == id));

                foreach (int subjectId in teacher.SubjectIds)
                {
                    dbTeacher.TeachersSubjects.Add(new TeachersSubject { SubjectId = subjectId, Teacher = dbTeacher });
                }

                foreach (int classId in teacher.ClassIds)
                {
                    dbTeacher.ClassesTeachers.Add(new ClassesTeacher { ClassId = classId, Teacher = dbTeacher });
                }
                _context.Teachers.Update(dbTeacher);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (_context)
            {
                Teacher teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);

                _context.TeachersSubjects.RemoveRange(_context.TeachersSubjects.Where(t => t.TeacherId == id));
                _context.ClassesTeachers.RemoveRange(_context.ClassesTeachers.Where(t => t.TeacherId == id));
                _context.Grades.RemoveRange(_context.Grades.Where(t => t.TeacherId == id));

                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
        }

        private Teacher toEntity(TeacherDTO teacherDTO)
        {
            Teacher teacher = new Teacher();
            teacher.FirstName = teacherDTO.FirstName;
            teacher.MiddleName = teacherDTO.MiddleName;
            teacher.LastName = teacherDTO.LastName;
            teacher.Email = teacherDTO.Email;
            teacher.Phone = teacherDTO.Phone;

            return teacher;
        }
    }
}
