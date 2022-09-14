using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respitory
{
    public class Repository<T, TUser,TContext> : IRepository<T,TUser, TContext> where T : BaseEntity where TUser : IdentityUser where TContext : DbContext
    {
        private IUserContext<TUser> userContext;
        private TContext context;

        public Repository(IUserContext<TUser> userContext, TContext context)
        {
            this.userContext = userContext;
            this.context = context;
        }
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().CountAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)

        {
            entity = await GetAddAsyncProperties(entity);
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync(cancellationToken);

            return entity;

        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            entity = await GetUpdateAsyncProperties(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(true);

        }


        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            TUser user = await userContext.GetCurrentUserAsync();
            List<T> EntityList = new List<T>();
            foreach (var item in entities)
            {
                item.CreatedBy = user != null ? user.Id : "Self Created";
                item.CreatedOn = DateTime.UtcNow;
                EntityList.Add(item);
            }
            await context.Set<T>().AddRangeAsync(EntityList);
            await context.SaveChangesAsync();
        }

        public IQueryable<T> AllAsIQueryable()
        {
            return context.Set<T>().AsQueryable();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return context.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public int Count()
        {
            return context.Set<T>().Count();
        }


        public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Remove(context.Set<T>().Find(id));
            await context.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(true);
        }
        public async Task<T> FirstAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().FirstAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var findData = await context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
            return findData;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetPaginatedAsync(int recSkip, int recTake, CancellationToken cancellationToken = default)
        {
            return await context.Set<T>().Skip(recSkip).Take(recTake).ToListAsync(cancellationToken);
        }

        private async Task<T> GetAddAsyncProperties(T entity)
        {
            TUser user = await userContext.GetCurrentUserAsync();
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = user != null ? user.Id : "Self Created";
            return entity;
        }
        private async Task<T> GetUpdateAsyncProperties(T entity)
        {
            TUser user = await userContext.GetCurrentUserAsync();
            entity.UpdatedOn = DateTime.UtcNow;
            entity.UpdatedBy = user != null ? user.Id : "Self updated";
            return entity;
        }
    }
}
