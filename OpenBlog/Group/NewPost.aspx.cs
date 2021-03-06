﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DLL;

public partial class Group_NewPost : UserPage
{
    protected int groupID;
    protected string groupName;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["q"] != null)
            {
                int.TryParse(Request.QueryString["q"], out groupID);
            }
            if (groupID < 10000)
            {
                Server.Transfer("~/note.aspx?q=Notfound");
            }
            
            GetBlog();
        }
    }

    private void GetBlog()
    {
        string sql = "select [g_name] from [group] where [g_id]={0};select count(*) from [group_member] where [gm_g_id]={0} and [gm_user_name]='{1}';";
        sql = String.Format(sql, groupID, CKUser.Username);
        DataSet ds = DB.GetDataSet(sql);

        if (ds.Tables[0].Rows.Count==0)
        {
            Server.Transfer("~/note.aspx?q=Notfound");
        }
        if ((int)ds.Tables[1].Rows[0][0] == 0)
        {
            Tools.PrintScript(Page, "alert('只有群组成员才有发贴权限。');window.history.go(-1);");
        }
        groupName = ds.Tables[0].Rows[0][0].ToString();
    }
}
