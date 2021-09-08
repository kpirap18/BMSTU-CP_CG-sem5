using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    struct Side
    {
        public Point3D[] P;
    }
    class House
    {
        public House(int w, int h, Point3D c, int angle, int kol)
        {
            _kol = kol;
            _center = new Point3D();
            _center = c;
            S = new Side[5];
            La = new Point3D[2];
            const int l = 40;
            int height = 36*kol;
            for (int i = 0; i < 5; i++)
            {
                S[i].P = new Point3D[4];
                for (int j = 0; j < 4; j++)
                {
                    S[i].P[j] = new Point3D();
                }
            }
            S[0].P[0].X = w / 2 - l;
            S[0].P[0].Z = -l;
            S[1].P[0].X = w / 2 - l;
            S[1].P[0].Z = l;
            S[2].P[0].X = w / 2 + l;
            S[2].P[0].Z = l;
            S[3].P[0].X = w / 2 + l;
            S[3].P[0].Z = -l;
            S[0].P[0].Y = h / 3 * 2 + 100;
            S[3].P[1] = new Point3D(S[0].P[0].X, S[0].P[0].Y, S[0].P[0].Z);
            for (int i = 1; i < 4; i++)
            {
                S[i].P[0].Y = h/3*2 + 100;
                S[i - 1].P[1] = new Point3D(S[i].P[0].X, S[i].P[0].Y, S[i].P[0].Z);
            }
            for (int i = 0; i < 4; i++)
            {
                S[i].P[2] = new Point3D(S[i].P[1].X, S[i].P[1].Y, S[i].P[1].Z);
                S[i].P[2].Y -= height;
            }
            for (int i = 0; i < 4; i++)
            {
                S[i].P[3] = new Point3D(S[i].P[0].X, S[i].P[0].Y, S[i].P[0].Z);
                S[i].P[3].Y -= height;
            }
            for (int i = 0; i < 4; i++)
            {
                S[4].P[i] = new Point3D(S[i].P[3].X, S[i].P[3].Y, S[i].P[3].Z);
            }
            La[0] = new Point3D((S[4].P[1].X + S[4].P[2].X) / 2, S[4].P[2].Y, (S[4].P[1].Z + S[4].P[0].Z) / 2);
            La[1] = new Point3D(La[0].X, La[0].Y-40, La[0].Z);

            _pl = new double[5][];
            for (int i = 0; i < 5; i++)
            {
                _pl[i] = new double[4];
            }
            WindowsCreate(c);
            RotateHouse(_center, new Point3D(0, angle, 0));
        }
        private void WindowsCreate(Point3D c)
        {
            W = new Window[4][];
            var p = new Point3D(S[1].P[0].X + 10, S[1].P[0].Y - 10, S[1].P[0].Z);
            for (int i = 0; i < 4; i++)
            {
                W[i] = new Window[2 * _kol];
            }
            for (int i = 0; i < _kol; i++)
            {
                int k = 2 * i;
                W[1][k] = new Window(p);
                W[2][k] = new Window(p);
                W[2][k].RotateW(new Point3D(0, 90, 0), c);
                W[3][k] = new Window(p);
                W[3][k].RotateW(new Point3D(0, 180, 0), c);
                W[0][k] = new Window(p);
                W[0][k].RotateW(new Point3D(0, 270, 0), c);
                p.X += 40;
                k++;
                W[1][k] = new Window(p);
                W[2][k] = new Window(p);
                W[2][k].RotateW(new Point3D(0, 90, 0), c);
                W[3][k] = new Window(p);
                W[3][k].RotateW(new Point3D(0, 180, 0), c);
                W[0][k] = new Window(p);
                W[0][k].RotateW(new Point3D(0, 270, 0), c);
                p.X -= 40;
                p.Y -= 35;
            }
        }
        public Trace TraceM(Ray ray)
        {
            var min = new Trace {Dist = -1, Per = new Point3D(0,0,0)};
            var per = new Point3D();
            for (int i = 0; i < 5; i++)
            {
                double t = (-_pl[i][0]*ray.Beg.X - _pl[i][1]*ray.Beg.Y - _pl[i][2]*ray.Beg.Z - _pl[i][3]) / 
                           (_pl[i][0]*ray.Vec.X + _pl[i][1]*ray.Vec.Y + _pl[i][2]*ray.Vec.Z);
                per.X = (float)(ray.Beg.X + ray.Vec.X * t);
                per.Y = (float)(ray.Beg.Y + ray.Vec.Y * t);
                per.Z = (float)(ray.Beg.Z + ray.Vec.Z * t);
                if (per.X >= Math.Min(S[i].P[0].X, S[i].P[2].X) && per.X <= Math.Max(S[i].P[0].X, S[i].P[2].X) &&
                    per.Y >= Math.Min(S[i].P[0].Y, S[i].P[2].Y) && per.Y <= Math.Max(S[i].P[0].Y, S[i].P[2].Y) &&
                    per.Z >= Math.Min(S[i].P[0].Z, S[i].P[2].Z) && per.Z <= Math.Max(S[i].P[0].Z, S[i].P[2].Z))
                {
                    double dist = Math.Sqrt(Math.Pow(per.X - ray.Beg.X, 2) + Math.Pow(per.Y - ray.Beg.Y, 2) +
                                            Math.Pow(per.Z - ray.Beg.Z, 2));
                    if (dist < min.Dist || min.Dist == -1)
                    {
                        min.Per = new Point3D(per.X, per.Y, per.Z);
                        min.Dist = dist;
                    }
                }
            }
            return min;
        }
        public int Color(Point3D at)
        {
            if (at.X == S[0].P[0].X && at.Y == S[0].P[0].Y || at.Z == S[0].P[0].Z && at.Y == S[0].P[0].Y ||
                at.X == S[0].P[0].X && at.Z == S[0].P[0].Z || at.X == S[2].P[0].X && at.Y == S[2].P[0].Y ||
                at.Z == S[2].P[0].Z && at.Y == S[2].P[0].Y || at.X == S[2].P[0].X && at.Z == S[2].P[0].Z ||
                at.X == S[0].P[2].X && at.Y == S[0].P[2].Y || at.Z == S[0].P[2].Z && at.Y == S[0].P[2].Y ||
                at.X == S[0].P[2].X && at.Z == S[0].P[2].Z || at.X == S[2].P[2].X && at.Y == S[2].P[2].Y ||
                at.Z == S[2].P[2].Z && at.Y == S[2].P[2].Y || at.X == S[2].P[2].X && at.Z == S[2].P[2].Z)
            {
                return 1;
            }
            return 0;
        }
        private void RotateHouse(Point3D p, Point3D angle)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 4; j++)
                {
                    Transform.RotateY(p, S[i].P[j], angle.Y);
                    Transform.RotateX(p, S[i].P[j], angle.X);
                    Transform.RotateZ(p, S[i].P[j], angle.Z);
                }
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2*_kol; j++)
                {
                    W[i][j].RotateW(angle, p);
                }
            Transform.RotateY(p, La[0], angle.Y);
            Transform.RotateX(p, La[0], angle.X);
            Transform.RotateZ(p, La[0], angle.Z);
            Transform.RotateY(p, La[1], angle.Y);
            Transform.RotateX(p, La[1], angle.X);
            Transform.RotateZ(p, La[1], angle.Z);
            for (int i = 0; i < 5; i++)
            {
                _pl[i][0] = (S[i].P[1].Y - S[i].P[0].Y) * (S[i].P[2].Z - S[i].P[0].Z) - (S[i].P[2].Y - S[i].P[0].Y) * (S[i].P[1].Z - S[i].P[0].Z);
                _pl[i][1] = (S[i].P[1].X - S[i].P[0].X) * (S[i].P[2].Z - S[i].P[0].Z) - (S[i].P[2].X - S[i].P[0].X) * (S[i].P[1].Z - S[i].P[0].Z);
                _pl[i][2] = (S[i].P[1].X - S[i].P[0].X) * (S[i].P[2].Y - S[i].P[0].Y) - (S[i].P[2].X - S[i].P[0].X) * (S[i].P[1].Y - S[i].P[0].Y);
                _pl[i][3] = -_pl[i][0] * S[i].P[3].X - _pl[i][1] * S[i].P[3].Y - _pl[i][2] * S[i].P[3].Z;
            }
        }
        public bool[] InShadow2(Point3D l)
        {
            var f = new bool[4];
            for (int j = 0; j < 4; j++)
            {
                var ray = new Ray(new Point3D((S[j].P[0].X + S[j].P[2].X) / 2 - l.X,
                                            (S[j].P[0].Y + S[j].P[2].Y) / 2 - l.Y,
                                            (S[j].P[0].Z + S[j].P[2].Z) / 2 - l.Z),
                                new Point3D(l.X, l.Y, l.Z));
                var per = new Point3D();
                double t = (-_pl[j][0]*ray.Beg.X - _pl[j][1]*ray.Beg.Y - _pl[j][2]*ray.Beg.Z - _pl[j][3])/
                           (_pl[j][0]*ray.Vec.X + _pl[j][1]*ray.Vec.Y + _pl[j][2]*ray.Vec.Z);
                per.X = (float) (ray.Beg.X + ray.Vec.X*t);
                per.Y = (float) (ray.Beg.Y + ray.Vec.Y*t);
                per.Z = (float) (ray.Beg.Z + ray.Vec.Z*t);
                double dist1 = Math.Sqrt(Math.Pow(per.X - ray.Beg.X, 2) + Math.Pow(per.Y - ray.Beg.Y, 2) +
                                         Math.Pow(per.Z - ray.Beg.Z, 2));

                double min = -10;
                for (int i = 0; i < 5; i++)
                {
                    double t2 = (-_pl[i][0]*ray.Beg.X - _pl[i][1]*ray.Beg.Y - _pl[i][2]*ray.Beg.Z - _pl[i][3])/
                                (_pl[i][0]*ray.Vec.X + _pl[i][1]*ray.Vec.Y + _pl[i][2]*ray.Vec.Z);
                    per.X = (float) (ray.Beg.X + ray.Vec.X*t2);
                    per.Y = (float) (ray.Beg.Y + ray.Vec.Y*t2);
                    per.Z = (float) (ray.Beg.Z + ray.Vec.Z*t2);
                    double dist2 = Math.Sqrt(Math.Pow(per.X - ray.Beg.X, 2) + Math.Pow(per.Y - ray.Beg.Y, 2) +
                                             Math.Pow(per.Z - ray.Beg.Z, 2));
                    if (per.X >= Math.Min(S[i].P[0].X, S[i].P[2].X) && per.X <= Math.Max(S[i].P[0].X, S[i].P[2].X) &&
                        per.Y >= Math.Min(S[i].P[0].Y, S[i].P[2].Y) && per.Y <= Math.Max(S[i].P[0].Y, S[i].P[2].Y) ||
                        per.Z >= Math.Min(S[i].P[0].Z, S[i].P[2].Z) && per.Z <= Math.Max(S[i].P[0].Z, S[i].P[2].Z) &&
                        per.X >= Math.Min(S[i].P[0].X, S[i].P[2].X) && per.X <= Math.Max(S[i].P[0].X, S[i].P[2].X))
                    {
                        if (dist2 < min || min == -10)
                        {
                            min = dist2;
                        }
                    }
                }
                if (Math.Abs(dist1 - min) > 1)
                    f[j] = false;
                else
                {
                    f[j] = true;
                }

            }
            return f;
        }
        private double DrawGr(int k, Lightning light)
        {
            double r;
            int n = 0;
            double sum = 0;

            float dy = S[k].P[0].Y - S[k].P[3].Y;
            float dx = S[k].P[0].X - S[k].P[3].X;
            float dz = S[k].P[0].Z - S[k].P[3].Z;

            float dy2 = S[k].P[0].Y - S[k].P[1].Y;
            float dx2 = S[k].P[0].X - S[k].P[1].X;
            float dz2 = S[k].P[0].Z - S[k].P[1].Z;

            double step = 1.0 / Math.Sqrt(dx * dx + dy * dy + dz * dz);

            for (r = 0.0; r <= 1; r = r + step)
            {
                var x = (float)(S[k].P[3].X + r * dx + 0.5);
                var y = (float)(S[k].P[3].Y + r * dy + 0.5);
                var z = (float)(S[k].P[3].Z + r * dz + 0.5);
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
                    var p = new Point3D(x3 - light.Model[0].X, y3 - light.Model[0].Y, z3 - light.Model[0].Z);
                    double a = Math.Abs(_pl[k][0] * p.X + _pl[k][1] * p.Y + _pl[k][2] * p.Z) /
                               Math.Sqrt(_pl[k][0] * _pl[k][0] + _pl[k][1] * _pl[k][1] +
                                         _pl[k][2] * _pl[k][2]) /
                               Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
                    a = Math.Cos(Math.Asin(a));
                    sum += a;
                    n++;
                }
            }
            sum /= n;
            return sum;
        }
        private void drawGr2(Graphics g, PointF[] pol)
        {
            var vectorTexture = new VectorTexture(TextureType.House, pol);
            vectorTexture.DrawTexture(g);
        }
        private void DrawWindow(Graphics g, int k, Lightning ll, bool[] sh, Point3D p, bool f, int w, int h)
        {
            for (int i = 0; i < 2*_kol; i++)
            {
                W[k][i].DrawWindow(g, sh, k, ll, _pl[k], p, f, w, h);
            }
        }
        public void DrawHouse(PictureBox pictureBox1, Lightning light, Point3D angle, bool[] sh, Point3D p, bool f)
        {
            var bmp = new Bitmap(pictureBox1.Image);
            Graphics g = Graphics.FromImage(bmp);
            var ll = new Lightning(light);
            ll.RotateLightning(_center, new Point3D(0, angle.X, angle.Z));
            double kof = (ll.Model[0].Z) / 400 * 0.5 + 1;
            ll.ScaleLightning(_center, kof);
            var pol = new PointF[5][];
            RotateHouse(_center, new Point3D(-15, angle.Y, angle.Z));
            for (int i = 0; i < 5; i++)
            {
                pol[i] = new PointF[4];
                for (int j = 0; j < 4; j++)
                {
                    pol[i][j].X = S[i].P[j].X;
                    pol[i][j].Y = S[i].P[j].Y;
                }
            }
            g.FillPolygon(new SolidBrush(System.Drawing.Color.FromArgb(0x44, 0x1C, 0x18)), pol[4]);
            Brush b;
            double yg = angle.X % 360;
            if (yg >= 0 && yg <= 90 || yg <= -270 && yg >= -360)
            {
                drawGr2(g, pol[0]);
                drawGr2(g, pol[1]);
                if (sh[0])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(0, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[0]);
                }
                if (sh[1])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(1, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[1]);
                }
                DrawWindow(g, 0, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
                DrawWindow(g, 1, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
            }
            if (yg >= 90 && yg <= 180 || yg <= -180 && yg >= -270)
            {
                drawGr2(g, pol[0]);
                drawGr2(g, pol[3]);
                if (sh[0])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(0, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[0]);
                }
                if (sh[3])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(3, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[3]);
                }
                DrawWindow(g, 0, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
                DrawWindow(g, 3, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
            }
            if (yg >= 180 && yg <= 270 || yg <= -90 && yg >= -180)
            {
                drawGr2(g, pol[2]);
                drawGr2(g, pol[3]);
                if (sh[2])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(2, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[2]);
                }
                if (sh[3])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(3, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[3]);
                }
                DrawWindow(g, 2, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
                DrawWindow(g, 3, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
            }
            if (yg >= 270 && yg <= 360 || yg <= 0 && yg >= -90)
            {
                drawGr2(g, pol[2]);
                drawGr2(g, pol[1]);
                if (sh[2])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(2, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[2]);
                }
                if (sh[1])
                {
                    b = new SolidBrush(System.Drawing.Color.FromArgb((int)(DrawGr(1, light) * 70), System.Drawing.Color.White));
                    g.FillPolygon(b, pol[1]);
                }
                DrawWindow(g, 2, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
                DrawWindow(g, 1, ll, sh, p, f, pictureBox1.Width, pictureBox1.Height);
            }
            g.DrawLine(new Pen(System.Drawing.Color.Brown), La[0].PointF, La[1].PointF);
            g.DrawLine(new Pen(System.Drawing.Color.Brown), La[0].PointF.X - 1, La[0].PointF.Y, La[1].PointF.X - 1, La[1].PointF.Y);

            pictureBox1.Image = bmp;
            RotateHouse(_center, new Point3D(15, 0, 0));
        }

        public readonly Side[] S;
        private readonly double[][] _pl;
        public readonly Point3D[] La;
        public Window[][] W;
        private readonly Point3D _center;
        private readonly int _kol;
    }
}