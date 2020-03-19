<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CollectHomework.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1,user-scalable=no" />
    <link rel="stylesheet" href="https://cdn.staticfile.org/twitter-bootstrap/4.3.1/css/bootstrap.min.css" />
    <script src="https://cdn.staticfile.org/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://cdn.staticfile.org/popper.js/1.15.0/umd/popper.min.js"></script>
    <script src="https://cdn.staticfile.org/twitter-bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <title>登陆页</title>
</head>
<body style="background-color:#b0d5df">
    <div class="container">
        <div class="row mt-5">
            <div class="col-10 col-sm-6 m-auto">
                <form id="form1" runat="server">
                    <div class="row" style="background-color:rgba(99,187,208,0.7);border-radius: 20px;">
                        <div class="col-12 col-sm-6 mt-3 mb-3 ml-auto mr-auto">
                            <h2 class="text-center">作业提交系统</h2>
                            <div class="form-group">
                                <label>学号:</label>
                                <asp:TextBox ID="txtUserId" runat="server" class="form-control" placeholder="请输入学号"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>密码:</label>
                                <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password" placeholder="请输入密码"></asp:TextBox>
                            </div>
                            <div class="form-check">
                                <label class="form-check-label">
                                    <input id="RememberMe" runat="server" class="form-check-input" type="checkbox" checked="checked"/>
                                    下次自动登陆
                                </label>
                            </div>
                            <asp:Button ID="btnLogin" class="btn btn-primary m-auto col-12" runat="server" Text="登陆" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </form>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-10 col-sm-6 m-auto" style="background-color:rgba(99,187,208,0.7);border-radius: 20px;">
                <p class='text-center m-2'>垃圾小站，请勿测试<br/><a href='http://beian.miit.gov.cn'>冀ICP备XXXXXXXX号</a></p>
            </div>
        </div>
    </div>
</body>
</html>
