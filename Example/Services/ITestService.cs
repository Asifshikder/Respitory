using NuGet.Protocol;
using Respitory.Example.Models;

namespace Respitory.Example.Services
{
    public interface ITestService
    {
        Task<IEnumerable<Test>> GetTests();
        Task<Test> AddAsync(Test model);
        Task<bool> UpdateAsync(Test model);
        Task<bool> Delete(Test model);
        Task<Test> GetByIdAsync(long id);
    }
    public class TestService : ITestService
    {
        private IRepository<Test, ApplicationUser, AppDbContext> repository;

        public TestService(IRepository<Test,ApplicationUser,AppDbContext> repository)
        {
            this.repository = repository;
        }
        public async Task<Test> AddAsync(Test model)
        {
            return await repository.AddAsync(model);
        }

        public async Task<bool> Delete(Test model)
        {
            return await repository.DeleteAsync(model);
        }

        public async Task<Test> GetByIdAsync(long id)
        {
            return await repository.GetByIdAsync(id); 
        }

        public async Task<IEnumerable<Test>> GetTests()
        {
            return await repository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(Test model)
        {
            return await repository.UpdateAsync(model);
        }
    }
}
