using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassPage.Models.DTOs
{
    public class ClassDTO
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int MainTeacherId { get; set; }
    }
}
