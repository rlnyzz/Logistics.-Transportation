using Logistics_Transportation.Migrations;

namespace Logistics_Transportation.Services
{
    public interface IActionLogService
    {
        Task LogAsync(Models.ActionLog actionLog);
    }
}
