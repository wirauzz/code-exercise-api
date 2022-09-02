using Data.Models;
using Data.Repository.Base;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(EnrollmentDBContext enrollmentDBContext) : base(enrollmentDBContext) { }
    }
}
