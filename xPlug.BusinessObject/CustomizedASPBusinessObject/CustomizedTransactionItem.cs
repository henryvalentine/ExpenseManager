namespace xPlug.BusinessObject
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class TransactionItem
	{
        public double TotalPrice { get; set; }
        public int TempId { get; set; }
        public int AccountsHeadId { get; set; }
        public string ExpenseItemName { get; set; }
        public string AccountsHeadTitle { get; set; }
        public double TotalRequestedUnitPrice { get; set; }
        public double TotalApprovedUnitPrice { get; set; }
        public int TotalRequestedQuantity { get; set; }
        public int TotalApprovedQuantity { get; set; }
        public double TotalApprovedPrice { get; set; }
        public double GrandTotalApprovedPrice { get; set; }
        public bool Ismultiple { get; set; }
        
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
