using Data.Exceptions;
using Data.Models.Base;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly EnrollmentDBContext dbContext;
        public Repository(EnrollmentDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                dbContext.Set<T>().Add(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                string message = $"Can not create the {typeof(T)} in Repository -> CreateAsync() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new DatabaseException($"Can not create the {typeof(T)}", ex.InnerException);
            }
        }

        public async Task<T> DeleteAsync(T entity)
        {
            try
            {
                dbContext.Set<T>().Remove(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                string message = $"Can not delete {typeof(T)}{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} tack trace: {Environment.NewLine}");
                throw new DatabaseException($"Can not delete {typeof(T)}", ex.InnerException);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                string message = $"Can not find any {typeof(T)} in Repository -> GetAllAsync() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new DatabaseException($"Can not find any {typeof(T)}", ex.InnerException);
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                string message = $"Can not find any {typeof(T)} by id in Repository -> GetByIdAsync() with this id: {id}{Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new DatabaseException($"Can not find any {typeof(T)} by id", ex.InnerException);
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                string message = $"Can not update {typeof(T)} in Repository -> UpdateAsync() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new DatabaseException($"Can not update {typeof(T)}", ex.InnerException);
            }
        }
    }
}
