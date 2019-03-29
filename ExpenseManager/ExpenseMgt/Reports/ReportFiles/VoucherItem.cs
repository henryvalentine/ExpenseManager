using System.Collections.Generic;
using xPlug.BusinessObject;

namespace ExpenseManager.ExpenseMgt.Reports.ReportFiles
{
    public class VoucherItem
    {
        public long TransactionId { get; set; }
        public string PaidBy { get; set; }
        public string RequestedBy { get; set; }
        public string ChequeNo { get; set; }
        public string ReceivedBy { get; set; }
        public string AmountPaid { get; set; }
        public string TotalApprovedAmount { get; set; }
        public string Approver { get; set; }
        public string DatePaid { get; set; }
        public string PcvNo { get; set; }
        public string PaymentMode { get; set; }
        public string AmountInWords { get; set; }
        public List<TransactionItem> Items { get; set; } 
        //Trans Items
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string DetailDesription { get; set; }
        public string ApprovedQuantity { get; set; }
        public string ApprovedUnitPrice { get; set; }
        public string ApprovedTotalPrice { get; set; }
    }
}