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
	///* Date Generated:	05-12-2013 11:55:57
	///*******************************************************************************


	public partial class PaymentVoucherNumber
	{
		#region Main Properties
		public int PaymentVoucherNumberId { get; set; }

		public long TransactionId { get; set; }

		public double TransactionTotalAmount { get; set; }

		public string PaymentDate { get; set; }

		public string DateSubmitted { get; set; }

		#endregion
		#region Navigation Properties
		#endregion
		#region Navigation Collections
		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
