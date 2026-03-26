using Logistics_Transportation.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Logistics_Transportation.Services
{
    public class TripMLService : ITripMLService
    {
        private readonly ApplicationDbContext _dbContext;
        public TripMLService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Trip?> CreateTripWithMLFunctionAsync(Order order)
        {
            string PickUpAddress = order.PickAppAddress;
            string DeliveryAdress = order.DeliveryAddress;
            string ApiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6IjY1NTA2MGU4N2FmYTQ4MzM5YWJkNjdkZmNmZDkzMTU4IiwiaCI6Im11cm11cjY0In0=";
            
            var client = new HttpClient();

            var urlStart = $"https://api.openrouteservice.org/geocode/search?api_key={ApiKey}&text={Uri.EscapeDataString(PickUpAddress)}";
            var urlEnd = $"https://api.openrouteservice.org/geocode/search?api_key={ApiKey}&text={Uri.EscapeDataString(DeliveryAdress)}";

            var responseStart = await client.GetStringAsync(urlStart);
            var responseEnd = await client.GetStringAsync(urlEnd);

            var startJson =JObject.Parse(responseStart);
            var endJson = JObject.Parse(responseEnd);

            var startFeature = startJson["features"]?.FirstOrDefault();
            var endFeature = endJson["features"]?.FirstOrDefault();

            if (startFeature == null)
            {
                Console.WriteLine("Не удалось найти координаты точки отправления");
                return null;
            }

            if (endFeature == null)
            {
                Console.WriteLine("Не удалось найти координаты точки отправки");
                return null;
            }

            var startCoords = (JArray?)startFeature["geometry"]?["coordinates"];
            var endCoords = (JArray?)endFeature["geometry"]?["coordinates"];

            if (startCoords == null || endCoords == null)
            {
                Console.WriteLine("Некоректный формат координат API");
                return null;
            }

            double startLon = startCoords[0].Value<double>();
            double startLat = startCoords[1].Value<double>();
            double endLon = endCoords[0].Value<double>();
            double endLat = endCoords[1].Value<double>();

            var inputML = new MLModel.ModelInput
            {
                Description = order.Description
            };

            var result = MLModel.Predict(inputML);
            var typeCar = result.PredictedLabel;

            Console.WriteLine($"Predicted car type: {typeCar}");

            var car = await _dbContext.Cars
                .Where(c => c.TypeOfCar == typeCar
                         && c.CargoCapacityT >= (decimal)order.CargoWeight / 1000
                         && c.TrunkVolumeL >= (decimal)order.CargoVolume)
                .OrderBy(c => c.CargoCapacityT)
                .ThenBy(c => c.TrunkVolumeL)
                .FirstOrDefaultAsync();

            Console.WriteLine($"Selected car: {car?.TypeOfCar}, Capacity: {order.CargoWeight} tons, Volume: {order.CargoVolume} liters");

            if (car == null)
            {
                Console.WriteLine("Car not found");
                return null;
            }

            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(c => c.CategoryLicenceId == car.LicenceCategoriesId);

            Console.WriteLine($"driver: {driver?.Id}");

            if (driver == null)
            {
                Console.WriteLine("Driver not found");
                return null;
            }

            var routeRequest = new
            {
                locations = new double[][]
                {
                    new double[] { startLon, startLat },
                    new double[] { endLon, endLat }
                },
                metrics = new string[] { "distance", "duration" }
            };

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKey);

            var routeResponse = await client.PostAsJsonAsync(
                "https://api.openrouteservice.org/v2/matrix/driving-car",
                routeRequest
            );

            if (!routeResponse.IsSuccessStatusCode)
            {
                var error = await routeResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"ORS ERROR: {error}");
                return null;
            }

            var routeContent = await routeResponse.Content.ReadAsStringAsync();
            var routeJson = JObject.Parse(routeContent);

            var distances = routeJson["distances"];
            var durations = routeJson["durations"];

            if (distances == null)
            {
                Console.WriteLine($"Проблема distance == null");
                return null;
            }
            if (durations == null)
            {
                Console.WriteLine($"Проблема durations == null");
                return null;
            }
            if (distances.Count() < 1)
            {
                Console.WriteLine($"Проблема distance.Count < 1");
                return null;
            }
            if (durations.Count() < 1)
            {
                Console.WriteLine($"Проблема durations.count < 1");
                return null;
            }
            double distanceMeters = distances[0]?[1]?.Value<double>() ?? 0;
            double durationSeconds = durations[0]?[1]?.Value<double>() ?? 0;

            double distanceKm = distanceMeters / 1000;
            double durationMinutes = durationSeconds / 60;

            Console.WriteLine($"distanceKm {distanceKm}");

            var priceMLData = new MLModelPrice.ModelInput()
            {
                DistanceKm = (float)distanceKm,
                DurationMin = (float)durationMinutes,
                Weight = (float)order.CargoWeight,
                Volume = (float)order.CargoVolume,
                CarType = car.TypeOfCar
            };

            var resultPrice = MLModelPrice.Predict(priceMLData);
            Console.WriteLine(resultPrice);

            var finalPrice = resultPrice.Score;
            Console.WriteLine(finalPrice);

            var trip = new Trip
            {
                OrderId = order.Id,
                CarId = car.Id,
                DriverId = driver.Id,
                FinalePrice = (decimal)finalPrice,
                FinaleTimeMinutes = (int)durationMinutes
            };

            await _dbContext.Trips.AddAsync(trip);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine("Start response: " + responseStart);
            Console.WriteLine("End response: " + responseEnd);

            return trip;
        }
    }
}
