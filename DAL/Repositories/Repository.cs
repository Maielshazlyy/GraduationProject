using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using Domain_layer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly string _primaryKeyName;
        
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
            
            // Get the primary key property name dynamically
            var entityType = context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey();
            _primaryKeyName = primaryKey?.Properties.FirstOrDefault()?.Name ?? "Id";
        }
        
        // Basic CRUD Operations
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        public async Task<T?> GetByIdAsync(string id)
        {
            // Use EF.Property to access the primary key dynamically
            return await _dbSet.FirstOrDefaultAsync(e => 
                EF.Property<string>(e, _primaryKeyName) == id);
        }
        
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        
        // Advanced Query Operations
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
        
        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            
            return await _dbSet.CountAsync(predicate);
        }
        
        // Include Related Entities
        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            
            return await query.ToListAsync();
        }
        
        public async Task<T?> GetByIdWithIncludeAsync(string id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            
            // Use the primary key name dynamically
            return await query.FirstOrDefaultAsync(e => 
                EF.Property<string>(e, _primaryKeyName) == id);
        }
    }
}
