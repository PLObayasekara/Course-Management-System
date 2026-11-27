using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Coursemanagement
{
    public partial class StudentManagement : Form
    {
        public StudentManagement()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        private void LoadStudent()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT StudentId,StudentName,Email,Phone FROM Student", con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Student: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int id=Convert.ToInt32(textBox1.Text);
            string name=textBox2.Text;
            string email=textBox3.Text;
            string phone=textBox4.Text;

            SqlConnection conn = new SqlConnection(Properties.Settings.Default.connString);

            try {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Student(StudentId,StudentName,Email,Phone)VALUES("+id+",'"+name+ "','"+email+"','"+phone+"')", conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student Added!");

                LoadStudent();
            }
            catch(Exception ex) {
                MessageBox.Show("adding is interrupted!"+ex.Message);
            }
            finally { conn.Close(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                int sid = Convert.ToInt32(textBox1.Text);
                string sname = textBox2.Text;
                string email = textBox3.Text;
                int number = Convert.ToInt32(textBox4.Text);

                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Student SET StudentName ='" + sname + "',Email='" + email + "',Phone=" + number +"WHERE StudentId="+sid+"", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update is Interrupted!" + ex.Message);
            }

            finally { con.Close(); }
            LoadStudent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int sid = Convert.ToInt32(textBox1.Text);
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Student WHERE StudentId=" + sid + "", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally { con.Close(); }
            LoadStudent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);
            int sid = Convert.ToInt32(textBox5.Text);

            try
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT StudentId,StudentName,Email,Phone FROM Student WHERE StudentId LIKE @sid", con);
                adapter.SelectCommand.Parameters.AddWithValue("@sid", "%" + sid + "%");
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error! " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void StudentManagement_Load(object sender, EventArgs e)
        {
            LoadStudent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

            textBox1.Text = Id.ToString();
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE StudentId=" + Id + "", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    textBox1.Text = reader.GetValue(0).ToString();
                    textBox2.Text = reader.GetValue(1).ToString();
                    textBox3.Text = reader.GetValue(2).ToString();
                    textBox4.Text = reader.GetValue(3).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }
    }
}
