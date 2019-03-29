using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExpenseManager.CoreFramework.AlertControl;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessService;

namespace ExpenseManager.ExpenseMgt
{
    public partial class FrmManageDepartments : UserControl
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDepartments();

            }

        }
        protected void BtnProcessDepartmentClick(object sender, EventArgs e)
        {
          
            try
            {
               
                if (!ValidateControls())
                {
                    return;
                }

                switch (int.Parse(btnProcessDepartment.CommandArgument))
                {
                    case 1:
                        if (!AddDepartment())
                        {
                            return;
                        }
                       
                        break;

                    case 2:
                        if (!UpdateDepartment())
                        {
                            return;
                        }
                        break;

                    default:
                        ErrorDisplayProcessDepartment.ShowError("Invalid process call!");
                        mpeProcessDepartment.Show();
                        break;
                }
                if (!LoadDepartments())
                {
                    ConfirmAlertBox1.ShowMessage("Department information was managed successfully but an error was encountered while trying to refresh the list.\n Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
               ConfirmAlertBox1.ShowSuccessAlert("Department information was processed successfully.");

            }

            catch (Exception ex)
            {
                ErrorDisplayProcessDepartment.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }


        }
        protected void DgDepartmentsEditCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayProcessDepartment.ClearError();
          
            txtName.Text = string.Empty;
            try
            {
                if (Session["_departmentsList"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty or session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var expenseaCategoriesList = (List<Department>)Session["_departmentsList"];

                if (expenseaCategoriesList == null)
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty or session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!expenseaCategoriesList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty or session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                dgDepartments.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgDepartments.DataKeys[e.Item.ItemIndex].ToString()))
                             ? long.Parse(dgDepartments.DataKeys[e.Item.ItemIndex].ToString())
                             : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var department = expenseaCategoriesList.Find(m => m.DepartmentId == id);
                if (department == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                if (department.DepartmentId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                txtName.Text = department.Name;
                chkDepartment.Checked = department.Status == 1;
                btnProcessDepartment.CommandArgument = "2";
                btnProcessDepartment.Text = "Update";
                lgTitle.InnerText = "Update Department";
                mpeProcessDepartment.Show();
                Session["_department"] = department;
            }

            catch (Exception ex)
            {
                ErrorDisplayProcessDepartment.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        protected void DgDepartmentsDeleteCommand(object source, DataGridCommandEventArgs e)
        {
            ErrorDisplayProcessDepartment.ClearError();
          
           
            try
            {
                if (Session["_departmentsList"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty or session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var departmentsList = Session["_departmentsList"] as List<Department>;

                if (departmentsList == null || !departmentsList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty or session has expired!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                
                dgDepartments.SelectedIndex = e.Item.ItemIndex;

                var id = (DataCheck.IsNumeric(dgDepartments.DataKeys[e.Item.ItemIndex].ToString()))
                             ? long.Parse(dgDepartments.DataKeys[e.Item.ItemIndex].ToString())
                             : 0;

                if (id < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid Selection", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var department = departmentsList.Find(m => m.DepartmentId == id);
                if (department == null)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }
                if (department.DepartmentId < 1)
                {
                    ConfirmAlertBox1.ShowMessage("Invalid selection!", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if(!ServiceProvider.Instance().GetDepartmentServices().DeleteDepartment(department.DepartmentId))
                {
                    ConfirmAlertBox1.ShowMessage("The requested operation could not be completed. The Department is likely being referenced by a Beneficiary.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                if (!LoadDepartments())
                {
                    ConfirmAlertBox1.ShowMessage("Department information was successfully deleted but an error was encountered while trying to refresh the list.\n Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Department information was successfully deleted.");
            }

            catch (Exception)
            {
                ConfirmAlertBox1.ShowMessage("An unknown error was encountered. Please try again soon or contact the Admin.", ConfirmAlertBox.PopupMessageType.Error);
            }
        }
        protected void BtnAddNewDepartmentClick(object sender, EventArgs e)
        {
          
            ClearControls();
            lgTitle.InnerText = "Add a New Department";
            btnProcessDepartment.CommandArgument = "1";
            btnProcessDepartment.Text = "Submit";
            mpeProcessDepartment.Show();
        }
        #endregion

        #region Page Helpers
        private bool LoadDepartments()
        {
            try
            { 
                dgDepartments.DataSource = new List<Department>();
                    dgDepartments.DataBind();

                    var departmentsList = ServiceProvider.Instance().GetDepartmentServices().GetOrderedDepartments();

                if (departmentsList == null)
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                if (!departmentsList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("Department list is empty!", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                dgDepartments.DataSource = departmentsList;
                dgDepartments.DataBind();
                Session["_departmentsList"] = departmentsList;
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessDepartment.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool AddDepartment()
        {
          
            try
            {

                var newDepartment = new Department
                                        {
                                            Name = txtName.Text.Trim(),
                                            Status = chkDepartment.Checked ? 1 : 0
                                        };

                var k = ServiceProvider.Instance().GetDepartmentServices().AddDepartmentCheckDuplicate(newDepartment);

                if (k < 1)
                {
                    if (k == -3)
                    {
                        ConfirmAlertBox1.ShowMessage("Department Information already exists.", ConfirmAlertBox.PopupMessageType.Error);
                        return false;
                    }
                    ConfirmAlertBox1.ShowMessage("Department Information could not be added.", ConfirmAlertBox.PopupMessageType.Error);
                    return false;
                }

                ConfirmAlertBox1.ShowSuccessAlert("Department Information was added to successfully.");
                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessDepartment.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }

        }
        private bool UpdateDepartment()
        {
            try
            {
                if (Session["_department"] == null)
                {
                    ErrorDisplayProcessDepartment.ShowError("Department list is empty or session has expired.");
                    mpeProcessDepartment.Show();
                    return false;
                }

                var department = Session["_department"] as Department;

                if (department == null || department.DepartmentId < 1)
                {
                    ErrorDisplayProcessDepartment.ShowError("Invalid selection!");
                    mpeProcessDepartment.Show();
                    return false;
                }

                department.Name = txtName.Text.Trim();
                department.Status = chkDepartment.Checked ? 1 : 0;

                var k = ServiceProvider.Instance().GetDepartmentServices().UpdateDepartmentCheckDuplicate(department);
                if (k < 1)
                {
                    if (k == -3)
                    {
                        ErrorDisplayProcessDepartment.ShowError("Department already exists.");
                        txtName.Focus();
                        mpeProcessDepartment.Show();
                    }


                    else
                    {
                        ErrorDisplayProcessDepartment.ShowError("Department information could not be updated.");
                        mpeProcessDepartment.Show();
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorDisplayProcessDepartment.ShowError("An unknown error was encountered. Please try again soon or contact the Admin.");
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                throw;
            }
        }
        private bool ValidateControls()
        {
          
            ErrorDisplayProcessDepartment.ClearError();
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ErrorDisplayProcessDepartment.ShowError("Please enter a Department.");
                txtName.Focus();
                mpeProcessDepartment.Show();
                return false;
            }
            
            //if (RegExValidation.IsNameValid(txtName.Text.Trim()))
            //{
            //    ErrorDisplayProcessDepartment.ShowError("Invalid entry!");
            //    txtName.Focus();
            //    mpeProcessDepartment.Show();
            //    return false;
            //}
            
            return true;
        }
        private void ClearControls()
        {
            ErrorDisplayProcessDepartment.ClearError();
            txtName.Text = string.Empty;
            chkDepartment.Checked = false;
        }
        #endregion
    }
}