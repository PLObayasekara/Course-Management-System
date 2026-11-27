using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Coursemanagement
{
    public partial class LecturerManagement : Form
    {
        public LecturerManagement()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        private void LoadLecturer()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT LecturerId,LecturerName,Email,Department FROM Lecturer", con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error loading lecturer: " + ex.Message);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                int Id = Convert.ToInt32(textBox1.Text);
                string name=textBox2.Text;
                string email=textBox3.Text;
                string department=comboBox1.SelectedItem.ToString();
                string Email = @"^[A-Za-z0-9]+@[A-Za-z]+\.[A-Za-z]{2,}$";

                if (Regex.IsMatch(textBox3.Text,Email)) {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Lecturer(LecturerId,LecturerName,Email,Department)VALUES('" + Id + "','" + name + "','" + email + "','" + department + "')", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Added!");
                    LoadLecturer();
                }
                else {
                    errorProvider1.SetError(textBox3,"Incorrect Email format!");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Adding Interrupted!"+ex.Message);
            }
            finally { conn.Close(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                int Id = Convert.ToInt32(textBox1.Text);
                string name = textBox2.Text;
                string email = textBox3.Text;
                string department = comboBox1.SelectedItem.ToString();
                string Email = @"^[A-Za-z0-9]+@[A-Za-z]+\.[A-Za-z]{2,}$";

                if (Regex.IsMatch(textBox3.Text, Email)) {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Lecturer SET LecturerName ='" + name + "',Email='" + email + "',Department='" + department + "' WHERE LecturerId=" + Id + "", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated!");
            }
            else {
                    errorProvider1.SetError(textBox3, "Incorrect Email format!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update is Interrupted!" + ex.Message);
            }
            finally { con.Close(); }
            LoadLecturer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void LecturerManagement_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("FOTS");
            comboBox1.Items.Add("BS");
            comboBox1.Items.Add("FAS");

            LoadLecturer();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int lid = Convert.ToInt32(textBox1.Text);
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Lecturer WHERE LecturerId=" + lid + "", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally { con.Close(); }
            LoadLecturer();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);
            int lid = Convert.ToInt32(textBox5.Text);

            try
            {

                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT LecturerId,LecturerName,Email,Department FROM Lecturer WHERE LecturerId LIKE @lid", con);
                adapter.SelectCommand.Parameters.AddWithValue("@lid", "%" + lid + "%");
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

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

            textBox1.Text = Id.ToString();
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Lecturer WHERE LecturerId=" + Id + "", con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    textBox1.Text = reader.GetValue(0).ToString();
                    textBox2.Text = reader.GetValue(1).ToString();
                    textBox3.Text = reader.GetValue(2).ToString();

                    comboBox1.Text = reader["Department"].ToString();

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
