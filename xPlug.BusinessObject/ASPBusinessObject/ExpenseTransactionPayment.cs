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
	///* Date Generated:	28-11-2013 04:15:13
	///*******************************************************************************


	public partial class ExpenseTransactionPayment
	{
		#region Main Properties
		public long ExpenseTransactionPaymentId { get; set; }

		public double TotalAmountPayable { get; set; }

		public double Balance { get; set; }

		public string LastPaymentDate { get; set; }

		public string LastPaymentTime { get; set; }

		public int Status { get; set; }

		public double AmountPaid { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int64 ExpenseTransactionId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 BeneficiaryId {get; set;}
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Required")]
        public Int32 DepartmentId { get; set; }
		#endregion
		#region Navigation Properties
		public ExpenseTransaction ExpenseTransaction {get; set;}
		public Beneficiary Beneficiary {get; set;}
        public Department Department { get; set; }
		#endregion
		#region Navigation Collections
		public ICollection<ExpenseTransactionPaymentHistory> ExpenseTransactionPaymentHistories {get; set;}

		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
