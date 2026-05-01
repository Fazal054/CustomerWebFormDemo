using System;
using System.Web.UI;

namespace CustomerWebFormDemo
{
    public partial class WelcomeBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Optional: logic when control loads
        }

        public string UserName
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = "Welcome, " + value + "!"; }
        }
    }
}