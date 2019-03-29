using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.AssetManagement
{
    public partial class FrmManageTypesOfAssets : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadAssetType();
                LoadAssetCategories();
                LoadFilterdAssetCategories();
            }

        }
        protected void BtnSubmitAssetTypeClick(object sender, EventArgs e)
        {

            ErrorDisplay1.ClearError();
            ErrorDisplayProcessAssetType.ClearError();
            try
            {
                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnSubmitAssetType.CommandArgument))
                {
                    case 1: //Add
                        if (!AddAssetType())
                        {
                            return;
                        }
                        
                        break;

                    case 2: //Update
                        if (!UpdateAssetType())
                        {
                            return;
                        }
                      
                        break;

                    default:
                        ErrorDisplayProcessAssetType.ShowError("Invalid process call!");
                        mpeProcessAssetType.Show();
                        break;
                }

                if (!LoadAssetType())
                {
                    return;
                }
               
                ErrorDisplay1.ShowSuccess("Asset Type information Was processed Successfully.");
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }


        }
        protected void BtnAddNewAssetTypeClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayProcessAssetType.ClearError();
            txtAssetType.Text = string.Empty;
            lgAssetTypeTitle.InnerHtml = "Create a New Asset Type";
            chkAssetType.Checked = false;
            btnSubmitAssetType.CommandArgument = "1";
            btnSubmitAssetType.Text = "Submit";
            mpeProcessAssetType.Show();
        }
        protected void DgAssetTypesEditCommand(object source, DataGridCommandEventArgs e)
        {

            ErrorDisplay1.ClearError();
            ErrorDisplayProcessAssetType.ClearError();
            txtAssetType.Text = string.Empty;
            ddlAssetCategory.SelectedIndex = 0;
            chkAssetType.Checked = false;
            try
            {
                
                dgAssetTypes.SelectedIndex = e.Item.ItemIndex;

                var assetTypeId = (DataCheck.IsNumeric(dgAssetTypes.DataKeys[e.Item.ItemIndex].ToString())) ? int.Parse(dgAssetTypes.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (assetTypeId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid AssetType Selection");
                    return;
                }

                var assetType = ServiceProvider.Instance().GetAssetTypeServices().GetAssetType(assetTypeId);
                if (assetType == null)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }
                if (assetType.AssetTypeId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }

                ddlAssetCategory.SelectedValue = assetType.AssetCategoryId.ToString(CultureInfo.InvariantCulture);
                txtAssetType.Text = assetType.Name;
                chkAssetType.Checked = assetType.Status == 1;
                btnSubmitAssetType.CommandArgument = "2"; //update
                btnSubmitAssetType.Text = "Update";
                lgAssetTypeTitle.InnerHtml = "Update Asset Type information";
                mpeProcessAssetType.Show();
                Session["_assetType"] = assetType;
            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.");
            }

        }
        protected void DdlAssetCategoriesSelectedChanged(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            if (int.Parse(ddlAssetCategories.SelectedValue) < 1)
            {
                ErrorDisplay1.ShowError("Please select an Asset Category");
                ddlAssetCategories.Focus();
                return;
            }

            GetOrderedAssetTypeByAssetCategory(int.Parse(ddlAssetCategories.SelectedValue));
        }
        #endregion

        #region Page Helpers

        private bool ValidateControls()
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (int.Parse(ddlAssetCategory.SelectedValue) < 1)
                {
                    ErrorDisplayProcessAssetType.ShowError("Please select an Asset Category");
                    ddlAssetCategory.Focus();
                    mpeProcessAssetType.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtAssetType.Text.Trim()))
                {
                    ErrorDisplayProcessAssetType.ShowError("Please enter an Asset Type.");
                    txtAssetType.Focus();
                    mpeProcessAssetType.Show();
                    return false;
                }

                //if (RegExValidation.IsNameValid(txtAssetType.Text.Trim()))
                //{
                //    ErrorDisplayProcessAssetType.ShowError("Invalid entry!");
                //    txtAssetType.Focus();
                //    mpeProcessAssetType.Show();
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessAssetType.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        private bool AddAssetType()
        {
            ErrorDisplay1.ClearError();
            try
            {
                var assetCategoryId = int.Parse(ddlAssetCategory.SelectedValue);
                long itemCode;
                var lastInsertedAssetType= ServiceProvider.Instance().GetAssetTypeServices().GetLastInsertedAssetTypeByAssetCategory(assetCategoryId);

                if (lastInsertedAssetType == null || lastInsertedAssetType.AssetTypeId < 1)
                {
                    var assetCategory = ServiceProvider.Instance().GetAssetCategoryServices().GetAssetCategory(assetCategoryId);
                    if (lastInsertedAssetType == null || lastInsertedAssetType.AssetTypeId < 1)
                    {
                        ErrorDisplayProcessAssetType.ShowError("An unknown error was encountered. Please try again later or contact the Administrator.");
                        mpeProcessAssetType.Show();
                        return false;
                    }

                    var code = assetCategory.Code.ToString(CultureInfo.InvariantCulture) + "1";
                    itemCode = long.Parse(code);
                }

                else
                {
                    itemCode = lastInsertedAssetType.Code + 1;
                }

                var newAssetType = new AssetType
                                        {
                                            AssetCategoryId = int.Parse(ddlAssetCategory.SelectedValue),
                                            Name = txtAssetType.Text.Trim(),
                                            Code = itemCode,
                                            Status = chkAssetType.Checked ? 1 : 0,

                                        };

                var k = ServiceProvider.Instance().GetAssetTypeServices().AddAssetType(newAssetType);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessAssetType.ShowError("Asset Type Information already Exists.");
                        mpeProcessAssetType.Show();
                        return false;
                    }

                    ErrorDisplayProcessAssetType.ShowError("Asset Type Information could not be added.");
                    mpeProcessAssetType.Show();
                    return false;
                }
                ddlAssetCategory.SelectedIndex = 0;
                txtAssetType.Text = string.Empty;
                chkAssetType.Checked = false;
                return true;
            }

            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact the Admin.");
                return false;
            }
        }

        private bool UpdateAssetType()
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (Session["_assetType"] == null)
                {
                    ErrorDisplay1.ShowError("Session has expired");
                    return false;
                }

                var assetType = (AssetType)Session["_assetType"];

                if (assetType == null)
                {
                    ErrorDisplayProcessAssetType.ShowError("Session has expired");
                    mpeProcessAssetType.Show();
                    return false;
                }

                if (assetType.AssetTypeId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return false;
                }

                assetType.Name = txtAssetType.Text.Trim();
                assetType.AssetCategoryId = int.Parse(ddlAssetCategory.SelectedValue.ToString(CultureInfo.InvariantCulture));
                assetType.Status = chkAssetType.Checked ? 1 : 0;

                var k = ServiceProvider.Instance().GetAssetTypeServices().UpdateAssetType(assetType);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessAssetType.ShowError("Asset Type already exists.");
                        txtAssetType.Focus();
                        mpeProcessAssetType.Show();
                        return false;
                    }

                    ErrorDisplayProcessAssetType.ShowError("The Asset Type information could not be updated. Please try again soon or contact the Admin.");
                    mpeProcessAssetType.Show();
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                return false;
            }
        }

        private bool LoadAssetType()
        {
            ErrorDisplay1.ClearError();
            try
            {
                var assetTypesList = ServiceProvider.Instance().GetAssetTypeServices().GetAllAssetTypes();

                if (!assetTypesList.Any())
                {
                    dgAssetTypes.DataSource = new List<AssetType>();
                    dgAssetTypes.DataBind();
                    return false;
                }

                dgAssetTypes.DataSource = assetTypesList;
                dgAssetTypes.DataBind();
                return true;
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }

        private void LoadAssetCategories()
        {
            ErrorDisplay1.ClearError();
            try
            {
                ddlAssetCategory.DataSource = new List<AssetCategory>();
                ddlAssetCategory.DataBind();
                ddlAssetCategory.Items.Insert(0, new ListItem("--List is empty--", "0"));
                ddlAssetCategory.SelectedIndex = 0;

                var assetCategoryList = ServiceProvider.Instance().GetAssetCategoryServices().GetActiveAssetCategories();
                
                if(!assetCategoryList.Any())
                {
                    return;
                }

                ddlAssetCategory.DataSource = assetCategoryList;
                ddlAssetCategory.DataValueField = "AssetCategoryId";
                ddlAssetCategory.DataTextField = "Name";
                ddlAssetCategory.DataBind();
                ddlAssetCategory.Items.Insert(0, new ListItem("---Select an Asset Category---", "0"));
                ddlAssetCategory.SelectedIndex = 0;
            }

            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }

        private void LoadFilterdAssetCategories()
        {
            ErrorDisplay1.ClearError();
            try
            {
                ddlAssetCategories.DataSource = new List<AssetCategory>();
                ddlAssetCategories.DataBind();
                ddlAssetCategories.Items.Insert(0, new ListItem("--List is empty--", "0"));
                ddlAssetCategories.SelectedIndex = 0;

                var assetCategoryList = ServiceProvider.Instance().GetAssetCategoryServices().LoadFilterdAssetCategories();

                if (!assetCategoryList.Any())
                {
                    return;
                }

                ddlAssetCategories.DataSource = assetCategoryList;
                ddlAssetCategories.DataValueField = "AssetCategoryId";
                ddlAssetCategories.DataTextField = "Name";
                ddlAssetCategories.DataBind();
                ddlAssetCategories.Items.Insert(0, new ListItem("---Select an Asset Category---", "0"));
                ddlAssetCategories.SelectedIndex = 0;

            }

            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }

        private void GetOrderedAssetTypeByAssetCategory(int assetCategoryId)
        {
            ErrorDisplay1.ClearError();
            try
            {
                var assetTypesList = ServiceProvider.Instance().GetAssetTypeServices().GetOrderedAssetTypeByAssetCategory(assetCategoryId);

                if (!assetTypesList.Any())
                {
                    dgAssetTypes.DataSource = new List<AssetType>();
                    dgAssetTypes.DataBind();
                    return;
                }

                dgAssetTypes.DataSource = assetTypesList;
                dgAssetTypes.DataBind();
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }

        
        #endregion
    }
}