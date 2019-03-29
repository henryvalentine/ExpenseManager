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
	///* Date Generated:	25-11-2013 09:26:21
	///*******************************************************************************


	public partial class AccountsHead
	{
		#region Main Properties
		public int AccountsHeadId { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Code { get; set; }

		public int Status { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 ExpenseCategoryId {get; set;}
		#endregion
		#region Navigation Properties
		public ExpenseCategory ExpenseCategory {get; set;}
		#endregion
		#region Navigation Collections
		public ICollection<ExpenseItem> ExpenseItems {get; set;}



		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
