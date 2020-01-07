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
    public partial class Schedule : Form
    {
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataSet dat;
        public Schedule()
        {
            InitializeComponent();
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            //switch (comboBox1.SelectedIndex.ToString())
            //{
            //    case "第1周":
            //        MessageBox.Show("hahahha");
            //        break;
            //}
            //comboBox1.SelectedIndex = comboBox1.Items.IndexOf("全部");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.AutoResizeColumn(20);
            //Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[0].Width = 100;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = "";
            string date = "";
            string time = "";
            string w1;
            int a = 0, b = 0;
            con.Open();
            string s = "select cname,cdate,ctime,cweek,croom,tname,carray from SC join C on SC.cno = C.cno join Teacher on Teacher.tno = SC.tno where sno = " + login.lg.textBox1.Text.Trim().ToString();
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
                w1 = dr["carray"].ToString().Trim();
                string[] w2 = w1.Split('/');
                string n = System.Text.RegularExpressions.Regex.Replace(comboBox1.SelectedItem.ToString(), @"[^0-9]+", "");
                int flag = Array.IndexOf(w2,n);
                if(comboBox1.SelectedItem.ToString()=="全部")
                {
                    flag = -2;
                }
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
                if(flag == -2)
                {
                    dt.Rows[b][a] = name;
                }
                else if(flag != -1)
                {
                    dt.Rows[b][a] = name;
                }
                
                
            }

            dataGridView1.DataSource = dt;
            con.Close();

        }
    }
}
