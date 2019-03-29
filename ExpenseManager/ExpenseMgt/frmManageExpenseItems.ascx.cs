using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageTransactionItems1 : UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Limit = 20;
                GetPageLimits();
                ddlLimit.SelectedValue = "20";
                btnAddNewExpenseItem.CommandArgument = "0";
                BindGridWithDefaultList();
                LoadAccountsHeads();
            }
        }
        protected void BtnProcessTransactionItemsClick(object sender, EventArgs e)
        {
            
            try
            {
                if (!ValidateExpenseItemControls())
                {
                    mpeExpenseItemsPopup.Show();
                    return;
                }

                switch (int.Parse(btnProcessTransactionItems.CommandArgument))
                {
                    case 1: //Add
                        if (!AddExpenseItem())
                        {
                            ConfirmAlertBox1.ShowMessage("Expense Item information could not be added. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                            return;
                        }
                            ConfirmAlertBox1.ShowSuccessAlert("Expense Item information was Added Successfully.");
                        
                       
                        break;

                    case 2: //Update
                        if (!UpdateExpenseItem())
                        {
                            ConfirmAlertBox1.ShowMessage("Expense Item information could not be updated. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                            return;
                        }
                        
                        ConfirmAlertBox1.ShowSuccessAlert("Expense Item information was updated Successfully.");
                       
                        break;
                    default:
                        ConfirmAlertBox1.ShowMessage("Invalid process call!", ConfirmAlertBox.PopupMessageType.Error);
                        break;

                }

                if(int.Parse((ddlAccountsHeads.SelectedValue)) < 1)
                {
                    LoadAllExpenseItems();
                }

                if(int.Parse((ddlAccountsHeads.SelectedValue)) > 0)
                {
                    LoadFilteredExpenseItems();
                }

            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplayProcessTransactionItems.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        protected void BtnAddNewExpenseItemClick(object sender, EventArgs e)
        {
            
            ErrorDisplayProcessTransactionItems.ClearError();
            lgTitleScope.InnerHtml = "Create new Expense Item";
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ccdAccountsHead.SelectedValue = "0";
            ccdExpenseCategory.SelectedValue = "0";
            chkActivateTransactionItem.Checked = false;
            btnProcessTransactionItems.Text = "Submit";
            btnProcessTransactionItems.CommandArgument = "1";
            mpeExpenseItemsPopup.Show();
        }
        protected void DgExpenseItemEditCommand(object source, DataGridCommandEventArgs e)
        {
            
            ErrorDisplayProcessTransactionItems.ClearError();
            try
            {
                dgExpenseItem.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgExpenseItem.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid ExpenseItem Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseItem = ServiceProvider.Instance().GetExpenseItemServices().GetExpenseItem(id);
                
                if (expenseItem == null || expenseItem.ExpenseItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ccdExpenseCategory.SelectedValue = expenseItem.ExpenseCategoryId.ToString(CultureInfo.InvariantCulture);
                ccdAccountsHead.SelectedValue = expenseItem.AccountsHeadId.ToString(CultureInfo.InvariantCulture);
                txtTitle.Text = expenseItem.Title;
                txtDescription.Text = expenseItem.Description;
                chkActivateTransactionItem.Checked = expenseItem.Status == 1;
                lgTitleScope.InnerHtml = "Update Expense Item";
                Session["_expenseItem"] = expenseItem;
                btnProcessTransactionItems.CommandArgument = "2";
                btnProcessTransactionItems.Text = "Update";
                mpeExpenseItemsPopup.Show();
            }

            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }


        }
        protected void DdlAccountsHeadsSelectedChanged(object sender, EventArgs e)
        {
            
            if (int.Parse(ddlAccountsHeads.SelectedValue) < 1)
            {
                ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                ddlAccountsHeads.Focus();
                return;
            }

            LoadFilteredExpenseItems();
        }
        #endregion

        #region Page Helpers
        private bool ValidateExpenseItemControls()
        {
            

            if (int.Parse(ddlExpenseCategory.SelectedValue) < 1)
            {
                ErrorDisplayProcessTransactionItems.ShowError("Invalid selection!");
                ddlExpenseCategory.Focus();
                mpeExpenseItemsPopup.Show();
                return false;
            }

            if (int.Parse(ddlAccountsHead.SelectedValue) < 1)
            {
                ErrorDisplayProcessTransactionItems.ShowError("Invalid selection!");
                ddlAccountsHead.Focus();
                mpeExpenseItemsPopup.Show();
                return false;
            }

            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                ErrorDisplayProcessTransactionItems.ShowError("Please supply a title for the Expense Item.");
                txtTitle.Focus();
                mpeExpenseItemsPopup.Show();
                return false;
            }

            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                ErrorDisplayProcessTransactionItems.ShowError("Please provide a Description for the Expense Item");
                txtDescription.Focus();
                mpeExpenseItemsPopup.Show();
                return false;
            }

            //if (RegExValidation.IsNameValid(txtTitle.Text.Trim()))
            //{
            //    ErrorDisplayProcessTransactionItems.ShowError("Invalid entry!");
            //    txtTitle.Focus();
            //    mpeExpenseItemsPopup.Show();
            //    return false;
            //}

            return true;
        }
        private bool AddExpenseItem()
        {
            
            ErrorDisplayProcessTransactionItems.ClearError();
            try
            {
                string itemCode;
                
                var itemsList = ServiceProvider.Instance().GetExpenseItemServices().GetLastInsertedExpenseItem(int.Parse(ddlAccountsHead.SelectedValue));

                if (!itemsList.Any())
                {
                    var accountsHead = ServiceProvider.Instance().GetAccountsHeadServices().GetAccountsHead(int.Parse(ddlAccountsHead.SelectedValue));

                    if (accountsHead == null || accountsHead.AccountsHeadId < 1)
                    {
                        return false;
                    }
                           
                    var accountsHeadCode = accountsHead.Code + "01";

                    itemCode = accountsHeadCode;
                }

                else
                {
                    var expenseItemCode = long.Parse(itemsList.First().Code) + 1;

                    itemCode = expenseItemCode.ToString(CultureInfo.InvariantCulture);
                }
                
                var newExpenseItem = new ExpenseItem
                                        {
                                            ExpenseCategoryId = int.Parse(ddlExpenseCategory.SelectedValue),
                                            AccountsHeadId = int.Parse(ddlAccountsHead.SelectedValue),
                                            Title = txtTitle.Text.Trim(),
                                            Code = itemCode,
                                            Description = txtDescription.Text,
                                            Status = (chkActivateTransactionItem.Checked) ? 1 : 0

                                        };

                var k = ServiceProvider.Instance().GetExpenseItemServices().AddExpenseItem(newExpenseItem);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessTransactionItems.ShowError("Expense Item information already exists.");
                        txtTitle.Focus();
                        mpeExpenseItemsPopup.Show();
                        return false;
                    }

                    ErrorDisplayProcessTransactionItems.ShowError("Expense Item could not be added.");
                    mpeExpenseItemsPopup.Show();
                    return false;
                }

                ErrorDisplayProcessTransactionItems.ShowSuccess("Expense Item information was added successfully.");
                txtDescription.Text = string.Empty;
                txtTitle.Text = string.Empty;
                ddlAccountsHead.SelectedIndex = 0;
                ddlExpenseCategory.SelectedIndex = 0;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplayProcessTransactionItems.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                mpeExpenseItemsPopup.Show();
                return false;
            }

        }
        private void BindGridWithDefaultList()
        {
            dgExpenseItem.DataSource = new List<ExpenseItem>();
            dgExpenseItem.DataBind();
        }
        private bool UpdateExpenseItem()
        {
            
            try
            {
                if (Session["_expenseItem"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Expense Item list is empty or session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                var expenseItem = Session["_expenseItem"] as ExpenseItem;

                if (expenseItem == null || expenseItem.ExpenseItemId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("The requested process failed!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                expenseItem.Description = txtDescription.Text.Trim();
                expenseItem.Title = txtTitle.Text.Trim();
                expenseItem.ExpenseCategoryId = int.Parse(ddlExpenseCategory.SelectedValue);
                expenseItem.AccountsHeadId = int.Parse(ddlAccountsHead.SelectedValue);
                expenseItem.Status = chkActivateTransactionItem.Checked ? 1 : 0;

                var k = ServiceProvider.Instance().GetExpenseItemServices().UpdateExpenseItemCheckDuplicate(expenseItem);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessTransactionItems.ShowError("Expense Item information already exists.");
                        txtTitle.Focus();
                        mpeExpenseItemsPopup.Show();
                        return false;
                    }

                    ErrorDisplayProcessTransactionItems.ShowError("Expense Item could not be updated.");
                    mpeExpenseItemsPopup.Show();
                    return false;
                }

                ConfirmAlertBox1.ShowSuccessAlert("ExpenseItem Information Was updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void LoadAccountsHeads()
        {
            
            try
            {
                var accountsHeadsList = ServiceProvider.Instance().GetAccountsHeadServices().GetActiveOrderedAccountsHeads();

                if (!accountsHeadsList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Accounts Heads list is empty.", ConfirmAlertBox.PopupMessageType.Error);
                    ddlAccountsHeads.DataSource = new List<AccountsHead>();
                    ddlAccountsHeads.DataBind();
                    ddlAccountsHeads.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlAccountsHeads.SelectedIndex = 0;
                    return;
                }

                ddlAccountsHeads.DataSource = accountsHeadsList;
                ddlAccountsHeads.DataValueField = "AccountsHeadId";
                ddlAccountsHeads.DataTextField = "Title";
                ddlAccountsHeads.DataBind();
                ddlAccountsHeads.Items.Insert(0, new ListItem("---Select an Accounts Head---", "0"));
                ddlAccountsHeads.SelectedIndex = 0;
            }

            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadFilteredExpenseItems()
        {
            try
            {
                if (int.Parse(ddlAccountsHeads.SelectedValue) > 0)
                {

                    var filteredExpenseItemsList = ServiceProvider.Instance().GetExpenseItemServices().GetOrderedExpenseItemsByAccountsHeadId(int.Parse(ddlAccountsHeads.SelectedValue));

                    if (!filteredExpenseItemsList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("No record found.", ConfirmAlertBox.PopupMessageType.Error);
                        dgExpenseItem.DataSource = new List<ExpenseItem>();
                        dgExpenseItem.DataBind();
                        return;
                    }

                    Session["_filteredExpenseItemsList"] = null;
                    Session["_filteredExpenseItemsList"] = filteredExpenseItemsList;
                    Limit = int.Parse(ddlLimit.SelectedValue);
                    FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.None, Limit, LoadMethod);

                }
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadAllExpenseItems()
        {
            try
            {
                var expenseItemsList = ServiceProvider.Instance().GetExpenseItemServices().GetAllOrderedExpenseItems();

                    if (!expenseItemsList.Any())
                    {
                        ConfirmAlertBox1.ShowMessage("Expense Item List is Empty.", ConfirmAlertBox.PopupMessageType.Error);
                        dgExpenseItem.DataSource = new List<ExpenseItem>();
                        dgExpenseItem.DataBind();
                        return;
                    }

                    Session["_filteredExpenseItemsList"] = null;
                    Session["_filteredExpenseItemsList"] = expenseItemsList;
                    Limit = int.Parse(ddlLimit.SelectedValue);
                    FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.None, Limit, LoadMethod);

                
            }
            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        #endregion


        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = (int)senderLinkArgument;
                FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.Sorting, Limit, LoadMethod);
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
                DataCount = sesslist.Count;
                if (sesslist != null)
                {
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
                FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.Next, Limit, LoadMethod);
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
                FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.First, Limit, LoadMethod);
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
                FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.Last, Limit, LoadMethod);
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
                FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.Previous, Limit, LoadMethod);
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
                FillRepeater<ExpenseItem>(dgExpenseItem, "_filteredExpenseItemsList", Navigation.None, Limit, LoadMethod);
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
        /* Method to hold all content from the Db*/
        protected bool LoadMethod()
        {
            LoadFilteredExpenseItems();
            return true;
        }
        #endregion
        #endregion
    }
}