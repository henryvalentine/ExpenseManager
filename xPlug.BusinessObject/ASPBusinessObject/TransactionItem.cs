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


	public partial class TransactionItem
	{
		#region Main Properties
		public int TransactionItemId { get; set; }

		public int RequestedQuantity { get; set; }

		public double RequestedUnitPrice { get; set; }

		public int ApprovedQuantity { get; set; }

		public double ApprovedUnitPrice { get; set; }

		public double ApprovedTotalPrice { get; set; }

		public string Description { get; set; }

		public int Status { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 ExpensenseItemId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int64 ExpenseTransactionId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 ExpenseCategoryId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 ExpenseTypeId {get; set;}
		#endregion
		#region Navigation Properties
		public ExpenseItem ExpenseItem {get; set;}
		public ExpenseTransaction ExpenseTransaction {get; set;}
		public ExpenseCategory ExpenseCategory {get; set;}
		public ExpenseType ExpenseType {get; set;}
		#endregion
		#region Navigation Collections
		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
