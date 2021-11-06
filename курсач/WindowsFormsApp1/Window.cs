using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    class Window
    {
        public Window(Point3D p)
        {
            Points = new Point3D[4];

            Points[0] = new Point3D(p.X, p.Y, p.Z);
            Points[1] = new Point3D(p.X + 40, p.Y, p.Z);
            Points[2] = new Point3D(p.X + 40, p.Y - 20, p.Z);
            Points[3] = new Point3D(p.X, p.Y - 20, p.Z);

           // System.Console.WriteLine("{0}, {1}, {2}, {3}", p.X, p.X + 40, p.Y, p.Y - 20);

            var r = new Random();
            Light = r.Next(0, 2);
        }

        public void TurnW(Point3D p, Point3D cen)
        {
            for (int i = 0; i < 4; i++)
            {
                Transform.TurnY(cen, Points[i], p.Y);
                Transform.TurnX(cen, Points[i], p.X);
            }
        }

        public void DrawWindow(Graphics gr, bool[] sh,
                               int k, Lightning lgh,
                               double[] no, Point3D p,
                               bool f, int w, int h)
        {
            if (Light == 1)
            {
                gr.FillPolygon(new SolidBrush(Color.Yellow), ToPolygon());

                gr.DrawLine(new Pen(Color.Black),
                            (Points[0].X + Points[1].X) / 2 - 6, (Points[0].Y + Points[1].Y) / 2,
                            (Points[2].X + Points[3].X) / 2 - 6, (Points[2].Y + Points[3].Y) / 2);

                gr.DrawLine(new Pen(Color.Black),
                            (Points[0].X + Points[1].X) / 2 + 6, (Points[0].Y + Points[1].Y) / 2,
                            (Points[2].X + Points[3].X) / 2 + 6, (Points[2].Y + Points[3].Y) / 2);


            }
            else
            {
                gr.FillPolygon(new SolidBrush(Color.Black), ToPolygon());

                if (sh[k])
                {
                    if (f)
                    {
                        lgh.turn_off_w++;
                        var points = RayTracer.Reflection(this, no, p, lgh);

                        if (points.Count > 1)
                        {
                            gr.DrawLines(new Pen(Color.White),
                                         points.ToArray());
                        }
                    }
                    else
                    {
                        var hl = new Highlight(lgh, this, w, h);
 
                        gr.DrawLine(new Pen(Color.White),
                                    hl.Edge[0].PointF,
                                    hl.Edge[1].PointF);
 
                        Lightning.Draworeol(hl.Edge[0].PointF,
                                            hl.Edge[1].PointF, gr);
                    }
                }
            }
        }

        private PointF[] ToPolygon()
        {
            var ps = new PointF[4];

            for (int i = 0; i < 4; i++)
            {
                ps[i] = Points[i].PointF;
            }

            return ps;
        }

        public readonly Point3D[] Points;
        public int Light;
    }
}
