using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    class Shadow
    {
        public Shadow(Point3D c, Point3D p, House h, double y)
        {
            _shadow = new PointF[4][];
            for (int i = 0; i < 4; i++)
            {
                var pol = new PointF[4];
                var point3 = new Point3D(h.S[i].P[0].X, h.S[i].P[0].Y, h.S[i].P[0].Z);
                Transform.RotateY(c, point3, y);
                Transform.RotateX(c, point3, -15);
                pol[0] = point3.PointF;
                point3 = new Point3D(h.S[i].P[1].X, h.S[i].P[1].Y, h.S[i].P[1].Z);
                Transform.RotateY(c, point3, y);
                Transform.RotateX(c, point3, -15);
                pol[1] = point3.PointF;
                for (int j = 2; j < 4; j++)
                {
                    var per = new Point3D();
                    var ray = new Ray(new Point3D(h.S[i].P[j].X - p.X, h.S[i].P[j].Y - p.Y, h.S[i].P[j].Z - p.Z), new Point3D(p.X, p.Y, p.Z));
                    double t = (ray.Beg.Y + 500) / (ray.Vec.Y);
                    per.X = (float)(ray.Beg.X + ray.Vec.X * t);
                    per.Y = 500;
                    per.Z = (float)(ray.Beg.Z + ray.Vec.Z * t);
                    Transform.RotateY(c, per, y);
                    Transform.RotateX(c, per, -15);
                    pol[j] = per.PointF;
                }
                _shadow[i] = pol;
            }
        }
       public void DrawShadow(Graphics g)
        {
            Brush b = new SolidBrush(Color.Black);
            for (int i = 0; i < 4; i++)
            {
                g.FillPolygon(b, _shadow[i]);
            }
        }

        private readonly PointF[][] _shadow;
    }
}
