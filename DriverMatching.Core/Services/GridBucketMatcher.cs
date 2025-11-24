using DriverMatching.core.Models;
using DriverMatching.core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverMatching.core.Services
{
    public class GridBucketMatcher : IDriverMatcher
    {
        private readonly Dictionary<string, Driver> _drivers = new();
        private readonly Dictionary<(int, int), List<Driver>> _buckets = new();
        private readonly int _cellSize;

        public GridBucketMatcher(int cellSize = 10)
        {
            _cellSize = cellSize;
        }

        private (int, int) GetCell(int x, int y) => (x / _cellSize, y / _cellSize);

        public void AddDriver(Driver driver)
        {
            _drivers[driver.Id] = driver;
            var cell = GetCell(driver.X, driver.Y);
            if (!_buckets.ContainsKey(cell)) _buckets[cell] = new List<Driver>();
            _buckets[cell].Add(driver);
        }

        public void UpdateDriver(string id, int newX, int newY)
        {
            if (!_drivers.TryGetValue(id, out var oldDriver))
                throw new ArgumentException($"Driver {id} not found.");

            var oldCell = GetCell(oldDriver.X, oldDriver.Y);
            if (_buckets.TryGetValue(oldCell, out var oldList))
            {
                oldList.RemoveAll(d => d.Id == id);
                if (oldList.Count == 0)
                    _buckets.Remove(oldCell);
            }

            var newDriver = new Driver(id, newX, newY);
            _drivers[id] = newDriver;

            var newCell = GetCell(newX, newY);
            if (!_buckets.ContainsKey(newCell))
                _buckets[newCell] = new List<Driver>();
            _buckets[newCell].Add(newDriver);
        }

        public void RemoveDriver(string id)
        {
            if (!_drivers.TryGetValue(id, out var driver)) return;
            _drivers.Remove(id);
            var cell = GetCell(driver.X, driver.Y);
            if (_buckets.TryGetValue(cell, out var list))
            {
                list.RemoveAll(d => d.Id == id);
                if (list.Count == 0) _buckets.Remove(cell);
            }
        }

        public IReadOnlyList<Driver> FindNearestDrivers(Order order, int count = 5)
        {
            var candidates = new List<Driver>();
            var targetCell = GetCell(order.X, order.Y);

            for (int radius = 0; radius <= 200; radius++)
            {
                foreach (var cell in GetCellsInRadius(targetCell, radius))
                {
                    if (_buckets.TryGetValue(cell, out var drivers))
                        candidates.AddRange(drivers);
                }
                if (candidates.Count > count * 10) break;
            }

            return candidates
                .OrderBy(d => Distance.SquaredEuclidean(d.X, d.Y, order.X, order.Y))
                .Take(count)
                .ToList();
        }

        private IEnumerable<(int, int)> GetCellsInRadius((int X, int Y) center, int r)
        {
            if (r == 0) yield return center;
            else
            {
                for (int dx = -r; dx <= r; dx++)
                {
                    yield return (center.X + dx, center.Y - r);
                    yield return (center.X + dx, center.Y + r);
                }
                for (int dy = -r + 1; dy <= r - 1; dy++)
                {
                    yield return (center.X - r, center.Y + dy);
                    yield return (center.X + r, center.Y + dy);
                }
            }
        }
    }
}
