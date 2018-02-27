using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GroupBuilder;

namespace GrouperApp
{
    public partial class ThankYou : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"].Trim().ToLower();
                Student thisStudent = GrouperMethods.GetStudent(int.Parse(id));

                //NameLabel.Text = " " + thisStudent.PreferredName;
            }
        }
    }
}