using System.Collections.Generic;

namespace xPlug.BusinessObject.CustomizedASPBusinessObject
{
    public class DictObject
    {
        public string TransactionTitle { get; set; }
        public string PaidBy { get; set; }
        public int RequestedById { get; set; }
        public int DepartmentId { get; set; }
        public string RequestedBy { get; set; }
        public string ChequeNo { get; set; }
        public string ReceivedBy { get; set; }
        public long TransactionId { get; set; }
        public double AmmountPaid { get; set; }
        public double TotalApprovedAmmount { get; set; }
        public long TransactionpaymentHistoryId { get; set; }
        public string Approver { get; set; }
        public int ApproverId { get; set; }
        public string DatePaid { get; set; }
        public object PaymentMode { get; set; }
        public string PcvNo { get; set; }
        public int PcvId { get; set; }
        public string TimePaid { get; set; }
        public string WordValue { get; set; }
        public List<TransactionItem> TransactionItems { get; set; }
    }
}
