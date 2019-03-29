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


	public partial class StaffUserService : MarshalByRefObject
	{
		private readonly StaffUserManager  _staffUserManager;
		public StaffUserService()
		{
			_staffUserManager = new StaffUserManager();
		}

		public int AddStaffUser(StaffUser staffUser)
		{
			try
			{
				return _staffUserManager.AddStaffUser(staffUser);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateStaffUser(StaffUser staffUser)
		{
			try
			{
				return _staffUserManager.UpdateStaffUser(staffUser);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteStaffUser(Int32 staffUserId)
		{
			try
			{
				return _staffUserManager.DeleteStaffUser(staffUserId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public StaffUser GetStaffUser(int staffUserId)
		{
			try
			{
				return _staffUserManager.GetStaffUser(staffUserId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new StaffUser();
			}
		}

		public List<StaffUser> GetStaffUsers()
		{
			try
			{
				var objList = new List<StaffUser>();
				objList = _staffUserManager.GetStaffUsers();
				if(objList == null) {return  new List<StaffUser>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<StaffUser>();
			}
		}

		public List<StaffUser>  GetStaffUsersByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				return _staffUserManager.GetStaffUsersByBeneficiaryId(beneficiaryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<StaffUser>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
