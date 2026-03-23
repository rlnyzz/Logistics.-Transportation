using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Transportation.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<User>> GetAllWithFilterAsync(string? email, string? phone)
        {
            var query = _dbContext.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(c => c.UserName.Contains(email));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(c => c.PhoneNumber.Contains(phone));
            }

            return await query.ToListAsync();
        }
        public async Task<User?> GetByIdAsync(string id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == id);
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            await _dbContext.Users.Where(c => c.Id == user.Id).ExecuteDeleteAsync();
        }
    }
}
