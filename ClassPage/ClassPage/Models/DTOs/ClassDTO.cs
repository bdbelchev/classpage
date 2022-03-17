using System.ComponentModel.DataAnnotations;

namespace ClassPage.Models.DTOs
{
    public class ClassDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(3)]
        public string ClassName { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int MainTeacherId { get; set; }
    }
}
