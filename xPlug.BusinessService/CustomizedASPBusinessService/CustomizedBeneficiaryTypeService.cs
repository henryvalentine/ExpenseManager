using System;
using System.Collections.Generic;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class BeneficiaryTypeService
	{
        public List<BeneficiaryType> GetAllBeneficiaryTypes()
        {
            try
            {
                return _beneficiaryTypeManager.GetAllBeneficiaryTypes();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BeneficiaryType>();
            }
        }
        public List<BeneficiaryType> GetActiveBeneficiaryTypes()
        {
            try
            {
                return _beneficiaryTypeManager.GetActiveBeneficiaryTypes();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BeneficiaryType>();
            }
        }
        public int UpdateBeneficiaryTypeCheckDuplicate(BeneficiaryType beneficiaryType)
        {
            try
            {
                return _beneficiaryTypeManager.UpdateBeneficiaryTypeCheckDuplicate(beneficiaryType);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public int AddBeneficiaryTypeCheckDuplicate(BeneficiaryType beneficiaryType)
        {
            try
            {
                return _beneficiaryTypeManager.AddBeneficiaryTypeCheckDuplicate(beneficiaryType);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
	}

	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
