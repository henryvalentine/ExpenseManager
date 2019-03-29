using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;



namespace xPlug.BusinessObject
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2014. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	17-01-2014 02:29:15
	///*******************************************************************************


	public partial class ExpenseTransaction
	{
		#region Main Properties
		public long ExpenseTransactionId { get; set; }

		public string ExpenseTitle { get; set; }

		public string Description { get; set; }

		public int RegisteredById { get; set; }

		public string TransactionDate { get; set; }

		public string TransactionTime { get; set; }

		public int Status { get; set; }

		public double TotalTransactionAmount { get; set; }

		public int ApproverId { get; set; }

		public string DateApproved { get; set; }

		public string TimeApproved { get; set; }

		public double TotalApprovedAmount { get; set; }

		public string ApproverComment { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 BeneficiaryId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 BeneficiaryTypeId {get; set;}
		#endregion
		#region Navigation Properties
		public Beneficiary Beneficiary {get; set;}
		public BeneficiaryType BeneficiaryType {get; set;}
		#endregion
		#region Navigation Collections
		public ICollection<ExpenseTransactionPaymentHistory> ExpenseTransactionPaymentHistories {get; set;}



		public ICollection<ExpenseTransactionPayment> ExpenseTransactionPayments {get; set;}



		public ICollection<TransactionItem> TransactionItems {get; set;}



		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
