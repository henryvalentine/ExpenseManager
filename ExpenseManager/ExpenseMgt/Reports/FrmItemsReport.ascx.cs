using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;
using XPLUG.WEBTOOLS;

namespace ExpenseManager.ExpenseMgt.Reports
{
    public partial class FrmItemsReport : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgCostItems.DataSource = new List<ExpenseTransaction>();
                dgCostItems.DataBind();
                Limit = 20;
                GetPageLimits();
                ddlLimit.SelectedValue = "20";
                LoadExpenseItems();
            }
        }

        protected void BtnDateFilterClick(object sender, EventArgs e)
        {
           ErrorDisplay1.ClearError();
            LoadExpenseItemCostsByDateRange();
        }
        private void LoadExpenseItems()
        {
            try
            {
                var items = ServiceProvider.Instance().GetExpenseItemServices().GetAllOrderedExpenseItems();
                if (!items.Any())
                {
                    ddlExpenseItems.DataSource = new List<ExpenseItem>();
                    ddlExpenseItems.DataBind();
                    ddlExpenseItems.Items.Insert(0, new ListItem("--- List is Empty ---", "0"));
                    ddlExpenseItems.SelectedIndex = 0;
                    return;
                }

                ddlExpenseItems.DataSource = items;
                ddlExpenseItems.DataValueField = "ExpenseItemId";
                ddlExpenseItems.DataTextField = "Title";
                ddlExpenseItems.DataBind();
                ddlExpenseItems.Items.Insert(0, new ListItem("--- Select Expense Item ---", "0"));
                ddlExpenseItems.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
              ErrorManager.LogApplicationError(ex.StackTrace,ex.Source,ex.Message);
            }
        }

        private bool LoadExpenseItemCostsByDateRange()
        {
            try
            {
                dgCostItems.DataSource = new List<ExpenseTransaction>();
                dgCostItems.DataBind();
                var itemId = int.Parse(ddlExpenseItems.SelectedValue);
                if (itemId < 1)
                {
                   ErrorDisplay1.ShowError("Please select an Expense Item.");
                    return false; 
                }

                var startDateString = DateMap.ReverseToServerDate(txtStart.Text.Trim());
                var startDate = DateTime.Parse(startDateString);
                var endDateString = DateMap.ReverseToServerDate(txtEndDate.Text.Trim());
                var endDate = DateTime.Parse(endDateString);
                if (endDate < startDate || startDate > endDate)
                {
                    ErrorDisplay1.ShowError("The 'From' date must not be LATER than the 'To' date.");
                    return false;
                }

                var transactionItems =
                    ServiceProvider.Instance()
                        .GetTransactionItemServices()
                        .GetExpenseItemCostsByDateRange(itemId, startDateString, endDateString);

                if (!transactionItems.Any())
                {
                    ErrorDisplay1.ShowError("No Record found!");
                    return false;
                }

                //dgCostItems.DataSource = transactionItems;
                //dgCostItems.DataBind();

                Session["_transCostItems"] = null;
                Session["_transCostItems"] = transactionItems;
                lblGrandTotal.InnerText = "N" + NumberMap.GroupToDigits(transactionItems[0].GrandTotalApprovedPrice.ToString(CultureInfo.InvariantCulture));
                itemTitle.InnerText = transactionItems[0].ExpenseItem.Title;
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<TransactionItem>(dgCostItems, "_transCostItems", Navigation.None, Limit, LoadMethod);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }


        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = senderLinkArgument;
                FillRepeater<ExpenseTransaction>(dgCostItems, "_transCostItems", Navigation.Sorting, Limit, LoadMethod);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }

        }
        protected void FillRepeater<T>(DataGrid dg, string str, Navigation navigation, int val, Func<bool> loadMethod)
        {
            try
            {
                if (Session[str] == null)
                {
                    loadMethod();
                }
                var sessionValue = Session[str];
                var sesslist = (sessionValue as IList<T>);

                if (sesslist == null)
                {
                    return;
                }
                DataCount = sesslist.Count;
                var objPds = new PagedDataSource { DataSource = sesslist.ToList(), AllowPaging = true, PageSize = val };
                    switch (navigation)
                    {
                        case Navigation.Next:
                            if (NowViewing < objPds.PageCount - 1)
                                NowViewing++;
                            break;
                        case Navigation.Previous:
                            if (NowViewing > 0)
                                NowViewing--;
                            break;
                        case Navigation.Last:
                            NowViewing = objPds.PageCount - 1;
                            break;
                        case Navigation.Sorting:
                            break;
                        default:
                            NowViewing = 0;
                            break;
                    }
                    objPds.CurrentPageIndex = NowViewing;
                    lblCurrentPage1.Text = "Page : " + (NowViewing + 1).ToString(CultureInfo.InvariantCulture) + " of " + objPds.PageCount.ToString(CultureInfo.InvariantCulture);
                    lblnPrev.Enabled = !objPds.IsFirstPage;
                    lblnNext.Enabled = !objPds.IsLastPage;
                    lblnFirst.Enabled = !objPds.IsFirstPage;
                    lblnLast.Enabled = !objPds.IsLastPage;
                    ActivateList();
                    if (objPds.IsFirstPage)
                    {
                        listNav2.Attributes.Add("class", "active");
                        listNav3.Attributes.Add("class", "active");
                    }
                    if (objPds.IsLastPage)
                    {
                        listNav4.Attributes.Add("class", "active");
                        listNav5.Attributes.Add("class", "active");
                    }
                    dg.DataSource = objPds;
                    dg.DataBind();
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        public int NowViewing
        {
            get
            {
                object obj = ViewState["_NowViewing"];
                if (obj == null)
                    return 0;

                return (int)obj;
            }
            set
            {
                ViewState["_NowViewing"] = value;
            }
        }
        protected void ActivateList()
        {
            try
            {
                listNav2.Attributes.Add("class", "disabled");
                listNav3.Attributes.Add("class", "disabled");
                listNav4.Attributes.Add("class", "disabled");
                listNav5.Attributes.Add("class", "disabled");
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnNextClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgCostItems, "_transCostItems", Navigation.Next, Limit, LoadMethod);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnFirstClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgCostItems, "_transCostItems", Navigation.First, Limit, LoadMethod);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnLastClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgCostItems, "_transCostItems", Navigation.Last, Limit, LoadMethod);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnPrevClick(object sender, EventArgs e)
        {

            try
            {
                FillRepeater<ExpenseTransaction>(dgCostItems, "_transCostItems", Navigation.Previous, Limit, LoadMethod);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void OnLimitChange(object sender, EventArgs e)
        {

            try
            {

                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<ExpenseTransaction>(dgCostItems, "_transCostItems", Navigation.None, Limit, LoadMethod);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }


        }
        private void GetPageLimits()
        {
            var intVal = new List<int>();
            for (var i = 20; i <= 200; i += 30)
            {
                if (i == 20)
                {
                    intVal.Add(i);
                }
                if (i == 50)
                {
                    intVal.Add(i);
                }
                if (i == 80)
                {
                    i = 100;
                    intVal.Add(i);
                }

                if (i == 130)
                {
                    i = 150;
                    intVal.Add(i);
                }

                if (i == 150)
                {
                    i = 200;
                    intVal.Add(i);
                }
            }
            ddlLimit.DataSource = intVal;
            ddlLimit.DataBind();
        }
        public int Limit
        {
            get
            {
                object obj = Session["_limit"];
                if (obj == null)
                {
                    Session["_limit"] = int.Parse(ddlLimit.SelectedValue);
                    return (int)Session["_limit"];
                }

                return (int)obj;
            }
            set
            {
                Session["_limit"] = value;
            }
        }
        #region PageEventMethod
        /* Method to hold all content frm the Db*/
        protected bool LoadMethod()
        {
            if (!LoadExpenseItemCostsByDateRange())
            {
                return false;
            }
            return true;
        }
        #endregion
        #endregion
    }
}