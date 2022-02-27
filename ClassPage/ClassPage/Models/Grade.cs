using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ClassPage.Models
{
    public partial class Grade
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }

        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
