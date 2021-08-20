using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp
{
    struct Side
    {
        public Point3D[] p;
    }

    class House
    {
        public House(int w, int h, Point3D c, int angle, int count)
        {

        }

        private void WindowsCreate(Point3D c)
        {

        }

        public Trace TraceM(Ray ray)
        {

        }

        public int Color(Point3D at)
        {

        }

        private void TurnHouse(Point3D p, Point3D angle)
        {

        }

        public bool[] InShadow2(Point3D l)
        {

        }

        private double DrawGr(int k, Lightning light)
        {

        }

        private void drawGr2(Graphics g, PointF[] pol)
        {

        }

        private void DrawWindow(Graphics g, int k, Lightning ll, bool[] sh, Point3D p, bool f, int w, int h)
        {

        }

        public void DrawHouse(PictureBox pictureBox1, Lightning light, Point3D angle, bool[] sh, Point3D p, bool f)
        {

        }

        public readonly Side[] S;
        private readonly double[][] _pl;
        public readonly Point3D[] La;
        public Window[][] W;
        private readonly Point3D _center;
        private readonly int _kol;
    }
}
