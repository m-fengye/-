<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CollectHomework.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1,user-scalable=no" />
    <link rel="stylesheet" href="https://cdn.staticfile.org/twitter-bootstrap/4.3.1/css/bootstrap.min.css" />
    <script src="https://cdn.staticfile.org/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://cdn.staticfile.org/popper.js/1.15.0/umd/popper.min.js"></script>
    <script src="https://cdn.staticfile.org/twitter-bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="dist/bs-custom-file-input.js"></script>
    <title>上传文件</title>
</head>
<body style="background-color: #b0d5df">
    <div class="container">
        <div class="row mt-5">
            <div class="col-10 col-sm-6 m-auto" style="background-color:rgba(99,187,208,0.7);border-radius: 20px;">
                <form id="form1" runat="server" method="post" enctype="multipart/form-data">
                    <label class="col-12 mt-4 text-center">你的学号为：
                        <asp:Label ID="lblUserId" runat="server" ></asp:Label>
                        <a href="loginOut.aspx" class="">退出登陆</a>
                    </label>
                    <label class="ml-2 mt-3">课程:</label>
                    <asp:DropDownList ID="ddlClassType" class="form-control" runat="server" OnSelectedIndexChanged="ddlClassType_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static"></asp:DropDownList>
                    <label class="ml-2 mt-3">作业:</label>
                    <asp:DropDownList ID="ddlWorkTitle" class="form-control" runat="server" OnSelectedIndexChanged="ddlWorkTitle_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static"></asp:DropDownList>
                    <label class="ml-2 mt-3">文件:</label>
                    <asp:Panel ID="panelFile" runat="server"></asp:Panel>
                    <asp:Button ID="btnSubmit" runat="server" class="btn btn-primary col-12 mb-4" Text="提交作业" OnClick="btnSubmit_Click" />
                </form>

            </div>
        </div>
        <div class="row mt-3">
            <div class="col-10 col-sm-6 m-auto" style="background-color:rgba(99,187,208,0.7);border-radius: 20px;">
                <p class='text-center m-2'>垃圾小站，请勿测试<br/><a href='http://beian.miit.gov.cn'>冀ICP备XXXXXXXX号</a></p>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            //添加选中文件后显示文件名
            bsCustomFileInput.init();
		});
	</script>
</body>
</html>
