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


	public partial class Bank
	{
		#region Main Properties
		public int BankId { get; set; }

		public string BankName { get; set; }

		#endregion
		#region Navigation Properties
		#endregion
		#region Navigation Collections
		public ICollection<Cheque> Cheques {get; set;}



		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
