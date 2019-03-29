namespace xPlug.BusinessObject.CustomizedASPBusinessObject
{
    public class VoucherObject
    {
            public string TransactionTitle { get; set; }
            public int PaidBy { get; set; }
            public int RequestedById { get; set; }
            public string ChequeNo { get; set; }
            public int BeneficiaryId { get; set; }
            public long ExpenseTransactionId { get; set; }
            public double AmountPaid { get; set; }
            public double TotalApprovedAmount { get; set; }
            public long TransactionPaymentHistoryId { get; set; }
            public int ApproverId { get; set; }
            public string PaymentDate { get; set; }
            public string PaymentTime { get; set; }
            public int PaymentModeId { get; set; }
    }
}
