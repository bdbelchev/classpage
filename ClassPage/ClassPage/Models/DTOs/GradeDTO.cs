using System;

namespace ClassPage.Models.DTOs
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
    }
}
