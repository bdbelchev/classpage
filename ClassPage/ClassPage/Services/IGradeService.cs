using System.Collections.Generic;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public interface IGradeService
    {
        void Add(GradeDTO gradeDto);
        void Edit(int id, GradeDTO gradeDto);
        void Delete(int id);
        List<GradeDTO> GetAll();
        GradeDTO GetById(int id);
    }
}