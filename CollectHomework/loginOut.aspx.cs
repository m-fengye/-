using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CollectHomework
{
    public partial class loginOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取cookie
            HttpCookie cookies = Request.Cookies["UserInfo"];
            if (cookies != null)
            {//获取到cookie后删除
                cookies.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookies);
            }
            //重定向
            Response.Write("<script>window.location.href='login.aspx'</script>");
        }
    }
}