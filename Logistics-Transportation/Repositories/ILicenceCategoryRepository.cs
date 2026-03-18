using Logistics_Transportation.Models;

namespace Logistics_Transportation.Repositories
{
    public interface ILicenceCategoryRepository
    {
        Task<List<LicenceCategories>> GetAllAsync();
        Task<LicenceCategories?> GetByIdAsync(int id);
        Task AddAsync(LicenceCategories licenceCategories);
        Task UpdateAsync(LicenceCategories licenceCategories);
        Task DeleteAsync(LicenceCategories licenceCategories);
    }
}
