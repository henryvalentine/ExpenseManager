using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExpenseManager.CoreFramework
{
    public class PortalServiceManager 
    {
        public long AddPortalUser(portaluser portaluserInfo)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    db.AddToportalusers(portaluserInfo);
                    db.SaveChanges();
                    return portaluserInfo.PortalUserId;
                }
            }
            catch (Exception ex)
            {
               XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }

        }
        public long AddSiteMap(sitemap siteMapInfo)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    db.AddTositemaps(siteMapInfo);
                    db.SaveChanges();
                    return siteMapInfo.ID;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                if (ex.Message.Contains("Duplicate entry") || ex.InnerException.Message.Contains("Duplicate entry"))
                {
                    return -1;
                }
              return 0;
            }

        }
        public List<portaluser> GetPortalUserList()
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    return db.portalusers.ToList();
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<portaluser>();
            }
        }
        public portaluser GetPortalUser(string username)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    return db.portalusers.SingleOrDefault(s => s.UserName.Trim().ToLower() == username);
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new portaluser();
            }
        }
        public int GetUserIdByUsername(string username)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    var user = db.my_aspnet_users.SingleOrDefault(s => s.name.Trim().ToLower() == username.Trim().ToLower());
                    if (user != null)
                        return user.id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public List<portaluser> GetLockedPortalUser()
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    var sql = @"SELECT * FROM portaluser WHERE UserId in (SELECT UserId FROM my_aspnet_membership WHERE IsLockedOut = true or IsApproved = false);";

                    var myList = db.ExecuteStoreQuery<portaluser>(sql).Where(item => item != null).Where(item => item.PortalUserId > 0).ToList();
                    if (myList == null)
                    {
                        return null;
                    }
                    if (!myList.Any())
                    {
                        return null;
                    }
                    return myList;}
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        public List<portaluser> GetActivePortalUser()
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    var sql = @"SELECT * FROM portaluser WHERE UserId In (SELECT UserId FROM my_aspnet_membership WHERE IsLockedOut = false and IsApproved = true);";

                    var myList = db.ExecuteStoreQuery<portaluser>(sql).Where(item => item != null).Where(item => item.PortalUserId > 0).ToList();
                    if (myList == null)
                    {
                        return null;
                    }
                    if (!myList.Any())
                    {
                        return null;
                    }
                    return myList;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        public bool UpdateLockedUser(int userId)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    string sql = string.Format("UPDATE my_aspnet_membership SET IsLockedOut = {0}, IsApproved = true, failedPasswordAttemptCount = {0} WHERE userId = {1};", 0, userId);

                    return db.ExecuteStoreCommand(sql) > 0;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        public List<sitemap> GetSiteMapList()
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    return db.sitemaps.ToList();
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<sitemap>();
            }
        }
        public sitemap GetSiteMap(int tabId , int tabParentId)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    return db.sitemaps.ToList().Find(m => m.ID == tabId && m.Parent == tabParentId);
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new sitemap();
            }
        }
        public portaluser GetPortalUserById(int userId)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    return db.portalusers.SingleOrDefault(s => s.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new portaluser();
            }
        }
        public bool UpdateSiteMap(sitemap siteMapInfo, ref int flag)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    db.sitemaps.Attach(siteMapInfo);
                    db.ObjectStateManager.ChangeObjectState(siteMapInfo, EntityState.Modified);
                    db.SaveChanges();
                    flag = 0;
                    return true;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                if (ex.Message.Contains("Duplicate entry") || ex.InnerException.Message.Contains("Duplicate entry"))
                {
                    flag = -1;
                    return false;
                }
                flag = 1;
                return false;
            }

        }
        public long GetPortalUserIdByUserName(string portalUserName)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    var portalUser = db.portalusers.FirstOrDefault(s => s.UserName == portalUserName);

                    if (portalUser != null)
                    {
                        return portalUser.PortalUserId;
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public bool CheckUniqueMobileNo(long portalUserId, string mobileNo)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    var myObj = db.portalusers.FirstOrDefault(m => m.MobileNumber.Equals(mobileNo) && m.PortalUserId == (portalUserId));
                   if(myObj == null)
                   {
                        var  myOtherObj = db.portalusers.FirstOrDefault(m => m.MobileNumber.Equals(mobileNo));
                        if (myOtherObj != null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                   }
                   else
                   {
                       return true;
                   }
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        public bool CheckUniqueEmailNo(long userId, string email)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    var myObj = db.my_aspnet_membership.FirstOrDefault(m => m.Email.Equals(email) && m.userId == (userId));
                    if (myObj == null)
                    {
                        var myOtherObj = db.my_aspnet_membership.FirstOrDefault(m => m.Email.Equals(email));
                        if (myOtherObj != null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        public int UpdatePortalUser(portaluser portaluserInfo)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    db.portalusers.Attach(portaluserInfo);
                    db.ObjectStateManager.ChangeObjectState(portaluserInfo, EntityState.Modified);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public bool DeletePortalUser(long userId)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    portaluser myObj = db.portalusers.Single(s => s.PortalUserId == userId);
                    if (myObj == null) { return false; }
                    db.portalusers.DeleteObject(myObj);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }
        public bool DeleteSiteMap(int sitemapId)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    sitemap myObj = db.sitemaps.Single(s => s.ID == sitemapId);
                    if (myObj == null) { return false; }
                    db.sitemaps.DeleteObject(myObj);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }
        public int GetMaxTabOrder(int parentId)
        {
            try
            {
                using (var db = new expensemanagerportaldbEntities())
                {
                    var items = from p in db.sitemaps where p.Parent == parentId orderby p.TabOrder ascending select p ;
                    return !items.Any() ? 0 : items.ToList()[items.Count() - 1].TabOrder;
                }
            }
            catch (Exception ex)
            {
               XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        //public int GetMaxTabOrder(int parentId)
        //{
        //    try
        //    {

        //        try
        //        {
        //            string sql = "Select Max(TabOrder) From `kportaldb`.`sitemap` Where `ParentId` =" + startDate +
        //                         "` And `ScheduleDate` <= `" + endDate + "`;";


        //            var pmisList = new List<pmis_trainingschedule>();
        //            using (var db = new shepherdholmesdbEntities())
        //            {

        //                pmisList.AddRange(from item in db.ExecuteStoreQuery<pmis_trainingschedule>(sql)
        //                                  where item != null
        //                                  select item);
        //                if (!pmisList.Any())
        //                {
        //                    return new List<pmis_trainingschedule>();
        //                }
        //                return pmisList;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
        //            return new List<pmis_trainingschedule>();
        //        }

        //    }
        //    catch { return new List<pmis_trainingschedule>(); }
        //}

        //public List<pmis_trainingschedule> GetPMISTrainingByDate(string startDate, string endDate)
        //{
        //    try
        //    {

        //        try
        //        {
        //            string sql = "Select * From `shepherdholmesdb`.`pmis_trainingschedule` Where `ScheduleDate` >= `" + startDate +
        //                         "` And `ScheduleDate` <= `" + endDate + "`;";


        //            var pmisList = new List<pmis_trainingschedule>();
        //            using (var db = new shepherdholmesdbEntities())
        //            {

        //                pmisList.AddRange(from item in db.ExecuteStoreQuery<pmis_trainingschedule>(sql)
        //                                  where item != null
        //                                  select item);
        //                if (!pmisList.Any())
        //                {
        //                    return new List<pmis_trainingschedule>();
        //                }
        //                return pmisList;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            XPLUG.WEBTOOLS.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
        //            return new List<pmis_trainingschedule>();
        //        }

        //    }
        //    catch { return new List<pmis_trainingschedule>(); }
        //}

    }
}