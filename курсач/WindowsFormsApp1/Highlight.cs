using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;


namespace WindowsFormsApp1
{
    class Highlight
    {
        public Highlight(Lightning lght, Window win, int w, int h)
        {
            Edge = new Point3D[2];

            float x1 = lght.Model[0].X / w;
            float x2 = lght.Model[lght.Model.Count - 1].X / w;
            float y = lght.Model[lght.Model.Count - 1].Y / h;

            if (x1 > 1)
                x1 = 1;

            if (x2 > 1)
                x2 = 1;

            if (y > 1)
                y = 1;

            Edge[1] = new Point3D(win.Points[3].X + x1 * (win.Points[2].X - win.Points[3].X),
                                  win.Points[3].Y - x1 * (win.Points[2].Y - win.Points[3].X),
                                  win.Points[3].Z);

            Edge[0] = new Point3D(win.Points[0].X + x2 * (win.Points[1].X - win.Points[0].X),
                                  win.Points[3].Y + y * (win.Points[0].Y - win.Points[3].Y) -
                                  x2 * (win.Points[3].Y - win.Points[2].Y), win.Points[0].Z);
        }
        public readonly Point3D[] Edge;
    }
}
