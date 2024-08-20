using Microsoft.EntityFrameworkCore;
using CrudApplication.Models;

namespace CrudApplication.Database
{
    public class StudentDBContext : DbContext
    {
        public StudentDBContext(DbContextOptions<StudentDBContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
