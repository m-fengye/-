using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace CollectHomework
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {//首次加载，获取cookie
                HttpCookie cookies = Request.Cookies["UserInfo"];
                if (cookies != null)
                {//cookie为不空，显示在lable
                    string userId = cookies.Values["UserId"];
                    lblUserId.Text = userId;
                }
                else
                {//cookie为空，重定向到登陆
                    Response.Write("<script>window.location.href='login.aspx'</script>");
                }
                //获取课程信息
                DataSet ds = DBHelper.SelectToDS("SelectClassType", CommandType.StoredProcedure, null);
                if (ds != null)
                {
                    //清除下拉框并添加项目
                    ddlClassType.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string classId = ds.Tables[0].Rows[i]["ClassId"].ToString();
                        string className = ds.Tables[0].Rows[i]["ClassName"].ToString();
                        ddlClassType.Items.Add(new ListItem(className, classId));
                    }
                    //只有首次加载需要，调用选中项改变事件，加载所有作业，如果每次加载，则选中项永远为第一项
                    ddlClassType_SelectedIndexChanged(sender, e);
                }
            }
            //每次加载都要调用，加载上传控件
            ddlWorkTitle_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 课程选中项改变
        /// </summary>
        protected void ddlClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] parameters = { new SqlParameter("@ClassId", SqlDbType.Int) };
            if (ddlClassType.SelectedItem == null)
            {//无选中项，直接结束
                return;
            }
            //查选中课程的所有作业
            parameters[0].Value = ddlClassType.SelectedItem.Value;
            DataSet ds = DBHelper.SelectToDS("SelectWorkInfoByClassId", CommandType.StoredProcedure, parameters);
            if (ds != null)
            {
                //清除作业下拉框，并加载
                ddlWorkTitle.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string WorkId = ds.Tables[0].Rows[i]["WorkId"].ToString();
                    string WorkTitle = ds.Tables[0].Rows[i]["WorkTitle"].ToString();
                    ddlWorkTitle.Items.Add(new ListItem(WorkTitle, WorkId));
                }
                ddlWorkTitle_SelectedIndexChanged(sender, e);
            }
        }

        /// <summary>
        /// 作业选中项改变
        /// </summary>
        protected void ddlWorkTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] parameters = { new SqlParameter("@WorkId", SqlDbType.Int) };
            if (ddlWorkTitle.SelectedItem == null)
            {//选中项为空，直接返回
                return;
            }
            //查询需提交文件数
            parameters[0].Value = ddlWorkTitle.SelectedItem.Value;
            DataSet ds = DBHelper.SelectToDS("SelectFileNumByWorkId", CommandType.StoredProcedure, parameters);
            if (ds != null)
            {
                //动态生成控件并添加
                panelFile.Controls.Clear();
                for (int i = 0; i < int.Parse(ds.Tables[0].Rows[0]["FileNum"].ToString()); i++)
                {
                    Panel panel = new Panel();
                    panel.CssClass = "custom-file mb-3";
                    panel.Controls.Add(new LiteralControl("<input id='" + "file_upload" + i + "' name='" + "file_upload" + i + "' class='custom-file-input' type='file' />"));
                    panel.Controls.Add(new LiteralControl("<label class='custom-file-label'>上传第" + (i + 1) + "个文件:</label>"));
                    panelFile.Controls.Add(panel);
                }
            }
        }

        /// <summary>
        /// 提交作业
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int rows = 0;//影响数据库总行数
                //获取文件对象
                HttpFileCollection files = HttpContext.Current.Request.Files;
                //遍历所有文件，判断所有文件都合法后在保存
                for (int i = 0; i < files.Count; i++)//检测所有文件类型是否合法
                {
                    //拿出每个文件，并获取文件名
                    HttpPostedFile postedFile = files[i];
                    string fileName = Path.GetFileName(postedFile.FileName);
                    if (fileName == "")
                    {//为空说明未上传，需全部提交
                        Response.Write("<script>alert('有文件未提交，回去看一下吧')</script>");
                        return;
                    }
                    else if (!isType(fileName))//符合返回true，取反是false，不执行下面内容
                    {
                        //文件类型有误，防止上传木马文件
                        //如需修改，去isType方法更改数组即可
                        Response.Write("<script>alert('只允许上传.doc .docx .txt .rar .zip .png .jpg .jpeg .xls .xlsx')</script>");
                        return;
                    }
                }

                //查询是否已经提交
                if (isSubmit())
                {
                    Response.Write("<script>alert('已经提交过啦')</script>");
                    return;
                }

                //开始提交，遍历所有文件
                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    //获取文件名和保存路径
                    HttpPostedFile postedFile = files[iFile];
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string workUrl = Server.MapPath("~/upload/" + ddlClassType.SelectedItem.Text + "/" + ddlWorkTitle.SelectedItem.Text + "/");
                    //文件夹不存在，创建文件夹
                    if (!Directory.Exists(workUrl))
                    {
                        Directory.CreateDirectory(workUrl);
                    }
                    //保存文件
                    postedFile.SaveAs(workUrl + fileName);
                    //数据库处理
                    SqlParameter[] parameters2 = {
                                    new SqlParameter("@UserId",SqlDbType.NVarChar,50),
                                    new SqlParameter("@ClassId",SqlDbType.Int),
                                    new SqlParameter("@WorkId",SqlDbType.Int),
                                    new SqlParameter("@WorkUrl",SqlDbType.NVarChar,150)};
                    parameters2[0].Value = lblUserId.Text;
                    parameters2[1].Value = ddlClassType.SelectedItem.Value;
                    parameters2[2].Value = ddlWorkTitle.SelectedItem.Value;
                    parameters2[3].Value = workUrl + fileName;
                    //将每个文件信息插入数据库，并累加影响行数
                    rows += DBHelper.ExecuteSql("InsertStuWork", CommandType.StoredProcedure, parameters2);
                }
                //影响行数等于文件数，说明提交成功
                if (rows == files.Count)
                {
                    Response.Write("<script>alert('提交成功，请勿重复提交')</script>");
                }
                else
                {
                    throw new Exception("数据库错误");
                }
            }
            catch (HttpException ex)
            {
                Response.Write("<script>alert('网络异常，请检测重试')</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('数据库错误')</script>");
            }
        }

        /// <summary>
        /// 判断文件名是否符合规范，符合返回true
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>符合:true,不符合:false</returns>
        private bool isType(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string[] types = { ".doc", ".docx", ".txt", ".rar", ".zip", ".png", ".jpg", ".jpeg", ".xls", ".xlsx" };
            int count = 0;
            foreach (string type in types)
            {
                if (extension != type)
                {
                    count++;
                }
                else
                {
                    continue;
                }
            }
            return count == types.Length ? false : true;
        }

        /// <summary>
        /// 查询是否提交,已经提交:true
        /// </summary>
        /// <returns>已经提交:true，未提交:false</returns>
        private bool isSubmit()
        {
            //根据用户id，课程，作业查询
            SqlParameter[] parameters1 = {
                                new SqlParameter("@UserId",SqlDbType.NVarChar,50),
                                new SqlParameter("@ClassId",SqlDbType.Int),
                                new SqlParameter("@WorkId",SqlDbType.Int)};
            parameters1[0].Value = lblUserId.Text;
            parameters1[1].Value = ddlClassType.SelectedItem.Value;
            parameters1[2].Value = ddlWorkTitle.SelectedItem.Value;
            DataSet ds = DBHelper.SelectToDS("SelectStuWork", CommandType.StoredProcedure, parameters1);
            if (ds != null)
            {
                //查询到数据说明已经提交
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            //没有提交
            return false;
        }

    }
}