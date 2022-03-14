using System.Collections.Generic;
using System.Linq;
using ClassPage.Data;
using ClassPage.Models;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly SchooldbContext _context;

        public SubjectService(SchooldbContext context)
        {
            _context = context;
        }

        public void Add(SubjectDTO subjectDTO)
        {
            Subject subject = toEntity(subjectDTO);

            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }

        public void Edit(int id, SubjectDTO subjectDTO)
        {
            Subject subject = _context.Subjects.FirstOrDefault(s => s.Id == id);
            subject.Id = subjectDTO.Id;
            subject.SubjectName = subjectDTO.SubjectName;

            _context.Subjects.Update(subject);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Subject subject = _context.Subjects.FirstOrDefault(s => s.Id == id);

            _context.TeachersSubjects.RemoveRange(_context.TeachersSubjects.Where(ts => ts.SubjectId == id));

            _context.Subjects.Remove(subject);
            _context.SaveChanges();
        }

        public SubjectDTO GetById(int id)
        {
            Subject subject = _context.Subjects.FirstOrDefault(s => s.Id == id);

            SubjectDTO subjectDTO = new SubjectDTO();
            subjectDTO.Id = subject.Id;
            subjectDTO.SubjectName = subject.SubjectName;

            return subjectDTO;
        }

        public List<SubjectDTO> GetAll()
        {
            List<Subject> dbSubjects = _context.Subjects.ToList();

            List<SubjectDTO> subjectDTOs = dbSubjects.Select(t => GetById(t.Id)).ToList();

            return subjectDTOs;
        }

        private Subject toEntity(SubjectDTO subjectDTO)
        {
            Subject subject = new Subject();

            subject.Id = subjectDTO.Id;
            subject.SubjectName = subjectDTO.SubjectName;

            return subject;
        }
    }
}
