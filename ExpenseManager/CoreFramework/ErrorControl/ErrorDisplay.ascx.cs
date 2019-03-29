using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseManager.CoreFramework.ErrorControl
{
    
    public partial class ErrorDisplay : System.Web.UI.UserControl
    {
        private string _errorMsg = string.Empty;
        private string _plainMsg = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tbError.Visible = false;
                tbMessage.Visible = false;
            }
        }
        #region

            public string LoadErrorMessage()
            {
                return _errorMsg;
            }
            public string LoadPlainMessage()
            {
               return _plainMsg;
            }
            public void ClearError()
            {
                _errorMsg = string.Empty;
                tbError.Visible = false;
                _plainMsg = string.Empty;
                tbMessage.Visible = false;
            }
            public void ShowError(string errMsg)
            {
                if (errMsg.Length > 0)
                {
                    _plainMsg = "";
                    this.tbMessage.Visible = false;
                    _errorMsg = errMsg;
                    LoadErrorMessage();
                    tbError.Visible = true;

                }

            }
            public void ShowSuccess(string msg)
            {
                if (msg.Length > 0)
                {
                    _errorMsg = "";
                    tbError.Visible = false;
                    _plainMsg = msg;
                    LoadPlainMessage();
                    tbMessage.Visible = true;

                }

            }
            public void ClearControls(System.Web.UI.Control parent)
            {
                if (parent == null)
                {
                    return;
                }
                foreach (Control c in parent.Controls)
                {
                    if (c.GetType() == typeof(TextBox))
                    {
                        ((TextBox)c).Text = "";
                    }
                    else if (c.GetType() == typeof(DropDownList))
                    {
                        if (((DropDownList)c).Items.Count > 0)
                        {
                            ((DropDownList)c).SelectedIndex = 0;
                        }
                    }
                    else if (c.GetType() == typeof(ListBox))
                    {
                        if (((ListBox)c).Items.Count > 0)
                        {
                            ((ListBox)c).Items.Clear();
                        }
                    }
                    
                    else
                    {
                        if (c.HasControls())
                        {
                            ClearControls(c);
                        }
                    }
                }
            }
        #endregion
    }
}