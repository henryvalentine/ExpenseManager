using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;
using ExpenseManager.CoreFramework;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;

namespace ExpenseManager
{
    /// <summary>
    /// Summary description for expenseManagerStructuredServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class ExpenseManagerStructuredServices : WebService
    {
        [WebMethod]
        public CascadingDropDownNameValue[] LoadExpenseCategoriesService(string knownCategoryValues, string category)
        {
            var expenseCategories =
                ServiceProvider.Instance().GetExpenseCategoryServices().GetExpenseCategories().Where(m => m.Status == 1)
                    .ToList();
            return !expenseCategories.Any()
                ? new List<CascadingDropDownNameValue>().ToArray()
                : expenseCategories.Select(
                    item =>
                        new CascadingDropDownNameValue(item.Title,
                            item.ExpenseCategoryId.ToString(CultureInfo.InvariantCulture))).ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadAccountsHeadList(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            int expenseCategoryId;

            var accountsHeadList = new List<AccountsHead>();

            if (!kv.ContainsKey("ExpenseCategoryId") || !Int32.TryParse(kv["ExpenseCategoryId"], out expenseCategoryId))
            {
                return
                    accountsHeadList.Select(
                        item =>
                            new CascadingDropDownNameValue(item.Title,
                                item.ExpenseCategoryId.ToString(CultureInfo.InvariantCulture))).
                        ToArray();

            }

            accountsHeadList =
                ServiceProvider.Instance().GetAccountsHeadServices().GetAccountsHeadsByExpenseCategoryId(
                    expenseCategoryId).Where(m => m.Status == 1).ToList();

            if (accountsHeadList.Count < 1)
            {
                return new List<CascadingDropDownNameValue>().ToArray();
            }

            accountsHeadList = accountsHeadList.OrderBy(m => m.Title).ToList();

            return
                accountsHeadList.Select(
                    item =>
                        new CascadingDropDownNameValue(item.Title,
                            item.AccountsHeadId.ToString(CultureInfo.InvariantCulture))).ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadAccountsHeadService(string knownCategoryValues, string category)
        {
            var accountsHeadList =
                ServiceProvider.Instance().GetAccountsHeadServices().GetActiveOrderedAccountsHeads().ToList();
            return !accountsHeadList.Any()
                ? new List<CascadingDropDownNameValue>().ToArray()
                : accountsHeadList.Select(
                    item =>
                        new CascadingDropDownNameValue(item.Title,
                            item.AccountsHeadId.ToString(CultureInfo.InvariantCulture))).ToArray();

        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadExpenseItemList(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            int accountHeadId;

            var expenseItemList = new List<ExpenseItem>();

            if (!kv.ContainsKey("AccountHeadId") || !Int32.TryParse(kv["AccountHeadId"], out accountHeadId))
            {
                return
                    expenseItemList.Select(
                        item =>
                            new CascadingDropDownNameValue(item.Title,
                                item.ExpenseItemId.ToString(CultureInfo.InvariantCulture))).ToArray();

            }

            expenseItemList =
                ServiceProvider.Instance().GetExpenseItemServices().GetExpenseItemsByAccountsHeadId(accountHeadId);

            if (expenseItemList.Count < 1)
            {
                return new List<CascadingDropDownNameValue>().ToArray();
            }
            return
                expenseItemList.Select(
                    item =>
                        new CascadingDropDownNameValue(item.Title,
                            item.ExpenseItemId.ToString(CultureInfo.InvariantCulture))).ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadDepartmentService(string knownCategoryValues, string category)
        {
            var departmentList =
                ServiceProvider.Instance().GetDepartmentServices().GetActiveFilteredOrderedDepartments();
            if (!departmentList.Any())
            {
                return new List<CascadingDropDownNameValue>().ToArray();
            }
            return
                departmentList.Select(
                    item =>
                        new CascadingDropDownNameValue(item.Name,
                            item.DepartmentId.ToString(CultureInfo.InvariantCulture))).ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadUnitList(string knownCategoryValues, string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            int departmentId;

            var unitList = new List<Unit>();

            if (!kv.ContainsKey("DepartmentId") || !Int32.TryParse(kv["DepartmentId"], out departmentId))
            {
                return
                    unitList.Select(
                        item =>
                            new CascadingDropDownNameValue(item.Name, item.UnitId.ToString(CultureInfo.InvariantCulture)))
                        .ToArray();

            }

            unitList = ServiceProvider.Instance().GetUnitServices().GetActiveOrderedUnitsByDepartment(departmentId);

            if (unitList.Count < 1)
            {
                return new List<CascadingDropDownNameValue>().ToArray();
            }

            return
                unitList.Select(
                    item =>
                        new CascadingDropDownNameValue(item.Name, item.UnitId.ToString(CultureInfo.InvariantCulture)))
                    .ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadBeneficiaryTypeService(string knownCategoryValues, string category)
        {
            var beneficiaryTypes =
                ServiceProvider.Instance().GetBeneficiaryTypeServices().GetActiveBeneficiaryTypes().ToList();
            return !beneficiaryTypes.Any()
                ? new List<CascadingDropDownNameValue>().ToArray()
                : beneficiaryTypes.Select(
                    item =>
                        new CascadingDropDownNameValue(item.Name,
                            item.BeneficiaryTypeId.ToString(CultureInfo.InvariantCulture))).ToArray();

        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadBeneficiariesByBeneficiaryType(string knownCategoryValues,
            string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            int beneficiaryTypeId;

            var beneficiaries = new List<Beneficiary>();

            if (!kv.ContainsKey("BeneficiaryTypeId") || !Int32.TryParse(kv["BeneficiaryTypeId"], out beneficiaryTypeId))
            {
                return
                    beneficiaries.Select(
                        item =>
                            new CascadingDropDownNameValue(item.FullName,
                                item.BeneficiaryId.ToString(CultureInfo.InvariantCulture))).ToArray();
            }

            beneficiaries =
                ServiceProvider.Instance()
                    .GetBeneficiaryServices()
                    .GetActiveBeneficiariesByBeneficiaryType(beneficiaryTypeId);

            if (beneficiaries.Count < 1)
            {
                return new List<CascadingDropDownNameValue>().ToArray();
            }
            return
                beneficiaries.Select(
                    item =>
                        new CascadingDropDownNameValue(item.FullName,
                            item.BeneficiaryId.ToString(CultureInfo.InvariantCulture))).ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadBeneficiariesWithApprovedUnpaidTransaction(string knownCategoryValues,
            string category)
        {
            var beneficiaries =
                ServiceProvider.Instance().GetBeneficiaryServices().GetBeneficiariesWithApprovedUnpaidTransactions();
            return !beneficiaries.Any()
                ? new List<CascadingDropDownNameValue>().ToArray()
                : beneficiaries.Select(
                    item =>
                        new CascadingDropDownNameValue(item.FullName,
                            item.BeneficiaryId.ToString(CultureInfo.InvariantCulture))).ToArray();

        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] LoadApprovedTransactionsByBeneficiary(string knownCategoryValues,
            string category)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            int beneficiaryId;

            var beneficiaryApprovedUnpaidTransactionList = new List<ExpenseTransaction>();

            if (!kv.ContainsKey("BeneficiaryId") || !Int32.TryParse(kv["BeneficiaryId"], out beneficiaryId))
            {
                return
                    beneficiaryApprovedUnpaidTransactionList.Select(
                        item =>
                            new CascadingDropDownNameValue(item.ExpenseTitle,
                                item.ExpenseTransactionId.ToString(CultureInfo.InvariantCulture))).ToArray();
            }

            beneficiaryApprovedUnpaidTransactionList =
                ServiceProvider.Instance()
                    .GetExpenseTransactionServices()
                    .GetBeneficiaryOnlyApprovedExpenseTransactions(beneficiaryId);

            if (beneficiaryApprovedUnpaidTransactionList.Count < 1)
            {
                return new List<CascadingDropDownNameValue>().ToArray();
            }
            return
                beneficiaryApprovedUnpaidTransactionList.Select(
                    item =>
                        new CascadingDropDownNameValue(item.ExpenseTitle,
                            item.ExpenseTransactionId.ToString(CultureInfo.InvariantCulture))).ToArray();
        }

        [WebMethod(EnableSession = true)]
        public string GetTransactionPamentsByDateRange(string startDate, string endDate)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return null;
            }

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return null;
            }

            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
                
                if (end < start || start > end)
                {
                    return null;
                }

                var dictObjeList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetMyGenericVoucherObjectsByDateRange(start, end);
                var allpaymentsByDateCollections = new List<List<DictObject>>();
                if (dictObjeList == null || !dictObjeList.Any())
                {
                    return null;
                }

                try
                {
                    var zerosLimit = ConfigurationManager.AppSettings["ZerosLimit"];

                    foreach (var dictObject in dictObjeList)
                    {
                        var user = new PortalServiceManager().GetPortalUserById(dictObject.RequestedById);
                        dictObject.ReceivedBy = dictObject.ReceivedBy;
                        dictObject.RequestedBy = user.FirstName + " " + user.LastName;
                        var approver = new PortalServiceManager().GetPortalUserById(dictObject.ApproverId);
                        dictObject.Approver = approver.FirstName + " " + approver.LastName;

                        if (string.IsNullOrWhiteSpace(zerosLimit))
                        {
                            dictObject.PcvNo = dictObject.PcvId.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            var zerosPrefix = int.Parse(zerosLimit);

                            dictObject.PcvNo =
                                new xPlug.BusinessObject.CustomizedASPBusinessObject.PCVHelper.PcvHelper().PcvGenerator(
                                    dictObject.PcvId, zerosPrefix);
                        }
                    }

                }
                catch (Exception)
                {
                    return null;
                }

                foreach (var payment in dictObjeList)
                {
                    if (
                        !allpaymentsByDateCollections.Exists(
                            m1 =>
                                m1.All(m2 => m2.DepartmentId == payment.DepartmentId && m2.DatePaid == payment.DatePaid)))
                    {
                        var newVoucherList =
                            dictObjeList.FindAll(
                                m =>
                                    m.DepartmentId == payment.DepartmentId && m.DatePaid == payment.DatePaid);

                        if (newVoucherList.Any())
                        {
                            allpaymentsByDateCollections.Add(newVoucherList);
                        }

                    }
                }

                var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
                return jsSerializer.Serialize(allpaymentsByDateCollections);
            }
            catch (Exception)
            {
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public string GetApprovedTransactionPaymentVouchersByDateRange(string startDate, string endDate)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return null;
            }

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return null;
            }

            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
               
                if (end < start || start > end)
                {
                    return null;
                }
                if (end < start || start > end)
                {
                    return null;
                }

                var dictObjeList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetApprovedTransactionPaymentVoucherObjectsByDateRange(start, end);
                var allpaymentsByDateCollections = new List<List<DictObject>>();
                if (dictObjeList == null || !dictObjeList.Any())
                {
                    return null;
                }

                try
                {
                    var zerosLimit = ConfigurationManager.AppSettings["ZerosLimit"];

                    foreach (var dictObject in dictObjeList)
                    {
                        var user = new PortalServiceManager().GetPortalUserById(dictObject.RequestedById);
                        dictObject.ReceivedBy = dictObject.ReceivedBy;
                        dictObject.RequestedBy = user.FirstName + " " + user.LastName;
                        var approver = new PortalServiceManager().GetPortalUserById(dictObject.ApproverId);
                        dictObject.Approver = approver.FirstName + " " + approver.LastName;

                        if (string.IsNullOrWhiteSpace(zerosLimit))
                        {
                            dictObject.PcvNo = dictObject.PcvId.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            var zerosPrefix = int.Parse(zerosLimit);

                            dictObject.PcvNo =
                                new xPlug.BusinessObject.CustomizedASPBusinessObject.PCVHelper.PcvHelper().PcvGenerator(
                                    dictObject.PcvId, zerosPrefix);
                        }
                    }

                }
                catch (Exception)
                {
                    return null;
                }

                foreach (var payment in dictObjeList)
                {
                    if (
                        !allpaymentsByDateCollections.Exists(
                            m1 =>
                                m1.All(m2 => m2.DepartmentId == payment.DepartmentId && m2.DatePaid == payment.DatePaid)))
                    {
                        var newVoucherList =
                            dictObjeList.FindAll(
                                m =>
                                    m.DepartmentId == payment.DepartmentId && m.DatePaid == payment.DatePaid);

                        if (newVoucherList.Any())
                        {
                            allpaymentsByDateCollections.Add(newVoucherList);
                        }

                    }
                }

                var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
                return jsSerializer.Serialize(allpaymentsByDateCollections);
            }
            catch (Exception)
            {
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public string GetVoidedTransactionPaymentVouchersByDateRange(string startDate, string endDate)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return null;
            }

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return null;
            }

            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
               
                if (end < start || start > end)
                {
                    return null;
                }

                var dictObjeList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetVoidedTransactionPaymentVoucherObjectsByDateRange(start, end);
                var allpaymentsByDateCollections = new List<List<DictObject>>();
                if (dictObjeList == null || !dictObjeList.Any())
                {
                    return null;
                }

                try
                {
                    var zerosLimit = ConfigurationManager.AppSettings["ZerosLimit"];

                    foreach (var dictObject in dictObjeList)
                    {
                        var user = new PortalServiceManager().GetPortalUserById(dictObject.RequestedById);
                        dictObject.ReceivedBy = dictObject.ReceivedBy;
                        dictObject.RequestedBy = user.FirstName + " " + user.LastName;
                        var approver = new PortalServiceManager().GetPortalUserById(dictObject.ApproverId);
                        dictObject.Approver = approver.FirstName + " " + approver.LastName;

                        if (string.IsNullOrWhiteSpace(zerosLimit))
                        {
                            dictObject.PcvNo = dictObject.PcvId.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            var zerosPrefix = int.Parse(zerosLimit);

                            dictObject.PcvNo =
                                new xPlug.BusinessObject.CustomizedASPBusinessObject.PCVHelper.PcvHelper().PcvGenerator(
                                    dictObject.PcvId, zerosPrefix);
                        }
                    }

                }
                catch (Exception)
                {
                    return null;
                }

                foreach (var payment in dictObjeList)
                {
                    if (
                        !allpaymentsByDateCollections.Exists(
                            m1 =>
                                m1.All(m2 => m2.DepartmentId == payment.DepartmentId && m2.DatePaid == payment.DatePaid)))
                    {
                        var newVoucherList =
                            dictObjeList.FindAll(
                                m =>
                                    m.DepartmentId == payment.DepartmentId && m.DatePaid == payment.DatePaid);

                        if (newVoucherList.Any())
                        {
                            allpaymentsByDateCollections.Add(newVoucherList);
                        }

                    }
                }

                var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
                return jsSerializer.Serialize(allpaymentsByDateCollections);
            }
            catch (Exception)
            {
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public void GetAssetReceipt(string receiptString)
        {
            if (receiptString.Contains("data:image/png;base64"))
            {
                return;
            }
            if (receiptString.Contains("data:image/gif;base64"))
            {
                return;
            }

            if (receiptString.Contains("data:image/jpeg;base64"))
            {
                receiptString = receiptString.Replace("data:image/jpeg;base64,", string.Empty);
            }

            Session["_receivedDataArray"] = Convert.FromBase64String(receiptString);

        }

        [WebMethod(EnableSession = true)]
        public void GetTransCollection(string transCollection)
        {
            Session["_idArrays"] = null;
            if (transCollection.Length < 1)
            {
                return;
            }

            var idArrays = transCollection.Split(',');

            var newIdArray = new List<int>();

            foreach (var id in idArrays)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (!newIdArray.Exists(m => m == int.Parse(id)))
                    {
                        newIdArray.Add(int.Parse(id));
                    }
                }
            }

            if (!newIdArray.Any())
            {
                return;
            }
            Session["_idArrays"] = newIdArray;

        }

        private static bool ValidateVouchersByDateArguments(string startDate, string endDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(startDate.Trim()))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(startDate.Trim())))
                    {
                        return false;
                    }
                }


                if (!string.IsNullOrEmpty(endDate.Trim()))
                {
                    if (!DateMap.IsDate(DateMap.ReverseToServerDate(endDate.Trim())))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        [WebMethod]
        public string GetLrDepartment(string refId)
        {
            var r = int.Parse(refId);
            const XplugDepartments lrGlobal = XplugDepartments.LrGlobal;
            var y = (int) Enum.Parse(typeof (XplugDepartments), Enum.GetName(typeof (XplugDepartments), lrGlobal));

            var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
            return y == r ? jsSerializer.Serialize(y) : jsSerializer.Serialize(-3);

        }

        [WebMethod]
        public string GetXplugDepartment(string refId)
        {
            var r = int.Parse(refId);
            const XplugDepartments xPlug = XplugDepartments.XPlug;
            var x = (int) Enum.Parse(typeof (XplugDepartments), Enum.GetName(typeof (XplugDepartments), xPlug));
            var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
            return x == r ? jsSerializer.Serialize(x) : jsSerializer.Serialize(-3);
        }

        [WebMethod]
        public List<TransactionObject> GetTransObject(string expenseItemId)
        {
            try
            {
                if (string.IsNullOrEmpty(expenseItemId))
                {
                    return null;
                }

                var itemId = int.Parse(expenseItemId);

                if (itemId < 1)
                {
                    return null;
                }
                var transObjList = ServiceProvider.Instance().GetExpenseTransactionServices().GetTransObject(itemId);
                return !transObjList.Any() ? null : transObjList;
            }
            catch (Exception)
            {
                return null;
            }

        }

        [WebMethod(EnableSession = true)]
        public string SetPaymentHistoryId(string paymentHistoryId)
        {
            if (string.IsNullOrEmpty(paymentHistoryId))
            {
                return "0";
            }
            Session["_paymentHistoryId"] = null;
            Int64 outLong = 0;
            var result = Int64.TryParse(paymentHistoryId, out outLong);
            if (result)
            {
                Session["_paymentHistoryId"] = outLong;
                return outLong.ToString(CultureInfo.InvariantCulture);
            }
            return "0";
        }

        [WebMethod(EnableSession = true)]
        public void SetWordEquiv(string wordString)
        {
            if (string.IsNullOrEmpty(wordString))
            {
                return;
            }
            Session["_wordString"] = null;
            Session["_wordString"] = wordString;
        }

        [WebMethod]
        public Guid GenerateGuid()
        {
            var xGuid = Guid.NewGuid();
            return xGuid;

        }

        [WebMethod(EnableSession = true)]
        public string GetTransactionPaymentsByDateRange(string startDate, string endDate, string dept)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return null;
            }

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(dept))
            {
                return null;
            }

            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));

                if (end < start || start > end)
                {
                    return null;
                }

                var id = 0;
                var deptId = 0;
                var result = int.TryParse(dept, out deptId);
                if (result)
                {
                    id = deptId;
                }
                var historyObjeList = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().GetTransactionPaymentsByDateRange(start, end, id);

                if (historyObjeList == null || !historyObjeList.Any())
                {
                    return null;
                }

                var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
                return jsSerializer.Serialize(historyObjeList);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetApprovedTransactionPaymentsByDateRange(string startDate, string endDate, string dept)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return null;
            }

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return null;
            }

            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
                var id = 0;
                var deptId = 0;
                var result = int.TryParse(dept, out deptId);
                if (result)
                {
                    id = deptId;
                }
                if (end < start || start > end)
                {
                    return null;
                }

                var historyObjeList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetApprovedTransactionPaymentsByDateRange(start, end, id);

                if (historyObjeList == null || !historyObjeList.Any())
                {
                    return null;
                }

                var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
                return jsSerializer.Serialize(historyObjeList);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetVoidedTransactionPaymentsByDateRange(string startDate, string endDate, string dept)
        {
            if (!ValidateVouchersByDateArguments(startDate, endDate))
            {
                return null;
            }

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return null;
            }

            try
            {
                var start = DateTime.Parse(DateMap.ReverseToServerDate(startDate.Trim()));
                var end = DateTime.Parse(DateMap.ReverseToServerDate(endDate.Trim()));
                var id = 0;
                var deptId = 0;
                var result = int.TryParse(dept, out deptId);
                if (result)
                {
                    id = deptId;
                }
                if (end < start || start > end)
                {
                    return null;
                }

                var historyObjeList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetVoidedTransactionPaymentsByDateRange(start, end, id);

                if (historyObjeList == null || !historyObjeList.Any())
                {
                    return null;
                }

                var jsSerializer = new JavaScriptSerializer {MaxJsonLength = Int32.MaxValue};
                return jsSerializer.Serialize(historyObjeList);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetVoucher(string paymentHistoryId)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentHistoryId))
                {
                    return null;
                }

                Int64 outLong = 0;

                var result = Int64.TryParse(paymentHistoryId, out outLong);

                if (result)
                {
                    var dictObject = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().GetMyGenericVoucherObject(outLong);

                    if (dictObject == null || dictObject.TransactionpaymentHistoryId < 1)
                    {
                        return null;
                    }

                    var zerosLimit = ConfigurationManager.AppSettings["ZerosLimit"];

                    if (string.IsNullOrWhiteSpace(zerosLimit))
                    {
                        dictObject.PcvNo = dictObject.PcvId.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        var zerosPrefix = int.Parse(zerosLimit);

                        dictObject.PcvNo = new xPlug.BusinessObject.CustomizedASPBusinessObject.PCVHelper.PcvHelper().PcvGenerator(dictObject.PcvId, zerosPrefix);
                    }

                    try
                    {
                        var user = new PortalServiceManager().GetPortalUserById(dictObject.RequestedById);
                        dictObject.ReceivedBy = dictObject.ReceivedBy;
                        dictObject.RequestedBy = user.FirstName + " " + user.LastName;
                        var approver = new PortalServiceManager().GetPortalUserById(dictObject.ApproverId);
                        dictObject.Approver = approver.FirstName + " " + approver.LastName;
                    }
                    catch (Exception)
                    {
                        dictObject.ReceivedBy = "N/A";
                        dictObject.Approver = "N/A";
                    }
                    Session["_paymentHistory"] = null;
                    Session["_paymentHistory"] = dictObject;

                    var jsSerializer = new JavaScriptSerializer
                                       {
                                           MaxJsonLength = Int32.MaxValue
                                       };
                    return jsSerializer.Serialize(dictObject);
                }

                return null;

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetTransactionPaymentVouchers(string paymentHistoryIds)
        {

            try
            {
                if (string.IsNullOrEmpty(paymentHistoryIds))
                {
                    return null;
                }

                var idArrays = paymentHistoryIds.Split(',');

                var newIdArray = new List<long>();

                foreach (var id in idArrays)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!newIdArray.Exists(m => m == long.Parse(id)))
                        {
                            newIdArray.Add(long.Parse(id));
                        }
                    }
                }

                if (!newIdArray.Any())
                {
                    return null;
                }

                var dictObjeList =
                    ServiceProvider.Instance()
                        .GetExpenseTransactionPaymentHistoryServices()
                        .GetMyGenericVoucherObjectsByIds(newIdArray);
                if (dictObjeList == null || !dictObjeList.Any())
                {
                    return null;
                }

                try
                {
                    var zerosLimit = ConfigurationManager.AppSettings["ZerosLimit"];

                    foreach (var dictObject in dictObjeList)
                    {
                        var user = new PortalServiceManager().GetPortalUserById(dictObject.RequestedById);
                        dictObject.ReceivedBy = dictObject.ReceivedBy;
                        dictObject.RequestedBy = user.FirstName + " " + user.LastName;
                        var approver = new PortalServiceManager().GetPortalUserById(dictObject.ApproverId);
                        dictObject.Approver = approver.FirstName + " " + approver.LastName;

                        if (string.IsNullOrWhiteSpace(zerosLimit))
                        {
                            dictObject.PcvNo = dictObject.PcvId.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            var zerosPrefix = int.Parse(zerosLimit);

                            dictObject.PcvNo =
                                new xPlug.BusinessObject.CustomizedASPBusinessObject.PCVHelper.PcvHelper().PcvGenerator(
                                    dictObject.PcvId, zerosPrefix);
                        }
                    }

                    var jsSerializer = new JavaScriptSerializer
                                       {
                                           MaxJsonLength = Int32.MaxValue
                                       };
                    return jsSerializer.Serialize(dictObjeList);
                }

                catch (Exception)
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        public void SetWordEquivs(string voucherCollection)
        {
            if (string.IsNullOrEmpty(voucherCollection))
            {
                return;
            }
            Session["_voucherCollection"] = null;
            Session["_voucherCollection"] = voucherCollection;
        }

        [WebMethod(EnableSession = true)]
        public string GetReportData(string reportId)
        {
            if (string.IsNullOrEmpty(reportId))
            {
                return "-1";
            }

            var id = int.Parse(reportId);

            if (id < 1)
            {
                return "-1";
            }

            var nestedItem = ServiceProvider.Instance().GetTransactionItemServices().GetSingleTransactionItems(id);
            
            if (nestedItem == null || !nestedItem.Any())
            {
                return "-1";
            }

            var js = new JavaScriptSerializer
            {
                MaxJsonLength = Int32.MaxValue
            };
            return js.Serialize(nestedItem);
         
        }

        [WebMethod(EnableSession = true)]
        public string GetDetailedReportData(string jxl)
        {
            if (string.IsNullOrEmpty(jxl))
            {
                return "-1";
            }

            var id = int.Parse(jxl);

            if (id < 1)
            {
                return "-1";
            }

            var nestedItem = ServiceProvider.Instance().GetTransactionItemServices().GetDetailedTransactionItems(id);

            if (nestedItem == null || !nestedItem.Any())
            {
                return "-1";
            }

            var js = new JavaScriptSerializer
            {
                MaxJsonLength = Int32.MaxValue
            };
            return js.Serialize(nestedItem);
        }
    }
}
