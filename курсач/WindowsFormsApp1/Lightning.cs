using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Lightning
    {
        public readonly List<Point3D> Model;
        public readonly List<List<Point3D>> SubModels;

        public Lightning(int h, int w, Point3D t)
        {
            var subl = new List<Point3D>();
            var r = new Random();
            var rm = new List<int>();
            bool f = true;
            float x = r.Next(0, w);
            float y = 0;
            float z = r.Next(-100, 100);
            float xk = r.Next(0, w), yk = r.Next((h / 8 * 7), h), zk = r.Next(-100, 100);
            Model = new List<Point3D>();
            SubModels = new List<List<Point3D>>();
            Model.Add(new Point3D(x, y, z));
            float x2;
            float y2;
            float z2;
            float length = 0;

            var vec1 = new Point3D(x - t.X, y - t.Y, z - t.Z);
            var vec2 = new Point3D(xk - x, yk - y, zk - z);
            var vec3 = new Point3D
            {
                X = vec1.Y * vec2.Z - vec1.Z * vec2.Y,
                Y = vec1.X * vec2.Z - vec1.Z * vec2.X,
                Z = vec1.X * vec2.Y - vec1.Y * vec2.X
            };
            double ll = Math.Sqrt(vec3.X * vec3.X + vec3.Y * vec3.Y + vec3.Z * vec3.Z) /
                       Math.Sqrt(vec2.X * vec2.X + vec2.Y * vec2.Y + vec2.Z * vec2.Z);

            if (ll < 300)
            {
                xk = t.X;
                yk = t.Y;
                zk = t.Z;
                f = false;
            }

            float yy = yk;
            xk = (xk - x) / 100;
            yk = (yk - y) / 100;
            zk = (zk - z) / 100;

            float x1 = x2 = x;
            float y1 = y2 = y;
            float z1 = z2 = z;
            while (y < yy)
            {
                if (f)
                {
                    x1 = x + r.Next(-3, 4);
                    z1 = z + r.Next(-3, 4);
                    y1 = y + r.Next(1, 4);
                }
                else
                {
                    x1 = x + r.Next(-3, 4) + xk + x2 - x1;
                    z1 = z + r.Next(-3, 4) + zk + z2 - z1;
                    y1 = y + r.Next(1, 4) + yk + y2 - y1;
                    x2 += xk;
                    y2 += yk;
                    z2 += zk;
                }
                if (r.Next(100) == 1 && y / yy < 0.8)
                {
                    subl.Add(new Point3D(x, y, z));
                    rm.Add(r.Next(0, 360));
                }
                length += y1 - y;
                x = x1;
                y = y1;
                z = z1;
                Model.Add(new Point3D(x, y, z));
            }
            if (ll < 300)
            {
                Model[Model.Count - 1].Z = t.Z;
            }

            for (int i = 0; i < rm.Count; i++)
            {
                SubModels.Add(new List<Point3D>());
                float sublength = r.Next((int)length / 4 * (int)(h - subl[i].Y) / h, (int)length / 4 * 2 * (int)(h - subl[i].Y) / h);
                float l = 0;
                x = subl[i].X;
                y = subl[i].Y;
                z = subl[i].Z;
                SubModels[i].Add(new Point3D(x, y, z));
                while (l < sublength)
                {
                    x1 = x + r.Next(-3, 4);
                    y1 = y + r.Next(-3, 7);
                    z1 = z - r.Next(1, 4);
                    l += z - z1;
                    x = x1;
                    y = y1;
                    z = z1;
                    var p3D = new Point3D(x, y, z);
                    Transform.TurnY(subl[i], p3D, rm[i]);
                    SubModels[i].Add(p3D);
                }
            }
        }

        public Lightning(Lightning light)
        {
            SubModels = new List<List<Point3D>>();
            Model = new List<Point3D>();

            for (int i = 0; i < light.SubModels.Count; i++)
            {
                SubModels.Add(new List<Point3D>());
                for (int j = 0; j < light.SubModels[i].Count; j++)
                {
                    SubModels[i].Add(new Point3D(light.SubModels[i][j].X,
                                                 light.SubModels[i][j].Y,
                                                 light.SubModels[i][j].Z));
                }
            }

            for (int i = 0; i < light.Model.Count; i++)
            {
                Model.Add(new Point3D(light.Model[i].X,
                                      light.Model[i].Y,
                                      light.Model[i].Z));
            }
        }

        public Lightning(IEnumerable<Point3D> model,
                         IEnumerable<Point3D> submodel)
        {
            Model = model.Distinct(new Comparator()).ToList();
            SubModels = new List<List<Point3D>> { new List<Point3D>() };
            SubModels[0] = submodel.Distinct(new Comparator()).ToList();
        }

        public Trace TraceM(Ray ray)
        {
            var end_t = new Trace { Dist = -1, Per = new Point3D(0, 0, 0) };
            const double eps = 5;
            foreach (Point3D t4 in Model)
            {
                double t1 = (t4.X - ray.Beg.X) / ray.Vec.X;
                double y = ray.Beg.Y + ray.Vec.Y * t1;
                if (Math.Abs(t4.Y - y) <= eps)
                {
                    double z = ray.Beg.Z + ray.Vec.Z * t1;
                    if (Math.Abs(t4.Z - z) <= eps)
                    {
                        end_t.Per = new Point3D(t4.X, t4.Y, t4.Z);
                        end_t.Dist = Math.Sqrt(Math.Pow(t4.X - ray.Beg.X, 2) +
                                               Math.Pow(t4.Y - ray.Beg.Y, 2) +
                                               Math.Pow(t4.Z - ray.Beg.Z, 2));
                        end_t.I = 1;
                        return end_t;
                    }
                }
            }

            foreach (List<Point3D> t4 in SubModels)
            {
                foreach (Point3D t5 in t4)
                {
                    double t1 = (t5.X - ray.Beg.X) / ray.Vec.X;
                    double y = ray.Beg.Y + ray.Vec.Y * t1;
                    if (Math.Abs(t5.Y - y) <= eps)
                    {
                        double z = ray.Beg.Z + ray.Vec.Z * t1;
                        if (Math.Abs(t5.Z - z) <= eps)
                        {
                            end_t.Per = new Point3D(t5.X, t5.Y, t5.Z);
                            end_t.Dist = Math.Sqrt(Math.Pow(t5.X - ray.Beg.X, 2) +
                                               Math.Pow(t5.Y - ray.Beg.Y, 2) +
                                               Math.Pow(t5.Z - ray.Beg.Z, 2));
                            end_t.I = 2;
                            return end_t;
                        }
                    }
                }
            }

            return end_t;
        }

        public void TurnLightning(Point3D p, Point3D angle)
        {
            foreach (Point3D t in Model)
            {
                Transform.TurnX(p, t, angle.X);
                Transform.TurnY(p, t, angle.Y);
                Transform.TurnZ(p, t, angle.Z);
            }

            foreach (List<Point3D> t in SubModels)
            {
                foreach (Point3D t1 in t)
                {
                    Transform.TurnX(p, t1, angle.X);
                    Transform.TurnY(p, t1, angle.Y);
                    Transform.TurnZ(p, t1, angle.Z);
                }
            }
        }

        public void ScaleLightning(Point3D p, double k)
        {
            foreach (Point3D t in Model)
            {
                Transform.Scale(p, t, k);
            }

            foreach (List<Point3D> t in SubModels)
            {
                foreach (Point3D t1 in t)
                {
                    Transform.Scale(p, t1, k);
                }
            }
        }

        static public void Draworeol(PointF p1, PointF p2, Graphics gr)
        {

            Color c = Color.FromArgb(160, Color.Azure);
            for (int i = 0; i < 10; i++)
            {
                c = Color.FromArgb(c.A * (9 - i) / 10, c);
                gr.DrawLine(new Pen(c), p1.X + 1 + i, p1.Y, p2.X + 1 + i, p2.Y);
                gr.DrawLine(new Pen(c), p1.X - 1 - i, p1.Y, p2.X - 1 - i, p2.Y);
            }
        }

        public void DrawLightning(PictureBox picBox)
        {
            var l = new Lightning(this);
            const int compar = 20;
            var bmp = new Bitmap(picBox.Image);
            Graphics gr = Graphics.FromImage(bmp);
            Color c1 = Color.FromArgb(255, Color.Azure);
            var pen = new Pen(c1);

            for (int i = 1; i < Model.Count; i++)
            {
                if (Math.Abs(l.Model[i - 1].X - l.Model[i].X) < compar &&
                    Math.Abs(l.Model[i - 1].Y - l.Model[i].Y) < compar &&
                    Math.Abs(l.Model[i - 1].Z - l.Model[i].Z) < compar)
                {
                    gr.DrawLine(pen, l.Model[i - 1].X,
                                     l.Model[i - 1].Y,
                                     l.Model[i].X,
                                     l.Model[i].Y);

                    Draworeol(new PointF(l.Model[i - 1].X, l.Model[i - 1].Y),
                              new PointF(l.Model[i].X, l.Model[i].Y), gr);

                    gr.DrawLine(pen, l.Model[i - 1].X - 1,
                                     l.Model[i - 1].Y,
                                     l.Model[i].X - 1,
                                     l.Model[i].Y);

                    gr.DrawLine(pen, l.Model[i - 1].X + 1,
                                     l.Model[i - 1].Y,
                                     l.Model[i].X + 1,
                                     l.Model[i].Y);
                }
            }

            for (int i = 0; i < SubModels.Count; i++)
            {
                for (int j = 1; j < SubModels[i].Count; j++)
                {
                    if (Math.Abs(l.SubModels[i][j - 1].X - l.SubModels[i][j].X) < compar &&
                        Math.Abs(l.SubModels[i][j - 1].Y - l.SubModels[i][j].Y) < compar &&
                        Math.Abs(l.SubModels[i][j - 1].Z - l.SubModels[i][j].Z) < compar)
                    {
                        gr.DrawLine(pen, l.SubModels[i][j - 1].X,
                                         l.SubModels[i][j - 1].Y,
                                         l.SubModels[i][j].X,
                                         l.SubModels[i][j].Y);

                        Draworeol(new PointF(l.SubModels[i][j - 1].X, l.SubModels[i][j - 1].Y),
                                  new PointF(l.SubModels[i][j].X, l.SubModels[i][j].Y), gr);
                    }
                }
            }

            picBox.Image = bmp;
        }

        public void DrawArea(Graphics gr, Point3D p)
        {
            var path = new GraphicsPath();
            path.AddEllipse(p.X - 400, p.Y - 300, 800, 600);
            var pthGrBrush = new PathGradientBrush(path) { CenterColor = Color.FromArgb(170, Color.Azure) };
            Color[] colors = { Color.FromArgb(0, Color.Azure) };
            pthGrBrush.SurroundColors = colors;
            gr.FillEllipse(pthGrBrush, p.X - 400, p.Y - 300, 800, 600);
        }
    }
}
