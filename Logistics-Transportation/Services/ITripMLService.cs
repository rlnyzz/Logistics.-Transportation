using Logistics_Transportation.Models;

namespace Logistics_Transportation.Services
{
    public interface ITripMLService
    {
        Task<Trip?> CreateTripWithMLFunctionAsync(Order order);
    }
}
