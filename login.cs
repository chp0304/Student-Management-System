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
using System.IO;
namespace 强智系统
{
    public partial class login : Form
    {
        int fff = 0;
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataSet dat;
        SqlDataReader dr;
        public static login lg;//静态变量
        public login()
        {
            InitializeComponent();
            lg = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stuinformation stu = new Stuinformation();//实例化学生
            Teainformation tea = new Teainformation();//实例化老师
            Administrator ad = new Administrator();//实例化管理员
            stu.label4.Text = textBox1.Text;//学号显示
            tea.label4.Text = textBox1.Text;//教工号显示
            int flag = 1;
            try
            {
                con.Open();
                string username = textBox1.Text.Trim();//用户名
                string pass = textBox2.Text.Trim();//密码
                string s1 = "";
                string s2 = "";
                if (fff == 1)//radioButton2.Checked
                {
                    s1 = "select * from login_T where 教工号= " + username + "and 密码 = " + pass;//SQL语句，教师端登录
                }
                else if (fff == 0)//radioButton1.Checked
                {
                    s1 = "select * from login_S where 学号= " + username + "and 密码 = " + pass;//SQL语句，学生端登录
                }
                else if (fff == 2)//radioButton1.Checked
                {
                    s1 = "select * from ADM where administrator = " + username + " and password = " + pass;//SQL语句，学生端登录
                }
                cmd = new SqlCommand(s1, con);//执行语句;
                try
                {
                    object obj = cmd.ExecuteScalar();//查询数据库有无所输数据的函数
                    int cnt = Convert.ToInt32(obj);//强制转换
                    if (cnt != 0)
                    {
                        MessageBox.Show("登录成功");
                    }
                    //con.Close();
                }
                catch (Exception ex)                           //异常处理，obj未在数据库中，此事为null
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("登陆失败\n账号或密码不正确！");
                    flag = 0;
                    con.Close();
                    return; //结束语句
                }
                // con.Open();
                if (fff == 1)//radioButton2.Checked
                {
                    s1 = "select * from login_T where 教工号= " + username + "and 密码 = " + pass;//SQL语句，教师端登录
                    s2 = "select tname,image from Teacher where tno= " + username;
                    cmd = new SqlCommand(s2, con);//获取查询值
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    //显示头像
                    byte[] btImage = (byte[])dr["image"];
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(btImage);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);//将二进制转换为流
                    tea.label3.Text = dr["tname"].ToString();
                    //dr.Read();
                    tea.pictureBox1.Image = image;
                }
                else if (fff == 0)//radioButton1.Checked
                {
                    s1 = "select * from login_S where 学号= " + username + "and 密码 = " + pass;//SQL语句，学生端登录
                    s2 = "select 姓名,image from S where 学号= " + username;
                    cmd = new SqlCommand(s2, con);
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    //显示头像
                    byte[] btImage = (byte[])dr["image"];
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(btImage);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);//将二进制转换为流
                    stu.label3.Text = dr["姓名"].ToString();
                    stu.pictureBox1.Image = image;
                }
                //else if (fff == 2)//radioButton1.Checked
                //{
                //    s1 = "select * from AD where administrator = " + username + "and password = " + pass;//SQL语句，学生端登录
                //    s2 = "select 姓名,image from S where 学号= " + username;
                //    cmd = new SqlCommand(s2, con);
                //    dr = cmd.ExecuteReader();
                //    dr.Read();
                //    //显示头像
                //    byte[] btImage = (byte[])dr["image"];
                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(btImage);
                //    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);//将二进制转换为流
                //    stu.label3.Text = dr["姓名"].ToString();
                //    stu.pictureBox1.Image = image;
                //}
                con.Close();//关闭con,便于下一次cmd的调用
                //con.Open();
            }
            catch
            {
                flag = 0;
                MessageBox.Show("数据库连接失败！");
                con.Close();
            }
            if(fff == 0&&flag==1)//学生登录//radioButton1.Checked
            {
                stu.Owner = this;
                stu.ShowDialog();
            }
            else if(fff == 1&&flag == 1)//教师登录//radioButton2.Checked
            {
                //Teainformation tea = new Teainformation();
                tea.Owner = this;
                tea.ShowDialog();
            }
            else if (fff == 2 && flag == 1)//管理员登录//radioButton2.Checked
            {
                //Teainformation tea = new Teainformation();
                ad.Owner = this;
                ad.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)//注册用户
        {
            con.Close();
            adduser add = new adduser();
            add.Owner = this;
            add.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)//找回密码
        {
            Find find = new Find();
            find.Owner = this;
            find.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem.ToString() =="我是学生")
            {
                fff = 0;
            }
            else if(comboBox1.SelectedItem.ToString() == "我是老师")
            {
                fff = 1;
            }
            else if(comboBox1.SelectedItem.ToString() == "我是管理员")
            {
                fff = 2;
            }
        }
    }
}
