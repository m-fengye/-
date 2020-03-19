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
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {//首次加载，有cookie获取，登陆
                HttpCookie cookies = Request.Cookies["UserInfo"];
                if (cookies != null)
                {
                    string userId = cookies.Values["UserId"];
                    string password = cookies.Values["Password"];
                    Login(userId, password);
                }
            }
        }
        /// <summary>
        /// 登陆
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //获取信息
            string userId = txtUserId.Text.Trim();
            string password = txtPassword.Text.Trim();
            //判空
            if (userId == "" || password == "")
            {
                Response.Write("<script>alert('请输入账号密码')</script>");
                return;
            }
            //调用登陆，使用时需要加上md5，毕竟盗库还是有可能滴
            Login(userId, password);
        }

        /// <summary>
        /// 登陆方法
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        private void Login(string userId,string password)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@UserId",SqlDbType.NVarChar,50),
                new SqlParameter("@Password",SqlDbType.NVarChar,50)};
            parameters[0].Value = userId;
            parameters[1].Value = password;
            DataSet ds = DBHelper.SelectToDS("SelectUser", CommandType.StoredProcedure, parameters);
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {//登陆成功，设置cookie
                HttpCookie Cookie = new HttpCookie("UserInfo");
                Cookie.Values.Add("UserId", userId);
                Cookie.Values.Add("Password", password);
                if (RememberMe.Checked)
                {//选择下次自动登陆，存cookie
                    Cookie.Expires = DateTime.Now.AddDays(14d);
                }
                Response.Cookies.Add(Cookie);
                Response.Write("<script>window.location.href='index.aspx'</script>");
            }
            else
            {
                Response.Write("<script>alert('登陆失败')</script>");
            }
        }
    }
}