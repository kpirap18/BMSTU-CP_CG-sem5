using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    class Window
    {
        public Window(Point3D p)
        {
            Points = new Point3D[4];
            Points[0] = new Point3D(p.X, p.Y, p.Z);
            Points[1] = new Point3D(p.X + 20, p.Y, p.Z);
            Points[2] = new Point3D(p.X + 20, p.Y - 20, p.Z);
            Points[3] = new Point3D(p.X, p.Y - 20, p.Z);
            var r = new Random();
            Light = r.Next(0, 2);
        }
        public void RotateW(Point3D p, Point3D c)
        {
            for (int i = 0; i < 4; i++)
            {
                Transform.RotateY(c, Points[i], p.Y);
                Transform.RotateX(c, Points[i], p.X);
            }
        }
        private PointF[] ToPolygon()
        {
            var p = new PointF[4];
            for (int i = 0; i < 4; i++)
                p[i] = Points[i].PointF;
            return p;
        }
        public void DrawWindow(Graphics g, bool[] sh, int k, Lightning ll, double[] no, Point3D p, bool f, int w, int h)
        {
            if (Light == 1)
            {
                g.FillPolygon(new SolidBrush(Color.Yellow), ToPolygon());
                g.DrawLine(new Pen(Color.Black), (Points[0].X + Points[1].X)/2, (Points[0].Y + Points[1].Y)/2,
                           (Points[2].X + Points[3].X)/2, (Points[2].Y + Points[3].Y)/2);
                g.DrawLine(new Pen(Color.Black), (Points[0].X + Points[3].X)/2, (Points[0].Y + Points[3].Y)/2 - 5,
                           (Points[2].X + Points[1].X)/2, (Points[2].Y + Points[1].Y)/2 - 5);
            }
            else
            {
                g.FillPolygon(new SolidBrush(Color.Black), ToPolygon());
                if (sh[k])
                {
                    if (f)
                    {
                        var points = RayTracer.Reflection(this, no, p, ll);
                        if (points.Count > 1)
                            g.DrawLines(new Pen(Color.White), points.ToArray());
                    }
                    else
                    {
                        var he = new Highlight(ll, this, w, h);
                        g.DrawLine(new Pen(Color.White), he.Edge[0].PointF, he.Edge[1].PointF);
                        Lightning.Draworeol(he.Edge[0].PointF, he.Edge[1].PointF, g);
                    }
                }
            }
        }

        public readonly Point3D[] Points;
        public int Light;
    }
}
