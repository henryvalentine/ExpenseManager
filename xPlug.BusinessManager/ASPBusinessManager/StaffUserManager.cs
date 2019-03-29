using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

using ExpenseManager.EF;


namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public partial class StaffUserManager
	{
		public StaffUserManager()
		{
		}

		public int AddStaffUser(xPlug.BusinessObject.StaffUser staffUser)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = StaffUserMapper.Map<xPlug.BusinessObject.StaffUser, StaffUser>(staffUser);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToStaffUsers(myEntityObj);
					db.SaveChanges();
					staffUser.StaffUserId = myEntityObj.StaffUserId;
					return staffUser.StaffUserId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateStaffUser(xPlug.BusinessObject.StaffUser staffUser)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = StaffUserMapper.Map<BusinessObject.StaffUser, StaffUser>(staffUser);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.StaffUsers.Attach(myEntityObj);
					 db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
					db.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteStaffUser(int staffUserId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.StaffUsers.Single(s => s.StaffUserId == staffUserId);
					if (myObj == null) { return false; };
					db.StaffUsers.DeleteObject(myObj);
					db.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public xPlug.BusinessObject.StaffUser GetStaffUser(int staffUserId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.StaffUsers.SingleOrDefault(s => s.StaffUserId == staffUserId);
					if(myObj == null){return new xPlug.BusinessObject.StaffUser();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = StaffUserMapper.Map<StaffUser, xPlug.BusinessObject.StaffUser>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.StaffUser();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.StaffUser();
			}
		}

		public List<xPlug.BusinessObject.StaffUser> GetStaffUsers()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.StaffUsers.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.StaffUser>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = StaffUserMapper.Map<StaffUser, xPlug.BusinessObject.StaffUser>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.StaffUser>();
			}
		}

		public List<xPlug.BusinessObject.StaffUser>  GetStaffUsersByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.StaffUsers.ToList().FindAll(m => m.BeneficiaryId == beneficiaryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.StaffUser>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = StaffUserMapper.Map<StaffUser, xPlug.BusinessObject.StaffUser>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.StaffUser>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
