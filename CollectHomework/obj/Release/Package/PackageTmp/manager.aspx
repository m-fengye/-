<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manager.aspx.cs" Inherits="CollectHomework.manager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1,user-scalable=no" />
    <link rel="stylesheet" href="https://cdn.staticfile.org/twitter-bootstrap/4.3.1/css/bootstrap.min.css" />
    <script src="https://cdn.staticfile.org/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://cdn.staticfile.org/popper.js/1.15.0/umd/popper.min.js"></script>
    <script src="https://cdn.staticfile.org/twitter-bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <title>课程管理</title>
</head>
<body style="background-color: #b0d5df">
    <div class="container">
        <div class="row mt-3">
            <div class="col-10 col-sm-6 m-auto">
                <form id="form1" runat="server">
                    <div class="row" style="background-color: rgba(99,187,208,0.7); border-radius: 20px;">
                        <div class="col-12 col-sm-6 mt-3 mb-3 ml-auto mr-auto text-center">
                            <div class="mb-3">
                                <h4>课程管理:</h4>
                                <asp:DropDownList ID="ddlClassType1" runat="server" class="form-control"></asp:DropDownList>
                                <asp:TextBox ID="txtClassName" runat="server" class="form-control mt-3" placeholder="请输入课程名，删除时不用输入"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnAddClassType" runat="server" class="btn btn-primary" Text="添加课程" OnClick="btnAddClassType_Click" />
                            <asp:Button ID="btnDelClassType" runat="server" class="btn btn-primary ml-4" Text="删除课程" OnClick="btnDelClassType_Click" />
                        </div>
                    </div>
                    <div class="row mt-3" style="background-color: rgba(99,187,208,0.7); border-radius: 20px;">
                        <div class="col-12 col-sm-6 mt-3 mb-3 ml-auto mr-auto text-center">
                            <div class="mb-3">
                                <h4>作业任务管理:</h4>
                                <asp:DropDownList ID="ddlClassType" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlClassType_SelectedIndexChanged"></asp:DropDownList>
                                <asp:DropDownList ID="ddlWorkTitle" runat="server" class="form-control mt-3" AutoPostBack="False"></asp:DropDownList>
                                <asp:TextBox ID="txtWorkTitle" runat="server" class="form-control mt-3" placeholder="请输入作业标题，删除时不用输入"></asp:TextBox>
                                <asp:TextBox ID="txtFileNum" runat="server" class="form-control mt-3" placeholder="该作业需上传的文件数，默认为1"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnAddWorkTitle" runat="server" class="btn btn-primary" Text="添加任务" OnClick="btnAddWorkTitle_Click" />
                            <asp:Button ID="btnDelWorkTitle" runat="server" class="btn btn-primary ml-4" Text="删除任务" OnClick="btnDelWorkTitle_Click" />
                        </div>
                    </div>
                </form>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-10 col-sm-6 m-auto" style="background-color:rgba(99,187,208,0.7);border-radius: 20px;">
                <p class='text-center m-2'>垃圾小站，请勿测试</br><a href='http://beian.miit.gov.cn'>冀ICP备18039362号-2</a></p>
            </div>
        </div>
    </div>
</body>
</html>
