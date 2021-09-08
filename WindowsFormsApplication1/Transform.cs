using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    static class Transform
    {
        static public void RotateX(Point3D c, Point3D c2, double angle)
        {
            float y = c2.Y;
            angle = angle * Math.PI / 180;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            c2.Y = y * cos - c2.Z * sin - c.Y * (cos - 1) + c.Z * sin;
            c2.Z = y * sin + c2.Z * cos - c.Y * sin + c.Z * (1 - cos);
        }
        static public void RotateY(Point3D c, Point3D c2, double angle)
        {
            float x = c2.X;
            angle = angle * Math.PI / 180;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            c2.X = x * cos + c2.Z * sin + c.X * (1 - cos) - c.Z * sin;
            c2.Z = -x * sin + c2.Z * cos + c.X * sin + c.Z * (1 - cos);
        }
        static public void RotateZ(Point3D c, Point3D c2, double angle)
        {
            float x = c2.X;
            angle = angle * Math.PI / 180;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            c2.X = x * cos - c2.Y * sin + c.X * (1 - cos) + c.Y * sin;
            c2.Y = x * sin + c2.Y * cos - c.X * sin + c.Y * (1 - cos);
        }
        static public void Scale(Point3D c, Point3D c2, double k)
        {
            c2.X = (float)(k * c2.X + c.X * (1 - k));
            c2.Y = (float)(k * c2.Y + c.Y * (1 - k));
            c2.Z = (float)(k * c2.Z + c.Z * (1 - k));
        }
    }
}
