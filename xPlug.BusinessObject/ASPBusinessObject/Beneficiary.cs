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
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public partial class Beneficiary
	{
		#region Main Properties
		public int BeneficiaryId { get; set; }

		public string FullName { get; set; }

		public string GSMNO2 { get; set; }

		public string GSMNO1 { get; set; }

		public string DateRegistered { get; set; }

		public string TimeRegistered { get; set; }

		public int Sex { get; set; }

		public string Email { get; set; }

		public int Status { get; set; }

		public string CompanyName { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 DepartmentId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 UnitId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 BeneficiaryTypeId {get; set;}
		#endregion
		#region Navigation Properties
		public Department Department {get; set;}
		public Unit Unit {get; set;}
		public BeneficiaryType BeneficiaryType {get; set;}
		#endregion
		#region Navigation Collections
		public ICollection<ExpenseTransaction> ExpenseTransactions {get; set;}



		public ICollection<StaffUser> StaffUsers {get; set;}



		public ICollection<ExpenseTransactionPaymentHistory> ExpenseTransactionPaymentHistories {get; set;}



		public ICollection<ExpenseTransactionPayment> ExpenseTransactionPayments {get; set;}



		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
