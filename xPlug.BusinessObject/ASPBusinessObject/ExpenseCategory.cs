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
	///* Date Generated:	25-11-2013 09:26:29
	///*******************************************************************************


	public partial class ExpenseCategory
	{
		#region Main Properties
		public int ExpenseCategoryId { get; set; }

		public string Title { get; set; }

		public string Code { get; set; }

		public int Status { get; set; }

		#endregion
		#region Navigation Properties
		#endregion
		#region Navigation Collections
		public ICollection<AccountsHead> AccountsHeads {get; set;}



		public ICollection<ExpenseItem> ExpenseItems {get; set;}



		public ICollection<TransactionItem> TransactionItems {get; set;}



		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
