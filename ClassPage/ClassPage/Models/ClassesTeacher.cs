#nullable disable

namespace ClassPage.Models
{
    public partial class ClassesTeacher
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
