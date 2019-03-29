using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;

namespace ExpenseManager.AssetManagement
{
    public partial class FrmManageCompanyAssets : System.Web.UI.UserControl
    {
        protected int Offset = 5;
        protected int DataCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Limit = 20;
                GetPageLimits();
                ddlLimit.SelectedValue = "20";
                dvSelectAssetCategory.Visible = true;
                dvManageFixedAssets.Visible = false;
                dvManageLiquidAssets.Visible = false;
                LoadAssetCategories();
                
            }
        }

        #region Fixed Assets
        #region Page Events
        protected void DdlAssetCategoriesIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(ddlAssetCategories.SelectedValue) < 1)
            {
                ErrorDisplay1.ShowError("Invalid selection!");
                ddlAssetCategories.Focus();
                return;
            }

            Session["_assetCategoryId"] = int.Parse(ddlAssetCategories.SelectedValue);
            LoadAssetTypes();
        }
        protected void BtnProcessFixedAssetsClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayProcessFixedAssets.ClearError();
            if (!ValidateControls())
            {
                return;
            }
            try
            {
                switch (int.Parse(btnProcessFixedAssets.CommandArgument))
                {
                    case 1: //Add

                        if (int.Parse(ddlAssetTypes.SelectedValue) < 1)
                        {
                            ErrorDisplay1.ShowError("Please select an Asset Type");
                            ddlAssetTypes.Focus();
                            return;
                        }

                        if(!fldAssetReceipt.HasFile)
                        {
                            ErrorDisplayProcessFixedAssets.ShowError("Please select the Asset Receipt to be attached!");
                            mpeProcessFixedAssetTypePopup.Show();
                            fldAssetReceipt.Focus();
                            return;
                        }

                        if (!AddFixedAsset())
                        {
                            return;
                        }
                        ConfirmAlertBox1.ShowSuccessAlert("Asset information was successfully Added.");
                        break;

                    case 2: //Update
                        if (!UpdateFixedAssetInformation())
                        {
                            return;
                        }
                        ConfirmAlertBox1.ShowSuccessAlert("Asset information was successfully updated.");
                        break;

                    default:
                        ErrorDisplayProcessFixedAssets.ShowError("Invalid process call!");
                        mpeProcessFixedAssetTypePopup.Show();
                        break;
                }

                Session["_fixedAssetsList"] = null;
                LoadFixedAssetsByAssetType();
               
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }

        }
        protected void LnkSelectNewAssetCategoryClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ddlAssetCategories.SelectedIndex = 0;
            dvSelectAssetCategory.Visible = true;
            dvManageFixedAssets.Visible = false;
            dvManageLiquidAssets.Visible = false;
        }
        protected void DdlAssetTypesSelectedChanged(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            if (int.Parse(ddlAssetTypes.SelectedValue) < 1)
            {
                ErrorDisplay1.ShowError("Invalid selection!");
                ddlAssetTypes.Focus();
                return;
            }

            LoadFixedAssetsByAssetType();
            btnAddNewFixedAsset.CommandArgument = "2";
        }
        protected void BtnAddNewFixedAssetClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();

            if (int.Parse(ddlAssetTypes.SelectedValue) < 1)
            {
                ErrorDisplay1.ShowError("Please select an Asset Type.");
                return;
            }
            ErrorDisplayProcessFixedAssets.ClearError();
            ErrorDisplayProcessFixedAssets.ClearControls(tblCreateTFixedAssets);
            chkFixedAssetType.Checked = false;
            txtQuantity.Text = string.Empty;
            lgFixedAssets.InnerHtml = "Add new Fixed Asset";
            btnProcessFixedAssets.CommandArgument = "1";
            btnProcessFixedAssets.Text = "Add";
            assetReceipt.Src = "#";
            mpeProcessFixedAssetTypePopup.CancelControlID = "btnClose";
            mpeProcessFixedAssetTypePopup.PopupControlID = "dvProcessFixedAssetTypes";
            mpeProcessFixedAssetTypePopup.Show();

        }
        protected void DgFixedAssetsItemCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayProcessFixedAssets.ClearError();
            errorDisplayProcessLiquidAssets.ClearControls(tblCreateTFixedAssets);
            ErrorDisplay1.ClearError();
            try
            {
                if (Session["_fixedAssetsList"] == null)
                {
                    ErrorDisplay1.ShowError("Asset list is empty or session has expired.");
                    return;
                }
                var fixedAssetsList = (List<FixedAsset>)Session["_fixedAssetsList"];

                if (fixedAssetsList == null)
                {
                    ErrorDisplay1.ShowError("Asset list is empty or session has expired.");
                    return;
                }
                if (!fixedAssetsList.Any())
                {
                    ErrorDisplay1.ShowError("Asset list is empty or session has expired.");
                    return;
                }

                dgFixedAssets.SelectedIndex = e.Item.ItemIndex;

                var fixedAssetId = (DataCheck.IsNumeric(dgFixedAssets.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgFixedAssets.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (fixedAssetId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid AssetType Selection");
                    return;
                }
               
                var fixedAsset = fixedAssetsList.Find(m => m.FixedAssetId == fixedAssetId);

                if (fixedAsset == null)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }
                if (fixedAsset.FixedAssetId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }

                if (e.CommandName == "Edit")
                {
                    var receipt = Convert.ToBase64String(fixedAsset.ScannedReceipt);
                    assetReceipt.Src = "data:image/jpeg;base64," + receipt;
                    btnProcessFixedAssets.CommandArgument = "2";
                    txtName.Text = fixedAsset.Name;
                    txtQuantity.Text = fixedAsset.Quantity.ToString(CultureInfo.InvariantCulture);
                    txtDescription.Text = fixedAsset.Description;
                    txtCostofPurchase.Text = fixedAsset.CostOfPurchase.ToString(CultureInfo.InvariantCulture);
                    txtInstallationcost.Text = fixedAsset.CostOfTransportationAndInstallation.ToString(CultureInfo.InvariantCulture);
                    txtDatePurchsed.Text = fixedAsset.DatePurchased;
                    txtBrand.Text = fixedAsset.Brand;
                    txtMake.Text = fixedAsset.Make;
                    txtModel.Text = fixedAsset.Model;
                    chkFixedAssetType.Checked = fixedAsset.Status == 1;
                    btnProcessFixedAssets.CommandArgument = "2";
                    btnProcessFixedAssets.Text = "Update";
                    lgFixedAssets.InnerHtml = "Update Asset information";
                    mpeProcessFixedAssetTypePopup.CancelControlID = "btnClose";
                    mpeProcessFixedAssetTypePopup.PopupControlID = "dvProcessFixedAssetTypes";
                    mpeProcessFixedAssetTypePopup.Show();
                    Session["_fixedAsset"] = fixedAsset;
                    Session["_receivedDataArray"] = fixedAsset.ScannedReceipt;
                }

                if (e.CommandName == "ViewReceipt")
                {
                    var receipt = Convert.ToBase64String(fixedAsset.ScannedReceipt);
                    fullReceiptView.Src = "data:image/jpeg;base64," + receipt;
                    lgAssetName.InnerHtml = fixedAsset.Name + " " + "Receipt";
                    mpeProcessFixedAssetTypePopup.CancelControlID = "btnCloseViewReceipt";
                    mpeProcessFixedAssetTypePopup.PopupControlID = "dvViewAssetReceipt";
                    mpeProcessFixedAssetTypePopup.Show();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }

        }
        #endregion

        #region Page Helpers
        private void LoadAssetCategories()
        {
            try
            {
                var assetCategoriesList = ServiceProvider.Instance().GetAssetCategoryServices().GetActiveAssetCategories();

                if (!assetCategoriesList.Any())
                {
                    ErrorDisplay1.ShowError("Asset Categories list is empty!");
                    ddlAssetCategories.DataSource = new List<AssetCategory>();
                    ddlAssetCategories.DataBind();
                    ddlAssetCategories.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlAssetCategories.SelectedIndex = 0;
                    return;
                }


                ddlAssetCategories.DataSource = assetCategoriesList;
                ddlAssetCategories.DataTextField = "Name";
                ddlAssetCategories.DataValueField = "AssetCategoryId";
                ddlAssetCategories.DataBind();
                ddlAssetCategories.Items.Insert(0, new ListItem("--Select an Asset Category--", "0"));
                ddlAssetCategories.SelectedIndex = 0;
                
            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        private void LoadAssetTypes()
        {
            try
            {
                var fixedAssetCategory = AssetCategories.Fixed_Asset;
                var liquidAssetCategory = AssetCategories.Liquid_Asset;
                var x = (int)Enum.Parse(typeof(AssetCategories), Enum.GetName(typeof(AssetCategories), fixedAssetCategory));
                var y = (int)Enum.Parse(typeof(AssetCategories), Enum.GetName(typeof(AssetCategories), liquidAssetCategory));

                ddlAssetTypes.DataSource = new List<AssetType>();
                ddlAssetTypes.DataBind();
                ddlAssetTypes.Items.Insert(0, new ListItem("--List is empty--", "0"));
                ddlAssetTypes.SelectedIndex = 0;
                dgFixedAssets.DataSource = new List<AssetType>();
                dgFixedAssets.DataBind();
                dvSelectAssetCategory.Visible = false;
                dvManageLiquidAssets.Visible = false;
                dvManageFixedAssets.Visible = true;

                var assetCategoryId = int.Parse(ddlAssetCategories.SelectedValue);

                if (assetCategoryId == x)
                {
                    Session["_liquidAssetTypesList"] = null;

                    var assetTypesList = ServiceProvider.Instance().GetAssetTypeServices().AssetTypesByActiveAssetCategory(assetCategoryId);
                    
                    if (!assetTypesList.Any())
                    {
                        ErrorDisplay1.ShowError("Asset Type list is empty.");
                        return;
                    }

                    ddlAssetTypes.DataSource = assetTypesList;
                    ddlAssetTypes.DataTextField = "Name";
                    ddlAssetTypes.DataValueField = "AssetTypeId";
                    ddlAssetTypes.DataBind();
                    ddlAssetTypes.Items.Insert(0, new ListItem("--Select an Asset Type--", "0"));
                    ddlAssetTypes.SelectedIndex = 0;

                    dgFixedAssets.DataSource = new List<AssetType>();
                    dgFixedAssets.DataBind();
                    dvSelectAssetCategory.Visible = false;
                    dvManageLiquidAssets.Visible = false;
                    dvManageFixedAssets.Visible = true;
                    Session["_assetTypes"] = assetTypesList;
                    return;
                }

                if (assetCategoryId == y)
                {
                    Session["_assetTypes"] = null;
                    var liquidAssetTypesList = ServiceProvider.Instance().GetAssetTypeServices().AssetTypesByActiveAssetCategory(assetCategoryId);
                    
                    if (!liquidAssetTypesList.Any())
                    {
                        ErrorDisplay1.ShowError("Asset Type list is empty.");
                        ddlLiquidAssetTypes.DataSource = new List<AssetType>();
                        ddlLiquidAssetTypes.DataBind();
                        ddlLiquidAssetTypes.Items.Insert(0, new ListItem("--List is empty--", "0"));
                        ddlLiquidAssetTypes.SelectedIndex = 0;
                        dgLiquidAssets.DataSource = new List<AssetType>();
                        dgLiquidAssets.DataBind();
                        dvSelectAssetCategory.Visible = false;
                        dvManageFixedAssets.Visible = false;
                        dvManageLiquidAssets.Visible = true;
                        return;
                    }

                    ddlLiquidAssetTypes.DataSource = liquidAssetTypesList;
                    ddlLiquidAssetTypes.DataTextField = "Name";
                    ddlLiquidAssetTypes.DataValueField = "AssetTypeId";
                    ddlLiquidAssetTypes.DataBind();
                    ddlLiquidAssetTypes.Items.Insert(0, new ListItem("--Select an Asset Type--", "0"));
                    ddlLiquidAssetTypes.SelectedIndex = 0;

                    dgLiquidAssets.DataSource = new List<AssetType>();
                    dgLiquidAssets.DataBind();
                    LoadAllLiquidAssets();
                    LoadLiquidAssetsFooter();
                    dvSelectAssetCategory.Visible = false;
                    dvManageFixedAssets.Visible = false;
                    dvManageLiquidAssets.Visible = true;
                    Session["_liquidAssetTypesList"] = liquidAssetTypesList;
                   
                }

            }
            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
               
            }
        }
        private void LoadFixedAssetsByAssetType()
        {
            try
            {
                ErrorDisplay1.ClearError();
                ErrorDisplayProcessFixedAssets.ClearError();
                dgFixedAssets.DataSource = new List<FixedAsset>();
                dgFixedAssets.DataBind();

                var assetTypeId = int.Parse(ddlAssetTypes.SelectedValue);

                if (assetTypeId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Please select an Asset Type.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var selectedFixedAssetTypesList = ServiceProvider.Instance().GetFixedAssetServices().GetFixedAssetsByActiveFixedAssetTypId(assetTypeId);

                if (selectedFixedAssetTypesList == null || !selectedFixedAssetTypesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("No record found!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                //dgFixedAssets.DataSource = selectedFixedAssetTypesList;
                //dgFixedAssets.DataBind();
                Session["_fixedAssetsList"] = null;
                Session["_fixedAssetsList"] = selectedFixedAssetTypesList;
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<FixedAsset>(dgFixedAssets, "_fixedAssetsList", Navigation.None, Limit, LoadMethod);
                LoadFixedAssetsFooter();
            }

            catch(Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace,ex.Source, ex.Message);
            }
        }
        private bool AddFixedAsset()
        {
            try
            {
                var make = string.Empty;
                var model = string.Empty;
                var brand = string.Empty;

                if (Session["_assetCategoryId"] == null)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Asset category List is empty or session has expired");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var assetCategoryId = (int)Session["_assetCategoryId"];

                if (assetCategoryId < 1)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Asset category List is empty or session has expired");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                 var imgBytes = FileUploader();

                 if (imgBytes == null)
                 {
                     ErrorDisplayProcessFixedAssets.ShowError("Please attach the Asset Receipt");
                     mpeProcessFixedAssetTypePopup.ShouldSerializeAnimations();
                     return false;
                 }
                
                if (!string.IsNullOrEmpty(txtBrand.Text.Trim()))
                {
                    brand = txtBrand.Text.Trim();
                }
                
                if (!string.IsNullOrEmpty(txtModel.Text.Trim()))
                {
                    model = txtModel.Text.Trim();
                }

               
                if (!string.IsNullOrEmpty(txtMake.Text.Trim()))
                {
                    make = txtMake.Text.Trim();
                }

                if (GetFixedAssetCode() < 1)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid Process call!");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var newFixedAsset = new FixedAsset
                                        {
                                            Name = txtName.Text.Trim(),
                                            CostOfPurchase = double.Parse(txtCostofPurchase.Text.Trim()),
                                            CostOfTransportationAndInstallation = double.Parse(txtInstallationcost.Text.Trim()),
                                            DatePurchased = txtDatePurchsed.Text.Trim(),
                                            ScannedReceipt = imgBytes,
                                            Status = chkFixedAssetType.Checked ? 1 : 0,
                                            AssetTypeId = int.Parse(ddlAssetTypes.SelectedValue),
                                            AssetCategoryId = assetCategoryId,
                                            Brand = brand,
                                            Model = model,
                                            Make = make,
                                            Description = txtDescription.Text.Trim(),
                                            Quantity = int.Parse(txtQuantity.Text.Trim()),
                                            Code = GetFixedAssetCode()
                                        };

                var k = ServiceProvider.Instance().GetFixedAssetServices().AddFixedAssetCheckDuplicate(newFixedAsset);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessFixedAssets.ShowError("Fixed Asset information already exists.");
                        mpeProcessFixedAssetTypePopup.Show();
                        return false;
                    }

                    ErrorDisplayProcessFixedAssets.ShowError("Fixed Asset information could not be added. Please try again soon or contact the Admin.");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }
               // asset information was successfully added
                return true;
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool UpdateFixedAssetInformation()
        {
            try
            {
                if (Session["_fixedAsset"] == null)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Fixed Asset list is empty or session has expired.");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var fixedAsset = (FixedAsset)Session["_fixedAsset"];

                if (fixedAsset == null || fixedAsset.FixedAssetId < 1)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Session");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (Session["_assetCategoryId"] == null)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Session has expired.");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var assetCategoryId = (int)Session["_assetCategoryId"];

                if (assetCategoryId < 1)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Session has expired.");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }
                
                var brand = string.Empty;

                if (!string.IsNullOrEmpty(txtBrand.Text.Trim()))
                {
                    brand = txtBrand.Text.Trim();
                }

                var model = string.Empty;
                if (!string.IsNullOrEmpty(txtModel.Text.Trim()))
                {
                    model = txtModel.Text.Trim();
                }

                var make = string.Empty;

                if (!string.IsNullOrEmpty(txtMake.Text.Trim()))
                {
                    make = txtMake.Text.Trim();
                }

                fixedAsset.Name = txtName.Text.Trim();
                fixedAsset.CostOfPurchase = double.Parse(txtCostofPurchase.Text.Trim());
                fixedAsset.CostOfTransportationAndInstallation = double.Parse(txtInstallationcost.Text.Trim());
                fixedAsset.DatePurchased = txtDatePurchsed.Text.Trim();
                if (fldAssetReceipt.HasFile)
                {
                   var imgBytes = FileUploader();
                   if (imgBytes != null && imgBytes.Length > 0)
                   {
                       fixedAsset.ScannedReceipt = imgBytes;

                   }
                }
                fixedAsset.Quantity = int.Parse(txtQuantity.Text.Trim());
                fixedAsset.Status = chkFixedAssetType.Checked ? 1 : 0;
                fixedAsset.Brand = brand;
                fixedAsset.Model = model;
                fixedAsset.Make = make;
                fixedAsset.Description = txtDescription.Text.Trim();

                var k = ServiceProvider.Instance().GetFixedAssetServices().UpdateFixedAssetCheckDuplicate(fixedAsset);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessFixedAssets.ShowError("Asset information already exists.");
                        mpeProcessFixedAssetTypePopup.Show();
                        return false;
                    }
                        ErrorDisplayProcessFixedAssets.ShowError("The Asset information could not be updated. Please try again soon or contact the Admin.");
                        mpeProcessFixedAssetTypePopup.Show();
                        return false;
                   
                }

                return true;
                
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private long GetFixedAssetCode()
        {
            var assetTypeId = int.Parse(ddlAssetTypes.SelectedValue);

            var item = ServiceProvider.Instance().GetFixedAssetServices().GetLastFixedAssetByActiveFixedAssetTypId(assetTypeId);

            if (item == null || item.FixedAssetId < 1)
            {
                AssetType assetType = ServiceProvider.Instance().GetAssetTypeServices().GetAssetType(assetTypeId);

                if (assetType == null || assetType.AssetTypeId < 1)
                {
                    return 0;
                }

                var code = assetType.Code + "01";
                long itemCode = long.Parse(code);
                return itemCode;
            }

            return item.Code + 1;
        }
        private bool ValidateControls()
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayProcessFixedAssets.ClearError();
            try
            {
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Please enter an Asset Name.");
                    txtName.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Please supply a Description Asset");
                    txtDescription.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtDatePurchsed.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Please Specify Asset Purchase date");
                    txtDatePurchsed.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (!DateMap.IsDate(DateMap.ReverseToServerDate(txtDatePurchsed.Text.Trim())))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid entry for Asset Purchase date");
                    txtDatePurchsed.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var assetDate = DateTime.Parse(DateMap.ReverseToServerDate(txtDatePurchsed.Text.Trim()));

                if (assetDate > DateTime.Today)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("The Asset Purchase date cannot be LATER than TODAY!");
                    txtDatePurchsed.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtQuantity.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Please specify Asset's Quantity.");
                    txtQuantity.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtQuantity.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid entry for Quantity!");
                    txtInstallationcost.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtCostofPurchase.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid entry for Cost of Purchase!");
                    txtCostofPurchase.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtInstallationcost.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid entry for Cost of Transportation and installation!");
                    txtInstallationcost.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (DataCheck.IsNumeric(txtInstallationcost.Text.Trim()))
                {
                    var installationCost = txtInstallationcost.Text.Trim();

                    if (installationCost.Count() > 1 && installationCost.ElementAt(0) == '0' && installationCost.ElementAt(1) != '0')
                    {
                        ErrorDisplayProcessFixedAssets.ShowError("Invalid entry for Cost of Transportation and installation!");
                        txtInstallationcost.Focus();
                        mpeProcessFixedAssetTypePopup.Show();
                        return false;    
                    }
                    
                }
                
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessFixedAssets.ShowError("An unknown error was encountered. Be sure to you supply the correct formats of all required fields.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private byte[] FileUploader()
        {

            if (!fldAssetReceipt.HasFile)
            {
                ErrorDisplayProcessFixedAssets.ShowError("Select the path to the Asset Receipt");
                return null;
            }

            var response = UploadManager2.UploadFile(ref fldAssetReceipt, UploadManager.ExtensionType.Scanned_Document, false, false);
            if (response == null)
            {
                ErrorDisplayProcessFixedAssets.ShowError("Asset Receipt upload failed.");
                return null;
            }
            if (!response.Succeed)
            {
                ErrorDisplayProcessFixedAssets.ShowError(response.Message);
                return null;
            }

            return response.UploadedData;

        }
        private void LoadFixedAssetsFooter()
        {
            try
            {
                if (dgFixedAssets.Items.Count <= 0) return;
                var totalQuantity = 0;
                double totalCostOfPurchse = 0;
                double subTotalCost = 0;
                double totalInstallationCost = 0;
                for (var i = 0; i < dgFixedAssets.Items.Count; i++)
                {
                    totalQuantity += DataCheck.IsNumeric(((Label)dgFixedAssets.Items[i].FindControl("lblQuantity")).Text)
                                         ? int.Parse(((Label)dgFixedAssets.Items[i].FindControl("lblQuantity")).Text)
                                         : 0;
                    totalCostOfPurchse += DataCheck.IsNumeric(((Label)dgFixedAssets.Items[i].FindControl("lblCostOfPurchase")).Text)
                                              ? double.Parse(((Label)dgFixedAssets.Items[i].FindControl("lblCostOfPurchase")).Text)
                                              : 0;

                    totalInstallationCost += DataCheck.IsNumeric(((Label)dgFixedAssets.Items[i].FindControl("lblTransportationInstallationCost")).Text)
                                                 ? double.Parse(((Label)dgFixedAssets.Items[i].FindControl("lblTransportationInstallationCost")).Text)
                                                 : 0;

                    subTotalCost += DataCheck.IsNumeric(((Label)dgFixedAssets.Items[i].FindControl("lblsubTotalCost")).Text)
                                                 ? double.Parse(((Label)dgFixedAssets.Items[i].FindControl("lblsubTotalCost")).Text)
                                                 : 0;

                }

                foreach (var item in from object item in dgFixedAssets.Controls[0].Controls where item.GetType() == typeof(DataGridItem) let itmType = ((DataGridItem)item).ItemType where itmType == ListItemType.Footer select item)
                {
                    if (((DataGridItem)item).FindControl("lblTotalQuantity") != null)
                    {
                        ((Label)((DataGridItem)item).FindControl("lblTotalQuantity")).Text = totalQuantity.ToString(CultureInfo.InvariantCulture);
                    }
                    if (((DataGridItem)item).FindControl("lblTotalCostOfPurchase") != null)
                    {
                        ((Label)((DataGridItem)item).FindControl("lblTotalCostOfPurchase")).Text = "N" + NumberMap.GroupToDigits(totalCostOfPurchse.ToString(CultureInfo.InvariantCulture));
                    }

                    if (((DataGridItem)item).FindControl("lblTotalTransportationInstallationCost") != null)
                    {
                        ((Label)((DataGridItem)item).FindControl("lblTotalTransportationInstallationCost")).Text = "N" + NumberMap.GroupToDigits(totalInstallationCost.ToString(CultureInfo.InvariantCulture));
                    }

                    if (((DataGridItem)item).FindControl("lblGrandTotalCost") != null)
                    {
                        ((Label)((DataGridItem)item).FindControl("lblGrandTotalCost")).Text = "N" + NumberMap.GroupToDigits(subTotalCost.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown was encountered.");
                
            }
        }
        #endregion
        #endregion

        #region Liquid Assets

        #region Page Events

        protected void BtnProcessLiquidAssetClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ErrorDisplayProcessFixedAssets.ClearError();
            if (!ValidateLiquidControls())
            {
                return;
            }
            try
            {
                switch (int.Parse(btnProcessLiquidAsset.CommandArgument))
                {
                    case 1: //Add

                        if (int.Parse(ddlLiquidAssetTypes.SelectedValue) < 1)
                        {
                            ErrorDisplay1.ShowError("Please select an Asset Type");
                            ddlLiquidAssetTypes.Focus();
                            return;
                        }

                        if (!AddLiquidAsset())
                        {
                            return;
                        }
                        
                        break;

                    case 2: //Update
                        if (!UpdateLiquidAssetInformation())
                        {
                            return;
                        }
                        break;

                    default:
                        ErrorDisplayProcessFixedAssets.ShowError("Invalid process call!");
                        mpeProcessFixedAssetTypePopup.Show();
                        break;
                }

                if (!LoadAllLiquidAssets())
                {
                    return;
                }

                if (btnAddNewLiquidAsset.CommandArgument == "1")
                {
                    if(!ShowAllLiquidAssets())
                    {
                        return;
                    }
                }
                else
                {
                    if(!LoadLiquidAssetsByAssetType())
                    {
                        return;
                    }
                }
                ErrorDisplay1.ShowSuccess("Asset information was successfully processed.");
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }

        }

        protected void LnkDifferentAssetCategoryClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ddlAssetCategories.SelectedIndex = 0;
            dvSelectAssetCategory.Visible = true;
            dvManageFixedAssets.Visible = false;
            dvManageLiquidAssets.Visible = false;
        }

        protected void DdlLiquidAssetTypesSelectedChanged(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            if (int.Parse(ddlLiquidAssetTypes.SelectedValue) < 1)
            {
                ErrorDisplay1.ShowError("Invalid selection!");
                ddlLiquidAssetTypes.Focus();
                return;
            }

            LoadLiquidAssetsByAssetType();
            btnAddNewLiquidAsset.CommandArgument = "2";
        }

        protected void LnkShowAllLiquidAssetsClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            ddlLiquidAssetTypes.SelectedIndex = 0;
            ShowAllLiquidAssets();
            btnAddNewLiquidAsset.CommandArgument = "1";
        }

        protected void BtnAddNewLiquidAssetClick(object sender, EventArgs e)
        {
            errorDisplayProcessLiquidAssets.ClearError();

            if (int.Parse(ddlLiquidAssetTypes.SelectedValue) < 1)
            {
                ErrorDisplay1.ShowError("Please select an Asset Category.");
                return;
            }
            errorDisplayProcessLiquidAssets.ClearControls(tblCreateTFixedAssets);
            chkLiquidAssetStatus.Checked = false;
            txtLiquidAssetName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            btnProcessLiquidAsset.Text = "Add";
            lgProcessLiquidAssets.InnerHtml = "Add new Liquid Asset";
            btnProcessLiquidAsset.CommandArgument = "1";
            mpeProcessFixedAssetTypePopup.CancelControlID = "btnCloseLiquidAssetPopup";
            mpeProcessFixedAssetTypePopup.PopupControlID = "dvProcessLiquidAssets";
            mpeProcessFixedAssetTypePopup.Show();

        }

        protected void DgLiquidAssetsEditCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayProcessFixedAssets.ClearError();
            ErrorDisplay1.ClearError();
            try
            {
                if (Session["_liquidAssetsList"] == null)
                {
                    ErrorDisplay1.ShowError("Asset list is empty or session has expired.");
                    return;
                }

                var liquidAssetsList = (List<LiquidAsset>)Session["_liquidAssetsList"];

                if (liquidAssetsList == null)
                {
                    ErrorDisplay1.ShowError("Asset list is empty or session has expired.");
                    return;
                }
                if (!liquidAssetsList.Any())
                {
                    ErrorDisplay1.ShowError("Asset list is empty or session has expired.");
                    return;
                }

                dgLiquidAssets.SelectedIndex = e.Item.ItemIndex;

                var liquidAssetId = (DataCheck.IsNumeric(dgLiquidAssets.DataKeys[e.Item.ItemIndex].ToString())) ? long.Parse(dgLiquidAssets.DataKeys[e.Item.ItemIndex].ToString()) : 0;

                if (liquidAssetId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid AssetType Selection");
                    return;
                }

                var liquidAsset = liquidAssetsList.Find(m => m.LiquidAssetId == liquidAssetId);

                if (liquidAsset == null)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }
                if (liquidAsset.LiquidAssetId < 1)
                {
                    ErrorDisplay1.ShowError("Invalid selection!");
                    return;
                }

                txtAmount.Text = liquidAsset.Amount.ToString(CultureInfo.InvariantCulture);
                txtLiquidAssetName.Text = liquidAsset.Name;
                chkLiquidAssetStatus.Checked = liquidAsset.Status == 1;
                btnProcessLiquidAsset.CommandArgument = "2"; //update
                btnProcessLiquidAsset.Text = "Update";
                lgProcessLiquidAssets.InnerHtml = "Update Liquid Asset information";
                mpeProcessFixedAssetTypePopup.CancelControlID = "btnCloseLiquidAssetPopup";
                mpeProcessFixedAssetTypePopup.PopupControlID = "dvProcessLiquidAssets";
                mpeProcessFixedAssetTypePopup.Show();
                Session["_liquidAsset"] = liquidAsset;
            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Please try again soon or contact your site Admin.");
                
            }

        }
        #endregion

        #region Page Helpers
        private bool LoadLiquidAssetsByAssetType()
        {
            try
            {
                dgLiquidAssets.DataSource = new List<LiquidAsset>();
                dgLiquidAssets.DataBind();

                if (Session["_allLiquidAssetsList"] == null)
                {
                    ErrorDisplay1.ShowError("Liquid Asset list is empty or session has expired.");
                    return false;
                }

                var allLiquidAssetsList = (List<LiquidAsset>)Session["_allLiquidAssetsList"];

                if (!allLiquidAssetsList.Any())
                {
                    ErrorDisplay1.ShowError("Liquid Asset list is empty or session has expired.");
                    return false;
                }

                var liquidAssetTypeId = int.Parse(ddlLiquidAssetTypes.SelectedValue);

                var selectedLiquidAssetTypesList = allLiquidAssetsList.FindAll(m => m.AssetTypeId == liquidAssetTypeId);

                if (!selectedLiquidAssetTypesList.Any())
                {
                    ErrorDisplay1.ShowError("Liquid Asset list is empty or session has expired.");
                    return false;
                }

                dgLiquidAssets.DataSource = selectedLiquidAssetTypesList.OrderBy(m => m.AssetCategory.Name).ThenBy(m => m.Name);
                dgLiquidAssets.DataBind();
                LoadLiquidAssetsFooter();
                Session["_liquidAssetsList"] = selectedLiquidAssetTypesList;
                return true;
            }

            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                return false;
            }
        }
        private bool LoadAllLiquidAssets()
        {
            try
            {
                var allLiquidAssetsList =
                    ServiceProvider.Instance().GetLiquidAssetServices().GetLiquidAssets();
                if (!allLiquidAssetsList.Any())
                {
                    dgFixedAssets.DataSource = new List<AssetType>();
                    dgFixedAssets.DataBind();
                    return false;
                }

                Session["_allLiquidAssetsList"] = allLiquidAssetsList;
                return true;
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool ShowAllLiquidAssets()
        {
            try
            {

                dgLiquidAssets.DataSource = new List<LiquidAsset>();
                dgLiquidAssets.DataBind();

                if (Session["_allLiquidAssetsList"] == null)
                {
                    ErrorDisplay1.ShowError("Liquid Asset list is empty or session has expired.");
                    return false;
                }

                var allLiquidAssetsList = (List<LiquidAsset>)Session["_allLiquidAssetsList"];

                if (!allLiquidAssetsList.Any())
                {
                    ErrorDisplay1.ShowError("Liquid Asset list is empty or session has expired.");
                    return false;
                }

                dgLiquidAssets.DataSource = allLiquidAssetsList.OrderBy(m => m.AssetCategory.Name).ThenBy(m => m.Name);
                dgLiquidAssets.DataBind();
                LoadLiquidAssetsFooter();
                Session["_liquidAssetsList"] = allLiquidAssetsList;
                return true;
            }

            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                return false;
            }
        }
        private bool AddLiquidAsset()
        {
            try
            {

                if (Session["_assetCategoryId"] == null)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Image data is empty or session has expired");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var assetCategoryId = (int)Session["_assetCategoryId"];

                if (assetCategoryId < 1)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Image data is empty or session has expired");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }


                var newLiquidAsset = new LiquidAsset
                                        {
                                            Name = txtLiquidAssetName.Text.Trim(),
                                            Status = chkLiquidAssetStatus.Checked ? 1 : 0,
                                            Amount = double.Parse(txtAmount.Text.Trim()),
                                            AssetTypeId = int.Parse(ddlLiquidAssetTypes.SelectedValue),
                                            AssetCategoryId = assetCategoryId,
                                            Code = GetLiquidAssetCode().ToString(CultureInfo.InvariantCulture)
                                        };

                var k = ServiceProvider.Instance().GetLiquidAssetServices().AddLiquidAsset(newLiquidAsset);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessFixedAssets.ShowError("Liquid Asset information already exists.");
                        return false;
                    }
                    ErrorDisplayProcessFixedAssets.ShowError("Liquid Asset information could not be added. Please try again soon or contact the Admin.");
                    return false;
                }
                return true;
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private bool UpdateLiquidAssetInformation()
        {
            try
            {
                if (Session["_liquidAsset"] == null)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Liquid Asset list is empty or session has expired");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var liquidAsset = (LiquidAsset)Session["_liquidAsset"];

                if (liquidAsset == null)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid process call!");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (liquidAsset.LiquidAssetId < 1)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid process call!");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (Session["_assetCategoryId"] == null)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Image data is empty or session has expired");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                var assetCategoryId = (int)Session["_assetCategoryId"];

                if (assetCategoryId < 1)
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Image data is empty or session has expired");
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }


                liquidAsset.Name = txtLiquidAssetName.Text.Trim();
                liquidAsset.Status = chkLiquidAssetStatus.Checked ? 1 : 0;
                liquidAsset.Amount = double.Parse(txtAmount.Text.Trim());

                if (!ServiceProvider.Instance().GetLiquidAssetServices().UpdateLiquidAsset(liquidAsset))
                {
                    ErrorDisplay1.ShowError("The Asset information could not be updated. Please try again soon or contact the Admin.");
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        private long GetLiquidAssetCode()
        {
            long itemCode;

            if (Session["_liquidAssetsList"] == null)
            {
                if (Session["_liquidAssetTypesList"] == null)
                {
                    return 0;
                }

                var liquidAssetTypes = (List<AssetType>)Session["_liquidAssetTypesList"];

                if (liquidAssetTypes == null)
                {
                    return 0;
                }

                if (!liquidAssetTypes.Any())
                {
                    return 0;
                }

                var liquidAssetType = liquidAssetTypes.Find(m => m.AssetTypeId == int.Parse(ddlLiquidAssetTypes.SelectedValue));

                if (liquidAssetType == null)
                {
                    return 0;
                }

                if (liquidAssetType.AssetTypeId < 1)
                {
                    return 0;
                }

                var code = liquidAssetType.Code + "01";

                itemCode = long.Parse(code);
                return itemCode;
            }

            var liquidAssetsList = (List<LiquidAsset>)Session["_liquidAssetsList"];

            if (!liquidAssetsList.Any())
            {
                if (Session["_liquidAssetTypesList"] == null)
                {
                    return 0;
                }

                var liquidAssetTypes = (List<AssetType>)Session["_liquidAssetTypesList"];

                if (liquidAssetTypes == null)
                {
                    return 0;
                }

                if (!liquidAssetTypes.Any())
                {
                    return 0;
                }

                var liquidAssetType = liquidAssetTypes.Find(m => m.AssetTypeId == int.Parse(ddlLiquidAssetTypes.SelectedValue));

                if (liquidAssetType == null)
                {
                    return 0;
                }

                if (liquidAssetType.AssetTypeId < 1)
                {
                    return 0;
                }

                var code = liquidAssetType.Code + "01";

                itemCode = long.Parse(code);
                return itemCode;
            }

            var itemsList = ServiceProvider.Instance().GetLiquidAssetServices().GetLasInsertedLiquidAsset(liquidAssetsList, int.Parse(ddlLiquidAssetTypes.SelectedValue));

            if (!itemsList.Any())
            {
                if (Session["_liquidAssetTypesList"] == null)
                {
                    return 0;
                }

                var liquidAssetTypes = (List<AssetType>)Session["_liquidAssetTypesList"];

                if (liquidAssetTypes == null)
                {
                    return 0;
                }

                if (!liquidAssetTypes.Any())
                {
                    return 0;
                }
                var liquidAssetType = liquidAssetTypes.Find(m => m.AssetTypeId == int.Parse(ddlLiquidAssetTypes.SelectedValue));

                if (liquidAssetType == null)
                {
                    return 0;
                }

                if (liquidAssetType.AssetTypeId < 1)
                {
                    return 0;
                }
                var code = liquidAssetType.Code + "01";
                itemCode = long.Parse(code);
                return itemCode;
            }

            return long.Parse(itemsList.First().Code) + 1;
        }
        private bool ValidateLiquidControls()
        {
            ErrorDisplay1.ClearError();
            try
            {
                if (string.IsNullOrEmpty(txtLiquidAssetName.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Please enter an Asset Name.");
                    txtLiquidAssetName.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Please supply the monetary equivalent of the Asset.");
                    txtAmount.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                if (!DataCheck.IsNumeric(txtAmount.Text.Trim()))
                {
                    ErrorDisplayProcessFixedAssets.ShowError("Invalid entry!");
                    txtAmount.Focus();
                    mpeProcessFixedAssetTypePopup.Show();
                    return false;
                }

                return true;
            }
            catch
            {
                ErrorDisplayProcessFixedAssets.ShowError("An unknown error was encountered. Please try again soon or contact the Admin. Make sure you supply all the required fields.");
                return false;
            }
        }
        private void LoadLiquidAssetsFooter()
        {
            try
            {
                if (dgLiquidAssets.Items.Count > 0)
                {
                    double totalAmount = 0;
                    for (var i = 0; i < dgLiquidAssets.Items.Count; i++)
                    {
                        totalAmount += kPortal.CoreUtilities.DataCheck.IsNumeric(((Label)dgLiquidAssets.Items[i].FindControl("lblLiquidAssetAmount")).Text)
                                     ? double.Parse(((Label)dgLiquidAssets.Items[i].FindControl("lblLiquidAssetAmount")).Text)
                                     : 0;

                    }

                    foreach (var item in dgLiquidAssets.Controls[0].Controls)
                    {
                        if (item.GetType() == typeof(DataGridItem))
                        {
                            var itmType = ((DataGridItem)item).ItemType;
                            if (itmType == ListItemType.Footer)
                            {
                                if (((DataGridItem)item).FindControl("lblLiquidAssetTotalAmount") != null)
                                {
                                    ((Label)((DataGridItem)item).FindControl("lblLiquidAssetTotalAmount")).Text = "N" + NumberMap.GroupToNumbers(totalAmount.ToString(CultureInfo.InvariantCulture));
                                }

                            }
                        }
                    }
                }
            }
            catch
            {
                ErrorDisplay1.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
            }
        }
        #endregion
        #endregion
        
        #region Pagination
        protected void FirstLink(object sender, EventArgs e)
        {
            try
            { 
                ErrorDisplay1.ClearError();
                var senderLinkArgument = int.Parse(((LinkButton)sender).CommandArgument);
                NowViewing = (int)senderLinkArgument;
                FillRepeater<FixedAsset>(dgFixedAssets, "_fixedAssetsList", Navigation.Sorting, Limit, LoadMethod);
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
            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<FixedAsset>(dgFixedAssets, "_fixedAssetsList", Navigation.Next, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnFirstClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<FixedAsset>(dgFixedAssets, "_fixedAssetsList", Navigation.First, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnLastClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<FixedAsset>(dgFixedAssets, "_fixedAssetsList", Navigation.Last, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void LbtnPrevClick(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                FillRepeater<FixedAsset>(dgFixedAssets, "_fixedAssetsList", Navigation.Previous, Limit, LoadMethod);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

            }
        }
        protected void OnLimitChange(object sender, EventArgs e)
        {
            ErrorDisplay1.ClearError();
            try
            {
                Limit = int.Parse(ddlLimit.SelectedValue);
                FillRepeater<FixedAsset>(dgFixedAssets, "_fixedAssetsList", Navigation.None, Limit, LoadMethod);
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
             LoadFixedAssetsByAssetType();
            return true;
        }
        #endregion
        #endregion
    }
}