using Data.Models;
using Data.Repository.Base;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(EnrollmentDBContext enrollmentDBContext) : base(enrollmentDBContext) { }
    }
}
