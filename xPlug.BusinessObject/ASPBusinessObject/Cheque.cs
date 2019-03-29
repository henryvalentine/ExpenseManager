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
	///* Date Generated:	01-12-2013 03:58:07
	///*******************************************************************************


	public partial class Cheque
	{
		#region Main Properties
		public int ChequePaymentId { get; set; }

		public double Amount { get; set; }

		public string ChequeNo { get; set; }

		public byte[] ScannedCopy { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int64 ExpenseTransactionPaymentHistoryId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 BankId {get; set;}
		#endregion
		#region Navigation Properties
		public ExpenseTransactionPaymentHistory ExpenseTransactionPaymentHistory {get; set;}
		public Bank Bank {get; set;}
		#endregion
		#region Navigation Collections
		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
