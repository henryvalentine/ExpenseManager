using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using xPlug.BusinessObject;



namespace xPlug.BusinessObjectMapper
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public class StaffUserMapper
	{

		public StaffUserMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(StaffUser))
			{
				var objItem = new ExpenseManager.EF.StaffUser();
				var myItem = sourceObject as StaffUser;
				if(myItem == null){return null;};
				try
				{
					objItem.StaffUserId = myItem.StaffUserId;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

					objItem.PortalUserId = myItem.PortalUserId;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new StaffUser() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.StaffUser))
			{
				var objItem = new StaffUser();
				var myItem = sourceObject as ExpenseManager.EF.StaffUser;
				if(myItem == null){return null;};
				try
				{
					objItem.StaffUserId = myItem.StaffUserId;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

					objItem.PortalUserId = myItem.PortalUserId;

					objItem.Status = myItem.Status;

					#region Included Tables
						try
						{
							objItem.Beneficiary = new Beneficiary();
							objItem.Beneficiary.BeneficiaryId = myItem.Beneficiary.BeneficiaryId;

							objItem.Beneficiary.FullName = myItem.Beneficiary.FullName;

							objItem.Beneficiary.GSMNO2 = myItem.Beneficiary.GSMNO2;

							objItem.Beneficiary.GSMNO1 = myItem.Beneficiary.GSMNO1;

							objItem.Beneficiary.DateRegistered = myItem.Beneficiary.DateRegistered;

							objItem.Beneficiary.TimeRegistered = myItem.Beneficiary.TimeRegistered;

							objItem.Beneficiary.Sex = myItem.Beneficiary.Sex;

							objItem.Beneficiary.Email = myItem.Beneficiary.Email;

							objItem.Beneficiary.Status = myItem.Beneficiary.Status;

							objItem.Beneficiary.CompanyName = myItem.Beneficiary.CompanyName;

							objItem.Beneficiary.DepartmentId = myItem.Beneficiary.DepartmentId;

							objItem.Beneficiary.UnitId = myItem.Beneficiary.UnitId;

							objItem.Beneficiary.BeneficiaryTypeId = myItem.Beneficiary.BeneficiaryTypeId;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new StaffUser() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
