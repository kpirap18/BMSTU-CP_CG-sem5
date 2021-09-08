using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    class Comparator : IEqualityComparer<Point3D>
    {
        public bool Equals(Point3D x, Point3D y)
        {
            if (x.X == y.X && x.Y == y.Y && x.Z == y.Z) return true;
            return false;
        }
        public int GetHashCode(Point3D product)
        {
            return 0;
        }

    }
}
