using Data.Models;
using Data.Repository.Base;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(EnrollmentDBContext enrollmentDBContext) : base(enrollmentDBContext) { }
    }
}
