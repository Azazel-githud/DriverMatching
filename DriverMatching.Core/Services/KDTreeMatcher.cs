using DriverMatching.core.Models;
using DriverMatching.core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DriverMatching.core.Services
{
    public class KDTreeMatcher : IDriverMatcher
    {
        private readonly Dictionary<string, Driver> _drivers = new();
        private KDNode? _root;
        private bool _dirty = true;

        public void AddDriver(Driver driver)
        {
            _drivers[driver.Id] = driver;
            _dirty = true;
        }

        public void UpdateDriver(string id, int x, int y)
        {
            _drivers[id] = new Driver(id, x, y);
            _dirty = true;
        }

        public void RemoveDriver(string id)
        {
            _drivers.Remove(id);
            _dirty = true;
        }

        private void RebuildTree()
        {
            var drivers = _drivers.Values.ToList();
            _root = BuildTree(drivers, depth: 0);
            _dirty = false;
        }

        private KDNode? BuildTree(List<Driver> drivers, int depth)
        {
            if (drivers.Count == 0) return null;

            bool isX = depth % 2 == 0;
            drivers.Sort((a, b) => isX ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));
            int mid = drivers.Count / 2;

            var node = new KDNode(drivers[mid]);
            node.Left = BuildTree(drivers.Take(mid).ToList(), depth + 1);
            node.Right = BuildTree(drivers.Skip(mid + 1).ToList(), depth + 1);
            return node;
        }

        public IReadOnlyList<Driver> FindNearestDrivers(Order order, int count = 5)
        {
            if (_dirty) RebuildTree();

            var nearest = new List<(Driver Driver, long DistSq)>();
            Search(_root, order, count, nearest, depth: 0);

            return nearest
                .OrderBy(p => p.DistSq)
                .Take(count)
            .Select(p => p.Driver)
            .ToList();
        }

        private void Search(KDNode? node, Order order, int count, List<(Driver, long)> nearest, int depth)
        {
            if (node == null) return;

            var d = Distance.SquaredEuclidean(node.Driver.X, node.Driver.Y, order.X, order.Y);
            nearest.Add((node.Driver, d));

            bool isX = depth % 2 == 0;
            int diff = isX ? order.X - node.Driver.X : order.Y - node.Driver.Y;
            KDNode? next = diff < 0 ? node.Left : node.Right;
            KDNode? other = diff < 0 ? node.Right : node.Left;

            Search(next, order, count, nearest, depth + 1);

            // Продолжаем и в другую ветку, если есть шанс найти ближе
            if (nearest.Count < count || d <= nearest.Max(x => x.Item2))
            {
                Search(other, order, count, nearest, depth + 1);
            }
        }
    }
}
