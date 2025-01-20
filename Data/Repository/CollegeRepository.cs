using CollegeApp_API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CollegeApp_API.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        private readonly CollegeDBContext _collegeDBContext;
        private DbSet<T> _dbSet;

        public CollegeRepository(CollegeDBContext collegeDBContext)
        {
            _collegeDBContext = collegeDBContext;
            _dbSet = _collegeDBContext.Set<T>();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _collegeDBContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _collegeDBContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T,bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
            {
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            }
            else
            {
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
            }
        }

        //public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await _dbSet.Where(filter).FirstOrDefaultAsync();
        //}

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);
            await _collegeDBContext.SaveChangesAsync();

            return dbRecord;
        }
    }
}
