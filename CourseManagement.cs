using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Coursemanagement
{
    public partial class CourseManagement : Form
    {
        public CourseManagement()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        private void LoadCourse() {
            using (SqlConnection con=new SqlConnection(Properties.Settings.Default.connString)) {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Course.CourseId,Course.CourseCode,Course.CourseName,Course.Credits,Lecturer.LecturerName FROM Course INNER JOIN Lecturer ON Course.LecturerId=Lecturer.LecturerId", con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void LoadLecturers()
        {
            try
            {
                using (SqlConnection con =new SqlConnection(Properties.Settings.Default.connString))
                {
                    

                    using (SqlDataAdapter adapter =new SqlDataAdapter("SELECT LecturerId, LecturerName FROM Lecturer ORDER BY LecturerName", con))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        comboBox1.DataSource = dt;
                        comboBox1.DisplayMember = "LecturerName";
                        comboBox1.ValueMember = "LecturerId";
                        comboBox1.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading lecturers: " + ex.Message);
                    
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con=new SqlConnection(Properties.Settings.Default.connString);

            try {
                int Id = Convert.ToInt32(textBox1.Text);
                string code=textBox2.Text;
                string name=textBox3.Text;
                int credit= Convert.ToInt32(textBox4.Text);
                int lecturerId = Convert.ToInt32(comboBox1.SelectedValue);

                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Course(CourseId,CourseCode,CourseName,Credits,LecturerId)VALUES("+Id+",'"+code+"','"+name+"',"+credit+","+lecturerId+")",con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex) {
                MessageBox.Show("Adding is Interrupted!"+ex.Message);
            } 
            finally { con.Close(); }
            LoadCourse();
            LoadLecturers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                int Id = Convert.ToInt32(textBox1.Text);
                string code = textBox2.Text;
                string name = textBox3.Text;
                int credit = Convert.ToInt32(textBox4.Text);
                int lecturerId = Convert.ToInt32(comboBox1.SelectedValue);

                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Course SET CourseCode ='"+ code + "',CourseName='"+ name + "',Credits="+credit+ ",LecturerId='"+ lecturerId + "'WHERE CourseId="+Id+"", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update is Interrupted!" + ex.Message);
            }

            finally { con.Close(); }
            LoadCourse();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void CourseManagement_Load(object sender, EventArgs e)
        {
            LoadCourse();
            LoadLecturers();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);
            int id= Convert.ToInt32(textBox5.Text);

            try {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Course.CourseId,Course.CourseCode,Course.CourseName,Course.Credits,Lecturer.LecturerName FROM Course INNER JOIN Lecturer ON Course.LecturerId=Lecturer.LecturerId WHERE CourseId LIKE @id", con);
                adapter.SelectCommand.Parameters.AddWithValue("@id", "%" + id + "%");
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex) {
                MessageBox.Show("Search Error! " + ex.Message);
            }
            finally {
                con.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

            textBox1.Text= Id.ToString();
            SqlConnection con=new SqlConnection(Properties.Settings.Default.connString);
            try {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Course WHERE CourseId="+Id+"",con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) { 
                    textBox1.Text=reader.GetValue(0).ToString();
                    textBox2.Text=reader.GetValue(1).ToString();
                    textBox3.Text=reader.GetValue(2).ToString();
                    textBox4.Text=reader.GetValue(3).ToString();

                    int lecturerId = Convert.ToInt32(reader.GetValue(4));

                    comboBox1.SelectedValue = lecturerId;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erorr"+ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);
            int id = Convert.ToInt32(textBox1.Text);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Course WHERE CourseId=" + id + "", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                LoadCourse();

            }
            catch (Exception ex) { 
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally { con.Close(); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }
    }
    
}
