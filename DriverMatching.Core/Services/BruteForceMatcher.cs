using DriverMatching.core.Models;
using DriverMatching.core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverMatching.core.Services
{
    public class BruteForceMatcher : IDriverMatcher
    {
        private readonly Dictionary<string, Driver> _drivers = new();

        public void AddDriver(Driver driver) => _drivers[driver.Id] = driver;
        public void UpdateDriver(string id, int x, int y) => _drivers[id] = new Driver(id, x, y);
        public void RemoveDriver(string id) => _drivers.Remove(id);

        public IReadOnlyList<Driver> FindNearestDrivers(Order order, int count = 5)
        {
            return _drivers.Values
                .OrderBy(d => Distance.SquaredEuclidean(d.X, d.Y, order.X, order.Y))
                .Take(count)
                .ToList();
        }
    }
}
