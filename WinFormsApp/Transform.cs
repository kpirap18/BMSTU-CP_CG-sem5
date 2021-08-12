using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp
{
    class Transform
    {
        static public void TurnX(Point3D p, Point3D p2, double angle)
        {
            float y = p2.Y;
            angle = angle * Math.PI / 180;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            p2.Y = y * cos - p2.Z * sin - p.Y * (cos - 1) + p.Z * sin;
            //p2.Z = y * sin - p2.Z * cos - p.Y * sin + p.Z * (1 - cos);
        }
        static public void TurnY(Point3D p, Point3D p2, double angle)
        {
            float x = p2.X;
            angle = angle * Math.PI / 180;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            p2.X = x * cos + p2.Z * sin + p.X * (1 - cos) - p.Z * sin;
            //p2.Z = -x * sin + p2.Z * cos + p.X * sin + p.Z * (1 - cos);
        }
        static public void TurnZ(Point3D p, Point3D p2, double angle)
        {
            float x = p2.X;
            angle = angle * Math.PI / 180;
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            p2.X = x * cos - p2.Y * sin + p.X * (1 - cos) + p.Y * sin;
            p2.Y = x * sin + p2.Y * cos - p.X * sin + p.Y * (1 - cos);
        }
        
        static public void Scale(Point3D p, Point3D p2, double k)
        {
            p2.X = (float)(k * p2.X + p.X * (1 - k));
            p2.Y = (float)(k * p2.Y + p.Y * (1 - k));
            p2.Z = (float)(k * p2.Z + p.Z * (1 - k));
        }
    }
}
