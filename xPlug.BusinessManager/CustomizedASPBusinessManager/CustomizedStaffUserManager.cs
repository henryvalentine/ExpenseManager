using System;
using System.Linq;
using ExpenseManager.EF;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	04-11-2013 03:10:16
	///*******************************************************************************


	public partial class StaffUserManager
	{
        public BusinessObject.StaffUser GetStaffUserByMembershipId(int membershipIdId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObj = db.StaffUsers.Where(s => s.PortalUserId == membershipIdId).ToList();
                    if (!myObj.Any())
                    {
                        return new BusinessObject.StaffUser();
                    }
                    //Re-Map Entity Object to Business Object
                    var myBusinessObj = StaffUserMapper.Map<StaffUser, BusinessObject.StaffUser>(myObj.ElementAt(0));
                    if (myBusinessObj == null)
                    {
                        return new BusinessObject.StaffUser();
                    }
                    { return myBusinessObj; }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.StaffUser();
            }
        }
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
