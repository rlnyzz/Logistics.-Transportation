using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface IDriverRepository
    {
        Task<List<Driver>> GetAllAsync();
        Task<Driver?> GetByIdAsync(int id);
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);
        Task DeleteAsync(Driver driver);
        Task<LicenceCategories?> GetLicenceCategoryByNameAsync(string name);
    }
}
