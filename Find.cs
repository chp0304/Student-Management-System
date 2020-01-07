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
    public partial class Find : Form
    {
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        SqlDataAdapter adp = new SqlDataAdapter();
        DataSet dat;
        string s = "";
        public Find()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string user = textBox1.Text.Trim();
                if (login.lg.radioButton1.Checked)
                {
                    s = "select 密码 from login_S where 学号 = " + user;
                }
                if (login.lg.radioButton2.Checked)
                {
                    s = "select 密码 from login_T where 教工号 = " + user;
                }
                
                cmd = new SqlCommand(s, con);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                string pass = dr["密码"].ToString().Trim();
                textBox2.Text = pass;
                //adp = new SqlDataAdapter(s,con);
                //dat = new DataSet();
                //adp.Fill(dat);
                //dataGridView1.DataSource = dat.Tables[0];
                con.Close();
            }
            catch
            {
                MessageBox.Show("数据库连接失败！");
            }
        }

        private void Find_Load(object sender, EventArgs e)
        {

        }
    }
}
