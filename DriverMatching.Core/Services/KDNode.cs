using DriverMatching.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverMatching.core.Services
{
    internal class KDNode
    {
        public Driver Driver { get; }
        public KDNode? Left { get; set; }
        public KDNode? Right { get; set; }

        public KDNode(Driver driver) => Driver = driver;
    }
}
