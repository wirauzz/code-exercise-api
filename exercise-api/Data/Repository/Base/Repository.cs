using Data.Models.Base;
using Microsoft.EntityFrameworkCore;
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
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
