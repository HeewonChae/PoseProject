using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pose_sports_statistics.Pages
{
	public partial class Basketball : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack)
				return;
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			if (cbDate.Checked)
			{
				Label1.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
				Label2.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
			}
			else
			{
				Label1.Text = DateTime.Now.ToLongTimeString();
				Label2.Text = DateTime.Now.ToLongTimeString();
			}
		}
		protected void Button2_Click(object sender, EventArgs e)
		{
			if (cbDate.Checked)
			{
				Label1.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
			}
			else
			{
				Label1.Text = DateTime.Now.ToLongTimeString();
			}
		}
		protected void cbDate_CheckedChanged(object sender, EventArgs e)
		{
			cbDate.Font.Bold = cbDate.Checked;
		}
		protected void ddlColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			Color c = Color.FromName(ddlColor.SelectedValue);
			Label2.ForeColor = c;
		}
	}
}