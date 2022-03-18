#nullable disable

namespace ClassPage.Models
{
    public class TeachersSubject
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
