using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;



namespace xPlug.BusinessObject
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	01-12-2013 03:58:06
	///*******************************************************************************


	public partial class ExpenseTransactionPaymentHistory
	{
		#region Main Properties
		public long ExpenseTransactionPaymentHistoryId { get; set; }

		public double AmountPaid { get; set; }

		public string PaymentDate { get; set; }

		public string PaymentTime { get; set; }

		public int PaidById { get; set; }

		public string Comment { get; set; }

		public int Status { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int64 ExpenseTransactionId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int64 ExpenseTransactionPaymentId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 PaymentModeId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 BeneficiaryId {get; set;}
		#endregion
		#region Navigation Properties
		public ExpenseTransaction ExpenseTransaction {get; set;}
		public ExpenseTransactionPayment ExpenseTransactionPayment {get; set;}
		public PaymentMode PaymentMode {get; set;}
		public Beneficiary Beneficiary {get; set;}
		#endregion
		#region Navigation Collections
		public ICollection<Cheque> Cheques {get; set;}



		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
