using System.Diagnostics;

namespace AN010.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal Height { get; set; }
        public float Weight { get; set; }
    }
}
