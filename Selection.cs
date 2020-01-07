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
    public partial class Selection : Form
    {
        static string ss = "Server = localhost; database = 学生信息管理系统;Integrated Security = True";//integrated 各部分密切协调的；综合的；完整统一的
        SqlConnection con = new SqlConnection(ss);//连接数据库
        SqlCommand cmd;
        SqlDataAdapter adp;
        DataSet dat;
        SqlDataReader dr;
        public Selection()
        {
            InitializeComponent();
        }

        private void Selection_Load(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("选课名称");
            //dt.
            //dataGridView1.DataSource = dt;
            con.Open();
            string s = "select DISTINCT C.cno,cname,cdate,ctime,croom, cweek,tname from C join SC on SC.cno = C.cno join teacher on teacher.tno = SC.tno";
            cmd = new SqlCommand(s, con);
            dr = cmd.ExecuteReader();
            while(dr.Read())//在datagridview中添加数据
            {
                int i = dataGridView1.Rows.Add();
                //int j = 1;
                dataGridView1.Rows[i].Cells["序号"].Value = i + 1;
                dataGridView1.Rows[i].Cells["选课名称"].Value = dr["cname"].ToString().Trim();
                dataGridView1.Rows[i].Cells["老师"].Value = dr["tname"].ToString().Trim();
                dataGridView1.Rows[i].Cells["操作"].Value = "选课";
                dataGridView1.Rows[i].Cells["课程号"].Value = dr["cno"].ToString().Trim();
                dataGridView1.Rows[i].Cells["上课地点"].Value = dr["croom"].ToString().Trim();
                dataGridView1.Rows[i].Cells["上课时间"].Value = dr["cweek"].ToString().Trim() + "\n" + "\n" + dr["cdate"].ToString().Trim() + "\n" + "\n" + dr["ctime"].ToString().Trim();
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            con.Open();
            string s = String.Format("select tno from teacher_course where cno = '{0}' and cname = '{1}'", dataGridView1.Rows[e.RowIndex].Cells["课程号"].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            cmd = new SqlCommand(s, con);
            dr = cmd.ExecuteReader();
            dr.Read();
            
            if (dataGridView1.Columns[e.ColumnIndex].Name == "操作"&&e.RowIndex>=0)//点击button事件
            {
                MessageBox.Show("确认选课？");
                string sno = login.lg.textBox1.Text.Trim();
                string cno = dataGridView1.Rows[e.RowIndex].Cells["课程号"].Value.ToString();
                string tno = dr["tno"].ToString().Trim();
                con.Close();
                con.Open();
                string sss = String.Format("select cno from SC where sno = '{0}' and cno = '{1}'" ,sno,cno);
                cmd = new SqlCommand(sss,con);
                if(cmd.ExecuteScalar() == null)
                {
                    con.Close();
                    con.Open();
                    string ss = String.Format("insert into SC values('{0}','{1}','{2}','{3}')", sno, cno, tno, null);
                    cmd = new SqlCommand(ss, con);
                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        MessageBox.Show("选课成功！");
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("与现有课程发生冲突！");
                    con.Close();
                }
            }
            con.Close();
        }
    }
}
