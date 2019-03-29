using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
  public  partial class StaffUserService
    {
        public StaffUser GetStaffUserByMembershipId(int membershipIdId)
        {
            try
            {
                return _staffUserManager.GetStaffUserByMembershipId(membershipIdId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.StaffUser();
            }
        }
    }
}
