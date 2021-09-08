using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows;


namespace WindowsFormsApp1
{
    struct Shape
    {
        public List<Point3D[]> Points;  // список массива точек, которые образуют прямоугольники
        public List<Color> Color;  // цвет прямоугольника из Elements,чтобы объект не был одного цвета (у сторон разные оттенки)
    }
    class Drawing
    {
        private ETL _etl;
        private Lightning _light;
        public Shape[] Sh;
        public Point3D[][] test1 = new Point3D[10][];
        int len_dort;
        int _ion;
        PointF[][] wire = new PointF[10][];
        public Drawing(Point3D center, ETL etl, Lightning light, PictureBox picture, int ion)
        {
            _etl = etl;
            _light = light;
            _ion = ion;
            
            Rewrite_ETL();
            Rewrite_Sub();
            
            Draw(picture);
            
        }

        // провода
        private int[] Knot(uint n, int c)
        {

            int max_value = (int)(n + c); // Максимальное значение вектора
            int first_max_index = (int)(n + 1); // Индекс x() для первого вхождения максимального значения вектора узла

            int[] x = new int[max_value];
            for (int i = 0; i < x.Length; i++)
                x[i] = 0;

            for (int i = 1; i < max_value; ++i)
            {
                if ((i + 1 > c) && (i < first_max_index)) x[i] = x[i - 1] + 1;
                else x[i] = x[i - 1];
            }

            return x;
        }

        private void RationalBasisFunctions(int c, double t, int npts, int[] x, double[] h, double[] r)
        {
            int nplusc = npts + c;

            double[] temp = new double[nplusc];
            for (int q = 0; q < temp.Length; q++)
                temp[q] = 0;

            // Вычислить нерациональные базисные функции первого порядка n[i]
            int i = 0;
            for (; i < nplusc - 1; ++i)
            { // (; i<nplusc; ++i)
                if ((t >= x[i]) && (t < x[i + 1])) temp[i] = 1;
                else temp[i] = 0;
            }

            // Вычислить нерациональные базисные функции высшего порядка
            double d = 0, e = 0;
            for (int k = 2; k <= c; ++k)
            {
                for (i = 0; i < nplusc - k; ++i)
                {

                    // Если базовая функция нижнего порядка равна нулю, пропустите расчет
                    if (temp[i] != 0) d = ((t - x[i]) * temp[i]) / (x[i + k - 1] - x[i]);
                    else d = 0;

                    // если базовая функция нижнего порядка равна нулю, пропустите расчет
                    if (temp[i + 1] != 0) e = ((x[i + k] - t) * temp[i + 1]) / (x[i + k] - x[i + 1]);
                    else e = 0;

                    temp[i] = d + e;
                }
            }

            // Подобрать последнюю точку
            if (t == Convert.ToDouble(x[nplusc - 1])) temp[npts - 1] = 1;

            double sum = 0;
            for (i = 0; i < npts; ++i) sum += temp[i] * h[i]; //???

            for (i = 0; i < npts; ++i)
            {
                if (sum != 0) r[i] = (temp[i] * h[i]) / sum;
                else r[i] = 0;
            }
        }

        /// <summary>
        /// возвращает кривую - кучу точек
        /// </summary>
        /// <param name="k"></param> порядок функции   лучше 2-3
        /// <param name="p1"></param> количество точек в кривой
        /// <param name="b"></param> точки, которые определяют кривую: начальная, конечная и те, которые тянут
        /// <param name="h"></param> веса определяющих точек 
        private Point3D[] RBSpline(int k, int p1, Point3D[] b, double[] h)
        {
            const double kEps = 5e-6;
            int npts = Convert.ToInt32(b.Length);
            int nplusc = npts + k - 1;

            var x = Knot((uint)npts, k); // Generate the uniform open knot vector

            //// Calculate the points on the rational B-spline curve
            double t = 0;
            double step = Convert.ToDouble(x[nplusc]) / Convert.ToDouble(p1 - 1);

            Point3D[] Curve = new Point3D[p1];
            for (int q = 0; q < p1; q++)
                Curve[q] = new Point3D();
            int i = 0, j = 0, icount = 0, jcount = 0;
            double temp = 0;

            double[] nbasis = new double[npts];
            for (int q = 0; q < nbasis.Length; q++)
                nbasis[q] = 0;
            for (int i1 = 0; i1 < p1; ++i1)
            {
                if (Convert.ToDouble(x[nplusc]) - t < kEps) t = Convert.ToDouble(x[nplusc]);

                RationalBasisFunctions(k, t, npts, x, h, nbasis);

                jcount = 0;//j;
                Curve[icount].X = 0;
                Curve[icount].Y = 0;
                Curve[icount].Z = 0;

                for (i = 0; i < npts; ++i)
                {
                    temp = nbasis[i] * b[jcount].X;
                    Curve[icount].X += (float)temp;
                    temp = nbasis[i] * b[jcount].Y;
                    Curve[icount].Y += (float)temp;
                    temp = nbasis[i] * b[jcount].Z;
                    Curve[icount].Z += (float)temp;

                    ++jcount;
                }

                ++icount;
                t += step;
            }

            return Curve;
        }

        private void Wires()
        {
            float step = (_etl.S[1].h_rope2[2].X - _etl.S[1].h_rope1[2].X) / 6;
            var dort1 = new Point3D[10][];
            dort1[0] = new Point3D[7] { _etl.S[1].h_rope1[2],
                new Point3D(_etl.S[1].h_rope1[2].X+step, _etl.S[1].h_rope1[2].Y+9, _etl.S[1].h_rope1[2].Z),
                new Point3D(_etl.S[1].h_rope1[2].X+step*2, _etl.S[1].h_rope1[2].Y+12, _etl.S[1].h_rope1[2].Z),
                new Point3D(_etl.S[1].h_rope1[2].X+step*3, _etl.S[1].h_rope1[2].Y+15, _etl.S[1].h_rope1[2].Z),
                new Point3D(_etl.S[1].h_rope1[2].X+step*4, _etl.S[1].h_rope1[2].Y+12, _etl.S[1].h_rope1[2].Z),
                new Point3D(_etl.S[1].h_rope1[2].X+step*5, _etl.S[1].h_rope1[2].Y+9, _etl.S[1].h_rope1[2].Z),
                _etl.S[1].h_rope2[2] };

            int[] tmp_d1 = { 0, 0, 0, 1, 1, 2, 2 };
            int[] tmp_d2 = { 0, 4, 5, 4, 5, 4, 5 };
            // 1 - передний; 2 - задний; 3 - передний; 4 - задний; 5 - передний; 6 - задний
            for (int i = 1; i < 5; i++)
            {
                dort1[i] = new Point3D[7] { new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*2, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*3, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+25, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*4, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*5, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                _etl.S[tmp_d1[i]].crossbars2[tmp_d2[i]]};
            }
            for (int i = 5; i < 7; i++)
            {
                dort1[i] = new Point3D[7] { new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*2, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*3, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+25, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*4, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*5, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, _etl.S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(_etl.S[tmp_d1[i]].crossbars2[tmp_d2[i]].X, _etl.S[tmp_d1[i]].crossbars2[tmp_d2[i]].Y, _etl.S[tmp_d1[i]].crossbars2[tmp_d2[i]].Z)};
            }

            
            dort1[7] = new Point3D[3] { new Point3D(_etl.S[0].crossbars2[5].X, _etl.S[0].crossbars2[5].Y, _etl.S[0].crossbars2[5].Z),
                new Point3D((_etl.S[0].crossbars2[5].X + 0.5f * _etl.Sub[1].building[0].X) / (1 + 0.5f),
                    (_etl.S[0].crossbars2[5].Y + 0.5f * _etl.Sub[1].building[0].Y) / (1 + 0.5f) + 30, 
                    (_etl.S[0].crossbars2[5].Z + 0.5f * _etl.Sub[1].building[0].Z) / (1 + 0.5f)),
                new Point3D(_etl.Sub[1].building[0].X, _etl.Sub[1].building[0].Y, _etl.Sub[1].building[0].Z)};

            dort1[8] = new Point3D[3] { new Point3D(_etl.S[1].crossbars2[5].X, _etl.S[1].crossbars2[5].Y, _etl.S[1].crossbars2[5].Z),
                new Point3D((_etl.S[1].crossbars2[5].X + 0.5f * _etl.Sub[1].building[0].X) / (1 + 0.5f),
                    (_etl.S[1].crossbars2[5].Y + 0.5f * _etl.Sub[1].building[0].Y+10) / (1 + 0.5f) + 30,
                    (_etl.S[1].crossbars2[5].Z + 0.5f * _etl.Sub[1].building[0].Z) / (1 + 0.5f)),
                new Point3D(_etl.Sub[1].building[0].X, _etl.Sub[1].building[0].Y+10, _etl.Sub[1].building[0].Z)};

            dort1[9] = new Point3D[3] { new Point3D(_etl.S[2].crossbars2[5].X, _etl.S[2].crossbars2[5].Y, _etl.S[2].crossbars2[5].Z),
                new Point3D((_etl.S[2].crossbars2[5].X + 0.5f * _etl.Sub[1].building[0].X) / (1 + 0.5f),
                    (_etl.S[2].crossbars2[5].Y + 0.5f * _etl.Sub[1].building[0].Y+20) / (1 + 0.5f) + 30,
                    (_etl.S[2].crossbars2[5].Z + 0.5f * _etl.Sub[1].building[0].Z) / (1 + 0.5f)),
                new Point3D(_etl.Sub[1].building[0].X, _etl.Sub[1].building[0].Y+20, _etl.Sub[1].building[0].Z)};

            len_dort = 60;
            double[] weight = new double[len_dort];
            for (int i = 0; i < len_dort; i++)
                weight[i] = 1;

            for (int i = 0; i < 10; i++)
                test1[i] = RBSpline(3, len_dort, dort1[i], weight);
        }

        private void Rewrite_Sub()
        {
            // дальние стержни
            for (int i = 2; i < 4; i++)
            {
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[0], _etl.Sub[i].basis[1], _etl.Sub[i].basis[3], _etl.Sub[i].basis[2] });  // 1
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[2], _etl.Sub[i].h_rods[2], _etl.Sub[i].h_rods[0], _etl.Sub[i].basis[0] });  // 2
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[3], _etl.Sub[i].h_rods[3], _etl.Sub[i].h_rods[2], _etl.Sub[i].basis[2] });  // 3
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[1], _etl.Sub[i].h_rods[1], _etl.Sub[i].h_rods[3], _etl.Sub[i].basis[3] });  // 4
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[0], _etl.Sub[i].h_rods[0], _etl.Sub[i].h_rods[1], _etl.Sub[i].basis[1] });  // 5
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].h_rods[0], _etl.Sub[i].h_rods[1], _etl.Sub[i].h_rods[3], _etl.Sub[i].h_rods[2] });  // 6

                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
            }  // Point.Count = 12

            // здания
            // трансформатор
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[0].building[0], _etl.Sub[0].building[1], _etl.Sub[0].building[3], _etl.Sub[0].building[2] });  // 1 низ
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[0].building[2], _etl.Sub[1].building[2], _etl.Sub[1].building[3], _etl.Sub[0].building[3] });  // 3 зад
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[0].building[1], _etl.Sub[1].building[1], _etl.Sub[1].building[3], _etl.Sub[0].building[3] });  // 4 право
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[0].building[0], _etl.Sub[1].building[0], _etl.Sub[1].building[2], _etl.Sub[0].building[2] });  // 2 лево
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[0].building[0], _etl.Sub[1].building[0], _etl.Sub[1].building[1], _etl.Sub[0].building[1] });  // 5 перед
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[1].building[0], _etl.Sub[1].building[1], _etl.Sub[1].building[3], _etl.Sub[1].building[2] });  // 6 верх
            // Point.Count = 18
           

            Sh[1].Color.Add(Color.FromArgb(76, 97, 103));  // низ    -
            Sh[1].Color.Add(Color.FromArgb(76, 97, 103));  // зад    -
            Sh[1].Color.Add(Color.FromArgb(76, 97, 103));  // право  -

            if (_ion != 0)  // молния есть
            {
                if (_light.yk >= _etl.Sub[2].h_rods[2].Y - 5 && _light.xk >= _etl.S[1].basis2[1].X - 2)  // область подстанции
                {
                    if (_light.xk >= _etl.Sub[2].h_rods[2].X - 5 && _light.xk <= _etl.Sub[0].h_rods[1].X + 5
                        && _light.yk <= _etl.Sub[0].h_rods[0].Y + 5)  // ударяет в 0й или 2й стержень
                    {
                        if (_light.yk <= _etl.Sub[2].h_rods[0].Y + 5)  // ударяет во 2й стержень
                        {
                            // подсветить трансформатор слева и спереди остается
                            Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // лево  // подсветить
                            Sh[1].Color.Add(Color.FromArgb(73, 75, 74));// перед  // остается
                        }
                        else // ударяет в 0й стержень
                        {
                            // подсветить трансформатор слева и спереди  // РУ потом чуть меньше спереди
                            Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // лево  // подсветить
                            Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // перед  // подсветить
                        }
                    }
                    else if (_light.xk >= _etl.Sub[1].h_rods[0].X - 5
                        && _light.yk <= _etl.Sub[1].h_rods[0].Y + 5)  // ударяет в 1й стержень 
                    {
                        // подсвететь трансформатор спереди немного  // РУ спереди
                        Sh[1].Color.Add(Color.FromArgb(73, 75, 74));  // лево  // остается
                        Sh[1].Color.Add(Color.FromArgb(80, 80, 80));  // перед  // чуть светлее
                    }
                    else if (_light.yk > _etl.Sub[0].h_rods[0].Y)  // ударяет перед подстанцией
                    {
                        Sh[1].Color.Add(Color.FromArgb(73, 75, 74));  // лево  // остается
                        if (_light.xk < _etl.Sub[2].building[0].X)
                            Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // перед  // чуть светлее
                        else if (_light.xk > _etl.Sub[2].building[0].X)
                            Sh[1].Color.Add(Color.FromArgb(80, 80, 80));  // перед  // чуть светлее
                        else
                            Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // перед  // чуть светлее
                    }
                    else
                    {
                        Sh[1].Color.Add(Color.FromArgb(73, 75, 74));  // лево
                        Sh[1].Color.Add(Color.FromArgb(73, 75, 74));// перед
                    }
                }
                else  // не область подстанции
                {
                    Sh[1].Color.Add(Color.FromArgb(73, 75, 74));  // лево
                    Sh[1].Color.Add(Color.FromArgb(73, 75, 74));// перед
                }
            }

            else
            {
                Sh[1].Color.Add(Color.FromArgb(73, 75, 74));  // лево
                Sh[1].Color.Add(Color.FromArgb(73, 75, 74));// перед
            }

            Sh[1].Color.Add(Color.FromArgb(60, 60, 60));  // верх   ?


            // распределительное устройство
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[2].building[0], _etl.Sub[2].building[1], _etl.Sub[2].building[3], _etl.Sub[2].building[2] });  // 1 низ
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[2].building[2], _etl.Sub[3].building[2], _etl.Sub[3].building[3], _etl.Sub[2].building[3] });  // 3 зад
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[2].building[0], _etl.Sub[3].building[0], _etl.Sub[3].building[2], _etl.Sub[2].building[2] });  // 4 право
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[2].building[1], _etl.Sub[3].building[1], _etl.Sub[3].building[3], _etl.Sub[2].building[3] });  // 2 лево
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[2].building[0], _etl.Sub[3].building[0], _etl.Sub[3].building[1], _etl.Sub[2].building[1] });  // 5 перед
            Sh[1].Points.Add(new Point3D[] { _etl.Sub[3].building[0], _etl.Sub[3].building[1], _etl.Sub[3].building[3], _etl.Sub[3].building[2] });  // 6 верх
            // Point.Count = 24

            Sh[1].Color.Add(Color.FromArgb(76, 97, 103));  // низ    -
            Sh[1].Color.Add(Color.FromArgb(76, 97, 103));  // зад    -
            Sh[1].Color.Add(Color.FromArgb(76, 97, 103));  // право  -
            Sh[1].Color.Add(Color.FromArgb(55, 55, 55));  // лево    +

            if (_ion != 0)  // молния есть
            {
                if (_light.yk >= _etl.Sub[0].h_rods[2].Y - 5 && _light.xk >= _etl.S[1].basis2[1].X - 2)  // область подстанции (от 0го стержня вниз)
                {
                    if (_light.xk <= _etl.Sub[0].h_rods[1].X + 5
                        && _light.yk <= _etl.Sub[0].h_rods[0].Y + 5)  // ударяет в 0й стержень
                    {
                        // подсветить трансформатор слева и спереди  // РУ потом чуть меньше спереди
                        Sh[1].Color.Add(Color.FromArgb(80, 80, 80));  // перед  // чуть подсветить
                    }
                    else if (_light.xk >= _etl.Sub[1].h_rods[0].X - 5
                        && _light.yk <= _etl.Sub[1].h_rods[0].Y + 5)  // ударяет в 1й стержень 
                    {
                        // подсвететь трансформатор спереди немного  // РУ спереди
                        Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // перед  // чуть светлее
                    }
                    else  // ударяет перед подстанцией
                    {
                        if (_light.xk < _etl.Sub[2].building[0].X)
                            Sh[1].Color.Add(Color.FromArgb(80, 80, 80));  // перед  // чуть светлее
                        else if (_light.xk > _etl.Sub[2].building[0].X)
                            Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // перед  // чуть светлее
                        else
                            Sh[1].Color.Add(Color.FromArgb(90, 90, 90));  // перед  // чуть светлее
                    }
                }
                else
                    Sh[1].Color.Add(Color.FromArgb(73, 75, 74));// перед
            }

            else
                Sh[1].Color.Add(Color.FromArgb(73, 75, 74));// перед
            
            Sh[1].Color.Add(Color.FromArgb(60, 60, 60));  // верх   +

            // ближние стержни
            for (int i = 0; i < 2; i++)
            {
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[0], _etl.Sub[i].basis[1], _etl.Sub[i].basis[3], _etl.Sub[i].basis[2] });  // 1
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[2], _etl.Sub[i].h_rods[2], _etl.Sub[i].h_rods[0], _etl.Sub[i].basis[0] });  // 2
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[3], _etl.Sub[i].h_rods[3], _etl.Sub[i].h_rods[2], _etl.Sub[i].basis[2] });  // 3
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[1], _etl.Sub[i].h_rods[1], _etl.Sub[i].h_rods[3], _etl.Sub[i].basis[3] });  // 4
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].basis[0], _etl.Sub[i].h_rods[0], _etl.Sub[i].h_rods[1], _etl.Sub[i].basis[1] });  // 5
                Sh[1].Points.Add(new Point3D[] { _etl.Sub[i].h_rods[0], _etl.Sub[i].h_rods[1], _etl.Sub[i].h_rods[3], _etl.Sub[i].h_rods[2] });  // 6

                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
                Sh[1].Color.Add(Color.FromArgb(76, 93, 98));
            }  // Point.Count = 36

            
            // ПРОВОДА
            for (int i = 0; i < 10; i++)
                wire[i] = new PointF[len_dort * 2 + 1];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < len_dort; j++)
                {
                    wire[i][j].X = Sh[0].Points[i][j].X;
                    wire[i][j].Y = Sh[0].Points[i][j].Y;
                }
                int w = len_dort;
                for (int j = len_dort - 1; j >= 0; j--)
                {
                    wire[i][w].X = Sh[0].Points[i][j].X;
                    wire[i][w].Y = Sh[0].Points[i][j].Y + 3;
                    w++;
                }
                wire[i][w].X = Sh[0].Points[i][0].X;
                wire[i][w].Y = Sh[0].Points[i][0].Y;
            }

            int k = 3;
            for (int i = 183; i < 187; i++)  // 183 // 187
            {
                for (int j = 0; j < len_dort; j++)
                {
                    wire[k][j].X = Sh[0].Points[i][j].X;
                    wire[k][j].Y = Sh[0].Points[i][j].Y;
                }
                int w = len_dort;
                for (int j = len_dort - 1; j >= 0; j--)
                {
                    wire[k][w].X = Sh[0].Points[i][j].X;
                    wire[k][w].Y = Sh[0].Points[i][j].Y + 3;
                    w++;
                }
                wire[k][w].X = Sh[0].Points[i][0].X;
                wire[k][w].Y = Sh[0].Points[i][0].Y;
                k++;
            }

            // еще провода
            for (int i = 7; i < 10; i++)
            {
                for (int j = 0; j < len_dort; j++)
                {
                    wire[i][j].X = test1[i][j].X;
                    wire[i][j].Y = test1[i][j].Y;
                }
                int w = len_dort;
                for (int j = len_dort - 1; j >= 0; j--)
                {
                    wire[i][w].X = test1[i][j].X;
                    wire[i][w].Y = test1[i][j].Y + 3;
                    w++;
                }
                wire[i][w].X = test1[i][0].X;
                wire[i][w].Y = test1[i][0].Y;
            }
        }

        private void Rewrite_ETL()
        {
            Sh = new Shape[3];

            for (int i = 0; i < 3; i++)
            {
                Sh[i].Points = new List<Point3D[]>();
                Sh[i].Color = new List<Color>();
            }

            // провода на заднем плане          +   +
            // опоры 2 3                        +   +
            // пролеты 0-2, 2-3, 3-1            +   +
            // опоры 0 1                        +   +
            // пролеты 1-0                      +   +
            // переднюю половину перекладин     +   +
            // провода на переднем плане        +   +
            // молниезащита                     +   +

            // вычисление проводов
            Wires();
            // провода заднего плана
            // задний - 2, 4, 6
            for (int i = 2; i < 7; i += 2)
            {
                Sh[0].Points.Add(test1[i]);
                Sh[0].Color.Add(Color.FromArgb(60, 60, 60));
            } // Points.Count = 3
            ///////////////////////////////////////////////////
            ///
            // передняя половина перекладин
            // crossbar1
            // вычисление половины перекладин

            Point3D[] tmpD = new Point3D[12];
            for (int i = 0; i < 12; i++)
                tmpD[i] = new Point3D();

            int q = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j += 2)
                {
                    tmpD[q].X = _etl.S[i].crossbars1[j + 1].X;
                    tmpD[q].Y = _etl.S[i].crossbars1[j + 1].Y;
                    tmpD[q].Z = _etl.S[i].crossbars1[j + 1].Z;

                    _etl.S[i].crossbars1[j + 1].X = (_etl.S[i].crossbars1[j].X + 0.5f * _etl.S[i].crossbars1[j + 1].X) / (1 + 0.5f);
                    _etl.S[i].crossbars1[j + 1].Y = (_etl.S[i].crossbars1[j].Y + 0.5f * _etl.S[i].crossbars1[j + 1].Y) / (1 + 0.5f);
                    _etl.S[i].crossbars1[j + 1].Z = (_etl.S[i].crossbars1[j].Z + 0.5f * _etl.S[i].crossbars1[j + 1].Z) / (1 + 0.5f);

                    q++;
                }
            }
            q = 0;
            for (int i = 0; i < 3; i++)
            {
                Sh[0].Points.Add(new Point3D[] { tmpD[3 + q], tmpD[2 + q], tmpD[1 + q], tmpD[0 + q] });  // зад
                Sh[0].Points.Add(new Point3D[] { tmpD[3 + q], _etl.S[i].crossbars1[6], _etl.S[i].crossbars1[0], tmpD[0 + q] });  // низ
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[0], tmpD[0 + q], tmpD[1 + q], _etl.S[i].crossbars1[2] });  // 3
                Sh[0].Points.Add(new Point3D[] { tmpD[1 + q], _etl.S[i].crossbars1[2], _etl.S[i].crossbars1[4], tmpD[2 + q] });  // 4
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[4], tmpD[2 + q], tmpD[3 + q], _etl.S[i].crossbars1[6] });  // 5
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[6], _etl.S[i].crossbars1[4], _etl.S[i].crossbars1[2], _etl.S[i].crossbars1[0] });  // 6

                q += 4;

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Points.Count = 135
            /////////////////////////////////////////////////////////////////////////////////////////////

            Point3D[] tmpD2 = new Point3D[12];
            for (int i = 0; i < 12; i++)
                tmpD2[i] = new Point3D();

            q = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j += 2)
                {
                    tmpD2[q].X = _etl.S[i].crossbars2[j + 1].X;
                    tmpD2[q].Y = _etl.S[i].crossbars2[j + 1].Y;
                    tmpD2[q].Z = _etl.S[i].crossbars2[j + 1].Z;

                    _etl.S[i].crossbars2[j + 1].X = (_etl.S[i].crossbars2[j].X + 0.5f * _etl.S[i].crossbars2[j + 1].X) / (1 + 0.5f);
                    _etl.S[i].crossbars2[j + 1].Y = (_etl.S[i].crossbars2[j].Y + 0.5f * _etl.S[i].crossbars2[j + 1].Y) / (1 + 0.5f);
                    _etl.S[i].crossbars2[j + 1].Z = (_etl.S[i].crossbars2[j].Z + 0.5f * _etl.S[i].crossbars2[j + 1].Z) / (1 + 0.5f);

                    q++;
                }
            }
            q = 0;
            for (int i = 0; i < 3; i++)
            {
                Sh[0].Points.Add(new Point3D[] { tmpD2[3 + q], tmpD2[2 + q], tmpD2[1 + q], tmpD2[0 + q] });  // зад
                Sh[0].Points.Add(new Point3D[] { tmpD2[3 + q], _etl.S[i].crossbars2[6], _etl.S[i].crossbars2[0], tmpD2[0 + q] });  // низ
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[0], tmpD2[0 + q], tmpD2[1 + q], _etl.S[i].crossbars2[2] });  // 3
                Sh[0].Points.Add(new Point3D[] { tmpD2[1 + q], _etl.S[i].crossbars2[2], _etl.S[i].crossbars2[4], tmpD2[2 + q] });  // 4
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[4], tmpD2[2 + q], tmpD2[3 + q], _etl.S[i].crossbars2[6] });  // 5
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[6], _etl.S[i].crossbars2[4], _etl.S[i].crossbars2[2], _etl.S[i].crossbars2[0] });  // 6

                q += 4;

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Points.Count = 135
            /////////////////////////////////////////////////////////////////////////////////////////////

            // многоугольники опор
            // basis и h_sup
            // опоры 2 3

            for (int i = 2; i < 4; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[0], _etl.S[i].basis[1], _etl.S[i].basis[3], _etl.S[i].basis[2] });  // 1  // низ
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[2], _etl.S[i].h_sup[2], _etl.S[i].h_sup[0], _etl.S[i].basis[0] });  // 2  // лево
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[3], _etl.S[i].h_sup[3], _etl.S[i].h_sup[2], _etl.S[i].basis[2] });  // 3  // зад
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[1], _etl.S[i].h_sup[1], _etl.S[i].h_sup[3], _etl.S[i].basis[3] });  // 4  // право
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[0], _etl.S[i].h_sup[0], _etl.S[i].h_sup[1], _etl.S[i].basis[1] });  // 5  // перед
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].h_sup[0], _etl.S[i].h_sup[1], _etl.S[i].h_sup[3], _etl.S[i].h_sup[2] });  // 6  // верх

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Poinst.Count = 15

            // многоугольники опор
            // basis2 и h_sup2
            // опоры 2 3
            for (int i = 2; i < 4; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[0], _etl.S[i].basis2[1], _etl.S[i].basis2[3], _etl.S[i].basis2[2] });  // 1
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[2], _etl.S[i].h_sup2[2], _etl.S[i].h_sup2[0], _etl.S[i].basis2[0] });  // 2
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[3], _etl.S[i].h_sup2[3], _etl.S[i].h_sup2[2], _etl.S[i].basis2[2] });  // 3
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[1], _etl.S[i].h_sup2[1], _etl.S[i].h_sup2[3], _etl.S[i].basis2[3] });  // 4
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[0], _etl.S[i].h_sup2[0], _etl.S[i].h_sup2[1], _etl.S[i].basis2[1] });  // 5
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].h_sup2[0], _etl.S[i].h_sup2[1], _etl.S[i].h_sup2[3], _etl.S[i].h_sup2[2] });  // 6

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Poinst.Count = 27
            ///////////////////////////////////////////////////


            // перекладины между опорами 0-2, 2-3, 3-1
            // многоугольники пролетов
            // support и support1
            int[] tmp = { 0, 2, 3, 1, 0 };
            for (int i = 0; i < 3; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support[0], _etl.S[tmp[i]].support1[0], _etl.S[tmp[i + 1]].support1[1], _etl.S[tmp[i + 1]].support[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support1[1], _etl.S[tmp[i + 1]].support[1], _etl.S[tmp[i]].support[2], _etl.S[tmp[i]].support1[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support[2], _etl.S[tmp[i]].support1[2], _etl.S[tmp[i + 1]].support1[3], _etl.S[tmp[i + 1]].support[3] });

                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support[0], _etl.S[tmp[i + 1]].support1[0], _etl.S[tmp[i]].support1[1], _etl.S[tmp[i]].support[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support1[1], _etl.S[tmp[i]].support[1], _etl.S[tmp[i + 1]].support[2], _etl.S[tmp[i + 1]].support1[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support[2], _etl.S[tmp[i + 1]].support1[2], _etl.S[tmp[i]].support1[3], _etl.S[tmp[i]].support[3] });

                for (int w = 0; w < 6; w++)
                    Sh[0].Color.Add(Color.FromArgb(60, 60, 60));
            }  // Poinst.Count = 45

            // перекладины между опорами 0-2, 2-3, 3-1
            // многоугольники пролетов
            // support2 и support12
            for (int i = 0; i < 3; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support2[0], _etl.S[tmp[i]].support12[0], _etl.S[tmp[i + 1]].support12[1], _etl.S[tmp[i + 1]].support2[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support12[1], _etl.S[tmp[i + 1]].support2[1], _etl.S[tmp[i]].support2[2], _etl.S[tmp[i]].support12[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support2[2], _etl.S[tmp[i]].support12[2], _etl.S[tmp[i + 1]].support12[3], _etl.S[tmp[i + 1]].support2[3] });

                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support2[0], _etl.S[tmp[i + 1]].support12[0], _etl.S[tmp[i]].support12[1], _etl.S[tmp[i]].support2[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support12[1], _etl.S[tmp[i]].support2[1], _etl.S[tmp[i + 1]].support2[2], _etl.S[tmp[i + 1]].support12[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support2[2], _etl.S[tmp[i + 1]].support12[2], _etl.S[tmp[i]].support12[3], _etl.S[tmp[i]].support2[3] });

                for (int w = 0; w < 6; w++)
                    Sh[0].Color.Add(Color.FromArgb(60, 60, 60));
            }  // Poinst.Count = 63
               ///////////////////////////////////////////////////


            // многоугольники опор
            // basis и h_sup
            // опоры 0 1
            for (int i = 0; i < 2; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[0], _etl.S[i].basis[1], _etl.S[i].basis[3], _etl.S[i].basis[2] });  // 1
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[2], _etl.S[i].h_sup[2], _etl.S[i].h_sup[0], _etl.S[i].basis[0] });  // 2
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[3], _etl.S[i].h_sup[3], _etl.S[i].h_sup[2], _etl.S[i].basis[2] });  // 3
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[1], _etl.S[i].h_sup[1], _etl.S[i].h_sup[3], _etl.S[i].basis[3] });  // 4
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis[0], _etl.S[i].h_sup[0], _etl.S[i].h_sup[1], _etl.S[i].basis[1] });  // 5
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].h_sup[0], _etl.S[i].h_sup[1], _etl.S[i].h_sup[3], _etl.S[i].h_sup[2] });  // 6

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Points.Count = 75

            // многоугольники опор
            // basis2 и h_sup2
            // опоры 0 1
            for (int i = 0; i < 2; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[0], _etl.S[i].basis2[1], _etl.S[i].basis2[3], _etl.S[i].basis2[2] });  // 1
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[2], _etl.S[i].h_sup2[2], _etl.S[i].h_sup2[0], _etl.S[i].basis2[0] });  // 2
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[3], _etl.S[i].h_sup2[3], _etl.S[i].h_sup2[2], _etl.S[i].basis2[2] });  // 3
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[1], _etl.S[i].h_sup2[1], _etl.S[i].h_sup2[3], _etl.S[i].basis2[3] });  // 4
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].basis2[0], _etl.S[i].h_sup2[0], _etl.S[i].h_sup2[1], _etl.S[i].basis2[1] });  // 5
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].h_sup2[0], _etl.S[i].h_sup2[1], _etl.S[i].h_sup2[3], _etl.S[i].h_sup2[2] });  // 6

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Poinst.Count = 87
               ///////////////////////////////////////////////////


            // перекладины между опорами 1-0
            // многоугольники пролетов
            // support и support1
            for (int i = 3; i < 4; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support[0], _etl.S[tmp[i]].support1[0], _etl.S[tmp[i + 1]].support1[1], _etl.S[tmp[i + 1]].support[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support1[1], _etl.S[tmp[i + 1]].support[1], _etl.S[tmp[i]].support[2], _etl.S[tmp[i]].support1[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support[2], _etl.S[tmp[i]].support1[2], _etl.S[tmp[i + 1]].support1[3], _etl.S[tmp[i + 1]].support[3] });

                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support[0], _etl.S[tmp[i + 1]].support1[0], _etl.S[tmp[i]].support1[1], _etl.S[tmp[i]].support[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support1[1], _etl.S[tmp[i]].support[1], _etl.S[tmp[i + 1]].support[2], _etl.S[tmp[i + 1]].support1[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support[2], _etl.S[tmp[i + 1]].support1[2], _etl.S[tmp[i]].support1[3], _etl.S[tmp[i]].support[3] });

                for (int w = 0; w < 6; w++)
                    Sh[0].Color.Add(Color.FromArgb(60, 60, 60));
            }  // Poinst.Count = 93

            // перекладины между опорами 1-0
            // многоугольники пролетов
            // support2 и support12
            for (int i = 3; i < 4; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support2[0], _etl.S[tmp[i]].support12[0], _etl.S[tmp[i + 1]].support12[1], _etl.S[tmp[i + 1]].support2[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support12[1], _etl.S[tmp[i + 1]].support2[1], _etl.S[tmp[i]].support2[2], _etl.S[tmp[i]].support12[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support2[2], _etl.S[tmp[i]].support12[2], _etl.S[tmp[i + 1]].support12[3], _etl.S[tmp[i + 1]].support2[3] });

                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support2[0], _etl.S[tmp[i + 1]].support12[0], _etl.S[tmp[i]].support12[1], _etl.S[tmp[i]].support2[1] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i]].support12[1], _etl.S[tmp[i]].support2[1], _etl.S[tmp[i + 1]].support2[2], _etl.S[tmp[i + 1]].support12[2] });
                Sh[0].Points.Add(new Point3D[] { _etl.S[tmp[i + 1]].support2[2], _etl.S[tmp[i + 1]].support12[2], _etl.S[tmp[i]].support12[3], _etl.S[tmp[i]].support2[3] });

                for (int w = 0; w < 6; w++)
                    Sh[0].Color.Add(Color.FromArgb(60, 60, 60));
            }  // Poinst.Count = 99
               ///////////////////////////////////////////////////


            // передняя половина перекладин
            // crossbar1
            // вычисление половины перекладин
            
            for (int i = 0; i < 3; i++)
            {

                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[7], _etl.S[i].crossbars1[5], _etl.S[i].crossbars1[3], _etl.S[i].crossbars1[1] });  // 1
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[7], _etl.S[i].crossbars1[6], _etl.S[i].crossbars1[0], _etl.S[i].crossbars1[1] });  // 2
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[0], _etl.S[i].crossbars1[1], _etl.S[i].crossbars1[3], _etl.S[i].crossbars1[2] });  // 3
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[3], _etl.S[i].crossbars1[2], _etl.S[i].crossbars1[4], _etl.S[i].crossbars1[5] });  // 4
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[4], _etl.S[i].crossbars1[5], _etl.S[i].crossbars1[7], _etl.S[i].crossbars1[6] });  // 5
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars1[6], _etl.S[i].crossbars1[4], _etl.S[i].crossbars1[2], _etl.S[i].crossbars1[0] });  // 6
                

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Points.Count = 135

            // crossbar2
            // вычисление половины перекладин
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j += 2)
                {
                    _etl.S[i].crossbars2[j + 1].X = (_etl.S[i].crossbars2[j].X + 0.5f * _etl.S[i].crossbars2[j + 1].X) / (1 + 0.5f);
                    _etl.S[i].crossbars2[j + 1].Y = (_etl.S[i].crossbars2[j].Y + 0.5f * _etl.S[i].crossbars2[j + 1].Y) / (1 + 0.5f);
                    _etl.S[i].crossbars2[j + 1].Z = (_etl.S[i].crossbars2[j].Z + 0.5f * _etl.S[i].crossbars2[j + 1].Z) / (1 + 0.5f);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[7], _etl.S[i].crossbars2[5], _etl.S[i].crossbars2[3], _etl.S[i].crossbars2[1] });  // 1
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[7], _etl.S[i].crossbars2[6], _etl.S[i].crossbars2[0], _etl.S[i].crossbars2[1] });  // 2
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[0], _etl.S[i].crossbars2[1], _etl.S[i].crossbars2[3], _etl.S[i].crossbars2[2] });  // 3
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[3], _etl.S[i].crossbars2[2], _etl.S[i].crossbars2[4], _etl.S[i].crossbars2[5] });  // 4
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[4], _etl.S[i].crossbars2[5], _etl.S[i].crossbars2[7], _etl.S[i].crossbars2[6] });  // 5
                Sh[0].Points.Add(new Point3D[] { _etl.S[i].crossbars2[6], _etl.S[i].crossbars2[4], _etl.S[i].crossbars2[2], _etl.S[i].crossbars2[0] });  // 6

                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
                Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
                Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
                Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            }  // Points.Count = 153
            ///////////////////////////////////////////////////

            // молниезащита
            // h_rope1 h_rope2
            // 1я опора
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope1[0], _etl.S[0].h_rope1[1], _etl.S[0].h_rope1[3], _etl.S[0].h_rope1[2] });  // 1
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope1[2], _etl.S[1].h_rope1[2], _etl.S[1].h_rope1[0], _etl.S[0].h_rope1[0] });  // 2
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope1[3], _etl.S[1].h_rope1[3], _etl.S[1].h_rope1[2], _etl.S[0].h_rope1[2] });  // 3
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope1[1], _etl.S[1].h_rope1[1], _etl.S[1].h_rope1[3], _etl.S[0].h_rope1[3] });  // 4
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope1[0], _etl.S[1].h_rope1[0], _etl.S[1].h_rope1[1], _etl.S[0].h_rope1[1] });  // 5
            Sh[0].Points.Add(new Point3D[] { _etl.S[1].h_rope1[0], _etl.S[1].h_rope1[1], _etl.S[1].h_rope1[3], _etl.S[1].h_rope1[2] });  // 6

            Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
            Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
            Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
            Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
            Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
            Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх

            // 2я опора
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope2[0], _etl.S[0].h_rope2[1], _etl.S[0].h_rope2[3], _etl.S[0].h_rope2[2] });  // 1
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope2[2], _etl.S[1].h_rope2[2], _etl.S[1].h_rope2[0], _etl.S[0].h_rope2[0] });  // 2
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope2[3], _etl.S[1].h_rope2[3], _etl.S[1].h_rope2[2], _etl.S[0].h_rope2[2] });  // 3
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope2[1], _etl.S[1].h_rope2[1], _etl.S[1].h_rope2[3], _etl.S[0].h_rope2[3] });  // 4
            Sh[0].Points.Add(new Point3D[] { _etl.S[0].h_rope2[0], _etl.S[1].h_rope2[0], _etl.S[1].h_rope2[1], _etl.S[0].h_rope2[1] });  // 5
            Sh[0].Points.Add(new Point3D[] { _etl.S[1].h_rope2[0], _etl.S[1].h_rope2[1], _etl.S[1].h_rope2[3], _etl.S[1].h_rope2[2] });  // 6

            Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // низ
            Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // лево
            Sh[0].Color.Add(Color.FromArgb(76, 97, 103));  // зад
            Sh[0].Color.Add(Color.FromArgb(76, 89, 99));  // право
            Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // перед
            Sh[0].Color.Add(Color.FromArgb(73, 75, 74));  // верх
            // Points.Count = 183  // 165

            // провода переднего плана и тросс
            // нужно отдельно их отрисовать, т.к. это линии, а не прямоугольники
            // задний - 0 и 1, 3, 5
            Sh[0].Points.Add(test1[0]);
            for (int i = 1; i < 6; i += 2)
            {
                Sh[0].Points.Add(test1[i]);
                Sh[0].Color.Add(Color.FromArgb(70, 70, 70));
            }
            // Points.Count = 187  // 169
        }
        
        // сортирует объекты
        private void Sort(float[] mass)
        {
            mass[0] = _etl.S[1].h_rope1[0].Z;  // координата молниезащиты ЛЭП
            mass[1] = _etl.Sub[0].building[1].Z; // координата подстанции
            mass[2] = _light.zk + 1;  // координата молнии

            for (int i = 0; i < 3; i++)
            {
                float tmp = mass[i];
                int j = i - 1;
                for (; j >= 0 && tmp < mass[j]; --j)
                    mass[j + 1] = mass[j];
                mass[j + 1] = tmp;
            }
        }

        // отрисовка ЛЭП
        private void Draw_ETL(Graphics g)
        {
            for (int i = 0; i < 4; i++)
            {
                SolidBrush Brush = new SolidBrush(Sh[0].Color[0]);
                g.FillPolygon(Brush, wire[i]);
            }

            // ЛЭП   [3] - [182]
            var etl = new PointF[180][];
            for (int i = 0; i < 180; i++)
                etl[i] = new PointF[5];

            int k = 0;
            for (int i = 3; i < 183; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    etl[k][j].X = Sh[0].Points[i][j].X;
                    etl[k][j].Y = Sh[0].Points[i][j].Y;
                }
                etl[k][4] = etl[k][0];
                k++;
            }

            int q = 3;
            for (int i = 0; i < 180; i++)
            {
                SolidBrush Brush = new SolidBrush(Sh[0].Color[q]);
                g.FillPolygon(Brush, etl[i]);
                q++;
            }

            for (int i = 4; i < 7; i++)
            {
                SolidBrush Brush = new SolidBrush(Sh[0].Color[0]);
                g.FillPolygon(Brush, wire[i]);
            }

            zoneETL(g);
        }

        // отрисовка зоны защиты
        private void zoneETL(Graphics g)
        {
            float r0 = (float)_etl.r0_ETL;
            float rx = (float)_etl.rx_ETL;
            float h0 = (float)_etl.h0_ETL;

            g.DrawLine(new Pen(Color.White), _etl.S[0].h_rope2[1].X, _etl.S[0].basis[0].Y - h0, _etl.S[0].h_rope2[1].X + r0, _etl.S[0].basis[0].Y);
            //g.DrawLine(new Pen(Color.White), _etl.S[0].h_rope2[1].X + r0, _etl.S[0].basis[0].Y, _etl.S[0].h_rope2[1].X + rx, _etl.S[0].crossbars2[0].Y);
        }

        // отрисовка подстанции
        private void Draw_Sub(Graphics g)
        {
            // ПОДСТАНЦИЯ
            var sub = new PointF[36][];
            for (int i = 0; i < 36; i++)
                sub[i] = new PointF[5];

            for (int i = 0; i < 36; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sub[i][j].X = Sh[1].Points[i][j].X;
                    sub[i][j].Y = Sh[1].Points[i][j].Y;
                }
                sub[i][4] = sub[i][0];
            }

            Pen pen_line = new Pen(Color.FromArgb(70, 70, 70));

            
            for (int i = 0; i < 24; i++)
            {
                SolidBrush Brush = new SolidBrush(Sh[1].Color[i]);
                g.FillPolygon(Brush, sub[i]);
                g.DrawLines(pen_line, sub[i]);
            }

            if (_etl.Sub[0].basis[0].Z > _etl.Sub[0].building[0].Z)
            {
                SolidBrush Brush1 = new SolidBrush(Sh[0].Color[0]);
                g.FillPolygon(Brush1, wire[7]);
                g.FillPolygon(Brush1, wire[8]);
                g.FillPolygon(Brush1, wire[9]);
                for (int i = 24; i < 36; i++)
                {
                    SolidBrush Brush = new SolidBrush(Sh[1].Color[i]);
                    g.FillPolygon(Brush, sub[i]);
                    g.DrawLines(pen_line, sub[i]);
                }
            }
            else
            {
                for (int i = 24; i < 36; i++)
                {
                    SolidBrush Brush = new SolidBrush(Sh[1].Color[i]);
                    g.FillPolygon(Brush, sub[i]);
                    g.DrawLines(pen_line, sub[i]);
                }
                SolidBrush Brush1 = new SolidBrush(Sh[0].Color[0]);
                g.FillPolygon(Brush1, wire[7]);
                g.FillPolygon(Brush1, wire[8]);
                g.FillPolygon(Brush1, wire[9]);
            }
        }

        private void Draw(PictureBox pictureBox)
        {
            var bmp = new Bitmap(pictureBox.Image);
            Graphics g = Graphics.FromImage(bmp);

            // молния есть
            if (_etl._flag_etl && _etl._flag_sub && _ion != 0)
            {
                float[] mass = { 0, 0, 0 };
                Sort(mass);

                for (int i = 0; i < 3; i++)
                {
                    if (mass[i] == _etl.S[1].h_rope1[0].Z)
                        Draw_ETL(g);
                    else if (mass[i] == _light.zk+1)
                        _light.DrawLightning(g);
                    else
                        Draw_Sub(g);
                }
            }
            else
            {
                Draw_Sub(g);
                Draw_ETL(g);
            }

            pictureBox.Image = bmp;
        }
    }
}