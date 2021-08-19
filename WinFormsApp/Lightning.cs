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

        static public void Draworeol(PointF p1, Point3D p2, Graphics gr)
        {

            Color c = Color.FromArgb(160, Color.White);
            for (int i = 0; i < 10; i++)
            {
                c = Color.FromArgb(c.A * (9 - i) / 10, c);
                g.DrawLine(new Pen(c), p1.X + 1 + i, p1.Y, p2.X + 1 + i, p2.Y);
                g.DrawLine(new Pen(c), p1.X - 1 - i, p1.Y, p2.X - 1 - i, p2.Y);
            }
        }

        public void DrawLightning(PictureBox picBox)
        {

        }

        public void DrawArea(Graphics gr, Point3D p)
        {
            var path = new GraphicsPath();
            path.AddEllipse(p.X - 200, p.Y - 100, 400, 200);
            var pthGrBrush = new PathGradientBrush(path) { CenterColor = Color.FromArgb(170, Color.White) };
            Color[] colors = { Color.FromArgb(0, Color.White) };
            pthGrBrush.SurroundColors = colors;
            g.FillEllipse(pthGrBrush, p.X - 200, p.Y - 100, 400, 200);
        }
    }
}
