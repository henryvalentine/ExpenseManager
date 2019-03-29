using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using xPlug.BusinessObject;
using xPlug.BusinessManager;
using kPortal.CoreUtilities;



namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public partial class BeneficiaryService : MarshalByRefObject
	{
		private readonly BeneficiaryManager  _beneficiaryManager;
		public BeneficiaryService()
		{
			_beneficiaryManager = new BeneficiaryManager();
		}

		public int AddBeneficiary(Beneficiary beneficiary)
		{
			try
			{
				return _beneficiaryManager.AddBeneficiary(beneficiary);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateBeneficiary(Beneficiary beneficiary)
		{
			try
			{
				return _beneficiaryManager.UpdateBeneficiary(beneficiary);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteBeneficiary(Int32 beneficiaryId)
		{
			try
			{
				return _beneficiaryManager.DeleteBeneficiary(beneficiaryId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public Beneficiary GetBeneficiary(int beneficiaryId)
		{
			try
			{
				return _beneficiaryManager.GetBeneficiary(beneficiaryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new Beneficiary();
			}
		}

		public List<Beneficiary> GetBeneficiaries()
		{
			try
			{
				var objList = new List<Beneficiary>();
				objList = _beneficiaryManager.GetBeneficiaries();
				if(objList == null) {return  new List<Beneficiary>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Beneficiary>();
			}
		}

		public List<Beneficiary>  GetBeneficiariesByDepartmentId(Int32 departmentId)
		{
			try
			{
				return _beneficiaryManager.GetBeneficiariesByDepartmentId(departmentId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Beneficiary>();
			}
		}

		public List<Beneficiary>  GetBeneficiariesByUnitId(Int32 unitId)
		{
			try
			{
				return _beneficiaryManager.GetBeneficiariesByUnitId(unitId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Beneficiary>();
			}
		}

		public List<Beneficiary>  GetBeneficiariesByBeneficiaryTypeId(Int32 beneficiaryTypeId)
		{
			try
			{
				return _beneficiaryManager.GetBeneficiariesByBeneficiaryTypeId(beneficiaryTypeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Beneficiary>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
