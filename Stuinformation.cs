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
namespace 强智系统
{
    public partial class Stuinformation : Form
    {
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataSet dat;
        public static Stuinformation stu;
        public Stuinformation()
        {
            InitializeComponent();
            stu = this;
        }

        private void Stuinformation_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //textBox1.Text = "";
            //this.textBox1.Text = "123";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            string s = "select C.cname as 课程,grade as 成绩 from SC join C on C.cno = SC.cno where sno = " + login.lg.textBox1.Text.Trim().ToString();
            adp = new SqlDataAdapter(s, con);
            dat = new DataSet();
            adp.Fill(dat);
            Result result = new Result();
            result.dataGridView1.DataSource = dat.Tables[0];
            result.Owner = this;
            result.ShowDialog();
            con.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //更改头像代码
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            //if (file.FileName != string.Empty)
            //{
            //    try
            //    {
            //        pathname = file.FileName;   //获得文件的绝对路径
            //        this.pictureBox1.Load(pathname);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //1801020601
            string name = "";
            string date = "";
            string time = "";
            int a = 0, b = 0;
            con.Open();
            string s = "select cname,cdate,ctime,cweek,croom,tname from SC join C on SC.cno = C.cno join Teacher on Teacher.tno = SC.tno where sno = " + login.lg.textBox1.Text.Trim().ToString();
            //adp = new SqlDataAdapter(s, con);
            //dat = new DataSet();
            DataTable dt = new DataTable();
            Schedule schedule = new Schedule();
            //课程表
            dt.Columns.Add(" ", typeof(string));
            dt.Columns.Add("星期一", typeof(string));
            dt.Columns.Add("星期二", typeof(string));
            dt.Columns.Add("星期三", typeof(string));
            dt.Columns.Add("星期四", typeof(string));
            dt.Columns.Add("星期五", typeof(string));
            dt.Columns.Add("星期六", typeof(string));
            dt.Columns.Add("星期日", typeof(string));
            dt.Rows.Add("第一大节", typeof(string));
            dt.Rows.Add("第二大节", typeof(string));
            dt.Rows.Add("第三大节", typeof(string));
            dt.Rows.Add("第四大节", typeof(string));
            dt.Rows.Add("第五大节", typeof(string));
            DataColumn column = new DataColumn();
            for (int i = 0; i < 5; i++)
            {
                dt.Rows[i][1] = " ";
                //dt.Rows[i][1] = DataGridViewAutoSizeRowMode.AllCells;
            }
            SqlDataReader dr;
            cmd = new SqlCommand(s, con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                name = dr["cname"].ToString().Trim() + "\n" + dr["tname"].ToString().Trim() + "\n" + dr["cweek"].ToString().Trim() + "\n" + dr["croom"].ToString().Trim();
                date = dr["cdate"].ToString().Trim();
                time = dr["ctime"].ToString().Trim();
                switch (date)
                {
                    case "星期一":
                        a = 1;
                        break;
                    case "星期二":
                        a = 2;
                        break;
                    case "星期三":
                        a = 3;
                        break;
                    case "星期四":
                        a = 4;
                        break;
                    case "星期五":
                        a = 5;
                        break;
                    case "星期六":
                        a = 6;
                        break;
                    case "星期七":
                        a = 7;
                        break;
                }
                switch (time)
                {
                    case "第一大节":
                        b = 1;
                        break;
                    case "第二大节":
                        b = 2;
                        break;
                    case "第三大节":
                        b = 3;
                        break;
                    case "第四大节":
                        b = 4;
                        break;
                    case "第五大节":
                        b = 5;
                        break;
                }
                dt.Rows[b][a] = name;
            }

            schedule.dataGridView1.DataSource = dt;
            schedule.Owner = this;
            schedule.ShowDialog();
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Selection selection = new Selection();
            selection.Owner = this;
            selection.ShowDialog();

        }
    }
}
