﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.UI.WebControls;
namespace WebPIB
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				string aa = Session.Contents.LCID.ToString();
			}
			else
			{
				string aa = Session.Contents.LCID.ToString();
			}

		}
	}
}