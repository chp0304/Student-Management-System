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
    public partial class Enter_rusult : Form
    {
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataSet dat;
        SqlDataReader dr;
        DataTable dt;
        int i = 0;
        string[] name = new string[100];
        public Enter_rusult()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //下拉框选择函数
        {
            dataGridView1.Rows.Clear();//清除表格，避免每次点击重复添加items——————很重要
            con.Open();
            string s1 = String.Format("select cname,姓名,SC.cno,SC.sno from teacher_course join SC on teacher_course.tno = SC.tno join S on SC.sno = S.学号 where teacher_course.tno = '{0}' and teacher_course.cname = '{1}'", login.lg.textBox1.Text.Trim(),comboBox1.SelectedItem.ToString().Trim());//SQL语句
            cmd = new SqlCommand(s1, con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                i = dataGridView1.Rows.Add();//添加行
                dataGridView1.Rows[i].Cells["姓名"].Value = dr["姓名"].ToString();//添加数据
                dataGridView1.Rows[i].Cells["学号"].Value = dr["sno"].ToString();
            }
            con.Close();
        }

        private void Enter_rusult_Load(object sender, EventArgs e)
        {
            //datagridview布局
            int j = 0;
            dt = new DataTable();
            con.Open();
            string s = String.Format("select cname from teacher_course where tno = '{0}'", login.lg.textBox1.Text.Trim());//获取所教课程名
            cmd = new SqlCommand(s, con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                name[j++] = dr["cname"].ToString().Trim();
                comboBox1.Items.Add(dr["cname"].ToString().Trim());//添加到items里面
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            for(int j=0;j<=i; j++)
            {
               if(dataGridView1.Rows[j].Cells["成绩"].Value != null)
                {
                    con.Open();
                    string s = String.Format("update SC set grade = '{0}' where sno = '{1}' and tno = '{2}'", dataGridView1.Rows[j].Cells["成绩"].Value.ToString(), dataGridView1.Rows[j].Cells["学号"].Value.ToString(), login.lg.textBox1.Text.Trim());
                    cmd = new SqlCommand(s, con);
                    cnt += cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            if(cnt <= i + 1 && cnt > 0)
            {
                MessageBox.Show("录入成功！");
            }
        }
    }
}
