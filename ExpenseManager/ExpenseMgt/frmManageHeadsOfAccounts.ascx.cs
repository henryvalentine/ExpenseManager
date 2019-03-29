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
    public partial class FrmManageHeadsOfAccounts1 : UserControl
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
                Session["_newAcctHeadList"] = null;
                LoadFiltteredExpenseCategories();
                btnAddNewAccountsHead.CommandArgument = "0";
                dgAccountsHead.DataSource = new List<AccountsHead>();
                dgAccountsHead.DataBind();
            }

        }

        protected void BtnSubmitAccountsHeadClick(object sender, EventArgs e)
        {
            ErrorDisplayProcessAccountsHead.ClearError();
            try
            {
                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnSubmitAccountsHead.CommandArgument))
                {
                    case 1: //Add
                        if (!AddAccountsHead())
                        {
                            ConfirmAlertBox1.ShowMessage("Accounts Head information could not be added. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                            return;
                        }
                        break;

                    case 2: //Update
                        if (!UpdateAccountsHead())
                        {
                            ConfirmAlertBox1.ShowMessage("Accounts Head information could not be updated. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                            
                        }
                        break;

                    default:
                        ErrorDisplayProcessAccountsHead.ShowError("Invalid process call!");
                        mpeProcessAccountsHead.Show();
                        break;
                }
                
                LoadAccountsHeads();
               
                ConfirmAlertBox1.ShowSuccessAlert("Accounts Head was Saved Successfully.");
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }

        protected void BtnAddNewAccountsHeadClick(object sender, EventArgs e)
        {
           
            ErrorDisplayProcessAccountsHead.ClearError();
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;

            LoadExpenseCategories();

            lgAccountsTitle.InnerHtml = "Create a New Accounts Head";
            chkAccountsHead.Checked = false;
            btnSubmitAccountsHead.CommandArgument = "1";
            btnSubmitAccountsHead.Text = "Submit";
            mpeProcessAccountsHead.Show();
        }

        protected void DgAccountsHeadEditCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayProcessAccountsHead.ClearError();
            LoadExpenseCategories();
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlExpenseCategory.SelectedIndex = 0;
            chkAccountsHead.Checked = false;
            try
            {
                dgAccountsHead.SelectedIndex = e.Item.ItemIndex;

                var accountsHeadId = (DataCheck.IsNumeric(dgAccountsHead.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgAccountsHead.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (accountsHeadId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid AccountsHead Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var accountsHead = ServiceProvider.Instance().GetAccountsHeadServices().GetAccountsHead(accountsHeadId);
                
                if (accountsHead == null || accountsHead.AccountsHeadId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ddlExpenseCategory.SelectedValue = accountsHead.ExpenseCategoryId.ToString(CultureInfo.InvariantCulture);
                txtTitle.Text = accountsHead.Title;
                txtDescription.Text = accountsHead.Description;
                chkAccountsHead.Checked = accountsHead.Status == 1;
                btnSubmitAccountsHead.CommandArgument = "2"; //update
                btnSubmitAccountsHead.Text = "Update";
                lgAccountsTitle.InnerHtml = "Update Accounts Head information";
                mpeProcessAccountsHead.Show();
                Session["_accountsHead"] = accountsHead;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }

        }

        protected void DdlCategorySelectedChanged(object sender, EventArgs e)
        {
           
            if (int.Parse(ddlCategory.SelectedValue) < 1)
            {
                ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                ddlCategory.Focus();
                return;
            }

            LoadFilteredAccountsHeads(int.Parse(ddlCategory.SelectedValue));
        }

        protected void LnkShowAllClick(object sender, EventArgs e)
        {
            ddlCategory.SelectedIndex = 0;
           
            LoadAccountsHeads();
            btnAddNewAccountsHead.CommandArgument = "1";
        }
        #endregion

        #region Page Helpers

        private bool ValidateControls()
        {
           
            try
            {
                if (int.Parse(ddlExpenseCategory.SelectedValue) < 1)
                {
                    ErrorDisplayProcessAccountsHead.ShowError("Please select an Expense Category");
                    ddlExpenseCategory.Focus();
                    mpeProcessAccountsHead.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
                {
                    ErrorDisplayProcessAccountsHead.ShowError("Please enter an Accounts Head.");
                    txtTitle.Focus();
                    mpeProcessAccountsHead.Show();
                    return false;
                }
                if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {
                    ErrorDisplayProcessAccountsHead.ShowError("Please,Specify Accounts Head Description");
                    txtDescription.Focus();
                    mpeProcessAccountsHead.Show();
                    return false;
                }

                //if (RegExValidation.IsNameValid(txtTitle.Text.Trim()))
                //{
                //    ErrorDisplayProcessAccountsHead.ShowError("Invalid entry!");
                //    txtTitle.Focus();
                //    mpeProcessAccountsHead.Show();
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessAccountsHead.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool AddAccountsHead()
        {
           
            try
            {
                string itemCode = string.Empty;

                var categoryId = int.Parse(ddlExpenseCategory.SelectedValue);

                var accountHead = ServiceProvider.Instance().GetAccountsHeadServices().GetLastInsertedAccountsHeadsByExpenseCategoryId(categoryId);

                if (accountHead == null || accountHead.AccountsHeadId < 1)
                {
                    var expenseCategory = ServiceProvider.Instance().GetExpenseCategoryServices().GetExpenseCategory(categoryId);

                    if (expenseCategory == null || expenseCategory.ExpenseCategoryId < 1)
                    {
                        ConfirmAlertBox1.ShowMessage("Invalid Process call!", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }
                    
                    itemCode = expenseCategory.Code + "1";
                }
                
                else
                {
                    var expenseItemCode = long.Parse(accountHead.Code + 1);

                    itemCode = expenseItemCode.ToString(CultureInfo.InvariantCulture);
                }

                var newAccountsHead = new AccountsHead
                                        {
                                            ExpenseCategoryId = int.Parse(ddlExpenseCategory.SelectedValue),
                                            Title = txtTitle.Text.Trim(),
                                            Code = itemCode,
                                            Description = txtDescription.Text.Trim(),
                                            Status = chkAccountsHead.Checked ? 1 : 0,
                                        };

                var k = ServiceProvider.Instance().GetAccountsHeadServices().AddAccountsHeadCheckDuplicate(newAccountsHead);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessAccountsHead.ShowError("Accounts Head Information already Exists.");
                        mpeProcessAccountsHead.Show();
                        return false;
                    }

                    ErrorDisplayProcessAccountsHead.ShowError("Accounts Head Information could not be added.");
                    mpeProcessAccountsHead.Show();
                    return false;
                }
                
                txtDescription.Text = string.Empty;
                txtTitle.Text = string.Empty;
                chkAccountsHead.Checked = false;
                return true;
            }

            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool UpdateAccountsHead()
        {
           
            try
            {
                if (Session["_accountsHeads"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Your session has expired", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }
                var accountsHead = Session["_accountsHead"] as AccountsHead;

                if (accountsHead == null || accountsHead.AccountsHeadId < 1)
                {
                    ErrorDisplayProcessAccountsHead.ShowError("Accounts Head list is empty or session has expired");
                    mpeProcessAccountsHead.Show();
                    return false;
                }

                accountsHead.Description = txtDescription.Text.Trim();
                accountsHead.Title = txtTitle.Text.Trim();
                accountsHead.ExpenseCategoryId = int.Parse(ddlExpenseCategory.SelectedValue.ToString(CultureInfo.InvariantCulture));
                accountsHead.Status = chkAccountsHead.Checked ? 1 : 0;

                var k = ServiceProvider.Instance().GetAccountsHeadServices().UpdateAccountsHeadCheckDuplicate(accountsHead);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessAccountsHead.ShowError("Accounts Head already exists.");
                        txtTitle.Focus();
                        mpeProcessAccountsHead.Show();
                        return false;
                    }

                    ErrorDisplayProcessAccountsHead.ShowError("The Accounts Head information could not be updated.");
                    mpeProcessAccountsHead.Show();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }
        }
        private void LoadFiltteredExpenseCategories()
        {
           
            try
            {
                var filteredExpenseCategoriesList = ServiceProvider.Instance().GetExpenseCategoryServices().GetFilteredExpenseCategories();

                if (!filteredExpenseCategoriesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Expense Category list is empty.", ConfirmAlertBox.PopupMessageType.Error);
                    ddlCategory.DataSource = new List<AccountsHead>();
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlCategory.SelectedIndex = 0;
                    return;
                }

                ddlCategory.DataSource = filteredExpenseCategoriesList;
                ddlCategory.DataValueField = "ExpenseCategoryId";
                ddlCategory.DataTextField = "Title";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("---Select an Expense Category---", "0"));
                ddlCategory.SelectedIndex = 0;
                Session["_expenseCategoryList"] = filteredExpenseCategoriesList;
            }

            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadExpenseCategories()
        {
            try
            {
                var filteredExpenseCategoriesList = ServiceProvider.Instance().GetExpenseCategoryServices().GetAllActiveExpenseCategories();

                if (!filteredExpenseCategoriesList.Any())
                {
                    ddlExpenseCategory.DataSource = new List<AccountsHead>();
                    ddlExpenseCategory.DataBind();
                    ddlExpenseCategory.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlExpenseCategory.SelectedIndex = 0;
                    return;
                }

                ddlExpenseCategory.DataSource = filteredExpenseCategoriesList;
                ddlExpenseCategory.DataValueField = "ExpenseCategoryId";
                ddlExpenseCategory.DataTextField = "Title";
                ddlExpenseCategory.DataBind();
                ddlExpenseCategory.Items.Insert(0, new ListItem("---Select an Expense Category---", "0"));
                ddlExpenseCategory.SelectedIndex = 0;
            }
            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadAccountsHeads()
        {
            try
            {
                var accountsHeadList = ServiceProvider.Instance().GetAccountsHeadServices().GetOrderedAccountsHeads();
                
                if (accountsHeadList == null || !accountsHeadList.Any())
                {
                    dgAccountsHead.DataSource = new List<AccountsHead>();
                    dgAccountsHead.DataBind();
                }
                Session["_accountsHeads"] = null;
                Session["_accountsHeads"] = accountsHeadList;
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.None, Limit, LoadMethod);
            }

            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private void LoadFilteredAccountsHeads(int expenseCategoryId)
        {
            try
            {
                var accountsHeadList = ServiceProvider.Instance().GetAccountsHeadServices().GetOrderedAccountsHeadsByExpenseCategoryId(expenseCategoryId);

                if (accountsHeadList == null || !accountsHeadList.Any())
                {
                    dgAccountsHead.DataSource = new List<AccountsHead>();
                    dgAccountsHead.DataBind();
                }
                Session["_accountsHeads"] = null;
                Session["_accountsHeads"] = accountsHeadList;
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.None, Limit, LoadMethod);
            }

            catch
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        private List<AccountsHead> AddAcctHeadToSession(AccountsHead accountHead)
        {
            var acctHeadList = new List<AccountsHead>();
            try
            {
                if (Session["_newAcctHeadList"] == null)
                {
                    acctHeadList.Add(accountHead);
                    Session["_newAcctHeadList"] = acctHeadList;
                    return acctHeadList;
                }

                var accountHeadList = Session["_newAcctHeadList"] as List<AccountsHead>;

                if (accountHeadList == null || !accountHeadList.Any())
                {
                    acctHeadList.Add(accountHead);
                    Session["_newAcctHeadList"] = acctHeadList;
                    return accountHeadList;
                }
                acctHeadList.Add(accountHead);
                Session["_newAcctHeadList"] = acctHeadList;
                return acctHeadList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            {
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = senderLinkArgument;
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.Sorting, Limit, LoadMethod);
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
                var obj = ViewState["_NowViewing"];
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
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.Next, Limit, LoadMethod);
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
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.First, Limit, LoadMethod);
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
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.Last, Limit, LoadMethod);
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
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.Previous, Limit, LoadMethod);
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
                FillRepeater<AccountsHead>(dgAccountsHead, "_accountsHeads", Navigation.None, Limit, LoadMethod);
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
            LoadAccountsHeads();
            return true;
        }
        #endregion
        #endregion

    }
}