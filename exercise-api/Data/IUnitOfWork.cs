using Data.Repository.Interfaces;
using System;

namespace Data
{
    public interface IUnitOfWork
    {
        IStudentRepository StudentRepository { get; }
        IClassRepository ClassRepository { get; }
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
        void Save();
    }
}
