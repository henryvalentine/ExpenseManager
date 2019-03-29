namespace xPlug.BusinessObject
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	06-09-2013 09:16:47
	///*******************************************************************************


	public partial class ExpenseTransaction
	{
        public string AccountsHead { get; set; }
        public int AccountHeadId { get; set; }
        public int TotalApprovedQuantity { get; set; }
        public string ApprovalStatus { get; set; }
        public double TotalPrice { get; set; }
        public string ApprovedBy { get; set; }
        public string RegisteredBy { get; set; }
        public string PaymentStatus { get; set; }
        public double AmountPaid { get; set; }
        public long TransactionId { get; set; }
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
