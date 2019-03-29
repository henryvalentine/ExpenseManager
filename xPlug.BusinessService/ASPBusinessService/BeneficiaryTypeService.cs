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
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class BeneficiaryTypeService : MarshalByRefObject
	{
		private readonly BeneficiaryTypeManager  _beneficiaryTypeManager;
		public BeneficiaryTypeService()
		{
			_beneficiaryTypeManager = new BeneficiaryTypeManager();
		}

		public int AddBeneficiaryType(BeneficiaryType beneficiaryType)
		{
			try
			{
				return _beneficiaryTypeManager.AddBeneficiaryType(beneficiaryType);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateBeneficiaryType(BeneficiaryType beneficiaryType)
		{
			try
			{
				return _beneficiaryTypeManager.UpdateBeneficiaryType(beneficiaryType);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteBeneficiaryType(Int32 beneficiaryTypeId)
		{
			try
			{
				return _beneficiaryTypeManager.DeleteBeneficiaryType(beneficiaryTypeId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public BeneficiaryType GetBeneficiaryType(int beneficiaryTypeId)
		{
			try
			{
				return _beneficiaryTypeManager.GetBeneficiaryType(beneficiaryTypeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new BeneficiaryType();
			}
		}

		public List<BeneficiaryType> GetBeneficiaryTypes()
		{
			try
			{
				var objList = new List<BeneficiaryType>();
				objList = _beneficiaryTypeManager.GetBeneficiaryTypes();
				if(objList == null) {return  new List<BeneficiaryType>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<BeneficiaryType>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
