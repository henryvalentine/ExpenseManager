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
	///* Date Generated:	25-11-2013 09:26:18
	///*******************************************************************************


	public partial class Department
	{
		#region Main Properties
		public int DepartmentId { get; set; }

		public string Name { get; set; }

		public int Status { get; set; }

		#endregion
		#region Navigation Properties
		#endregion
		#region Navigation Collections
		public ICollection<Beneficiary> Beneficiaries {get; set;}



		public ICollection<Unit> Units {get; set;}



		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
