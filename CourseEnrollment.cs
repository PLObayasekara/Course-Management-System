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
    public partial class CourseEnrollment : Form
    {
        public CourseEnrollment()
        {
            InitializeComponent();
            panel1.Visible = false;
        }
        private void LoadStudentId() {
            
            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connString))
                {


                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT StudentId, StudentName FROM Student ORDER BY StudentName", con))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        comboBox1.DataSource = dt;
                        comboBox1.DisplayMember = "StudentName";
                        comboBox1.ValueMember = "StudentId";
                        comboBox1.SelectedIndex = -1;
                    }
                }

            }

            catch (Exception ex){
                MessageBox.Show("Loading Erorr"+ex.Message);
            }

        }
        private void LoadCourseId() {
            try {
                using (SqlConnection con=new SqlConnection(Properties.Settings.Default.connString)) {
                    using (SqlDataAdapter adapter=new SqlDataAdapter("SELECT CourseId,CourseName FROM Course ORDER BY CourseName",con)) {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        comboBox2.DataSource = dt;
                        comboBox2.DisplayMember = "CourseName";
                        comboBox2.ValueMember = "CourseId";
                        comboBox2.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show("Erorr of loading course name",ex.Message);
            }

        }
        private void LoadCourseAndStudent()
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Enrollment.EnrollmentId,Enrollment.StudentId,Enrollment.CourseId,Enrollment.EnrollmentDate FROM Enrollment INNER JOIN Student ON Enrollment.StudentId=Student.StudentId INNER JOIN Course ON Enrollment.CourseId = Course.CourseId", con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection con=new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                int id = Convert.ToInt32(textBox1.Text);
                int sid = Convert.ToInt32(comboBox1.SelectedValue);
                int cid= Convert.ToInt32(comboBox2.SelectedValue);
                DateTime date = dateTimePicker1.Value;

                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Enrollment(EnrollmentId,StudentId,CourseId,EnrollmentDate)VALUES(@id,@sid,@cid,@date)", con);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@sid",sid);
                cmd.Parameters.AddWithValue("@cid",cid);
                cmd.Parameters.AddWithValue("@date",date);
                cmd.ExecuteNonQuery();
                LoadCourseAndStudent();
            }
            catch (Exception ex){
                MessageBox.Show("Adding Interrupted "+ex.Message);
            }
            finally {
                con.Close();
            }


        }

        private void CourseEnrollment_Load(object sender, EventArgs e)
        {
            LoadCourseAndStudent();
            LoadCourseId();
            LoadStudentId();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                int id = Convert.ToInt32(textBox1.Text);
                int sid = Convert.ToInt32(comboBox1.SelectedValue);
                int cid = Convert.ToInt32(comboBox2.SelectedValue);
                DateTime date = dateTimePicker1.Value;
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE Enrollment SET StudentId=@sid,CourseId=@cid,EnrollmentDate=@date WHERE EnrollmentId=" + id + "", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@sid", sid);
                cmd.Parameters.AddWithValue("@cid", cid);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated!");
                LoadCourseAndStudent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update is Interrupted!" + ex.Message);
            }

            finally { con.Close(); }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = " ";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);
            int id = Convert.ToInt32(textBox5.Text);

            try
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Enrollment.EnrollmentId,Enrollment.StudentId,Enrollment.CourseId,Enrollment.EnrollmentDate FROM Enrollment INNER JOIN Student ON Enrollment.StudentId=Student.StudentId INNER JOIN Course ON Enrollment.CourseId = Course.CourseId WHERE EnrollmentId LIKE @id", con);
                adapter.SelectCommand.Parameters.AddWithValue("@id", "%" + id + "%");
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

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

            textBox1.Text=id.ToString();

            SqlConnection con=new SqlConnection(Properties.Settings.Default.connString);

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Enrollment WHERE EnrollmentId="+id+"",con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) { 
                    textBox1.Text=reader.GetValue(0).ToString();

                    comboBox1.SelectedValue = Convert.ToInt32(reader["StudentId"]);
                    comboBox2.SelectedValue = Convert.ToInt32(reader["CourseId"]);

                    if (reader["EnrollmentDate"] != DBNull.Value)
                    {
                        dateTimePicker1.Format =
                            DateTimePickerFormat.Short;

                        dateTimePicker1.Value =
                            Convert.ToDateTime(
                                reader["EnrollmentDate"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erorr "+ex.Message);
            }
            finally { 
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox1.Text);
            SqlConnection con = new SqlConnection(Properties.Settings.Default.connString);

            try {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Enrollment WHERE EnrollmentId="+id+"", con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                LoadCourseAndStudent();
            }
            catch(Exception ex) {
                MessageBox.Show("Erorr "+ex.Message);
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
