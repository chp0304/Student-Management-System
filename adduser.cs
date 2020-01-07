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
    public partial class adduser : Form
    {
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        string s = "";
        string sql = "";
        int falg = 1;
        //login lg ;
        public adduser()
        {
            InitializeComponent();
            //login lg = new login();
        }
        //public adduser(login lgo)
        //{
        //    InitializeComponent();
        //    lg = lgo;
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string lguser = textBox1.Text.Trim();//用户名
                string lgpass = textBox2.Text.Trim();//密码
                string lgrepass = textBox3.Text.Trim();//确认密码

                //确认是否选择身份
                if(login.lg.radioButton1.Checked)
                {
                    sql = String.Format("select * from login_S where 学号 = {0}", lguser);
                }
                else if (login.lg.radioButton2.Checked)
                {
                    sql = String.Format("select * from login_T where 教工号 = {0}", lguser);
                }
                else
                {
                    falg = 0;
                    MessageBox.Show("请选择您的身份!");
                    con.Close();
                    this.Close();
                }
                if(falg == 1)
                {

                    cmd = new SqlCommand(sql, con);
                    object obj = cmd.ExecuteScalar();//判断注册用户名与现有用户民重复
                    if (obj == null)
                    {
                        if (lgpass != lgrepass)//密码与重复密码的比较
                        {
                            MessageBox.Show("两次密码不一致");
                            textBox2.Text = "";
                            textBox3.Text = "";
                            con.Close();
                        }
                        if (lgpass == lgrepass)
                        {
                            if (login.lg.radioButton1.Checked)
                            {
                                s = String.Format("insert into login_S values ('{0}','{1}')", lguser, lgpass);
                            }
                            else if (login.lg.radioButton2.Checked)
                            {
                                s = String.Format("insert into login_T values ('{0}','{1}')", lguser, lgpass);
                            }
                            else
                            {
                                MessageBox.Show("请选择您的身份!");
                                //con.Close();
                            }
                            cmd = new SqlCommand(s, con);
                            int cnt = cmd.ExecuteNonQuery();
                            if (cnt > 0)
                            {
                                MessageBox.Show("注册成功！");
                                con.Close();
                                this.Close();
                            }
                            else
                            {
                                con.Close();
                                MessageBox.Show("注册失败！");
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("该用户名已存在！");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        con.Close();
                    }
                }
                //else
                //{
                //    MessageBox.Show("请选择您的身份!");
                //    con.Close();
                //}
            }
            catch
            {
                MessageBox.Show("数据库连接失败！");
            }
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
