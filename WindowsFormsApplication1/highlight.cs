using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    class Highlight
    {
        public Highlight(Lightning l, Window wi, int w, int h)
        {
            Edge = new Point3D[2];
            float x1 = l.Model[0].X / w, x2 = l.Model[l.Model.Count - 1].X / w, y = l.Model[l.Model.Count - 1].Y / h;
            if (x1 > 1)
                x1 = 1;
            if (x2 > 1)
                x2 = 1;
            if (y > 1)
                y = 1;
            Edge[1] = new Point3D(wi.Points[3].X + x1*(wi.Points[2].X - wi.Points[3].X),
                                  wi.Points[3].Y - x1*(wi.Points[3].Y - wi.Points[2].Y),
                                  wi.Points[3].Z);
            Edge[0] = new Point3D(wi.Points[0].X + x2*(wi.Points[1].X - wi.Points[0].X),
                                  wi.Points[3].Y + y*(wi.Points[0].Y - wi.Points[3].Y) -
                                  x2*(wi.Points[3].Y - wi.Points[2].Y),
                                  wi.Points[0].Z);
        }

        public readonly Point3D[] Edge;
    }
}
