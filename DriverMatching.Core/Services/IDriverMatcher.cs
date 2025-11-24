using DriverMatching.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverMatching.core.Services
{
    public interface IDriverMatcher
    {
        void AddDriver(Driver driver);
        void UpdateDriver(string id, int x, int y);
        void RemoveDriver(string id);
        IReadOnlyList<Driver> FindNearestDrivers(Order order, int count = 5);
    }
}
