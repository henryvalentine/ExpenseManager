using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageUnit : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               if(!LoadDepartments())
                {
                    return; 
                }

                LoadUnits();

            }

        }
        protected void BtnProcessUnitClick(object sender, EventArgs e)
        {
            try
            {

                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnProcessUnit.CommandArgument))
                {
                    case 1:
                        if (!AddUnit())
                        {
                            return;
                        }

                        break;

                    case 2:
                        if (!UpdateUnit())
                        {
                            return;
                        }
                        break;

                    default:
                        ConfirmAlertBox1.ShowMessage("Invalid process call!", ConfirmAlertBox.PopupMessageType.Error);
                        mpeProcessUnit.Show();
                        break;
                }

                if (!LoadUnits())
                {
                    ConfirmAlertBox1.ShowMessage("Unit information was processed successfully but an error was encountered while trying to refresh the list.\n Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Unit information was processed successfully.");

            }

            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }


        }
        protected void DgUnitsEditCommand(object source, DataGridCommandEventArgs e)
        {
            txtName.Text = string.Empty;
            try
            {
              
                dgUnits.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgUnits.DataKeys[e.Item.ItemIndex].ToString()))
                             ? int.Parse(dgUnits.DataKeys[e.Item.ItemIndex].ToString())
                             : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var unit = ServiceProvider.Instance().GetUnitServices().GetUnit(id);
                if (unit == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                if (unit.UnitId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                txtName.Text = unit.Name;
                ddlDepartment.SelectedValue = unit.DepartmentId.ToString(CultureInfo.InvariantCulture);
                chkUnit.Checked = unit.Status == 1;
                btnProcessUnit.CommandArgument = "2";
                btnProcessUnit.Text = "Update";
                lgTitle.InnerText = "Update Unit";
                mpeProcessUnit.Show();
                Session["_unit"] = unit;
            }

            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        protected void DgUnitsDeleteCommand(object source, DataGridCommandEventArgs e)
        {

            try
            {
                dgUnits.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgUnits.DataKeys[e.Item.ItemIndex].ToString()))
                             ? int.Parse(dgUnits.DataKeys[e.Item.ItemIndex].ToString())
                             : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!ServiceProvider.Instance().GetUnitServices().DeleteUnit(id))
                {
                    ConfirmAlertBox1.ShowMessage("The requested operation could not be completed. The Unit is likely being referenced by a Beneficiary.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!LoadUnits())
                {
                    ConfirmAlertBox1.ShowMessage("Unit information was successfully deleted but an error was encountered while trying to refresh the list.\n Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Unit information was successfully deleted.");
            }

            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnAddNewUnitClick(object sender, EventArgs e)
        {
            ClearControls();
            lgTitle.InnerText = "Add New Unit";
            btnProcessUnit.CommandArgument = "1";
            btnProcessUnit.Text = "Submit";
            mpeProcessUnit.Show();
        }
        #endregion

        #region Page Helpers
        private bool LoadUnits()
        {
            try
            {
                dgUnits.DataSource = new List<xPlug.BusinessObject.Unit>();
                dgUnits.DataBind();

                var unitList = ServiceProvider.Instance().GetUnitServices().GetAllOrderedUnits();

                if (unitList == null || !unitList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Unit list is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                dgUnits.DataSource = unitList;
                dgUnits.DataBind();
                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool LoadDepartments()
        {
            try
            {
                var filteredDepartmentsList = ServiceProvider.Instance().GetDepartmentServices().GetActiveOrderedDepartments();

                if (filteredDepartmentsList == null || !filteredDepartmentsList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty", ConfirmAlertBox.PopupMessageType.Error);
                    ddlDepartment.DataSource = new List<Department>();
                    ddlDepartment.Items.Insert(0, new ListItem("--List is empty--", "0"));
                    ddlDepartment.SelectedIndex = 0;
                    return false;
                }

                ddlDepartment.DataSource = filteredDepartmentsList;
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DepartmentId";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("--Select Department--", "0"));
                ddlDepartment.SelectedIndex = 0;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                return false;
            }

        }
        private bool AddUnit()
        {
            try
            {

                var newUnit = new xPlug.BusinessObject.Unit
                                    {
                                        Name = txtName.Text.Trim(),
                                        DepartmentId = int.Parse(ddlDepartment.SelectedValue),
                                        Status = chkUnit.Checked ? 1 : 0
                                    };

                var k = ServiceProvider.Instance().GetUnitServices().AddUnitCheckDuplicate(newUnit);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDispManageUnit.ShowError("Unit Information already exists.");
                        txtName.Focus();
                        mpeProcessUnit.Show();
                        return false;
                    }
                    ErrorDispManageUnit.ShowError("Unit Information could not be added. Please try again later or contact the Administrator.");
                    mpeProcessUnit.Show();
                    return false;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Unit Information was added to successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }

        }
        private bool UpdateUnit()
        {
            try
            {
                if (Session["_unit"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    Response.Redirect("~/Login.aspx");
                    return false;
                }

                var unit = Session["_unit"] as xPlug.BusinessObject.Unit;

                if (unit == null || unit.UnitId < 1)
                {
                    ErrorDispManageUnit.ShowError("Session has expired");
                    Response.Redirect("~/Login.aspx");
                    return false;
                }

                unit.Name = txtName.Text.Trim();
                unit.DepartmentId = int.Parse(ddlDepartment.SelectedValue);
                unit.Status = chkUnit.Checked ? 1 : 0;

                var k = ServiceProvider.Instance().GetUnitServices().UpdateUnitCheckDuplicate(unit);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDispManageUnit.ShowError("Unit already exists.");
                        txtName.Focus();
                        mpeProcessUnit.Show();
                    }


                    else
                    {
                        ErrorDispManageUnit.ShowError("Unit information could not be updated.");
                        mpeProcessUnit.Show();
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool ValidateControls()
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ErrorDispManageUnit.ShowError("Please enter a Unit.");
                txtName.Focus();
                mpeProcessUnit.Show();
                return false;
            }

            if (int.Parse(ddlDepartment.SelectedValue) < 1)
            {
                ErrorDispManageUnit.ShowError("Please select a Department.");
                ddlDepartment.Focus();
                mpeProcessUnit.Show();
                return false;
            }
            return true;
        }
        private void ClearControls()
        {
            txtName.Text = string.Empty;
            ddlDepartment.SelectedIndex = 0;
            chkUnit.Checked = false;
        }
        #endregion
    }
}