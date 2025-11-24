using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverMatching.core.Models
{
    public class Driver
    {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Driver(string id, int x, int y)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            X = x;
            Y = y;
        }
    }
}
