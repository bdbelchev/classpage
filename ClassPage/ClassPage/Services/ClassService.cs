using System.Collections.Generic;
using System.Linq;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public class ClassService : IClassService
    {
        private readonly SchooldbContext _context;

        public ClassService(SchooldbContext context)
        {
            _context = context;
        }

        public void Add(ClassDTO classDTO)
        {
            Class dbClass = toEntity(classDTO);

            _context.Classes.Add(dbClass);
            _context.SaveChanges();
        }

        public void Edit(int id, ClassDTO classDTO)
        {
            Class dbClass = _context.Classes.FirstOrDefault(s => s.Id == id);
            dbClass.Id = classDTO.Id;
            dbClass.ClassName = classDTO.ClassName;
            dbClass.MainTeacherId = classDTO.MainTeacherId;

            _context.Classes.Update(dbClass);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Class dbClass = _context.Classes.FirstOrDefault(s => s.Id == id);

            _context.ClassesTeachers.RemoveRange(_context.ClassesTeachers.Where(t => t.ClassId == id));

            List<Student> delStudents = _context.Students.Where(t => t.ClassId == id).ToList();
            _context.Grades.RemoveRange(_context.Grades.Where(t => delStudents.Select(s => s.Id).Contains(t.StudentId)));
            _context.Students.RemoveRange(delStudents);

            _context.Classes.Remove(dbClass);
            _context.SaveChanges();
        }

        public ClassDTO GetById(int id)
        {
            Class dbClass = _context.Classes.FirstOrDefault(s => s.Id == id);

            ClassDTO classDTO = new ClassDTO();
            classDTO.Id = dbClass.Id;
            classDTO.ClassName = dbClass.ClassName;
            classDTO.MainTeacherId = dbClass.MainTeacherId;

            return classDTO;
        }

        public List<ClassDTO> GetAll()
        {
            List<Class> dbClasses = _context.Classes.ToList();

            List<ClassDTO> classDTOs = dbClasses.Select(t => GetById(t.Id)).ToList();

            return classDTOs;
        }

        private Class toEntity(ClassDTO classDTO)
        {
            Class dbClass = new Class();

            dbClass.Id = classDTO.Id;
            dbClass.ClassName = classDTO.ClassName;
            dbClass.MainTeacherId = classDTO.MainTeacherId;

            return dbClass;
        }
    }
}
