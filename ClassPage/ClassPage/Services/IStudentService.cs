using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public interface IStudentService
    {
        void Add(StudentDTO studentDTO);
        void Edit(int id, StudentDTO studentDTO);
        void Delete(int id);
    }
}