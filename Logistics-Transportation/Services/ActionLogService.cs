using Logistics_Transportation.Migrations;
using Microsoft.IdentityModel.Tokens;
using Logistics_Transportation.Models;

namespace Logistics_Transportation.Services
{
    public class ActionLogService : IActionLogService
    {
        private readonly ApplicationDbContext _dbContext;
        public ActionLogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task LogAsync(Models.ActionLog actionLog)
        {
            await _dbContext.ActionLogs.AddAsync(actionLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}
