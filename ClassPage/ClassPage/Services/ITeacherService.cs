using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public interface ITeacherService
    {
        void Add(TeacherDTO teacherDTO);
        void Edit(int id, TeacherDTO teacher);
        void Delete(int id);
    }
}