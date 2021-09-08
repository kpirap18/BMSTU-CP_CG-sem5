using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
//using System.Windows.Media.Media3D;
//using System.Numerics;

namespace WindowsFormsApp1
{
    struct Support
    {
        public Point3D[] basis; // координаты основания опоры
        public Point3D[] support; // координаты пролетов на каждой опоре
        public Point3D[] support1; // координаты пролетов на каждой опоре1
        public Point3D[] h_sup; // координаты верхней части опоры

        public Point3D[] basis2; // координаты основания опоры
        public Point3D[] support2; // координаты пролетов на каждой опоре
        public Point3D[] support12; // координаты пролетов на каждой опоре1
        public Point3D[] h_sup2; // координаты верхней части опоры

        public Point3D[] crossbars1; // перекладины
        public Point3D[] crossbars2;

        public Point3D[] h_rope1;  // молниезащита
        public Point3D[] h_rope2;
    } 
    struct Substation
    {
        public Point3D[] basis; // координаты основания стержней
        public Point3D[] h_rods; // координаты верхней части стержней
        public Point3D[] building; // здания
    }
    class ETL
    {
        private readonly Point3D _center;
        public readonly Support[] S;
        public Substation[] Sub;
        public int _h_support;
        public int _h_rope;
        public int _dist_support;

        public int _width_rods; // ширина подстанции (расстояние между стержнями)
        public int _height_rods;  // длина подстанции (расстояние между стержнями)
        public int _h_building;  // высота зданий
        public int _width_building; // ширина зданий
        public int _h_rods; // высота стержней
        public Point3D[][] test1 = new Point3D[7][];

        public bool _flag_etl; // защищена ли ЛЭП
        public bool _flag_sub; // защищена ли подстанция

        public double r0_ETL;  // радиус зоны защиты на уровне земли
        public double rx_ETL;  // радиус зоны защиты на уровне верхней перекладины
        public double h0_ETL;  // высота молниезащиты

        public int used_widht;
        public ETL(int w, int h, int h_s, int h_r, int dist_support, int width_rods,
            int height_rods, int h_building, int width_building, int h_rods,Point3D c, int angle)
        {
            used_widht = w / 2;
            S = new Support[4];
            _h_support = (int)(h_s * 2 + 50);
            _h_rope = (int)(h_r * 2 + 50);
            _dist_support = (int)(dist_support * 2);

            _width_rods = width_rods*2 + 50;
            _height_rods = height_rods*2 + 50;
            _h_building = h_building*2+50;
            _width_building = width_building*2 + 50;
            _h_rods = h_rods*2+50; 

            _center = new Point3D();
            _center = c;

            int six = used_widht / 3;
            int six_center = six / 2;
            int h_width = _h_support / 5 /2;

            // // // basis
            for (int i = 0; i < 4; i++)
            {
                S[i].basis = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    S[i].basis[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j += 2)
                    S[i].basis[j].X = six_center - h_width * (float)Math.Pow((-1), i) - h_width / 5;


            for (int i = 0; i < 4; i++)
                for (int j = 1; j < 4; j += 2)
                    S[i].basis[j].X = six_center - h_width * (float)Math.Pow((-1), i);



            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    S[i].basis[j].Y = h - 10;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    S[i].basis[j].Z = 1;

            for (int i = 2; i < 4; i++)
                for (int j = 0; j < 2; j++)
                    S[i].basis[j].Z = -six_center;

            for (int i = 0; i < 2; i++)
                for (int j = 2; j < 4; j++)
                    S[i].basis[j].Z = 1 - h_width / 5;

            for (int i = 2; i < 4; i++)
                for (int j = 2; j < 4; j++)
                    S[i].basis[j].Z = -six_center - h_width / 5;

            // // // h_sup
            for (int i = 0; i < 4; i++)
            {
                S[i].h_sup = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    S[i].h_sup[j] = new Point3D();
            }

            S[0].h_sup[0].X = six_center - h_width / 5;
            S[0].h_sup[1].X = six_center;
            S[0].h_sup[2].X = six_center - h_width / 5;
            S[0].h_sup[3].X = six_center;

            S[2].h_sup[0].X = six_center - h_width / 5;
            S[2].h_sup[1].X = six_center;
            S[2].h_sup[2].X = six_center - h_width / 5;
            S[2].h_sup[3].X = six_center;

            S[1].h_sup[0].X = six_center;
            S[1].h_sup[1].X = six_center + h_width / 5;
            S[1].h_sup[2].X = six_center;
            S[1].h_sup[3].X = six_center + h_width / 5;

            S[3].h_sup[0].X = six_center;
            S[3].h_sup[1].X = six_center + h_width / 5;
            S[3].h_sup[2].X = six_center;
            S[3].h_sup[3].X = six_center + h_width / 5;


            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    S[i].h_sup[j].Y = h - _h_support;

            S[0].h_sup[0].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[0].Z - S[0].basis[0].Z)/2);
            S[0].h_sup[1].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[1].Z - S[0].basis[1].Z)/2);
            S[0].h_sup[2].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[2].Z - S[0].basis[2].Z)/2 - h_width / 5);
            S[0].h_sup[3].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[3].Z - S[0].basis[3].Z)/2 - h_width / 5);

            S[1].h_sup[0].Z = S[0].h_sup[0].Z; 
            S[1].h_sup[1].Z = S[0].h_sup[1].Z;
            S[1].h_sup[2].Z = S[0].h_sup[2].Z;
            S[1].h_sup[3].Z = S[0].h_sup[3].Z;

            S[2].h_sup[0].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[0].Z - S[0].basis[0].Z) / 2) - h_width / 5;
            S[2].h_sup[1].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[1].Z - S[0].basis[1].Z) / 2) - h_width / 5;
            S[2].h_sup[2].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[2].Z - S[0].basis[2].Z) / 2) - h_width / 5 * 2;
            S[2].h_sup[3].Z = S[1].basis[0].Z - Math.Abs((S[2].basis[3].Z - S[0].basis[3].Z) / 2) - h_width / 5 * 2;

            S[3].h_sup[0].Z = S[2].h_sup[0].Z;
            S[3].h_sup[1].Z = S[2].h_sup[0].Z;
            S[3].h_sup[2].Z = S[2].h_sup[0].Z;
            S[3].h_sup[3].Z = S[2].h_sup[0].Z;


            // // // support
            for (int i = 0; i < 4; i++)
            {
                S[i].support = new Point3D[5];
                for (int j = 0; j < 5; j++)
                    S[i].support[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
            {
                S[i].support1 = new Point3D[5];
                for (int j = 0; j < 5; j++)
                    S[i].support1[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
            {
                S[i].support[0].X = (S[i].basis[3 - i].X + 0.25f * S[i].h_sup[3 - i].X) / (1 + 0.25f);
                S[i].support[0].Y = (S[i].basis[3 - i].Y + 0.25f * S[i].h_sup[3 - i].Y) / (1 + 0.25f);
                S[i].support[0].Z = (S[i].basis[3 - i].Z + 0.25f * S[i].h_sup[3 - i].Z) / (1 + 0.25f);
                S[i].support[1].X = (S[i].basis[3 - i].X + 0.66f * S[i].h_sup[3 - i].X) / (1 + 0.66f);
                S[i].support[1].Y = (S[i].basis[3 - i].Y + 0.66f * S[i].h_sup[3 - i].Y) / (1 + 0.66f);
                S[i].support[1].Z = (S[i].basis[3 - i].Z + 0.66f * S[i].h_sup[3 - i].Z) / (1 + 0.66f);
                S[i].support[2].X = (S[i].basis[3 - i].X + 1.5f * S[i].h_sup[3 - i].X) / (1 + 1.5f);
                S[i].support[2].Y = (S[i].basis[3 - i].Y + 1.5f * S[i].h_sup[3 - i].Y) / (1 + 1.5f);
                S[i].support[2].Z = (S[i].basis[3 - i].Z + 1.5f * S[i].h_sup[3 - i].Z) / (1 + 1.5f);
                S[i].support[3].X = (S[i].basis[3 - i].X + 4f * S[i].h_sup[3 - i].X) / (1 + 4f);
                S[i].support[3].Y = (S[i].basis[3 - i].Y + 4f * S[i].h_sup[3 - i].Y) / (1 + 4f);
                S[i].support[3].Z = (S[i].basis[3 - i].Z + 4f * S[i].h_sup[3 - i].Z) / (1 + 4f);

                S[i].support1[0].X = (S[i].basis[3 - i].X + 0.3f * S[i].h_sup[3 - i].X) / (1 + 0.31f);
                S[i].support1[0].Y = (S[i].basis[3 - i].Y + 0.31f * S[i].h_sup[3 - i].Y) / (1 + 0.31f);
                S[i].support1[0].Z = (S[i].basis[3 - i].Z + 0.31f * S[i].h_sup[3 - i].Z) / (1 + 0.31f);
                S[i].support1[1].X = (S[i].basis[3 - i].X + 0.78f * S[i].h_sup[3 - i].X) / (1 + 0.78f);
                S[i].support1[1].Y = (S[i].basis[3 - i].Y + 0.78f * S[i].h_sup[3 - i].Y) / (1 + 0.78f);
                S[i].support1[1].Z = (S[i].basis[3 - i].Z + 0.78f * S[i].h_sup[3 - i].Z) / (1 + 0.78f);
                S[i].support1[2].X = (S[i].basis[3 - i].X + 1.78f * S[i].h_sup[3 - i].X) / (1 + 1.78f);
                S[i].support1[2].Y = (S[i].basis[3 - i].Y + 1.78f * S[i].h_sup[3 - i].Y) / (1 + 1.78f);
                S[i].support1[2].Z = (S[i].basis[3 - i].Z + 1.78f * S[i].h_sup[3 - i].Z) / (1 + 1.78f);
                S[i].support1[3].X = (S[i].basis[3 - i].X + 5.26f * S[i].h_sup[3 - i].X) / (1 + 5.26f);
                S[i].support1[3].Y = (S[i].basis[3 - i].Y + 5.26f * S[i].h_sup[3 - i].Y) / (1 + 5.26f);
                S[i].support1[3].Z = (S[i].basis[3 - i].Z + 5.26f * S[i].h_sup[3 - i].Z) / (1 + 5.26f);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < 4; i++)
            {
                S[i].basis2 = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    S[i].basis2[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
            {
                S[i].h_sup2 = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    S[i].h_sup2[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
            {
                S[i].support2 = new Point3D[5];
                for (int j = 0; j < 5; j++)
                    S[i].support2[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
            {
                S[i].support12 = new Point3D[5];
                for (int j = 0; j < 5; j++)
                    S[i].support12[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    S[i].basis2[j].X = S[i].basis[j].X + _dist_support;
                    S[i].basis2[j].Y = S[i].basis[j].Y;
                    S[i].basis2[j].Z = S[i].basis[j].Z;

                    S[i].h_sup2[j].X = S[i].h_sup[j].X + _dist_support;
                    S[i].h_sup2[j].Y = S[i].h_sup[j].Y;
                    S[i].h_sup2[j].Z = S[i].h_sup[j].Z;

                    S[i].support2[j].X = S[i].support[j].X + _dist_support;
                    S[i].support2[j].Y = S[i].support[j].Y;
                    S[i].support2[j].Z = S[i].support[j].Z;

                    S[i].support12[j].X = S[i].support1[j].X + _dist_support;
                    S[i].support12[j].Y = S[i].support1[j].Y;
                    S[i].support12[j].Z = S[i].support1[j].Z;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                S[i].crossbars1 = new Point3D[8];
                for (int j = 0; j < 8; j++)
                    S[i].crossbars1[j] = new Point3D();
            }

            for (int i = 0; i < 3; i++)
            {
                S[i].crossbars2 = new Point3D[8];
                for (int j = 0; j < 8; j++)
                    S[i].crossbars2[j] = new Point3D();
            }


            float[] tmp_cros1 = { 5.8f, 2.9f, 2f };
            float[] tmp_cros2 = { 1.3f, 0.7f, 1.1f };
            for (int i = 0; i < 3; i++)
            {
                S[i].crossbars1[0].X = S[0].h_sup[3].X;
                S[i].crossbars1[0].Y = h - _h_support + (int)(_h_support / tmp_cros1[i]);
                S[i].crossbars1[0].Z = S[0].h_sup[3].Z + (int)(h_width / tmp_cros2[i]);  ///////////////////////
                S[i].crossbars1[1].X = S[i].crossbars1[0].X;
                S[i].crossbars1[1].Y = S[i].crossbars1[0].Y;
                S[i].crossbars1[1].Z = S[0].h_sup[3].Z - (int)(h_width / tmp_cros2[i]);
                S[i].crossbars1[2].X = S[i].crossbars1[0].X;
                S[i].crossbars1[2].Y = S[i].crossbars1[0].Y - h_width / 5;
                S[i].crossbars1[2].Z = S[i].crossbars1[0].Z;
                S[i].crossbars1[3].X = S[i].crossbars1[0].X;
                S[i].crossbars1[3].Y = S[i].crossbars1[2].Y;
                S[i].crossbars1[3].Z = S[i].crossbars1[1].Z;
                S[i].crossbars1[4].X = S[i].crossbars1[0].X + h_width / 5;
                S[i].crossbars1[4].Y = S[i].crossbars1[2].Y;
                S[i].crossbars1[4].Z = S[i].crossbars1[2].Z;
                S[i].crossbars1[5].X = S[i].crossbars1[4].X;
                S[i].crossbars1[5].Y = S[i].crossbars1[4].Y;
                S[i].crossbars1[5].Z = S[i].crossbars1[3].Z;
                S[i].crossbars1[6].X = S[i].crossbars1[4].X;
                S[i].crossbars1[6].Y = S[i].crossbars1[0].Y;
                S[i].crossbars1[6].Z = S[i].crossbars1[0].Z;
                S[i].crossbars1[7].X = S[i].crossbars1[6].X;
                S[i].crossbars1[7].Y = S[i].crossbars1[6].Y;
                S[i].crossbars1[7].Z = S[i].crossbars1[1].Z;

                S[i].crossbars2[0].X = S[0].h_sup2[3].X;
                S[i].crossbars2[0].Y = h - _h_support + (int)(_h_support / tmp_cros1[i]);
                S[i].crossbars2[0].Z = S[0].h_sup2[3].Z + (int)(h_width / tmp_cros2[i]);
                S[i].crossbars2[1].X = S[i].crossbars2[0].X;
                S[i].crossbars2[1].Y = S[i].crossbars2[0].Y;
                S[i].crossbars2[1].Z = S[0].h_sup2[3].Z - (int)(h_width / tmp_cros2[i]);
                S[i].crossbars2[2].X = S[i].crossbars2[0].X;
                S[i].crossbars2[2].Y = S[i].crossbars2[0].Y - h_width / 5;
                S[i].crossbars2[2].Z = S[i].crossbars2[0].Z;
                S[i].crossbars2[3].X = S[i].crossbars2[0].X;
                S[i].crossbars2[3].Y = S[i].crossbars2[2].Y;
                S[i].crossbars2[3].Z = S[i].crossbars2[1].Z;
                S[i].crossbars2[4].X = S[i].crossbars2[0].X + h_width / 5;
                S[i].crossbars2[4].Y = S[i].crossbars2[2].Y;
                S[i].crossbars2[4].Z = S[i].crossbars2[2].Z;
                S[i].crossbars2[5].X = S[i].crossbars2[4].X;
                S[i].crossbars2[5].Y = S[i].crossbars2[4].Y;
                S[i].crossbars2[5].Z = S[i].crossbars2[3].Z;
                S[i].crossbars2[6].X = S[i].crossbars2[4].X;
                S[i].crossbars2[6].Y = S[i].crossbars2[0].Y;
                S[i].crossbars2[6].Z = S[i].crossbars2[0].Z;
                S[i].crossbars2[7].X = S[i].crossbars2[6].X;
                S[i].crossbars2[7].Y = S[i].crossbars2[6].Y;
                S[i].crossbars2[7].Z = S[i].crossbars2[1].Z;
            }

            // // // h_rope
            for (int i = 0; i < 2; i++)
            {
                S[i].h_rope1 = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    S[i].h_rope1[j] = new Point3D();
            }

            S[0].h_rope1[0].X = S[0].h_sup[3].X - h_width / 5/2;
            S[0].h_rope1[0].Y = S[0].h_sup[3].Y;
            S[0].h_rope1[0].Z = S[0].h_sup[3].Z + h_width / 5/2;
            S[0].h_rope1[1].X = S[0].h_sup[3].X + h_width / 5/2;
            S[0].h_rope1[1].Y = S[0].h_sup[3].Y;
            S[0].h_rope1[1].Z = S[0].h_sup[3].Z + h_width / 5/2;
            S[0].h_rope1[2].X = S[0].h_sup[3].X - h_width / 5/2;
            S[0].h_rope1[2].Y = S[0].h_sup[3].Y;
            S[0].h_rope1[2].Z = S[0].h_sup[3].Z - h_width / 5/2;
            S[0].h_rope1[3].X = S[0].h_sup[3].X + h_width / 5/2;
            S[0].h_rope1[3].Y = S[0].h_sup[3].Y;
            S[0].h_rope1[3].Z = S[0].h_sup[3].Z - h_width / 5/2;

            for (int i = 0; i < 4; i++)
            {
                S[1].h_rope1[i].X = S[0].h_rope1[i].X;
                S[1].h_rope1[i].Y = h - _h_rope;
                S[1].h_rope1[i].Z = S[0].h_rope1[i].Z;
            }

            for (int i = 0; i < 2; i++)
            {
                S[i].h_rope2 = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    S[i].h_rope2[j] = new Point3D();
            }

            S[0].h_rope2[0].X = S[0].h_sup2[3].X - h_width / 5 / 2;
            S[0].h_rope2[0].Y = S[0].h_sup2[3].Y;
            S[0].h_rope2[0].Z = S[0].h_sup2[3].Z + h_width / 5 / 2;
            S[0].h_rope2[1].X = S[0].h_sup2[3].X + h_width / 5 / 2;
            S[0].h_rope2[1].Y = S[0].h_sup2[3].Y;
            S[0].h_rope2[1].Z = S[0].h_sup2[3].Z + h_width / 5 / 2;
            S[0].h_rope2[2].X = S[0].h_sup2[3].X - h_width / 5 / 2;
            S[0].h_rope2[2].Y = S[0].h_sup2[3].Y;
            S[0].h_rope2[2].Z = S[0].h_sup2[3].Z - h_width / 5 / 2;
            S[0].h_rope2[3].X = S[0].h_sup2[3].X + h_width / 5 / 2;
            S[0].h_rope2[3].Y = S[0].h_sup2[3].Y;
            S[0].h_rope2[3].Z = S[0].h_sup2[3].Z - h_width / 5 / 2;

            S[1].h_rope2[0].X = S[0].h_rope2[0].X;
            S[1].h_rope2[0].Y = h - _h_rope;
            S[1].h_rope2[0].Z = S[1].h_sup2[3].Z + h_width / 5 / 2;
            S[1].h_rope2[1].X = S[0].h_rope2[1].X;
            S[1].h_rope2[1].Y = h - _h_rope;
            S[1].h_rope2[1].Z = S[1].h_sup2[3].Z + h_width / 5 / 2;
            S[1].h_rope2[2].X = S[0].h_rope2[2].X;
            S[1].h_rope2[2].Y = h - _h_rope;
            S[1].h_rope2[2].Z = S[1].h_sup2[3].Z - h_width / 5 / 2;
            S[1].h_rope2[3].X = S[0].h_rope2[3].X;
            S[1].h_rope2[3].Y = h - _h_rope;
            S[1].h_rope2[3].Z = S[1].h_sup2[3].Z - h_width / 5 / 2;

            Substation(w, h);
            RotateETL(_center, new Point3D(-15, -15, 0));
        }
        public void Substation(int w, int h)
        {
            int used = w - w/4;
            Sub = new Substation[4];
            int h_width = _h_rods/10/4;

            int begining = used - _height_rods; // начало по X подстанции

            // // // basis
            for (int i = 0; i < 4; i++)
            {
                Sub[i].basis = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    Sub[i].basis[j] = new Point3D();
            }
            for (int i = 0; i < 3; i += 2)
            {
                Sub[i].basis[0].X = begining+100;
                Sub[i].basis[1].X = begining + h_width + 100;
                Sub[i].basis[2].X = begining + 100;
                Sub[i].basis[3].X = begining + h_width + 100;
            }
            for (int i = 1; i < 4; i += 2)
            {
                Sub[i].basis[0].X = used + 100;
                Sub[i].basis[1].X = used + h_width + 100;
                Sub[i].basis[2].X = used + 100;
                Sub[i].basis[3].X = used + h_width + 100;
            }
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Sub[i].basis[j].Y = h - 100;
            for (int i = 0; i < 2; i++)
            {
                Sub[i].basis[0].Z = S[3].basis2[3].Z -20 * 2; // по госту должно быть 10 м между лэп и подстанцией, но на всякий случай пусть будет 20
                Sub[i].basis[1].Z = Sub[i].basis[0].Z;
                Sub[i].basis[2].Z = Sub[i].basis[0].Z - h_width;
                Sub[i].basis[3].Z = Sub[i].basis[0].Z - h_width;
            }
            for (int i = 2; i < 4; i++)
            {
                Sub[i].basis[0].Z = S[3].basis2[3].Z - 20 * 2 - _width_rods;
                Sub[i].basis[1].Z = Sub[i].basis[0].Z;
                Sub[i].basis[2].Z = Sub[i].basis[0].Z - h_width;
                Sub[i].basis[3].Z = Sub[i].basis[0].Z - h_width;
            }

            // // // h_rods
            for (int i = 0; i < 4; i++)
            {
                Sub[i].h_rods = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    Sub[i].h_rods[j] = new Point3D();
            }
            for (int i = 0; i < 3; i += 2)
            {
                Sub[i].h_rods[0].X = begining + 100;
                Sub[i].h_rods[1].X = begining + h_width + 100;
                Sub[i].h_rods[2].X = begining + 100;
                Sub[i].h_rods[3].X = begining + h_width + 100;
            }
            for (int i = 1; i < 4; i += 2)
            {
                Sub[i].h_rods[0].X = used + 100;
                Sub[i].h_rods[1].X = used + h_width + 100;
                Sub[i].h_rods[2].X = used + 100;
                Sub[i].h_rods[3].X = used + h_width + 100;
            }
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Sub[i].h_rods[j].Y = h - 100 - _h_rods;
            for (int i = 0; i < 2; i++)
            {
                Sub[i].h_rods[0].Z = S[3].basis2[3].Z - 20 * 2; // по госту должно быть 10 м между лэп и подстанцией, но на всякий случай пусть будет 20
                Sub[i].h_rods[1].Z = S[3].basis2[3].Z - 20 * 2;
                Sub[i].h_rods[2].Z = S[3].basis2[3].Z - 20 * 2 - h_width;
                Sub[i].h_rods[3].Z = S[3].basis2[3].Z - 20 * 2 - h_width;
            }
            for (int i = 2; i < 4; i++)
            {
                Sub[i].h_rods[0].Z = S[3].basis2[3].Z - 20 * 2 - _width_rods;
                Sub[i].h_rods[1].Z = S[3].basis2[3].Z - 20 * 2 - _width_rods;
                Sub[i].h_rods[2].Z = S[3].basis2[3].Z - 20 * 2 - _width_rods - h_width;
                Sub[i].h_rods[3].Z = S[3].basis2[3].Z - 20 * 2 - _width_rods - h_width;
            }

            // // // здания
            float centre_x = Sub[0].basis[1].X + (Sub[1].basis[0].X - Sub[0].basis[1].X)/2;
            float centre_z = Sub[0].basis[2].Z - (Sub[0].basis[2].Z - Sub[2].basis[0].Z)/2;

            for (int i = 0; i < 4; i++)
            {
                Sub[i].building = new Point3D[4];
                for (int j = 0; j < 4; j++)
                    Sub[i].building[j] = new Point3D();
            }

            for (int i = 0; i < 4; i++)
            {
                Sub[0].building[i].Y = Sub[0].basis[i].Y;
                Sub[2].building[i].Y = Sub[0].basis[i].Y;
            }

            for (int i = 0; i < 4; i++)
                Sub[1].building[i].Y = Sub[0].basis[i].Y - _h_building;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j += 2)
                    Sub[i].building[j].X = centre_x - _width_building / 2;// + 100;

                for (int j = 1; j < 4; j += 2)
                    Sub[i].building[j].X = centre_x - _width_building / 10;// + 100;

                for (int j = 0; j < 2; j++)
                    Sub[i].building[j].Z = centre_z + _width_building / 2;

                for (int j = 2; j < 4; j++)
                    Sub[i].building[j].Z = centre_z - _width_building / 2;
            }
            //
            for (int i = 0; i < 4; i++)
                Sub[3].building[i].Y = Sub[2].basis[i].Y - _h_building*2/3;

            for (int i = 2; i < 4; i++)
            {
                for (int j = 0; j < 3; j += 2)
                    Sub[i].building[j].X = centre_x + _width_building / 2;// + 100;

                for (int j = 1; j < 4; j += 2)
                    Sub[i].building[j].X = centre_x + _width_building / 10;// + 100;

                for (int j = 0; j < 2; j++)
                    Sub[i].building[j].Z = centre_z + _width_building / 2 ;

                for (int j = 2; j < 4; j++)
                    Sub[i].building[j].Z = centre_z - _width_building / 2;
            }

            ProtectionZoneETL(h);
            ProtectionZoneSubstation(w, h);
        }

        private void RotateETL(Point3D c, Point3D angle)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    Transform.RotateX(c, S[i].basis[j], angle.X);
                    Transform.RotateY(c, S[i].basis[j], angle.Y);

                    Transform.RotateX(c, S[i].h_sup[j], angle.X);
                    Transform.RotateY(c, S[i].h_sup[j], angle.Y);

                    Transform.RotateX(c, S[i].support[j], angle.X);
                    Transform.RotateY(c, S[i].support[j], angle.Y);

                    Transform.RotateX(c, S[i].support1[j], angle.X);
                    Transform.RotateY(c, S[i].support1[j], angle.Y);

                    Transform.RotateX(c, S[i].basis2[j], angle.X);
                    Transform.RotateY(c, S[i].basis2[j], angle.Y);

                    Transform.RotateX(c, S[i].h_sup2[j], angle.X);
                    Transform.RotateY(c, S[i].h_sup2[j], angle.Y);

                    Transform.RotateX(c, S[i].support2[j], angle.X);
                    Transform.RotateY(c, S[i].support2[j], angle.Y);

                    Transform.RotateX(c, S[i].support12[j], angle.X);
                    Transform.RotateY(c, S[i].support12[j], angle.Y);
                    
                    Transform.RotateX(c, Sub[i].basis[j], angle.X);
                    Transform.RotateY(c, Sub[i].basis[j], 10);

                    Transform.RotateX(c, Sub[i].h_rods[j], angle.X);
                    Transform.RotateY(c, Sub[i].h_rods[j], 10);

                    Transform.RotateX(c, Sub[i].building[j], angle.X);
                    Transform.RotateY(c, Sub[i].building[j], 10);

                }
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                {
                    Transform.RotateX(c, S[i].crossbars1[j], angle.X);
                    Transform.RotateY(c, S[i].crossbars1[j], angle.Y);

                    Transform.RotateX(c, S[i].crossbars2[j], angle.X);
                    Transform.RotateY(c, S[i].crossbars2[j], angle.Y);

                }
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                {
                    Transform.RotateX(c, S[i].h_rope1[j], angle.X);
                    Transform.RotateY(c, S[i].h_rope1[j], angle.Y);

                    Transform.RotateX(c, S[i].h_rope2[j], angle.X);
                    Transform.RotateY(c, S[i].h_rope2[j], angle.Y);
                }
        }
        
        // вычисление зоны защиты
        public void ProtectionZoneETL(int height) // wigth - ширина, height - высота
        {
            

            float l = _dist_support;  // расстояние между опорами
            //float w = Math.Abs(S[0].crossbars1[0].Z - S[0].crossbars1[1].Z);  // ширина перекладин
            float h = (_h_rope - 50) /2;//height - S[1].h_rope1[0].Y;  // высота молниеотвода
            //float hx = h - (Math.Abs(S[1].h_rope1[0].Y - S[0].crossbars1[1].Y) - 50)/2;  //(_h_rope - Math.Abs(S[1].h_rope1[0].Y - S[0].crossbars1[1].Y))/2 - 50;  // высота перекладин ЛЭП  // height - 
            float hx = h/5.8f;  // неправильно
            double h0 = 0.85 * h;
            r0_ETL = (1.35 - 0.0025*h)*h;
            rx_ETL = Math.Abs((1.35 - 0.0025 * h) * (h - hx / 0.85));  // r0 * (h0 - hx) / h0;

            // посчитать на листочке правильно ли считается зона зыщиты

            float w = h / 8.9f;

            if (rx_ETL < w)
                _flag_etl = false; // не защищает
            else
                _flag_etl = true;

            h0 = 0.85 * _h_rope;
            h = h  + 50;
            hx = hx * 2 + 50;
            r0_ETL = r0_ETL * 2 + 50;
            rx_ETL = rx_ETL * 2;// + 50;

            h0_ETL = Math.Floor(h0 * 100) / 100; // высота молниезащиты
            r0_ETL = Math.Floor(r0_ETL * 100) / 100; // радиус зоны защиты на уровне земли
            rx_ETL = Math.Floor(rx_ETL * 100) / 100;  // радиус зоны защиты на уровне верхней перекладины
        }

        public void ProtectionZoneSubstation(int width, int height)
        {
            float hx = Sub[0].building[0].Y - Sub[1].building[0].Y;
            float a = Sub[2].building[1].X - Sub[0].building[0].X;// + 100;  // ????? +100 ????? почему не хватает 100, но отрисовывается все нормально
            float b = Math.Abs(Sub[0].building[0].Z - Sub[0].building[2].Z);
            float l1 = Math.Abs(Sub[0].basis[0].Z - Sub[2].basis[2].Z);
            float l2 = Sub[1].basis[1].X - Sub[0].basis[0].X;
            double L = Math.Sqrt(l1 * l1 + l2 * l2);
            float h = Sub[0].basis[0].Y - Sub[2].h_rods[0].Y;
            double h0 = 0.92 * h;
            double rx = 1.5 * (h - 1.1 * hx);
            // высота средней части попарно взятых молниеотводов
            double h_min1 = h0 - 0.14 * (l1 - h); 
            double h_min2 = h0 - 0.14 * (l2 - h);
            // ширина средней части зоны попарно взятых молниеотводов на уровне земли
            double r0 = 1.5 * h;
            // на уровне высоты защищаемого объекта
            double rc1 = r0 * (h_min1 - hx) / h_min1;
            double rc2 = r0 * (h_min2 - hx) / h_min2;

            if (rc1 > 0 && a < (l2 + rc2))
                _flag_sub = true;
            else
                _flag_sub = false;

        }
    }
}
