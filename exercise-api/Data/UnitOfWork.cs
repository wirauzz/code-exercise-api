using Data.Exceptions;
using Data.Repository;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnrollmentDBContext _enrollmentDBContext;
        private readonly IStudentRepository _student;
        private readonly IClassRepository _class;
        private readonly IEnrollmentRepository _enrollment;

        public UnitOfWork(EnrollmentDBContext dBContext)
        {
            _enrollmentDBContext = dBContext;
            _student = new StudentRepository(_enrollmentDBContext);
            _class = new ClassRepository(_enrollmentDBContext);
            _enrollment = new EnrollmentRepository(_enrollmentDBContext);
        }

        public IStudentRepository StudentRepository
        {
            get
            {
                CheckConnection();
                return _student;
            }
        }

        public IClassRepository ClassRepository
        {
            get
            {
                CheckConnection();
                return _class;
            }
        }

        public IEnrollmentRepository EnrollmentRepository
        {
            get
            {
                CheckConnection();
                return _enrollment;
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
        public void CheckConnection()
        {
            try
            {
                _enrollmentDBContext.Database.OpenConnection();
                _enrollmentDBContext.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                throw new DBConnetionException($"Can not connect to Database", ex.InnerException);
            }
        }
        public void Save()
        {
            try
            {
                BeginTransaction();
                _enrollmentDBContext.SaveChanges();
                CommitTransaction();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                RollBackTransaction();
                string message = $"Error to save changes on Database -> Save() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new DataException("Can not save changes, error in Database", ex.InnerException);
            }
            catch (DbUpdateException ex)
            {
                RollBackTransaction();
                string message = $"Error to save changes on Database -> Save() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new DataException("Can not save changes, error in Database", ex.InnerException);
            }
            catch (Exception ex)
            {
                RollBackTransaction();
                string message = $"Error to save changes on Database -> Save() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new DataException("Can not save changes, error in Database", ex.InnerException);
            }
        }
    }
}
