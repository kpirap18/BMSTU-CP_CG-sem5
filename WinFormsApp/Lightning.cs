using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace WinFormsApp
{
    class Lightning
    {
        public readonly List<Point3D> Model;
        public readonly List<List<Point3D>> SubModels;

        public Lightning(int h, int w, Point3D t)
        {

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

            Color c = Color.FromArgb(160, Color.White);
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
            Color c1 = Color.FromArgb(255, Color.White);
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
            path.AddEllipse(p.X - 200, p.Y - 100, 400, 200);
            var pthGrBrush = new PathGradientBrush(path) { CenterColor = Color.FromArgb(170, Color.White) };
            Color[] colors = { Color.FromArgb(0, Color.White) };
            pthGrBrush.SurroundColors = colors;
            gr.FillEllipse(pthGrBrush, p.X - 200, p.Y - 100, 400, 200);
        }
    }
}
