using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        //public Point3D point3D;
        private readonly Scene _scene;
        public Form1()
        {
            InitializeComponent();
            _scene = new Scene(pictureBox1);
            _scene.Click();
            //Check();
        }

        private void Check()
        {
            if (!_scene._flag_etl && _scene._flag_sub)
            {
                // сообщение о том, что ЛЭП не защищена
                string message = "ЛЭП не защищена. Имеется угроза взрыва. Измените входные данные.";
                string caption = "Ошибка";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
            else if (_scene._flag_etl && !_scene._flag_sub)
            {
                // сообщение о том, что подстанция не защищена
                string message = "Подстанция не защищена. Имеется угроза взрыва. Измените входные данные.";
                string caption = "Ошибка";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
            else if (!_scene._flag_etl && !_scene._flag_sub)
            {
                // сообщение о том, что ничего не защищено
                string message = "ЛЭП и подстанция не защищены. Имеется угроза взрыва. Измените входные данные.";
                string caption = "Ошибка";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

     //   private void trackBar1_Scroll(object sender, EventArgs e)
     //   {
     //       // ионизация воздуха
     //       label4.Text = String.Format("{0} Кл/м^3", trackBar1.Value);
     //   }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // высота опор
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            // выста расположения тросса
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDown2.Value < (int)numericUpDown1.Value)
            {
                string message = "Высота расположения тросса должна быть выше: чем высота опор ЛЭП.";
                string caption = "Ошибка";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);
                if (result == DialogResult.OK)
                {
                    numericUpDown2.Value = numericUpDown1.Value + 1;
                    _scene.ion = (int)trackBar1.Value;
                    _scene.distance_support = (int)numericUpDown3.Value;
                    _scene.Click();
                }
            }
            _scene.h_support = (int)numericUpDown1.Value;
            _scene.h_rope = (int)numericUpDown2.Value;
             _scene.ion = (int)trackBar1.Value;
            _scene.distance_support = (int)numericUpDown3.Value;
            _scene.width_rods = (int)numericUpDown4.Value;
            _scene.height_rods = (int)numericUpDown5.Value;
            _scene.h_building = (int)numericUpDown6.Value;
            _scene.width_building = (int)numericUpDown4.Value;
            _scene.h_rods = (int)numericUpDown5.Value;
            _scene.Click();
            Check();
            // рендер
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            //if ((int)numericUpDown2.Value < (int)numericUpDown1.Value)
            //{
            //    string message = "Высота расположения тросса должна быть выше: чем высота опор ЛЭП.";
            //    string caption = "Ошибка";
            //    MessageBoxButtons buttons = MessageBoxButtons.OK;
            //    DialogResult result;
            //    result = MessageBox.Show(message, caption, buttons);
            //    if (result == DialogResult.OK)
            //    {
            //        numericUpDown2.Value = numericUpDown1.Value + 1;
            //        _scene.ion = (int)trackBar1.Value;
            //        _scene.distance_support = (int)numericUpDown3.Value;
            //        _scene.Click();
            //    }
            //}
            //_scene.h_support = (int)numericUpDown1.Value;
            //_scene.h_rope = (int)numericUpDown2.Value;
            _scene.ion = (int)trackBar1.Value;
            //_scene.distance_support = (int)numericUpDown3.Value;
            //_scene.width_rods = (int)numericUpDown4.Value;
            //_scene.height_rods = (int)numericUpDown5.Value;
            //_scene.h_building = (int)numericUpDown6.Value;
            //_scene.width_building = (int)numericUpDown8.Value;
            //_scene.h_rods = (int)numericUpDown7.Value;

            //_scene.Click();
            //Check();
            // пуск/стоп
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            //_scene.Timer1();
            _scene.ion = (int)trackBar1.Value;
            _scene.Click();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Left)
            //    _scene.Arrow(true);
            //else if (e.KeyCode == Keys.Right)
            //    _scene.Arrow(false);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            // расстояние между опорами лэп
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _scene.voltage_etl = 100;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            _scene.voltage_etl = 220;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _scene.voltage_etl = 300;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _scene.voltage_etl = 500;
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            _scene.voltage_substation = 110;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            _scene.voltage_substation = 220;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged_2(object sender, EventArgs e)
        {

        }
    }
}
