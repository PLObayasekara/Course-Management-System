using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursemanagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CourseManagement CM=new CourseManagement();
            CM.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StudentManagement SM=new StudentManagement();
            SM.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LecturerManagement LM=new LecturerManagement();
            LM.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CourseEnrollment CM = new CourseEnrollment();
            CM.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
