using System.Collections.Generic;
using ClassPage.Models.DTOs;

namespace ClassPage.Services
{
    public interface IGradeService
    {
        void Add(GradeDTO gradeDTO);
        void Edit(int id, GradeDTO gradeDTO);
        void Delete(int id);
        List<GradeDTO> GetAll();
        GradeDTO GetById(int id);
    }
}