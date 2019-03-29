using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ExpenseManager.CoreFramework;
using ExpenseManager.CoreFramework.AlertControl;
using ExpenseManager.ExpenseMgt.Reports.ReportFiles;
using xPlug.BusinessObject.CustomizedASPBusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessService;
using XPLUG.WEBTOOLS;

namespace ExpenseManager.ExpenseMgt.Voucher
{
    public partial class MultiVoucherManager : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var thread = new Thread(CleanUpFolder);
            thread.Start();
            GetVouchers();
        }
        private void GetVouchers()
        {
            try
            {

                if (Session["_voucherCollection"] == null)
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var genericCollection = (string)Session["_voucherCollection"];

                if (string.IsNullOrEmpty(genericCollection))
                {
                    ConfirmAlertBox1.ShowMessage("Session has expired.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var firstColl = genericCollection.Split(',');
                var dictCollection = new Dictionary<long, string>();
                foreach (var th in firstColl)
                {
                    if (string.IsNullOrEmpty(th)) continue;
                    var secondcoll = th.Split('+');
                    var id = long.Parse(secondcoll.ElementAt(0));
                    var word = secondcoll.ElementAt(1);
                    dictCollection.Add(id, word);
                }

                if (!dictCollection.Any())
                {
                    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. The process call was invalid.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                var paymentVoucherList = ServiceProvider.Instance().GetExpenseTransactionPaymentHistoryServices().GetVoucherObjects(dictCollection);

                if (paymentVoucherList == null || !paymentVoucherList.Any())
                {
                    ConfirmAlertBox1.ShowMessage("An unknown error was encountered. The process call was invalid.", ConfirmAlertBox.PopupMessageType.Error);
                    return;
                }

                try
                {
                    var zerosLimit = ConfigurationManager.AppSettings["ZerosLimit"];

                    foreach (var dictObject in paymentVoucherList)
                    {
                        var user = new PortalServiceManager().GetPortalUserById(dictObject.RequestedById);
                        dictObject.ReceivedBy = dictObject.ReceivedBy;
                        dictObject.RequestedBy = user.FirstName + " " + user.LastName;
                        var approver = new PortalServiceManager().GetPortalUserById(dictObject.ApproverId);
                        dictObject.Approver = approver.FirstName + " " + approver.LastName;

                        if (string.IsNullOrWhiteSpace(zerosLimit))
                        {
                            dictObject.PcvNo = dictObject.PcvId.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            var zerosPrefix = int.Parse(zerosLimit);

                            dictObject.PcvNo = new xPlug.BusinessObject.CustomizedASPBusinessObject.PCVHelper.PcvHelper().PcvGenerator(dictObject.PcvId, zerosPrefix);
                        }
                    }

                }
                catch (Exception)
                {
                    return;
                }

                GenerateReport(paymentVoucherList);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
        public bool GenerateReport(List<DictObject> dictObjList )
        {
            try
            {
                const XplugDepartments lrGlobal = XplugDepartments.LrGlobal;
                var z = Enum.GetName(typeof (XplugDepartments), lrGlobal);
                var y = 0;
                if (z != null)
                {
                    y = (int)Enum.Parse(typeof (XplugDepartments), z);
                }
                
                const XplugDepartments xPlug = XplugDepartments.XPlug;
                var s = Enum.GetName(typeof (XplugDepartments), xPlug);
                var x = 0;
                if (s != null)
                {
                    x = (int)Enum.Parse(typeof(XplugDepartments), s);
                }

                var repFilePath = "";

                if (dictObjList.ElementAt(0).DepartmentId == x)
                {
                    repFilePath = Server.MapPath("~/ExpenseMgt/Reports/ReportFiles/XplugMultiVoucher.rpt");
                }

                if (dictObjList.ElementAt(0).DepartmentId == y)
                {
                    repFilePath = Server.MapPath("~/ExpenseMgt/Reports/ReportFiles/LRMultiVoucher.rpt");
                }

                if (dictObjList.ElementAt(0).DepartmentId != x && dictObjList.ElementAt(0).DepartmentId != y)
                {
                    repFilePath = Server.MapPath("~/ExpenseMgt/Reports/ReportFiles/XplugMultiVoucher.rpt");
                }

                var voucherItemList = new List<VoucherItem>();

                foreach (var dictObj in dictObjList)
                {
                    foreach (var dictItem in dictObj.TransactionItems)
                    {
                        var newVoucherItem = new VoucherItem
                                             {
                                                 AmountInWords = dictObj.WordValue,
                                                 AmountPaid = string.Format("{0}{1}", "N", NumberMap.GroupToDigits(dictObj.AmmountPaid.ToString(CultureInfo.InvariantCulture))),
                                                 Approver = dictObj.Approver,
                                                 DatePaid = dictObj.DatePaid,
                                                 ChequeNo = dictObj.ChequeNo,
                                                 PcvNo = dictObj.PcvNo,
                                                 TransactionId = dictObj.TransactionpaymentHistoryId,
                                                 RequestedBy = dictObj.RequestedBy,
                                                 ReceivedBy = dictObj.ReceivedBy,
                                                 TotalApprovedAmount = string.Format("{0}{1}", "N", NumberMap.GroupToDigits(dictObj.TotalApprovedAmmount.ToString(CultureInfo.InvariantCulture))),
                                                 ApprovedQuantity =dictItem.ApprovedQuantity.ToString(CultureInfo.InvariantCulture),
                                                 ApprovedUnitPrice = NumberMap.GroupToDigits(dictItem.ApprovedUnitPrice.ToString(CultureInfo.InvariantCulture)),
                                                 ApprovedTotalPrice = NumberMap.GroupToDigits(dictItem.ApprovedTotalPrice.ToString(CultureInfo.InvariantCulture)),
                                                 ItemName = dictItem.ExpenseItem.Title,
                                                 ItemCode = dictItem.ExpenseItem.Code,
                                                 DetailDesription = dictItem.Description
                                             };

                        if (newVoucherItem.TransactionId > 0 && !string.IsNullOrEmpty(newVoucherItem.ItemName) && !string.IsNullOrEmpty(newVoucherItem.ItemCode))
                        {
                            voucherItemList.Add(newVoucherItem);
                        }
                        
                    }
                }

                var pdfPath = Session.SessionID + DateTime.Now.Millisecond.ToString(CultureInfo.InvariantCulture) + Environment.TickCount.ToString(CultureInfo.InvariantCulture) + ".pdf";
                var path = Server.MapPath("~/GeneratedDocuments/");
                pdfPath = path + pdfPath;

                var repDoc = new ReportDocument();
                repDoc.Load(repFilePath);

                repDoc.SetDataSource(voucherItemList);

                var crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                var crFormatTypeOptions = new PdfRtfWordFormatOptions();
                crDiskFileDestinationOptions.DiskFileName = pdfPath;
                var crExportOptions = repDoc.ExportOptions;
                {
                    crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
                    crExportOptions.FormatOptions = crFormatTypeOptions;
                }
                repDoc.Export();
                Session["myPDF.InvoicePath"] = pdfPath;

                Context.Response.Buffer = false;
                var buffer = new byte[1024];
                var inStr = File.OpenRead(pdfPath);
                while ((inStr.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (Context.Response.IsClientConnected)
                    {
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        Context.Response.Flush();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace,ex.Source, ex.Message);
                return false;
            }
        }
        private void CleanUpFolder()
        {
            try
            {
                string folderPath = Server.MapPath("~/GeneratedDocuments/");

                var dir = new DirectoryInfo(folderPath);
                var file = dir.GetFiles();
                if (file.Length <= 1)
                {
                }
                else
                {
                    foreach (var f in file)
                    {
                        if (f.CreationTime.Date < DateTime.Now.Date)
                        {
                            f.Delete();
                        }
                        else if (f.CreationTime.Date == DateTime.Now.Date)
                        {
                            if (f.CreationTime.Hour.CompareTo(DateTime.Now.Hour) == -1)
                            {
                                f.Delete();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }
        }
    }
}