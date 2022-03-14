using System.Collections.Generic;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public interface IClassService
    {
        void Add(ClassDTO classDTO);
        void Edit(int id, ClassDTO classDTO);
        void Delete(int id);
        ClassDTO GetById(int id);
        List<ClassDTO> GetAll();
    }
}
