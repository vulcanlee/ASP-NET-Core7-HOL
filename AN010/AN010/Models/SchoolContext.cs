using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace AN010.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
    }
}
