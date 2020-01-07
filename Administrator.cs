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
    public partial class Administrator : Form
    {
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        SqlDataAdapter adp,adp1;
        DataSet dat;
        DataTable dt,dt1,dt2,dt3;
        SqlDataReader dr;
        TreeNode Main;//主节点
        TreeNode stu;//学生节点
        TreeNode tea;//教师节点
        public Administrator()
        {
            InitializeComponent();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)//学生更新
        {
            string sno = textBox1.Text.Trim();
            string name = textBox2.Text.Trim();
            string sex = textBox3.Text.Trim();
            string sage = textBox4.Text.Trim();
            string special = textBox5.Text.Trim();
            string college = textBox6.Text.Trim();
            string year = textBox7.Text.Trim();
            string cla = textBox9.Text.Trim();
            string phone = textBox8.Text.Trim();
            con.Close();
            con.Open();
            if (textBox2.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox2, "姓名不能为空！");
                return;
            }
            else if (textBox1.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox1, "学号不能为空！");
                return;
            }
            if (treeView1.SelectedNode.Text == "学生" || treeView1.SelectedNode.Parent.Text == "学生")
            {
                //sqlcommand.ExecuteNonQuery() 执行select之后，总是返回-1的
                //sqlcommand.ExecuteNonQuery()返回的是受影响的行数，只对update，delete，insert 有效
                string s = "select * from S where 学号 =" + textBox1.Text.Trim();
                cmd = new SqlCommand(s, con);
                if (cmd.ExecuteScalar() == null)
                {
                    errorProvider1.SetError(textBox1, "此学号不存在，不能更新只能添加，请重新输入！");
                    return;
                }
                else
                {
                    string pOldName = dataGridView2.Rows[0].Cells["姓名"].Value.ToString().Trim();
                    con.Close();
                    con.Open();
                    s = String.Format("update S set 学号 = '{0}',姓名 = '{1}',性别 = '{2}',班级 = '{3}',年龄 = '{4}',专业 = '{5}',学院 = '{6}',年级 = '{7}',电话 = '{8}' where 学号 = '{9}'", sno, name, sex, cla, sage, special, college, year, phone, sno);
                    cmd = new SqlCommand(s,con);
                    int cnt = cmd.ExecuteNonQuery();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        //实时更新
                        for (int i = 0; i < stu.Nodes.Count; i++)
                        {
                            if (stu.Nodes[i].Text.Trim() == pOldName)
                            {
                                stu.Nodes[i].Text = textBox2.Text;
                                break;
                            }
                        }
                        con.Close();
                        con.Open();
                        string sql = "select 学号,姓名,性别,班级,年龄,专业,学院,年级,电话 from S where 学号 = " + "'" + sno + "'";
                        adp = new SqlDataAdapter(sql,con);
                        dt2 = new DataTable();
                        adp.Fill(dt2);
                        dataGridView2.DataSource = dt2;
                        con.Close();
                        MessageBox.Show("更新成功！");
                    }
                    else
                    {
                        MessageBox.Show("更新失败！");
                    }
                }
            }
            con.Close();
        }
        private void button3_Click(object sender, EventArgs e)//学生删除
        {
            con.Close();
            con.Open();
            MessageBox.Show("确认删除？");
            if (treeView1.SelectedNode.Text == "学生" || treeView1.SelectedNode.Parent.Text == "学生")
            {
                if (dataGridView2.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请先选中一行信息！");
                    return;
                }
                else
                {
                    if (MessageBox.Show("确定删除选中行的信息吗?") == System.Windows.Forms.DialogResult.OK)
                    {
                        //TreeNode stu_1 = new TreeNode(dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
                        string s = "delete from S where 学号 = " + "'" + dataGridView2.SelectedRows[0].Cells[0].Value.ToString ()  + "'";
                        cmd = new SqlCommand(s,con);
                        if(cmd.ExecuteNonQuery() > 0)
                        {
                            for (int i = 0; i < stu.Nodes.Count; i++)
                            {
                                if (stu.Nodes[i].Text.Trim() == dataGridView2.SelectedRows[0].Cells[1].Value.ToString().Trim())
                                {
                                    stu.Nodes.Remove(stu.Nodes[i]);
                                    break;
                                }
                            }
                            MessageBox.Show("删除成功!");
                        }
                        else
                        {
                            MessageBox.Show("删除失败！");
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)//学生查找
        {
            con.Close();
            con.Open();
            string sno = textBox1.Text.Trim();
            string name = textBox2.Text.Trim();
            string sex = textBox3.Text.Trim();
            string sage = textBox4.Text.Trim();
            string special = textBox5.Text.Trim();
            string college = textBox6.Text.Trim();
            string year = textBox7.Text.Trim();
            string cla = textBox9.Text.Trim();
            string phone = textBox8.Text.Trim();
            //if (treeView1.SelectedNode.Text == "学生" || treeView1.SelectedNode.Parent.Text == "学生")
            //{
                string s = "select * from S where ";
                if (textBox2.Text != "") s = s + " 姓名 like '%" + name + "%' and ";
                if (textBox1.Text != "") s = s + " 学号 like '%" + sno + "%' and ";
                if (textBox9.Text != "") s = s + " 班级 like '%" + cla + "%' and";
                if (textBox3.Text != "") s = s + " 性别 like '%" + sex + "%' and ";
                if (textBox5.Text != "") s = s + " 专业 like '%" + special + "%' and ";
                if (textBox6.Text != "") s = s + " 学院 like '%" + college + "%' and ";
                if (textBox7.Text != "") s = s + " 年级 like '%" + year + "%' and ";
                if (textBox8.Text != "") s = s + " 电话 like '%" + phone + "%' and ";
                if (textBox4.Text != "") s = s + " 年龄 like '%" + sage + "%' and ";
                s = s.Substring(0, s.Length - 4).Trim();
                adp = new SqlDataAdapter(s,con);
                dt3 = new DataTable();
                adp.Fill(dt3);
                dataGridView2.DataSource = dt3;
            //}
            con.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView2_RowHeaderCellChanged(object sender, DataGridViewRowEventArgs e)
        {
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)//datagridview2点击事件
        {
            
            if (e.RowIndex >= 0)
            {
                tabControl1.SelectedIndex = 0;
                textBox1.Text = dataGridView2.Rows[e.RowIndex].Cells["学号"].Value.ToString();
                textBox2.Text = dataGridView2.Rows[e.RowIndex].Cells["姓名"].Value.ToString();
                textBox3.Text = dataGridView2.Rows[e.RowIndex].Cells["性别"].Value.ToString();
                textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells["年龄"].Value.ToString();
                textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells["专业"].Value.ToString();
                textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells["学院"].Value.ToString();
                textBox7.Text = dataGridView2.Rows[e.RowIndex].Cells["年级"].Value.ToString();
                textBox8.Text = dataGridView2.Rows[e.RowIndex].Cells["电话"].Value.ToString();
                textBox9.Text = dataGridView2.Rows[e.RowIndex].Cells["班级"].Value.ToString();
                byte[] btImage = (byte[])dataGridView2.Rows[e.RowIndex].Cells["image"].Value;
                System.IO.MemoryStream ms = new System.IO.MemoryStream(btImage);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);//将二进制转换为流
                pictureBox2.Image = image;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)//教师删除
        {
            con.Close();
            con.Open();
            MessageBox.Show("确认删除？");
            if (treeView1.SelectedNode.Text == "老师" || treeView1.SelectedNode.Parent.Text == "老师")
            {
                if (dataGridView1.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请先选中一行信息！");
                    return;
                }
                else
                {
                    if (MessageBox.Show("确定删除选中行的信息吗?") == System.Windows.Forms.DialogResult.OK)
                    {
                        //TreeNode stu_1 = new TreeNode(dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
                        string s = "delete from Teacher where tno = " + "'" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'";
                        cmd = new SqlCommand(s, con);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            for (int i = 0; i < tea.Nodes.Count; i++)
                            {
                                if (tea.Nodes[i].Text.Trim() == dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim())
                                {
                                    tea.Nodes.Remove(tea.Nodes[i]);
                                    break;
                                }
                            }
                            MessageBox.Show("删除成功!");
                        }
                        else
                        {
                            MessageBox.Show("删除失败！");
                        }
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)//教师添加
        {
            pictureBox2.Load(Application.StartupPath + "\\默认头像.jpg");//默认头像
            //textBox10.Text = "";
            //textBox11.Text = "";
            //textBox12.Text = "";
            //textBox13.Text = "";
            //textBox14.Text = "";
            //textBox15.Text = "";
            //textBox16.Text = "";
            string tno = textBox10.Text.Trim();
            string tname = textBox11.Text.Trim();
            string tsex = textBox12.Text.Trim();
            string tage = textBox13.Text.Trim();
            string tcollege = textBox14.Text.Trim();
            string tclass = textBox15.Text.Trim();
            string tphone = textBox16.Text.Trim();
            con.Close();
            con.Open();
            if (textBox11.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox2, "姓名不能为空！");
                return;
            }
            else if (textBox10.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox1, "教工号不能为空！");
                return;
            }
            if (treeView1.SelectedNode.Text == "老师" || treeView1.SelectedNode.Parent.Text == "老师")
            {
                //sqlcommand.ExecuteNonQuery() 执行select之后，总是返回-1的
                //sqlcommand.ExecuteNonQuery()返回的是受影响的行数，只对update，delete，insert 有效
                string s = "select * from Teacher where tno =" + textBox10.Text.Trim();
                cmd = new SqlCommand(s, con);
                if (cmd.ExecuteScalar() != null)
                {
                    errorProvider1.SetError(textBox10, "此学号已经存在，不能重复，请重新输入！");
                    return;
                }
                else
                {
                    con.Close();
                    con.Open();
                    s = String.Format("insert into Teacher values( '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", tno, tname, tsex, tage, tcollege, tclass,tphone, " ");
                    cmd = new SqlCommand(s, con);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        Image picture = pictureBox1.Image;
                        MemoryStream ms = new MemoryStream();
                        picture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);//转换成数据流 
                        byte[] bPicture = ms.GetBuffer();
                        string ss = "Server = localhost;database = 学生信息管理系统;Integrated Security = True";
                        SqlConnection con = new SqlConnection(ss);
                        //string sql = "insert into Teacher(image) values(@image)";
                        string sql = "update Teacher set image = @image where tno =  " + textBox10.Text.Trim();
                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.Add("@image", SqlDbType.Image);
                        cmd.Parameters["@image"].Value = bPicture;
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            //MessageBox.Show("修改成功!");
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }
                        finally
                        {
                            con.Close();
                        }
                        
                        TreeNode tea_1 = new TreeNode(tname);
                        tea.Nodes.Add(tea_1);
                        MessageBox.Show("添加成功！");
                    }
                    else
                    {
                        MessageBox.Show("添加失败！");
                    }
                }
            }
            con.Close();
        }

        private void button9_Click(object sender, EventArgs e)//更新事件
        {
            string tno = textBox10.Text.Trim();
            string tname = textBox11.Text.Trim();
            string tsex = textBox12.Text.Trim();
            string tage = textBox13.Text.Trim();
            string tcollege = textBox14.Text.Trim();
            string tclass = textBox15.Text.Trim();
            string tphone = textBox16.Text.Trim();
            con.Close();
            con.Open();
            if (textBox11.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox2, "姓名不能为空！");
                return;
            }
            else if (textBox10.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox1, "学号不能为空！");
                return;
            }
            if (treeView1.SelectedNode.Text == "老师" || treeView1.SelectedNode.Parent.Text == "老师")
            {
                //sqlcommand.ExecuteNonQuery() 执行select之后，总是返回-1的
                //sqlcommand.ExecuteNonQuery()返回的是受影响的行数，只对update，delete，insert 有效
                string s = "select * from Teacher where tno =" + textBox10.Text.Trim();
                cmd = new SqlCommand(s, con);
                if (cmd.ExecuteScalar() == null)
                {
                    errorProvider1.SetError(textBox10, "此学号不存在，不能更新只能添加，请重新输入！");
                    return;
                }
                else
                {
                    string oldname = dataGridView1.Rows[0].Cells["tname"].Value.ToString().Trim();
                    con.Close();
                    con.Open();
                    s = String.Format("update Teacher set tno = '{0}',tname = '{1}',tsex = '{2}',tage = '{3}',tcollege = '{4}',tclass = '{5}',tphone = '{6}' where tno = '{7}'", tno, tname, tsex, tage, tcollege, tclass, tphone, tno);
                    cmd = new SqlCommand(s, con);
                    int cnt = cmd.ExecuteNonQuery();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        //实时更新
                        for(int i = 0;i<tea.Nodes.Count;i++)
                        {
                            if(tea.Nodes[i].Text.Trim() == oldname)
                            {
                                tea.Nodes[i].Text = textBox11.Text;
                                break;
                            }
                        }
                        con.Close();
                        con.Open();
                        string sql = "select tno,tname,tsex,tage,tcollege,tclass,tphone,tno from Teacher where tno = " + "'" + tno + "'";
                        adp = new SqlDataAdapter(sql, con);
                        dt2 = new DataTable();
                        adp.Fill(dt2);
                        dataGridView1.DataSource = dt2;
                        con.Close();
                        MessageBox.Show("更新成功！");
                    }
                    else
                    {
                        MessageBox.Show("更新失败！");
                    }
                }
            }
            con.Close();
        }

        private void button6_Click(object sender, EventArgs e)//学生查询
        {
            con.Close();
            con.Open();
            string tno = textBox10.Text.Trim();
            string tname = textBox11.Text.Trim();
            string tsex = textBox12.Text.Trim();
            string tage = textBox13.Text.Trim();
            string tcollege = textBox14.Text.Trim();
            string tclass = textBox15.Text.Trim();
            string tphone = textBox16.Text.Trim();
            //if (treeView1.SelectedNode.Text == "老师" || treeView1.SelectedNode.Parent.Text == "老师")
            //{
            //模糊查找
            string s = "select * from Teacher where ";
            if (textBox10.Text != "") s = s + " tno like '%" + tno + "%' and ";
            if (textBox11.Text != "") s = s + " tname like '%" + tname + "%' and ";
            if (textBox12.Text != "") s = s + " tsex like '%" + tsex + "%' and";
            if (textBox13.Text != "") s = s + " tage like '%" + tage + "%' and ";
            if (textBox14.Text != "") s = s + " tcollege like '%" + tcollege + "%' and ";
            if (textBox15.Text != "") s = s + " tclass like '%" + tclass + "%' and ";
            if (textBox16.Text != "") s = s + " tphone like '%" + tphone + "%' and ";
            s = s.Substring(0, s.Length - 4).Trim();
            adp = new SqlDataAdapter(s, con);
            dt3 = new DataTable();
            adp.Fill(dt3);
            dataGridView1.DataSource = dt3;
            //}
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)//datagridview1点击事件
        {
            //string tno = textBox10.Text.Trim();
            //string tname = textBox11.Text.Trim();
            //string tsex = textBox12.Text.Trim();
            //string tage = textBox13.Text.Trim();
            //string tcollege = textBox14.Text.Trim();
            //string tclass = textBox15.Text.Trim();
            //string tphone = textBox16.Text.Trim();
            if (e.RowIndex >= 0)
            {
                tabControl1.SelectedIndex = 1;
                textBox10.Text = dataGridView1.Rows[e.RowIndex].Cells["tno"].Value.ToString();
                textBox11.Text = dataGridView1.Rows[e.RowIndex].Cells["tname"].Value.ToString();
                textBox12.Text = dataGridView1.Rows[e.RowIndex].Cells["tsex"].Value.ToString();
                textBox13.Text = dataGridView1.Rows[e.RowIndex].Cells["tage"].Value.ToString();
                textBox14.Text = dataGridView1.Rows[e.RowIndex].Cells["tcollege"].Value.ToString();
                textBox15.Text = dataGridView1.Rows[e.RowIndex].Cells["tclass"].Value.ToString();
                textBox16.Text = dataGridView1.Rows[e.RowIndex].Cells["tphone"].Value.ToString();
                byte[] btImage = (byte[])dataGridView1.Rows[e.RowIndex].Cells["image"].Value;
                System.IO.MemoryStream ms = new System.IO.MemoryStream(btImage);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);//将二进制转换为流
                pictureBox1.Image = image;
            }
        }

        private void button11_Click(object sender, EventArgs e) //学生修改头像
        {
            if(treeView1.SelectedNode.Text == "学生" || treeView1.SelectedNode.Parent.Text == "学生")
            {
                OpenFileDialog open = new OpenFileDialog(); // 打开文件，选择头像
                open.ShowDialog();
                string path = open.FileName;
                pictureBox2.Load(path);//显示所选头像
                //储存二进制头像到数据库
                Image picture = pictureBox2.Image;
                MemoryStream ms = new MemoryStream();
                picture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);//转换成数据流 
                byte[] bPicture = ms.GetBuffer();//注意这一条与下两条语句的区别
                string ss = "Server = localhost;database = 学生信息管理系统;Integrated Security = True";
                SqlConnection con = new SqlConnection(ss);
                //string sql = "insert into Teacher(image) values(@image)";
                string sql = "update S set image = @image where 学号 =  " + textBox1.Text.Trim();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@image", SqlDbType.Image);
                cmd.Parameters["@image"].Value = bPicture;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("修改成功!");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e) //老师修改头像
        {
            if (treeView1.SelectedNode.Text == "老师" || treeView1.SelectedNode.Parent.Text == "老师")
            {
                OpenFileDialog open = new OpenFileDialog(); // 打开文件，选择头像
                open.ShowDialog();
                string path = open.FileName;
                pictureBox1.Load(path);//显示所选头像
                //储存二进制头像到数据库
                Image picture = pictureBox1.Image;
                MemoryStream ms = new MemoryStream();
                picture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);//转换成数据流 
                byte[] bPicture = ms.GetBuffer();//注意这一条与下两条语句的区别
                string ss = "Server = localhost;database = 学生信息管理系统;Integrated Security = True";
                SqlConnection con = new SqlConnection(ss);
                //string sql = "insert into Teacher(image) values(@image)";
                string sql = "update Teacher set image = @image where tno =  " + textBox10.Text.Trim();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@image", SqlDbType.Image);
                cmd.Parameters["@image"].Value = bPicture;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("修改成功!");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)//退出
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) //添加学生信息
        {
            pictureBox2.Load(Application.StartupPath + "\\默认头像.jpg");//默认头像
            //文本框置空
            //textBox1.Text = "";
            //textBox2.Text = "";
            //textBox3.Text = "";
            //textBox4.Text = "";
            //textBox5.Text = "";
            //textBox6.Text = "";
            //textBox7.Text = "";
            //textBox8.Text = "";
            //textBox9.Text = "";
            string sno = textBox1.Text.Trim();
            string name = textBox2.Text.Trim();
            string sex = textBox3.Text.Trim();
            string sage = textBox4.Text.Trim();
            string special = textBox5.Text.Trim();//专业
            string college = textBox6.Text.Trim();//学院
            string year = textBox7.Text.Trim();//年级
            string cla = textBox9.Text.Trim();
            string phone = textBox8.Text.Trim();
            con.Close();
            con.Open();
            if (textBox2.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox2, "姓名不能为空！");
                return;
            }
            else if (textBox1.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox1, "学号不能为空！");
                return;
            }
            if(treeView1.SelectedNode.Text == "学生" || treeView1.SelectedNode.Parent.Text == "学生")
            {
                //sqlcommand.ExecuteNonQuery() 执行select之后，总是返回-1的
                //sqlcommand.ExecuteNonQuery()返回的是受影响的行数，只对update，delete，insert 有效
                string s = "select * from S where 学号 =" + textBox1.Text.Trim();
                cmd = new SqlCommand(s,con);
                if(cmd.ExecuteScalar() != null )
                {
                    errorProvider1.SetError(textBox1, "此学号已经存在，不能重复，请重新输入！");
                    return;
                }
                else
                {
                    con.Close();
                    con.Open();
                    s = String.Format("insert into S values( '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",sno,name,sex,cla,sage,special,college,year,phone, "");
                    cmd = new SqlCommand(s,con);
                    if(cmd.ExecuteNonQuery()>0)
                    {
                        //默认头像
                        Image picture = pictureBox2.Image;
                        MemoryStream ms = new MemoryStream();
                        picture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);//转换成数据流 
                        byte[] bPicture = ms.GetBuffer();
                        string ss = "Server = localhost;database = 学生信息管理系统;Integrated Security = True";
                        SqlConnection con = new SqlConnection(ss);
                        //string sql = "insert into Teacher(image) values(@image)";
                        string sql = "update S set image = @image where 学号 =  " + textBox1.Text.Trim();
                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.Add("@image", SqlDbType.Image);
                        cmd.Parameters["@image"].Value = bPicture;
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            //MessageBox.Show("修改成功!");
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                        }
                        finally
                        {
                            con.Close();
                        }
                        TreeNode stu_1 = new TreeNode(name);
                        stu.Nodes.Add(stu_1);
                        MessageBox.Show("添加成功！");
                    }
                    else
                    {
                        MessageBox.Show("添加失败！");
                    }
                }
            }
            con.Close();
        }

        private void Administrator_Load(object sender, EventArgs e)//添加节点
        {
            Main = new TreeNode("学生信息管理系统");//主节点
            Main = treeView1.Nodes[0];
            stu = new TreeNode("学生");//学生节点
            tea = new TreeNode("老师");//教师节点
            Main.Nodes.Add(stu);
            Main.Nodes.Add(tea);
            con.Open();
            string s_stu = "select * from S ";
            dat = new DataSet();
            //dat.Tables[0] = new DataTable();
            adp = new SqlDataAdapter(s_stu, con);
            adp.Fill(dat);
            for(int i=0;i<dat.Tables[0].Rows.Count;i++) //添加学生
            {
                TreeNode stu_1 = new TreeNode(dat.Tables[0].Rows[i]["姓名"].ToString());
                stu.Nodes.Add(stu_1);
            }
            con.Close();
            con.Open();
            string s_tea = "select * from Teacher";
            adp = new SqlDataAdapter(s_tea,con);
            dt = new DataTable();
            adp.Fill(dt);
            for(int j=0;j<dt.Rows.Count;j++) // 添加教师
            {
                TreeNode tea_1 = new TreeNode(dt.Rows[j]["tname"].ToString());
                tea.Nodes.Add(tea_1);
            }
            con.Close();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)//tree选择事件
        {
            con.Close();
            con.Open();
            dt1 = new DataTable();
            if(e.Node.Parent != null && e.Node.Parent.Text == "学生")
            {
                tabControl1.SelectedIndex = 0;
                string s = "select 学号,姓名,性别,班级,年龄,专业,学院,年级,电话,image from S where 姓名 =" + "'" + e.Node.Text.ToString().Trim() + "'";
                adp1 = new SqlDataAdapter(s,con);
                adp1.Fill(dt1);
                dataGridView2.DataSource = dt1;
                textBox1.Text = dt1.Rows[0]["学号"].ToString();
                textBox2.Text = dt1.Rows[0]["姓名"].ToString();
                textBox3.Text = dt1.Rows[0]["性别"].ToString();
                textBox4.Text = dt1.Rows[0]["年龄"].ToString();
                textBox5.Text = dt1.Rows[0]["专业"].ToString();
                textBox6.Text = dt1.Rows[0]["学院"].ToString();
                textBox7.Text = dt1.Rows[0]["年级"].ToString();
                textBox8.Text = dt1.Rows[0]["电话"].ToString();
                textBox9.Text = dt1.Rows[0]["班级"].ToString();
                //显示头像
                byte[] btImage = (byte[])dt1.Rows[0]["image"];
                System.IO.MemoryStream ms = new System.IO.MemoryStream(btImage);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);//将二进制转换为流
                pictureBox2.Image = image;
            }
            else if(e.Node.Parent != null && e.Node.Parent.Text == "老师")
            {
                tabControl1.SelectedIndex = 1;
                string s = "select tno,tname,tsex,tage,tcollege,tclass,tphone,image from teacher where tname = " + "'" + e.Node.Text.ToString().Trim() + "'";
                adp1 = new SqlDataAdapter(s, con);
                adp1.Fill(dt1);
                dataGridView1.DataSource = dt1;
                textBox10.Text = dt1.Rows[0]["tno"].ToString();
                textBox11.Text = dt1.Rows[0]["tname"].ToString();
                textBox12.Text = dt1.Rows[0]["tsex"].ToString();
                textBox13.Text = dt1.Rows[0]["tage"].ToString();
                textBox14.Text = dt1.Rows[0]["tcollege"].ToString();
                textBox15.Text = dt1.Rows[0]["tclass"].ToString();
                textBox16.Text = dt1.Rows[0]["tphone"].ToString();
                //textBox8.Text = dt1.Rows[0]["电话"].ToString();
                byte[] btImage = (byte[])dt1.Rows[0]["image"];
                System.IO.MemoryStream ms = new System.IO.MemoryStream(btImage);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);//将二进制转换为流
                pictureBox1.Image = image;
            }
        }
    }
}
