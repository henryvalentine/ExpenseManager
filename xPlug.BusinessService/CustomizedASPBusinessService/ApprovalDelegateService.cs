using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using XPLUG.WEBTOOLS;

namespace xPlug.BusinessService.CustomizedASPBusinessService
{
    public class ApprovalDelegateService
    {
        private static string filepath = HttpContext.Current.Server.MapPath("DelegateUser/ApprovalDelegates.xml");

        public bool CheckFileExist()
        {
            var file = new FileInfo(filepath);
            try
            {
                if(file.Exists)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
            return false;
        }

        public void CreateFile()
        {
            var file = new FileInfo(filepath);
            try
            {
                if(file.Exists)
                {
                    return;
                }
                else
                {

                    var delegates = new XDocument(
                                                new XDeclaration(@"1.0",@"utf-8",""),
                                                new XElement("Delegates", "")
                                                );

                    delegates.Save(filepath);
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                
            }
        }

        public bool DeleteFile()
        {
            var file = new FileInfo(filepath);
            try
            {
                if (file.Exists)
                {
                    file.Delete();
                    return true;
                }
            }
            catch (Exception ex)
            {
               ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
            return false;
        }
        
        public bool AddDelegateApprover(long portalUserId,int status,string email)
        {
            try
            {
                if(!CheckFileExist())
                {
                    CreateFile();
                }

                  var approverFile = XElement.Load(filepath);
                  var delegateApprover = new XElement("DelegateUser",
                                           new XElement("PortalUserId", portalUserId.ToString(CultureInfo.InvariantCulture)),
                                           new XElement("Status", status.ToString(CultureInfo.InvariantCulture)),
                                           new XElement("Email", email)
                        );

                if (approverFile.HasElements)
                {

                    foreach (var delegateUser in approverFile.Descendants("DelegateUser").ToList()) 
                    {
                        if (delegateUser != null)   
                        {
                            delegateUser.Remove();                                 
                        }
                    }
                    approverFile.Add(delegateApprover);
                    approverFile.Save(filepath);
                    return true;
                }
                else
                {
                    approverFile.Add(delegateApprover);
                    approverFile.Save(filepath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool CheckDelegate(int portalUserId)
        {
            var approverFile = XElement.Load(filepath);
            var idCollection = new List<int>();
            foreach (var delegateUser in approverFile.Descendants("DelegateUser").ToList())
            {
                var xAttribute = delegateUser.Element("PortalUserId");
                if (xAttribute != null && int.Parse(xAttribute.Value) == portalUserId)
                {
                    idCollection.Add(int.Parse(xAttribute.Value));
                }
            }
            if(!idCollection.Any())
            {
                return false;
                
            }
            return true;
        }

        public string GetApproverEmail()
        {
            var approverFile = XElement.Load(filepath);
            var emailCollection = (from delegateUser in approverFile.Descendants("DelegateUser").ToList() select delegateUser.Element("Email") into xAttribute where xAttribute != null select xAttribute.Value).ToList();
            return !emailCollection.Any() ? null : emailCollection.ElementAtOrDefault(0);
        }
    }
}