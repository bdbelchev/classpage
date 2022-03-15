using System.Collections.Generic;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public interface ITeacherService
    {
        void Add(TeacherDTO teacherDTO);
        void Edit(int id, TeacherDTO teacherDTO);
        void Delete(int id);
        TeacherDTO GetById(int id);
        List<TeacherDTO> GetAll();
    }
}