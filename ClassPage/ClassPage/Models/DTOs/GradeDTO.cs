using System;
using System.ComponentModel.DataAnnotations;

namespace ClassPage.Models.DTOs
{
    public class GradeDTO
    {
        public int Id { get; set; }
        [Required]
        [Range(2, 6)]
        public double Value { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
    }
}
