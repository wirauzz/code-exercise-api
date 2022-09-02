using Data.Repository;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnrollmentDBContext _enrollmentDBContext;
        private readonly IStudentRepository _student;

        public UnitOfWork(EnrollmentDBContext dBContext)
        {
            _enrollmentDBContext = dBContext;
            _student = new StudentRepository(_enrollmentDBContext);
        }

        public IStudentRepository StudentRepository
        {
            get
            {
                return _student;
            }
        }

        public void BeginTransaction()
        {
            _enrollmentDBContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _enrollmentDBContext.Database.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            _enrollmentDBContext.Database.RollbackTransaction();
        }

        public void Save()
        {
            BeginTransaction();
            _enrollmentDBContext.SaveChanges();
            CommitTransaction();
        }
    }
}
