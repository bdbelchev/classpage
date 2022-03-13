using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public interface ISubjectService
    {
        void Add(SubjectDTO subjectDTO);
        void Edit(int id, SubjectDTO subjectDTO);
        void Delete(int id);
        SubjectDTO GetById(int id);
        List<SubjectDTO> GetAll();
    }
}
