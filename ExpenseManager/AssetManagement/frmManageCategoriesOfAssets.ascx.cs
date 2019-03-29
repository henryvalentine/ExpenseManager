using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.AssetManagement
{
    public partial class FrmManageCategoriesOfAssets : System.Web.UI.UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadAssetCategories();
            }
        }
        protected void BtnprocessAssetCategoryClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
               
                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnProcessAssetCategory.CommandArgument))
                {
                    case 1:
                        if (!AddAssetCategory())
                        {
                            return;
                        }
                        
                        break;

                    case 2:
                        if (!UpdateAssetCategory())
                        {
                            return;
                        }
                        
                        break;

                    default:
                        ErrorDisplayProcessAssetCategory.ShowError("Invalid process call!");
                        mpeProcessAssetCategory.Show();
                        break;
                }

                if (!LoadAssetCategories())
                {
                    return;
                }

                ErrorDisplay1.ShowSuccess("Asset Category information was successfully processed.");
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        protected void BtnAddNewAssetCategoryClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ClearControls();
            btnProcessAssetCategory.CommandArgument = "1";
            btnProcessAssetCategory.Text = "Submit";
            lgAssetCategoryTitle.InnerHtml = "Create a new Asset Category";
            mpeProcessAssetCategory.Show();
        }
        protected void DgAssetCategoriesEditCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayProcessAssetCategory.ClearError();
            ErrorDisplay1.ClearError();
            txtName.Text = string.Empty;
            try
            {
                if (Session["_assetCategories"] == null)
                {
                    ErrorDisplay1.ShowError("Asset Category list is empty or session has expired!");
                    return;
                }

                var assetCategories = (List<AssetCategory>)Session["_assetCategories"];

                if (assetCategories == null)
                {
                    ErrorDisplay1.ShowError("Asset Category list is empty or session has expired!");
                    return;
                }

                if (!assetCategories.Any())
                {
                    ErrorDisplay1.ShowError("Asset Category list is empty or session has expired!");
                    return;
                }

                dgAssetCategories.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgAssetCategories.DataKeys[e.Item.ItemIndex].ToString()))
                             ? long.Parse(dgAssetCategories.DataKeys[e.Item.ItemIndex].ToString())
                             : 0;

                if (id < 1)
                {
                    ErrorDisplay1.ShowError("Invalid Selection");
                    return;
                }

                var assetCategory = assetCategories.Find(m => m.AssetCategoryId == id);
                if (assetCategory == null)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }
                if (assetCategory.AssetCategoryId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }

                txtName.Text = assetCategory.Name;
                txtCode.Text = assetCategory.Code.ToString(CultureInfo.InvariantCulture);
                chkAssetCategory.Checked = assetCategory.Status == 1;
                btnProcessAssetCategory.CommandArgument = "2";
                btnProcessAssetCategory.Text = "Update";
                lgAssetCategoryTitle.InnerHtml = "Update Asset Category";
                mpeProcessAssetCategory.Show();
                Session["_assetCategory"] = assetCategory;
            }

            catch (Exception)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please refresh the page or try again soon.");
            }
        }

        #endregion

        #region Page Helpers
        private bool AddAssetCategory()
        {
            try
            {
                var newAssetCategory = new AssetCategory
                {
                    Name = txtName.Text.Trim(),
                    Code = long.Parse(txtCode.Text.Trim()),
                    Status = chkAssetCategory.Checked ? 1 : 0
                };
                var k = ServiceProvider.Instance().GetAssetCategoryServices().AddAssetCategoryCheckDuplicate(newAssetCategory);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplay1.ShowError("Asset Category already exists.");
                        txtName.Focus();
                        mpeProcessAssetCategory.Show();
                        return false;
                    }
                    ErrorDisplay1.ShowError("Asset Category could not be added. Please contact the Admin.");
                    txtName.Focus();
                    mpeProcessAssetCategory.Show();
                    return false;
                }

                ErrorDisplay1.ShowSuccess("Asset Category was added successfully.");
                return true;
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool UpdateAssetCategory()
        {
            try
            {
                if (Session["_assetCategory"] == null)
                {
                    ErrorDisplayProcessAssetCategory.ShowError("Asset Categories list is empty or session has expired.");
                    mpeProcessAssetCategory.Show();
                    return false;
                }

                var assetCategory = (AssetCategory)Session["_assetCategory"];

                if (assetCategory == null)
                {
                    ErrorDisplayProcessAssetCategory.ShowError("Invalid selection!");
                    mpeProcessAssetCategory.Show();
                    return false;
                }

                if (assetCategory.AssetCategoryId < 1)
                {
                    ErrorDisplayProcessAssetCategory.ShowError("Invalid selection!");
                    mpeProcessAssetCategory.Show();
                    return false;
                }

                assetCategory.Name = txtName.Text.Trim();
                assetCategory.Code = long.Parse(txtCode.Text.Trim());
                assetCategory.Status = chkAssetCategory.Checked ? 1 : 0;
                var k = ServiceProvider.Instance().GetAssetCategoryServices().UpdateAssetCategoryCheckDuplicate(assetCategory);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessAssetCategory.ShowError("Asset Category information already exists.");
                        txtName.Focus();
                        mpeProcessAssetCategory.Show();
                    }


                    else
                    {
                        ErrorDisplayProcessAssetCategory.ShowError("Asset Category information could not be updated. Please try again soon or contact the Admin.");
                        mpeProcessAssetCategory.Show();
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessAssetCategory.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool LoadAssetCategories()
        {
            try
            {
                dgAssetCategories.DataSource = new List<AssetCategory>();
                dgAssetCategories.DataBind();

                var assetCategories = ServiceProvider.Instance().GetAssetCategoryServices().GetAssetCategoryList();
                if (assetCategories == null)
                {
                    ErrorDisplay1.ShowError("Asset Category List is empty.");
                   return false;
                }

                if (!assetCategories.Any())
                {
                    ErrorDisplay1.ShowError("Asset Category List is empty.");
                    return false;
                }

                dgAssetCategories.DataSource = assetCategories;
                dgAssetCategories.DataBind();
                Session["_assetCategories"] = assetCategories;
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool ValidateControls()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayProcessAssetCategory.ClearError();
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ErrorDisplayProcessAssetCategory.ShowError("Please supply an Asset Category.");
                txtName.Focus();
                mpeProcessAssetCategory.Show();
                return false;
            }

            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                ErrorDisplayProcessAssetCategory.ShowError("Please supply a code for the Asset Category.");
                txtCode.Focus();
                mpeProcessAssetCategory.Show();
                return false;
            }
            //Use reg validator to check name
            //if (RegExValidation.IsNameValid(txtName.Text.Trim()))
            //{
            //    ErrorDisplayProcessAssetCategory.ShowError("Invalid entry!");
            //    txtName.Focus();
            //    mpeProcessAssetCategory.Show();
            //    return false;
            //}

            if (!DataCheck.IsNumeric(txtCode.Text.Trim()))
            {
                ErrorDisplayProcessAssetCategory.ShowError("Invalid entry!");
                txtCode.Focus();
                mpeProcessAssetCategory.Show();
                return false;
            }

            return true;
        }
        private void ClearControls()
        {
            ErrorDisplayProcessAssetCategory.ClearError();
            txtName.Text = string.Empty;
            txtCode.Text = string.Empty;
            chkAssetCategory.Checked = false;
        }

        #endregion
    }
}