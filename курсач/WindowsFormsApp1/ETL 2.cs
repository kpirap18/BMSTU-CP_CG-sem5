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
        //public Point3D[] door; // дверь
        public Point3D[][] test1 = new Point3D[7][];

        public bool _flag_etl; // защищена ли ЛЭП
        public bool _flag_sub; // защищена ли подстанция

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
            int h_width = _h_support / 5 / 2;

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

            S[0].h_sup[0].Z = S[1].basis[0].X - S[0].basis[0].X;
            S[0].h_sup[1].Z = S[1].basis[1].X - S[0].basis[1].X;
            S[0].h_sup[2].Z = S[1].basis[2].X - S[0].basis[2].X - h_width / 5;
            S[0].h_sup[3].Z = S[1].basis[3].X - S[0].basis[3].X - h_width / 5;

            S[1].h_sup[0].Z = S[1].basis[0].X - S[0].basis[0].X;
            S[1].h_sup[1].Z = S[1].basis[1].X - S[0].basis[1].X;
            S[1].h_sup[2].Z = S[1].basis[2].X - S[0].basis[2].X - h_width / 5;
            S[1].h_sup[3].Z = S[1].basis[3].X - S[0].basis[3].X - h_width / 5;

            S[2].h_sup[0].Z = S[1].basis[0].X - S[0].basis[0].X - h_width / 5;
            S[2].h_sup[1].Z = S[1].basis[1].X - S[0].basis[1].X - h_width / 5;
            S[2].h_sup[2].Z = S[1].basis[2].X - S[0].basis[2].X - h_width / 5 * 2;
            S[2].h_sup[3].Z = S[1].basis[3].X - S[0].basis[3].X - h_width / 5 * 2;

            S[3].h_sup[0].Z = S[1].basis[0].X - S[0].basis[0].X - h_width / 5;
            S[3].h_sup[1].Z = S[1].basis[1].X - S[0].basis[1].X - h_width / 5;
            S[3].h_sup[2].Z = S[1].basis[2].X - S[0].basis[2].X - h_width / 5 * 2;
            S[3].h_sup[3].Z = S[1].basis[3].X - S[0].basis[3].X - h_width / 5 * 2;


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
                    //S[i].basis2[j].Z = (float)Math.Pow((_dist_support*_dist_support - 
                    //    Math.Pow((S[i].basis2[j].X - S[i].basis[j].X), 2)), 1/2);

                    S[i].h_sup2[j].X = S[i].h_sup[j].X + _dist_support;
                    S[i].h_sup2[j].Y = S[i].h_sup[j].Y;
                    S[i].h_sup2[j].Z = S[i].h_sup[j].Z;
                    //S[i].h_sup2[j].Z = (float)Math.Pow((_dist_support * _dist_support -
                    //    Math.Pow((S[i].h_sup2[j].X - S[i].h_sup[j].X), 2)), 1 / 2);

                    S[i].support2[j].X = S[i].support[j].X + _dist_support;
                    S[i].support2[j].Y = S[i].support[j].Y;
                    S[i].support2[j].Z = S[i].support[j].Z;
                    //S[i].support2[j].Z = (float)Math.Pow((_dist_support * _dist_support -
                    //    Math.Pow((S[i].support2[j].X - S[i].support[j].X), 2)), 1 / 2);

                    S[i].support12[j].X = S[i].support1[j].X + _dist_support;
                    S[i].support12[j].Y = S[i].support1[j].Y;
                    S[i].support12[j].Z = S[i].support1[j].Z;
                    //S[i].support2[j].Z = (float)Math.Pow((_dist_support * _dist_support -
                    //    Math.Pow((S[i].support12[j].X - S[i].support1[j].X), 2)), 1 / 2);
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
                S[i].crossbars1[0].X = S[0].h_sup[3].X;//six_center - h_width/5/2;
                S[i].crossbars1[0].Y = h - _h_support + (int)(_h_support / tmp_cros1[i]);
                S[i].crossbars1[0].Z = S[0].h_sup[3].Z + (int)(h_width / tmp_cros2[i]);
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

                S[i].crossbars2[0].X = S[0].h_sup2[3].X;//six_center - h_width / 5 / 2;
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

            

            //S[1].h_rope1[0].X = S[0].h_rope1[0].X;
            //S[1].h_rope1[0].Y = h - _h_rope;
            //S[1].h_rope1[0].Z = S[1].h_sup[3].Z + h_width / 5/2;
            //S[1].h_rope1[1].X = S[0].h_rope1[1].X;
            //S[1].h_rope1[1].Y = h - _h_rope;
            //S[1].h_rope1[1].Z = S[1].h_sup[3].Z + h_width / 5/2;
            //S[1].h_rope1[2].X = S[0].h_rope1[2].X;
            //S[1].h_rope1[2].Y = h - _h_rope;
            //S[1].h_rope1[2].Z = S[1].h_sup[3].Z - h_width / 5/2;
            //S[1].h_rope1[3].X = S[0].h_rope1[3].X;
            //S[1].h_rope1[3].Y = h - _h_rope;
            //S[1].h_rope1[3].Z = S[1].h_sup[3].Z - h_width / 5/2;

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
        }
        public void Substation(int w, int h)
        {
            int used = w - w/4;
            Sub = new Substation[4];
            int h_width = _h_rods/10/5;

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

            // door
            //for (int i = 0; i < 4; i++)
            //    door[i] = new Point3D();

            ProtectionZoneETL(h);
            ProtectionZoneSubstation(w, h);
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
            for(; i<nplusc-1; ++i) { // (; i<nplusc; ++i)
                if ((t >= x[i]) && (t<x[i + 1])) temp[i] = 1;
                else temp[i] = 0;
            }

            // Вычислить нерациональные базисные функции высшего порядка
            double d = 0, e = 0;
            for(int k = 2; k <= c; ++k) {
                for(i = 0; i < nplusc - k; ++i) {

                    // Если базовая функция нижнего порядка равна нулю, пропустите расчет
                    if (temp[i] != 0) d = ((t-x[i])* temp[i])/(x[i + k - 1]-x[i]);
                    else d = 0;

                    // если базовая функция нижнего порядка равна нулю, пропустите расчет
                    if (temp[i + 1] != 0) e = ((x[i + k]-t)* temp[i + 1])/(x[i + k]-x[i + 1]);
                    else e = 0;

                    temp[i] = d + e;
                }
            }

            // Подобрать последнюю точку
            if (t == Convert.ToDouble(x[nplusc - 1])) temp[npts - 1] = 1;

            double sum = 0;
            for(i = 0; i<npts; ++i) sum += temp[i]* h[i]; //???

            for(i = 0; i<npts; ++i) {
                if(sum != 0) r[i] = (temp[i]* h[i])/sum;
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

        // удалить эту функцию
        //private void RotateETL(Point3D c, Point3D angle, int flag)
        //{
        //    for (int i = 0; i < 4; i++)
        //        for (int j = 0; j < 4; j++)
        //        {
        //            Transform.RotateX(c, S[i].basis[j], angle.X);
        //            Transform.RotateY(c, S[i].basis[j], angle.Y);
        //            Transform.RotateZ(c, S[i].basis[j], angle.Z);

        //            Transform.RotateX(c, S[i].h_sup[j], angle.X);
        //            Transform.RotateY(c, S[i].h_sup[j], angle.Y);
        //            Transform.RotateZ(c, S[i].h_sup[j], angle.Z);

        //            Transform.RotateX(c, S[i].support[j], angle.X);
        //            Transform.RotateY(c, S[i].support[j], angle.Y);
        //            Transform.RotateZ(c, S[i].support[j], angle.Z);

        //            Transform.RotateX(c, S[i].support1[j], angle.X);
        //            Transform.RotateY(c, S[i].support1[j], angle.Y);
        //            Transform.RotateZ(c, S[i].support1[j], angle.Z);


        //            Transform.RotateX(c, S[i].basis2[j], angle.X);
        //            Transform.RotateY(c, S[i].basis2[j], angle.Y);
        //            Transform.RotateZ(c, S[i].basis2[j], angle.Z);

        //            Transform.RotateX(c, S[i].h_sup2[j], angle.X);
        //            Transform.RotateY(c, S[i].h_sup2[j], angle.Y);
        //            Transform.RotateZ(c, S[i].h_sup2[j], angle.Z);

        //            Transform.RotateX(c, S[i].support2[j], angle.X);
        //            Transform.RotateY(c, S[i].support2[j], angle.Y);
        //            Transform.RotateZ(c, S[i].support2[j], angle.Z);

        //            Transform.RotateX(c, S[i].support12[j], angle.X);
        //            Transform.RotateY(c, S[i].support12[j], angle.Y);
        //            Transform.RotateZ(c, S[i].support12[j], angle.Z);

        //            ////////////////////////////////////////
        //            ///
        //            if (flag == 0)
        //            {
        //                Transform.RotateX(c, Sub[i].basis[j], -5);
        //                Transform.RotateY(c, Sub[i].basis[j], -10);
        //                Transform.RotateZ(c, Sub[i].basis[j], angle.Z);

        //                Transform.RotateX(c, Sub[i].h_rods[j], -5);
        //                Transform.RotateY(c, Sub[i].h_rods[j], -10);
        //                Transform.RotateZ(c, Sub[i].h_rods[j], angle.Z);

        //                Transform.RotateX(c, Sub[i].building[j], -5);
        //                Transform.RotateY(c, Sub[i].building[j], -10);
        //                Transform.RotateZ(c, Sub[i].building[j], angle.Z);
        //            }
        //            else
        //            {
        //                Transform.RotateX(c, Sub[i].basis[j], 5);
        //                Transform.RotateY(c, Sub[i].basis[j], 10);
        //                Transform.RotateZ(c, Sub[i].basis[j], angle.Z);

        //                Transform.RotateX(c, Sub[i].h_rods[j], 5);
        //                Transform.RotateY(c, Sub[i].h_rods[j], 10);
        //                Transform.RotateZ(c, Sub[i].h_rods[j], angle.Z);

        //                Transform.RotateX(c, Sub[i].building[j], 5);
        //                Transform.RotateY(c, Sub[i].building[j], 10);
        //                Transform.RotateZ(c, Sub[i].building[j], angle.Z);
        //            }
        //        }
        //    for (int i = 0; i < 3; i++)
        //        for (int j = 0; j < 8; j++)
        //        {
        //            Transform.RotateX(c, S[i].crossbars1[j], angle.X);
        //            Transform.RotateY(c, S[i].crossbars1[j], angle.Y);
        //            Transform.RotateZ(c, S[i].crossbars1[j], angle.Z);

        //            Transform.RotateX(c, S[i].crossbars2[j], angle.X);
        //            Transform.RotateY(c, S[i].crossbars2[j], angle.Y);
        //            Transform.RotateZ(c, S[i].crossbars2[j], angle.Z);

        //        }
        //    for (int i = 0; i < 2; i++)
        //        for (int j = 0; j < 4; j++)
        //        {
        //            Transform.RotateX(c, S[i].h_rope1[j], angle.X);
        //            Transform.RotateY(c, S[i].h_rope1[j], angle.Y);
        //            Transform.RotateZ(c, S[i].h_rope1[j], angle.Z);

        //            Transform.RotateX(c, S[i].h_rope2[j], angle.X);
        //            Transform.RotateY(c, S[i].h_rope2[j], angle.Y);
        //            Transform.RotateZ(c, S[i].h_rope2[j], angle.Z);

        //        }

        //}

        private void RotateETL(Point3D c, Point3D angle, int flag)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    Transform.RotateX(c, S[i].basis[j], angle.X);
                    Transform.RotateY(c, S[i].basis[j], angle.Y);
                    //Transform.RotateZ(c, S[i].basis[j], angle.Z);

                    Transform.RotateX(c, S[i].h_sup[j], angle.X);
                    Transform.RotateY(c, S[i].h_sup[j], angle.Y);
                    //Transform.RotateZ(c, S[i].h_sup[j], angle.Z);

                    Transform.RotateX(c, S[i].support[j], angle.X);
                    Transform.RotateY(c, S[i].support[j], angle.Y);
                    //Transform.RotateZ(c, S[i].support[j], angle.Z);

                    Transform.RotateX(c, S[i].support1[j], angle.X);
                    Transform.RotateY(c, S[i].support1[j], angle.Y);
                    //Transform.RotateZ(c, S[i].support1[j], angle.Z);


                    Transform.RotateX(c, S[i].basis2[j], angle.X);
                    Transform.RotateY(c, S[i].basis2[j], angle.Y);
                    //Transform.RotateZ(c, S[i].basis2[j], angle.Z);

                    Transform.RotateX(c, S[i].h_sup2[j], angle.X);
                    Transform.RotateY(c, S[i].h_sup2[j], angle.Y);
                    //Transform.RotateZ(c, S[i].h_sup2[j], angle.Z);

                    Transform.RotateX(c, S[i].support2[j], angle.X);
                    Transform.RotateY(c, S[i].support2[j], angle.Y);
                    //Transform.RotateZ(c, S[i].support2[j], angle.Z);

                    Transform.RotateX(c, S[i].support12[j], angle.X);
                    Transform.RotateY(c, S[i].support12[j], angle.Y);
                    //Transform.RotateZ(c, S[i].support12[j], angle.Z);

                    ////////////////////////////////////////
                    ///
                        Transform.RotateX(c, Sub[i].basis[j], angle.X);
                        Transform.RotateY(c, Sub[i].basis[j], angle.Y);
                        //Transform.RotateZ(c, Sub[i].basis[j], angle.Z);

                        Transform.RotateX(c, Sub[i].h_rods[j], angle.X);
                        Transform.RotateY(c, Sub[i].h_rods[j], angle.Y);
                        //Transform.RotateZ(c, Sub[i].h_rods[j], angle.Z);

                        Transform.RotateX(c, Sub[i].building[j], angle.X);
                        Transform.RotateY(c, Sub[i].building[j], angle.Y);
                        //Transform.RotateZ(c, Sub[i].building[j], angle.Z);
                    
                }
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                {
                    Transform.RotateX(c, S[i].crossbars1[j], angle.X);
                    Transform.RotateY(c, S[i].crossbars1[j], angle.Y);
                    //Transform.RotateZ(c, S[i].crossbars1[j], angle.Z);

                    Transform.RotateX(c, S[i].crossbars2[j], angle.X);
                    Transform.RotateY(c, S[i].crossbars2[j], angle.Y);
                    //Transform.RotateZ(c, S[i].crossbars2[j], angle.Z);

                }
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                {
                    Transform.RotateX(c, S[i].h_rope1[j], angle.X);
                    Transform.RotateY(c, S[i].h_rope1[j], angle.Y);
                    //Transform.RotateZ(c, S[i].h_rope1[j], angle.Z);

                    Transform.RotateX(c, S[i].h_rope2[j], angle.X);
                    Transform.RotateY(c, S[i].h_rope2[j], angle.Y);
                    //Transform.RotateZ(c, S[i].h_rope2[j], angle.Z);

                }

        }

        public void DrawETL(PictureBox pictureBox, Point3D angle)  // angle = (1, 0, 0)
        {
            var bmp = new Bitmap(pictureBox.Image);
            Graphics g = Graphics.FromImage(bmp);
            RotateETL(_center, new Point3D(-15,-15, 0), 0);

            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// создать функцию для вычисления координат провода
            float step = (S[1].h_rope2[2].X - S[1].h_rope1[2].X) / 6;
            var dort1 = new Point3D[7][];
            dort1[0] = new Point3D[7] { S[1].h_rope1[2],
                new Point3D(S[1].h_rope1[2].X+step, S[1].h_rope1[2].Y+9, S[1].h_rope1[2].Z),
                new Point3D(S[1].h_rope1[2].X+step*2, S[1].h_rope1[2].Y+12, S[1].h_rope1[2].Z),
                new Point3D(S[1].h_rope1[2].X+step*3, S[1].h_rope1[2].Y+15, S[1].h_rope1[2].Z),
                new Point3D(S[1].h_rope1[2].X+step*4, S[1].h_rope1[2].Y+12,S[1].h_rope1[2].Z),
                new Point3D(S[1].h_rope1[2].X+step*5, S[1].h_rope1[2].Y+9,S[1].h_rope1[2].Z),
                S[1].h_rope2[2] };

            int[] tmp_d1 = { 0, 0, 0, 1, 1, 2, 2 };
            int[] tmp_d2 = { 0, 4, 5, 4, 5, 4, 5 };
            for (int i = 1; i < 5; i++)
            {
                dort1[i] = new Point3D[7] { new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*2, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*3, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+25, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*4, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*5, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                S[tmp_d1[i]].crossbars2[tmp_d2[i]]};
            }
            for (int i = 5; i < 7; i++)
            {
                dort1[i] = new Point3D[7] { new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*2, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*3, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+25, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*4, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+22, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars1[tmp_d2[i]].X+step*5, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Y+15, S[tmp_d1[i]].crossbars1[tmp_d2[i]].Z),
                new Point3D(S[tmp_d1[i]].crossbars2[tmp_d2[i]].X, S[tmp_d1[i]].crossbars2[tmp_d2[i]].Y,S[tmp_d1[i]].crossbars2[tmp_d2[i]].Z)};
            }
            int len_dort = 60;
            double[] weight = new double[len_dort];
            for (int i = 0; i < len_dort; i++)
                weight[i] = 1;
            
            for (int i = 0; i < 7; i++)
                test1[i] = RBSpline(3, len_dort, dort1[i], weight);

            // провода
            var wire = new PointF[7][];
            for (int i = 0; i < 7; i++)
                wire[i] = new PointF[len_dort];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < len_dort; j++)
                {
                    wire[i][j].X = test1[i][j].X;
                    wire[i][j].Y = test1[i][j].Y;
                }
            }

            for (int i = 0; i < 7; i++)
                g.DrawLines(new Pen(Color.White), wire[i]);
            ////////////////////////////////////////////////////////////////////////////////////////////////

            var pol1 = new PointF[4][];
            var pol2 = new PointF[4][];
            for (int i = 0; i < 4; i++)
                pol1[i] = new PointF[4];
            for (int i = 0; i < 4; i++)
                pol2[i] = new PointF[4];

            var pol12 = new PointF[4][];
            var pol22 = new PointF[4][];
            for (int i = 0; i < 4; i++)
                pol12[i] = new PointF[4];
            for (int i = 0; i < 4; i++)
                pol22[i] = new PointF[4];


            // // // basis
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    pol1[i][j].X = S[i].basis[j].X;
                    pol1[i][j].Y = S[i].basis[j].Y;
                }

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    pol12[i][j].X = S[i].basis2[j].X;
                    pol12[i][j].Y = S[i].basis2[j].Y;
                }

            // // // h_sup
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    pol2[i][j].X = S[i].h_sup[j].X;
                    pol2[i][j].Y = S[i].h_sup[j].Y;
                }

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    pol22[i][j].X = S[i].h_sup2[j].X;
                    pol22[i][j].Y = S[i].h_sup2[j].Y;
                }

            // // // support
            var pol3 = new PointF[4][];
            for (int i = 0; i < 4; i++)
                pol3[i] = new PointF[4];

            var pol32 = new PointF[4][];
            for (int i = 0; i < 4; i++)
                pol32[i] = new PointF[4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    pol3[i][j].X = S[i].support[j].X;
                    pol3[i][j].Y = S[i].support[j].Y; 
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    pol32[i][j].X = S[i].support2[j].X;
                    pol32[i][j].Y = S[i].support2[j].Y;
                }
            }

            var pol31 = new PointF[4][];
            for (int i = 0; i < 4; i++)
                pol31[i] = new PointF[4];

            var pol312 = new PointF[4][];
            for (int i = 0; i < 4; i++)
                pol312[i] = new PointF[4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    pol31[i][j].X = S[i].support1[j].X;
                    pol31[i][j].Y = S[i].support1[j].Y;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    pol312[i][j].X = S[i].support12[j].X;
                    pol312[i][j].Y = S[i].support12[j].Y;
                }
            }

            // // // crossbars
            var pol4 = new PointF[3][];
            for (int i = 0; i < 3; i++)
                pol4[i] = new PointF[8];

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0)
                        pol4[i][j].X = S[i].crossbars1[j].X;// +_h_support/5/5/2;
                    else if (i == 1)
                        pol4[i][j].X = S[i].crossbars1[j].X;// + _h_support / 5 / 5;
                    else
                        pol4[i][j].X = S[i].crossbars1[j].X;// + _h_support / 5 / 3;
                    pol4[i][j].Y = S[i].crossbars1[j].Y;

                }


            var pol42 = new PointF[3][];
            for (int i = 0; i < 3; i++)
                pol42[i] = new PointF[8];

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0)
                        pol42[i][j].X = S[i].crossbars2[j].X;// + _h_support / 5 / 5 / 2;
                    else if (i == 1)
                        pol42[i][j].X = S[i].crossbars2[j].X;// + _h_support / 5 / 5;
                    else
                        pol42[i][j].X = S[i].crossbars2[j].X;// + _h_support / 5 / 3;
                    pol42[i][j].Y = S[i].crossbars2[j].Y;
                }

            // // // h_rope
            var pol5 = new PointF[2][];
            for (int i = 0; i < 2; i++)
                pol5[i] = new PointF[4];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0)
                        pol5[i][j].X = S[i].h_rope1[j].X;
                    else if (i == 1)
                        pol5[i][j].X = S[i].h_rope1[j].X;// -_h_support/5/5/2;
                    pol5[i][j].Y = S[i].h_rope1[j].Y;
                }
            }

            var pol52 = new PointF[2][];
            for (int i = 0; i < 2; i++)
                pol52[i] = new PointF[4];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 0)
                        pol52[i][j].X = S[i].h_rope2[j].X;
                    else if (i == 1)
                        pol52[i][j].X = S[i].h_rope2[j].X;// - _h_support / 5 / 5 / 2;
                    pol52[i][j].Y = S[i].h_rope2[j].Y;
                }
            }




            int[] tmp0 = { 0, 1, 3, 2, 0 };
            for (int i = 0; i < 4; i++)
            {
                g.DrawLine(new Pen(Color.White), pol1[0][tmp0[i]], pol1[0][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.White), pol2[0][tmp0[i]], pol2[0][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.White), pol1[0][i], pol2[0][i]);
                g.DrawLine(new Pen(Color.Blue), pol1[1][tmp0[i]], pol1[1][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Blue), pol2[1][tmp0[i]], pol2[1][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Blue), pol1[1][i], pol2[1][i]);
                g.DrawLine(new Pen(Color.Red), pol1[2][tmp0[i]], pol1[2][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Red), pol2[2][tmp0[i]], pol2[2][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Red), pol1[2][i], pol2[2][i]);
                g.DrawLine(new Pen(Color.Green), pol1[3][tmp0[i]], pol1[3][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Green), pol2[3][tmp0[i]], pol2[3][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Green), pol1[3][i], pol2[3][i]);

                g.DrawLine(new Pen(Color.White), pol12[0][tmp0[i]], pol12[0][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.White), pol22[0][tmp0[i]], pol22[0][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.White), pol12[0][i], pol22[0][i]);
                g.DrawLine(new Pen(Color.Blue), pol12[1][tmp0[i]], pol12[1][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Blue), pol22[1][tmp0[i]], pol22[1][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Blue), pol12[1][i], pol22[1][i]);
                g.DrawLine(new Pen(Color.Red), pol12[2][tmp0[i]], pol12[2][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Red), pol22[2][tmp0[i]], pol22[2][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Red), pol12[2][i], pol22[2][i]);
                g.DrawLine(new Pen(Color.Green), pol12[3][tmp0[i]], pol12[3][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Green), pol22[3][tmp0[i]], pol22[3][tmp0[i + 1]]);
                g.DrawLine(new Pen(Color.Green), pol12[3][i], pol22[3][i]);
            }
            

            int[] tmp = { 0, 1, 3, 2, 0 };
            for (int i = 0; i < 4; i++)
            {
                g.DrawLine(new Pen(Color.SkyBlue), pol3[tmp[i]][0], pol3[tmp[i + 1]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol3[tmp[i + 1]][1], pol3[tmp[i]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol3[tmp[i]][2], pol3[tmp[i + 1]][3]);
                g.DrawLine(new Pen(Color.SkyBlue), pol3[tmp[i + 1]][0], pol3[tmp[i]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol3[tmp[i]][1], pol3[tmp[i + 1]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol3[tmp[i + 1]][2], pol3[tmp[i]][3]);

                g.DrawLine(new Pen(Color.SkyBlue), pol32[tmp[i]][0], pol32[tmp[i + 1]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol32[tmp[i + 1]][1], pol32[tmp[i]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol32[tmp[i]][2], pol32[tmp[i + 1]][3]);
                g.DrawLine(new Pen(Color.SkyBlue), pol32[tmp[i + 1]][0], pol32[tmp[i]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol32[tmp[i]][1], pol32[tmp[i + 1]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol32[tmp[i + 1]][2], pol32[tmp[i]][3]);
            }

            int[] tmp1 = { 0, 1, 3, 2, 0 };
            for (int i = 0; i < 4; i++)
            {
                g.DrawLine(new Pen(Color.SkyBlue), pol31[tmp1[i]][0], pol31[tmp1[i + 1]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol31[tmp1[i + 1]][1], pol31[tmp1[i]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol31[tmp1[i]][2], pol31[tmp1[i + 1]][3]);
                g.DrawLine(new Pen(Color.SkyBlue), pol31[tmp1[i + 1]][0], pol31[tmp1[i]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol31[tmp1[i]][1], pol31[tmp1[i + 1]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol31[tmp1[i + 1]][2], pol31[tmp1[i]][3]);

                g.DrawLine(new Pen(Color.SkyBlue), pol312[tmp1[i]][0], pol312[tmp1[i + 1]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol312[tmp1[i + 1]][1], pol312[tmp1[i]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol312[tmp1[i]][2], pol312[tmp1[i + 1]][3]);
                g.DrawLine(new Pen(Color.SkyBlue), pol312[tmp1[i + 1]][0], pol312[tmp1[i]][1]);
                g.DrawLine(new Pen(Color.SkyBlue), pol312[tmp1[i]][1], pol312[tmp1[i + 1]][2]);
                g.DrawLine(new Pen(Color.SkyBlue), pol312[tmp1[i + 1]][2], pol312[tmp1[i]][3]);
            }

            for (int i = 0; i < 3; i++)
            {
                g.DrawLine(new Pen(Color.White), pol4[i][0], pol4[i][1]);
                g.DrawLine(new Pen(Color.White), pol4[i][2], pol4[i][3]);
                g.DrawLine(new Pen(Color.White), pol4[i][4], pol4[i][5]);
                g.DrawLine(new Pen(Color.White), pol4[i][6], pol4[i][7]);
                g.DrawLine(new Pen(Color.Blue), pol4[i][0], pol4[i][2]);
                g.DrawLine(new Pen(Color.Blue), pol4[i][2], pol4[i][4]);
                g.DrawLine(new Pen(Color.Blue), pol4[i][4], pol4[i][6]);
                g.DrawLine(new Pen(Color.Blue), pol4[i][6], pol4[i][0]);
                g.DrawLine(new Pen(Color.White), pol4[i][1], pol4[i][3]);
                g.DrawLine(new Pen(Color.White), pol4[i][3], pol4[i][5]);
                g.DrawLine(new Pen(Color.White), pol4[i][5], pol4[i][7]);
                g.DrawLine(new Pen(Color.White), pol4[i][7], pol4[i][1]);

                g.DrawLine(new Pen(Color.White), pol42[i][0], pol42[i][1]);
                g.DrawLine(new Pen(Color.White), pol42[i][2], pol42[i][3]);
                g.DrawLine(new Pen(Color.White), pol42[i][4], pol42[i][5]);
                g.DrawLine(new Pen(Color.White), pol42[i][6], pol42[i][7]);
                g.DrawLine(new Pen(Color.Blue), pol42[i][0], pol42[i][2]);
                g.DrawLine(new Pen(Color.Blue), pol42[i][2], pol42[i][4]);
                g.DrawLine(new Pen(Color.Blue), pol42[i][4], pol42[i][6]);
                g.DrawLine(new Pen(Color.Blue), pol42[i][6], pol42[i][0]);
                g.DrawLine(new Pen(Color.White), pol42[i][1], pol42[i][3]);
                g.DrawLine(new Pen(Color.White), pol42[i][3], pol42[i][5]);
                g.DrawLine(new Pen(Color.White), pol42[i][5], pol42[i][7]);
                g.DrawLine(new Pen(Color.White), pol42[i][7], pol42[i][1]);
            }


            //
            //PointF[] point_rope = { pol5[0][0], pol5[1][0], pol5[1][1], pol5[0][1], pol5[0][3], pol5[1][3],
            //                        pol5[1][2], pol5[0][2], pol5[0][0]};
            //g.DrawPolygon(new Pen(Color.Blue), point_rope);
            g.DrawLine(new Pen(Color.Blue), pol5[1][0], pol5[1][1]);
            g.DrawLine(new Pen(Color.Blue), pol5[1][1], pol5[1][3]);
            g.DrawLine(new Pen(Color.Blue), pol5[1][3], pol5[1][2]);
            g.DrawLine(new Pen(Color.Blue), pol5[0][0], pol5[1][0]);
            g.DrawLine(new Pen(Color.Blue), pol5[0][1], pol5[1][1]);
            g.DrawLine(new Pen(Color.Blue), pol5[0][2], pol5[1][2]);
            g.DrawLine(new Pen(Color.Blue), pol5[0][3], pol5[1][3]);

            g.DrawLine(new Pen(Color.Blue), pol52[1][0], pol52[1][1]);
            g.DrawLine(new Pen(Color.Blue), pol52[1][1], pol52[1][3]);
            g.DrawLine(new Pen(Color.Blue), pol52[1][3], pol52[1][2]);
            g.DrawLine(new Pen(Color.Blue), pol52[0][0], pol52[1][0]);
            g.DrawLine(new Pen(Color.Blue), pol52[0][1], pol52[1][1]);
            g.DrawLine(new Pen(Color.Blue), pol52[0][2], pol52[1][2]);
            g.DrawLine(new Pen(Color.Blue), pol52[0][3], pol52[1][3]);

            /////////////////////////////////////////////////////////////////////////////////////////
            // // // basis
            var sub_bas = new PointF[4][];
            for (int i = 0; i < 4; i++)
                sub_bas[i] = new PointF[4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sub_bas[i][j].X = Sub[i].basis[j].X;// + 100; ///////////////////////////
                    sub_bas[i][j].Y = Sub[i].basis[j].Y;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                g.DrawLine(new Pen(Color.White), sub_bas[i][0], sub_bas[i][1]);
                g.DrawLine(new Pen(Color.White), sub_bas[i][1], sub_bas[i][3]);
                g.DrawLine(new Pen(Color.White), sub_bas[i][3], sub_bas[i][2]);
                g.DrawLine(new Pen(Color.White), sub_bas[i][0], sub_bas[i][2]);
            }

            // // // h_rods
            var sub_h = new PointF[4][];
            for (int i = 0; i < 4; i++)
                sub_h[i] = new PointF[4];


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sub_h[i][j].X = Sub[i].h_rods[j].X; // + 100;
                    sub_h[i][j].Y = Sub[i].h_rods[j].Y;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                g.DrawLine(new Pen(Color.White), sub_h[i][0], sub_h[i][1]);
                g.DrawLine(new Pen(Color.White), sub_h[i][1], sub_h[i][3]);
                g.DrawLine(new Pen(Color.White), sub_h[i][3], sub_h[i][2]);
                g.DrawLine(new Pen(Color.White), sub_h[i][0], sub_h[i][2]);
            }

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    g.DrawLine(new Pen(Color.White), sub_bas[i][j], sub_h[i][j]);

            // // // building
            var sub_build = new PointF[4][];
            for (int i = 0; i < 4; i++)
                sub_build[i] = new PointF[4];


            int[] tmp_b = { 0, 1, 3, 2 };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sub_build[i][j].X = Sub[i].building[tmp_b[j]].X;// + 100;
                    sub_build[i][j].Y = Sub[i].building[tmp_b[j]].Y;
                }
            }

            var sub_build_tmp = new PointF[8][];
            for (int i = 0; i < 8; i++)
                sub_build_tmp[i] = new PointF[4];
            

            int[] tmp_help_build = {0, 1, 3, 2, 0};

            ///////////////////
            var sub_h_tmp = new PointF[4][];
            for (int i = 0; i < 4; i++)
                sub_h_tmp[i] = new PointF[4];

            //sub_h_tmp[0].X = Sub[0].basis[0];

            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        sub_h[i][j].X = Sub[i].h_rods[j].X + 100;
            //        sub_h[i][j].Y = Sub[i].h_rods[j].Y;
            //    }
            //}//////////////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////

            for (int i = 0; i < 4; i++)
            {
                sub_build_tmp[i][0].X = Sub[0].building[tmp_help_build[i]].X;// + 100;
                sub_build_tmp[i][0].Y = Sub[0].building[tmp_help_build[i]].Y;
                sub_build_tmp[i][1].X = Sub[0].building[tmp_help_build[i + 1]].X;// + 100;
                sub_build_tmp[i][1].Y = Sub[0].building[tmp_help_build[i + 1]].Y;
                sub_build_tmp[i][2].X = Sub[1].building[tmp_help_build[i + 1]].X;// + 100;
                sub_build_tmp[i][2].Y = Sub[1].building[tmp_help_build[i + 1]].Y;
                sub_build_tmp[i][3].X = Sub[1].building[tmp_help_build[i]].X;// + 100;
                sub_build_tmp[i][3].Y = Sub[1].building[tmp_help_build[i]].Y;
            }
            for (int i = 0; i < 4; i++)
            {
                sub_build_tmp[i + 4][0].X = Sub[2].building[tmp_help_build[i]].X;// + 100;
                sub_build_tmp[i + 4][0].Y = Sub[2].building[tmp_help_build[i]].Y;
                sub_build_tmp[i + 4][1].X = Sub[2].building[tmp_help_build[i + 1]].X;// + 100;
                sub_build_tmp[i + 4][1].Y = Sub[2].building[tmp_help_build[i + 1]].Y;
                sub_build_tmp[i + 4][2].X = Sub[3].building[tmp_help_build[i + 1]].X;// + 100;
                sub_build_tmp[i + 4][2].Y = Sub[3].building[tmp_help_build[i + 1]].Y;
                sub_build_tmp[i + 4][3].X = Sub[3].building[tmp_help_build[i]].X;// + 100;
                sub_build_tmp[i + 4][3].Y = Sub[3].building[tmp_help_build[i]].Y;
            }
            //////////
            SolidBrush Brush = new SolidBrush(Color.FromArgb(83, 98, 103));
            SolidBrush Brush1 = new SolidBrush(Color.FromArgb(7, 13, 13));
            Pen Pen1 = new Pen(Color.FromArgb(7, 13, 13));

            for (int i = 0; i < 8; i++)
                g.FillPolygon(Brush, sub_build_tmp[i]);

            // крыша
            for (int i = 1; i < 4; i += 2)
                g.FillPolygon(Brush1, sub_build[i]);

            for (int i = 0; i < 4; i += 3)
                g.DrawPolygon(Pen1, sub_build_tmp[i]);
            for (int i = 4; i < 6; i ++)
                g.DrawPolygon(Pen1, sub_build_tmp[i]);


            //g.DrawLine(new Pen(Color.White), new Point(518, 0), new Point(-63, 521));
            //g.DrawLine(new Pen(Color.White), new Point(0, 600), new Point(700, 600));


            pictureBox.Image = bmp;
            //RotateETL(_center, new Point3D(5, -20, 0), 1);  // по сути не нужен
        }  // сделать провода от ЛЭП к подстанции

        // вычисление зоны защиты
        public void ProtectionZoneETL(int height) // wigth - ширина, height - высота
        {
            float l = _dist_support;  // расстояние между опорами
            float w = Math.Abs(S[0].crossbars1[0].Z - S[0].crossbars1[1].Z);  // ширина перекладин
            float h = height - S[1].h_rope1[0].Y; // S[0].crossbars1[4].Y;  // высота молниеотвода
            float hx = height - S[0].crossbars1[1].Y;  // высота перекладин ЛЭП
            double h0 = 0.85 * h;
            double r0 = (1.35 - 0.0025*h)*h;
            double rx = (1.35 - 0.0025 * h) * (h - hx / 0.85);  // r0 * (h0 - hx) / h0;
            if (rx < w)
                _flag_etl = false; // не защищает
            else
                _flag_etl = true;
            h0 = Math.Floor(h0 * 100) / 100; // высота молниезащиты
            r0 = Math.Floor(r0 * 100) / 100; // радиус зоны защиты на уровне земли
            rx = Math.Floor(rx * 100) / 100;  // радиус зоны защиты на уровне верхней перекладины
            
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
