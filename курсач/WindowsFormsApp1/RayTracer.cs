using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    struct Trace
    {
        public Point3D Per;
        public double Dist;
        public int I;
    }

    struct Ray
    {
        public readonly Point3D Vec;
        public readonly Point3D Beg;

        public Ray(Point3D vec, Point3D beg) : this()
        {
            Vec = vec;
            Beg = beg;
        }
    }
    class RayTracer
    {
        public Trace Trace(Ray ray, House house, Lightning lightning)
        {
            Trace t1 = house.TraceM(ray);
            Trace t2 = lightning.TraceM(ray);
            var per = new Trace { Dist = 0 };
            if (t1.Dist == -1 && t2.Dist == -1)
            {
                return per;
            }
            if (t1.Dist == -1)
            {
                per.Dist = 1;
                per.Per = t2.Per;
                per.I = t2.I;
                return per;
            }
            if (t2.Dist == -1)
            {
                per.Dist = house.Color(t2.Per) + 2;
                per.Per = t1.Per;
                return per;
            }
            if (t2.Dist < t1.Dist)
            {
                per.Dist = 1;
                per.Per = t2.Per;
                per.I = t2.I;
                return per;
            }
            per.Dist = house.Color(t2.Per) + 2;
            per.Per = t1.Per;
            return per;
        }

        static public List<PointF> Reflection(Window w, double[] nor, Point3D p, Lightning lightning)
        {
            int count_ray = 0;
            var po = new Point3D(p.X, p.Y, -200);
            var no = new double[4];
            var refl = new List<PointF>();
            double r;

            float dx = w.Points[0].X - w.Points[3].X;
            float dy = w.Points[0].Y - w.Points[3].Y;
            float dz = w.Points[0].Z - w.Points[3].Z;

            float dx2 = w.Points[0].X - w.Points[1].X;
            float dy2 = w.Points[0].Y - w.Points[1].Y;
            float dz2 = w.Points[0].Z - w.Points[1].Z;

            double step = 1.0 / Math.Sqrt(dx * dx + dy * dy + dz * dz);

            nor.CopyTo(no, 0);
            for (int i = 0; i < 4; i++)
            {
                no[i] /= 100000;
            }

            for (r = 0.0; r <= 1; r = r + step)
            {
                var x = (float)(w.Points[3].X + r * dx + 0.5);
                var y = (float)(w.Points[3].Y + r * dy + 0.5);
                var z = (float)(w.Points[3].Z + r * dz + 0.5);
                float x2 = x + dx2;
                float y2 = y + dy2;
                float z2 = z + dz2;

                float dx3 = x2 - x;
                float dy3 = y2 - y;
                float dz3 = z2 - z;

                double step2 = 1.0 / Math.Sqrt(dx3 * dx3 + dy3 * dy3 + dz3 * dz3);
                double r2;

                for (r2 = 0.0; r2 <= 1; r2 = r2 + step2)
                {
                    var x3 = (float)(x - r2 * dx3 + 0.5);
                    var y3 = (float)(y - r2 * dy3 + 0.5);
                    var z3 = (float)(z - r2 * dz3 + 0.5);

                    var poi = new Point3D(x3 - po.X, y3 - po.Y, z3 - po.Z);
                    double a = Math.Abs(no[0] * poi.X + no[1] * poi.Y + no[2] * poi.Z) /
                               Math.Sqrt(no[0] * no[0] + no[1] * no[1] + no[2] * no[2]) /
                               Math.Sqrt(poi.X * poi.X + poi.Y * poi.Y + poi.Z * poi.Z);

                    a = Math.Asin(a);

                    var os = new Point3D((float)(poi.Y * no[2] - poi.Z * no[1]),
                                         (float)(no[0] * poi.Z - poi.X * no[2]),
                                         (float)(poi.X * no[1] - poi.Y * no[0]));

                    var vector = new Point3D();

                    double cos = Math.Cos(a), sin = Math.Sin(a);

                    vector.X = (float)(no[0] * (cos + (1 - cos) * os.X * os.X) + 
                        no[1] * ((1 - cos) * os.Y * os.X + sin * os.Z) +
                        no[2] * ((1 - cos) * os.Z * os.X - sin * os.Y));
                    vector.Y = (float)(no[0] * (-sin * os.Z + (1 - cos) * os.X * os.Y) + 
                        no[1] * ((1 - cos) * os.Y * os.Y + cos) +
                        no[2] * ((1 - cos) * os.Z * os.Y + sin * os.X));
                    vector.Z = (float)(no[0] * (sin * os.Y + (1 - cos) * os.X * os.Z) + 
                        no[1] * ((1 - cos) * os.Y * os.Z - sin * os.X) +
                        no[2] * ((1 - cos) * os.Z * os.Z + cos));

                    count_ray++;
                    var ray = new Ray(vector, new Point3D(x3, y3, z3));
                    Trace t2 = lightning.TraceM(ray);

                    if (t2.Dist != -1)
                    {
                        refl.Add(new PointF(x3, y3));
                    }
                }
            }

            System.Console.WriteLine("ray_trace = {0}", count_ray);
            return refl;
        }
    }
}