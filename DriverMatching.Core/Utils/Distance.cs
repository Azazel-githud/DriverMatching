using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverMatching.core.Utils
{
    public static class Distance
    {
        public static long SquaredEuclidean(int x1, int y1, int x2, int y2)
            => (long)(x1 - x2) * (x1 - x2) + (long)(y1 - y2) * (y1 - y2);
    }
}
