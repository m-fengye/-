using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CollectHomework
{
    public partial class manager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {//加载课程和作业
                LoadDdlClassType();
                LoadDdlWorkTitle();
            }
        }

        /// <summary>
        /// 加载课程到下拉框
        /// </summary>
        private void LoadDdlClassType()
        {
            DataSet ds = DBHelper.SelectToDS("SelectClassType", CommandType.StoredProcedure, null);
            if (ds != null)
            {
                ddlClassType.Items.Clear();
                ddlClassType1.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string classId = ds.Tables[0].Rows[i]["ClassId"].ToString();
                    string className = ds.Tables[0].Rows[i]["ClassName"].ToString();
                    //同时加载两个下拉框
                    ddlClassType.Items.Add(new ListItem(className, classId));
                    ddlClassType1.Items.Add(new ListItem(className, classId));
                }
            }
        }

        /// <summary>
        /// 加载作业到下拉框
        /// </summary>
        private void LoadDdlWorkTitle()
        {
            SqlParameter[] parameters = { new SqlParameter("@ClassId", SqlDbType.Int) };
            if (ddlClassType.SelectedItem == null)//没有选中项，无需加载
            {
                return;
            }
            parameters[0].Value = ddlClassType.SelectedItem.Value;
            DataSet ds = DBHelper.SelectToDS("SelectWorkInfoByClassId", CommandType.StoredProcedure, parameters);
            if (ds != null)
            {
                //加载所有作业标题
                ddlWorkTitle.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string WorkId = ds.Tables[0].Rows[i]["WorkId"].ToString();
                    string WorkTitle = ds.Tables[0].Rows[i]["WorkTitle"].ToString();
                    ddlWorkTitle.Items.Add(new ListItem(WorkTitle, WorkId));
                }
            }
        }

        /// <summary>
        /// 添加课程
        /// </summary>
        protected void btnAddClassType_Click(object sender, EventArgs e)
        {
            //获取课程名并判断
            string className = txtClassName.Text.Trim();
            if (className == "")
            {
                Response.Write("<script>alert('请输入课程名')</script>");
                return;
            }
            //构造sql并插入
            SqlParameter[] parameters = { new SqlParameter("@ClassName", SqlDbType.NVarChar, 50) };
            parameters[0].Value = className;
            int rows = DBHelper.ExecuteSql("InsertClassType", CommandType.StoredProcedure, parameters);
            if (rows > 0)
            {
                Response.Write("<script>alert('添加课程成功')</script>");
                //清空并重新加载
                txtClassName.Text = "";
                LoadDdlClassType();
                LoadDdlWorkTitle();
            }
            else
            {
                //有可能是唯一约束导致
                Response.Write("<script>alert('添加课程失败，请确认该课程是否已经存在')</script>");
            }
        }

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelClassType_Click(object sender, EventArgs e)
        {
            if (ddlClassType1.SelectedItem == null)
            {//没选中
                return;
            }
            //获取选中项值并执行删除操作
            string classId = ddlClassType1.SelectedItem.Value;
            SqlParameter[] parameters = { new SqlParameter("@ClassId", SqlDbType.Int) };
            parameters[0].Value = classId;
            int rows = DBHelper.ExecuteSql("DeleteClassType", CommandType.StoredProcedure, parameters);
            if (rows > 0)
            {
                //影响行数大于0，删除成功
                Response.Write("<script>alert('删除课程成功')</script>");
                //重新加载
                txtClassName.Text = "";
                LoadDdlClassType();
                LoadDdlWorkTitle();
            }
            else
            {
                //外键约束，当课程下有作业任务时无法删除课程
                Response.Write("<script>alert('删除课程失败，请确认该课程下无作业')</script>");
            }
        }

        /// <summary>
        /// 添加作业
        /// </summary>
        protected void btnAddWorkTitle_Click(object sender, EventArgs e)
        {
            //获取值，判空
            if (ddlClassType.SelectedItem == null)
            {
                Response.Write("<script>alert('课程为空，请先添加课程')</script>");
                return;
            }
            string workTitle = txtWorkTitle.Text;
            if (workTitle == "")
            {
                Response.Write("<script>alert('请输入作业标题')</script>");
                return;
            }
            //三目运算，没有输入时返回1，输入则返回该数字，有可能发生异常，暂无处理
            int fileNum = txtFileNum.Text.Trim() == "" ? 1 : int.Parse(txtFileNum.Text.Trim());
            //构造参数，执行添加
            SqlParameter[] parameters = {
                    new SqlParameter("ClassId",SqlDbType.Int),
                    new SqlParameter("FileNum",SqlDbType.Int),
                    new SqlParameter("WorkTitle",SqlDbType.NVarChar,100)};
            parameters[0].Value = ddlClassType.SelectedItem.Value;
            parameters[1].Value = fileNum;
            parameters[2].Value = workTitle;
            int rows = DBHelper.ExecuteSql("InsertWorkInfo", CommandType.StoredProcedure, parameters);
            if (rows > 0)
            {
                Response.Write("<script>alert('添加作业成功')</script>");
                //添加成功，重置加载
                txtWorkTitle.Text = "";
                txtFileNum.Text = "";
                LoadDdlWorkTitle();
            }
            else
            {
                Response.Write("<script>alert('添加作业失败')</script>");
            }
        }

        /// <summary>
        /// 删除作业
        /// </summary>
        protected void btnDelWorkTitle_Click(object sender, EventArgs e)
        {
            //课程为空，说明没有课程
            if (ddlClassType.SelectedItem == null)
            {
                Response.Write("<script>alert('课程为空，请先添加课程')</script>");
                return;
            }
            //作业未空，说明没有作业
            if (ddlWorkTitle.SelectedItem == null)
            {
                Response.Write("<script>alert('作业为空，请先添加作业')</script>");
                return;
            }
            //构造参数，执行
            SqlParameter[] parameters = {
                    new SqlParameter("@WorkId",SqlDbType.Int),
                    new SqlParameter("@ClassId",SqlDbType.Int)};
            parameters[0].Value = ddlWorkTitle.SelectedItem.Value;
            parameters[1].Value = ddlClassType.SelectedItem.Value;
            int rows = DBHelper.ExecuteSql("DeleteWorkInfo", CommandType.StoredProcedure, parameters);
            if (rows > 0)
            {
                Response.Write("<script>alert('删除作业成功')</script>");
                //执行成功，重置加载
                txtWorkTitle.Text = "";
                txtFileNum.Text = "";
                LoadDdlWorkTitle();
            }
            else
            {
                //本有（作业-学生作业）外键，后不方便操作，删除
                Response.Write("<script>alert('删除作业失败')</script>");
            }
        }

        /// <summary>
        /// 当课程选中项改变时，加载作业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //调用加载作业方法
            LoadDdlWorkTitle();
        }
    }
}