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


	public class BeneficiaryMapper
	{

		public BeneficiaryMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(Beneficiary))
			{
				var objItem = new ExpenseManager.EF.Beneficiary();
				var myItem = sourceObject as Beneficiary;
				if(myItem == null){return null;};
				try
				{
					objItem.BeneficiaryId = myItem.BeneficiaryId;

					objItem.FullName = myItem.FullName;

					objItem.GSMNO2 = myItem.GSMNO2;

					objItem.GSMNO1 = myItem.GSMNO1;

					objItem.DateRegistered = myItem.DateRegistered;

					objItem.TimeRegistered = myItem.TimeRegistered;

					objItem.Sex = myItem.Sex;

					objItem.Email = myItem.Email;

					objItem.Status = myItem.Status;

					objItem.CompanyName = myItem.CompanyName;

					objItem.DepartmentId = myItem.DepartmentId;

					objItem.UnitId = myItem.UnitId;

					objItem.BeneficiaryTypeId = myItem.BeneficiaryTypeId;

				}
				catch(Exception ex)
				{
					return new Beneficiary() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.Beneficiary))
			{
				var objItem = new Beneficiary();
				var myItem = sourceObject as ExpenseManager.EF.Beneficiary;
				if(myItem == null){return null;};
				try
				{
					objItem.BeneficiaryId = myItem.BeneficiaryId;

					objItem.FullName = myItem.FullName;

					objItem.GSMNO2 = myItem.GSMNO2;

					objItem.GSMNO1 = myItem.GSMNO1;

					objItem.DateRegistered = myItem.DateRegistered;

					objItem.TimeRegistered = myItem.TimeRegistered;

					objItem.Sex = myItem.Sex;

					objItem.Email = myItem.Email;

					objItem.Status = myItem.Status;

					objItem.CompanyName = myItem.CompanyName;

					objItem.DepartmentId = myItem.DepartmentId;

					objItem.UnitId = myItem.UnitId;

					objItem.BeneficiaryTypeId = myItem.BeneficiaryTypeId;

					#region Included Tables
						try
						{
							objItem.Department = new Department();
							objItem.Department.DepartmentId = myItem.Department.DepartmentId;

							objItem.Department.Name = myItem.Department.Name;

							objItem.Department.Status = myItem.Department.Status;

						}
						catch{}
						try
						{
							objItem.Unit = new Unit();
							objItem.Unit.UnitId = myItem.Unit.UnitId;

							objItem.Unit.Name = myItem.Unit.Name;

							objItem.Unit.DepartmentId = myItem.Unit.DepartmentId;

							objItem.Unit.Status = myItem.Unit.Status;

						}
						catch{}
						try
						{
							objItem.BeneficiaryType = new BeneficiaryType();
							objItem.BeneficiaryType.BeneficiaryTypeId = myItem.BeneficiaryType.BeneficiaryTypeId;

							objItem.BeneficiaryType.Name = myItem.BeneficiaryType.Name;

							objItem.BeneficiaryType.Status = myItem.BeneficiaryType.Status;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new Beneficiary() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
