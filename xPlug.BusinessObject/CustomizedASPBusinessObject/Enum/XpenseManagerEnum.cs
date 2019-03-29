using System.ComponentModel;

namespace xPlug.BusinessObject.CustomizedASPBusinessObject.Enum
{
    public enum AssetCategories
    {
        [Description("Fixed Asset")]
        Fixed_Asset = 1,
        [Description("Liquid Asset")]
        Liquid_Asset = 2,
    }
    
    public enum XplugDepartments
    {
        [Description("LrGlobal")]
        LrGlobal = 5,
        [Description("xPlug")]
        XPlug = 6
    }

    public enum ExpenseApprovalStatus
    {
        [Description("Pending")]
        Pending = 0,
        [Description("Approved")]
        Approved = 1,
        [Description("Rejected")]
        Rejected = 2,
        [Description("Voided")]
        Voided = 3,
    }



    public enum TimeFrame
    {
        //[Description("Yearly")]
        //Yearly = 1,
        [Description("Monthly")]
        Monthly = 1,
        [Description("Weekly")]
        Weekly = 2,
        [Description("Date Range")]
        Date_Range = 3,
    }

    public enum MonthList
    {
        Janurary = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum GrdColumns
    {
        Approval_Status = 1,
        Approver = 2,
        Approval_Comment = 3,
        Beneficiary = 4,
        Date_Requested = 5,
        Date_Approved = 6,
        Description = 7,
        Payment_Status = 8,
        Requested_By = 9,
        Time_Approved = 10,
        Time_Requested = 11,
        Transaction = 12,
        Total_Approved_Amount = 13,
        Total_Amount_Requested = 14,

    }

    public enum TransactionPaymentMode
    {
        [Description("Cash")]
        Pending = 1,
        [Description("Cheque")]
        Approved = 2,
        [Description("Both")]
        Rejected = 3
    }

    public enum FilterOptions
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Rejected")]
        Rejected = 3,
        [Description("Voided")]
        Voided = 4,
    }

    public enum WeekOptions
    {
        [Description("Week 1")]
        Week_1 = 1,
        [Description("Week 2")]
        Week_2 = 2,
        [Description("Week 3")]
        Week_3 = 3,
        [Description("Week 4")]
        Week_4 = 4,
    }

    public enum ApprovedVoidedFilterOptions
    {
        [Description("Approved")]
        Approved = 1,
        [Description("Voided")]
        Voided = 2,
    }

    public enum PendingRejectedFilterOptions
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Rejected")]
        Rejected = 2,
    }

    public enum ExpenseApprovalDateTime
    {
        [Description("Pending")]
        N_A = 0
    }

    public enum ExpensePaymentStatus
    {
        [Description("Not Paid")]
        Not_Paid = 0,
        [Description("Partly Paid")]
        Partly_Paid = 1,
        [Description("Fully Paid")]
        Fully_Paid = 2,
    }

    public enum PaymentStatusReport
    {
        [Description("Partly Paid")]
        Partly_Paid = 1,
        [Description("Fully Paid")]
        Fully_Paid = 2,
    }

    public enum DeptAndUnitStatus
    {
        [Description("Not Applicable")]
        Not_Applicable = 1
    }

    public enum PendingStatus
    {
        [Description("N/A")]
        N_A = 1
    }

    public enum Navigation
    {
        None,
        First,
        Next,
        Previous,
        Last,
        Pager,
        Sorting
    }

    public enum TransactionMessage
    {
        [Description("N/A")]
        N_A = 1
    }

}