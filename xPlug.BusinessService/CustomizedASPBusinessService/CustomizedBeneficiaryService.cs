using System;
using System.Collections.Generic;
using System.Linq;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
    /// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
    /// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
    ///*******************************************************************************
    ///* Date Created: 	04-06-2013
    ///* Date Generated:	12-09-2013 10:06:29
    ///*******************************************************************************


    public partial class BeneficiaryService
    {
        public Dictionary<List<Beneficiary>, List<Beneficiary>> GetFilteredBeneficiaries()
        {
            try
            {
                return _beneficiaryManager.GetFilteredBeneficiaries();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new Dictionary<List<Beneficiary>, List<Beneficiary>>();
            }
        }
        public List<Beneficiary> GeteBeneficiaryBySearchText(string searchText)
        {
            try
            {
                return _beneficiaryManager.GeteBeneficiaryBySearchText(searchText);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
        }
        public List<Beneficiary> GetActiveBeneficiariesByBeneficiaryType(int beneficiaryTypeId)
        {
            try
            {
                return _beneficiaryManager.GetActiveBeneficiariesByBeneficiaryType(beneficiaryTypeId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
        }
        public List<Beneficiary> GetBeneficiariesWithApprovedUnpaidTransactions()
        {
            try
            {
                return _beneficiaryManager.GetBeneficiariesWithApprovedUnpaidTransactions();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
        }
        public List<Beneficiary> GetActiveBeneficiaries()
        {
            try
            {
                return _beneficiaryManager.GetActiveBeneficiaries();

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
        }
        public List<Beneficiary> GetAllBeneficiaries()
        {
            try
            {
                return _beneficiaryManager.GetAllBeneficiaries();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
        }
        public List<Beneficiary> GetBeneficiariesWithUnCompletedTransactionPayments()
        {
            try
            {
                return _beneficiaryManager.GetBeneficiariesWithUnCompletedTransactionPayments();

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
        }
        public int AddBeneficiaryCheckDuplicate(Beneficiary beneficiary)
        {
            try
            {
                return _beneficiaryManager.AddBeneficiaryCheckDuplicate(beneficiary);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public int UpdateBeneficiaryCheckDuplicate(Beneficiary beneficiary)
        {
            try
            {
                return _beneficiaryManager.UpdateBeneficiaryCheckDuplicate(beneficiary);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
    }

    //
	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
