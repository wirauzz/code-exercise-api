using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class EnrollmentDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public static EnrollmentDBContext _instance;
        public DbSet<Student> Student { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public EnrollmentDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MyConnection"));
        }
    }
}