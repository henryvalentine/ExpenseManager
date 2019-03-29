using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject.CustomizedASPBusinessObject;

namespace ExpenseManager.ExpenseMgt.Voucher
{
    public partial class VoucherManager : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var thread = new Thread(CleanUpFolder);
            thread.Start();
            GetVoucher();
        }

        private void GetVoucher()
        {
            try
            {
                if (Session["_paymentHistoryId"] == null)
                {
                    return;
                }

                var paymentHistoryId = (long)Session["_paymentHistoryId"];

                if (paymentHistoryId  < 1)
                {
                    return;
                }
                
                if (Session["_paymentHistory"] == null)
                {
                    return;
                }
                var paymentHistory = Session["_paymentHistory"] as DictObject;

                if (paymentHistory == null || paymentHistory.TransactionpaymentHistoryId < 1)
                {
                    return;
                }

                if (paymentHistory.TransactionpaymentHistoryId != paymentHistoryId)
                {
                    return;
                }
                
                GenerateReport(paymentHistory);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace,ex.Source,ex.Message);
            }
        }

        public bool GenerateReport(DictObject dictObject)
        {
            try
            {
                string wordValue;
                if (Session["_wordString"] == null)
                {
                    wordValue = "N/A";
                }

                wordValue = (string)Session["_wordString"];

                if (string.IsNullOrEmpty(wordValue))
                {
                    wordValue = "N/A";
                }

                var dataTable = new DataTable("TransactionPayment");
                dataTable.Columns.Add(new DataColumn("TransactionTitle", typeof(string)));
                dataTable.Columns.Add(new DataColumn("PaidBy", typeof(string)));
                dataTable.Columns.Add(new DataColumn("RequestedBy", typeof(string)));
                dataTable.Columns.Add(new DataColumn("ChequeNo", typeof(string)));
                dataTable.Columns.Add(new DataColumn("ReceivedBy", typeof(string)));
                dataTable.Columns.Add(new DataColumn("AmountPaid", typeof(string)));
                dataTable.Columns.Add(new DataColumn("TotalApprovedAmount", typeof(string)));
                dataTable.Columns.Add(new DataColumn("Approver", typeof(string)));
                dataTable.Columns.Add(new DataColumn("DatePaid", typeof(string)));
                dataTable.Columns.Add(new DataColumn("PaymentMode", typeof(string)));
                dataTable.Columns.Add(new DataColumn("PcvNo", typeof(string)));
                dataTable.Columns.Add(new DataColumn("AmountInWords", typeof(string)));

                DataRow dr = dataTable.NewRow();
                dr["TransactionTitle"] = dictObject.TransactionTitle;
                dr["PaidBy"] = dictObject.PaidBy;
                dr["RequestedBy"] = dictObject.RequestedBy;
                dr["ChequeNo"] = dictObject.ChequeNo;
                dr["ReceivedBy"] = dictObject.ReceivedBy;
                dr["AmountPaid"] = string.Format("{0}{1}", "N", NumberMap.GroupToDigits(dictObject.AmmountPaid.ToString(CultureInfo.InvariantCulture))); 
                dr["TotalApprovedAmount"] = string.Format("{0}{1}", "N", NumberMap.GroupToDigits(dictObject.TotalApprovedAmmount.ToString(CultureInfo.InvariantCulture))); 
                dr["Approver"] = dictObject.Approver;
                dr["DatePaid"] = dictObject.DatePaid;
                dr["PcvNo"] = dictObject.PcvNo;
                dr["AmountInWords"] = wordValue;
                dataTable.Rows.Add(dr);

                //NumbersToWord(double s, string naira, string kobo)

                var dataTable2 = new DataTable("TransactionItem");
                dataTable2.Columns.Add(new DataColumn("ItemCode", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("ItemName", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("DetailDesription", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("ApprovedQuantity", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("ApprovedUnitPrice", typeof(string)));
                dataTable2.Columns.Add(new DataColumn("ApprovedTotalPrice", typeof(string)));

                foreach (var transactionItem in dictObject.TransactionItems)
                {
                    var dr2 = dataTable2.NewRow();
                    dr2["ItemCode"] = transactionItem.ExpenseItem.Code;
                    dr2["ItemName"] = transactionItem.ExpenseItem.Title;
                    dr2["DetailDesription"] = transactionItem.Description;
                    dr2["ApprovedQuantity"] = transactionItem.ApprovedQuantity;
                    dr2["ApprovedUnitPrice"] = NumberMap.GroupToDigits(transactionItem.ApprovedUnitPrice.ToString(CultureInfo.InvariantCulture));
                    dr2["ApprovedTotalPrice"] = NumberMap.GroupToDigits(transactionItem.ApprovedTotalPrice.ToString(CultureInfo.InvariantCulture));
                    dataTable2.Rows.Add(dr2);
                }
      
                const XplugDepartments lrGlobal = XplugDepartments.LrGlobal;
                var y = (int) Enum.Parse(typeof (XplugDepartments), Enum.GetName(typeof (XplugDepartments), lrGlobal));

                const XplugDepartments xPlug = XplugDepartments.XPlug;
                var x = (int) Enum.Parse(typeof (XplugDepartments), Enum.GetName(typeof (XplugDepartments), xPlug));

                var repFilePath = "";
                if (dictObject.DepartmentId == x)
                {
                    repFilePath = Server.MapPath("~/ExpenseMgt/Reports/ReportFiles/xPlugVoucher.rpt");
                }
               
                if (dictObject.DepartmentId == y)
                {
                    repFilePath = Server.MapPath("~/ExpenseMgt/Reports/ReportFiles/LRVoucher.rpt");
                }

                if (dictObject.DepartmentId != x && dictObject.DepartmentId != y)
                {
                    repFilePath = Server.MapPath("~/ExpenseMgt/Reports/ReportFiles/xPlugVoucher.rpt");
                }
                
                var pdfPath = Session.SessionID + DateTime.Now.Millisecond.ToString(CultureInfo.InvariantCulture) + Environment.TickCount.ToString(CultureInfo.InvariantCulture) + ".pdf";
                var path = Server.MapPath("~/GeneratedDocuments/");
                pdfPath = path + pdfPath;

                var repDoc = new ReportDocument();
                repDoc.Load(repFilePath);

                repDoc.Database.Tables[0].SetDataSource(dataTable2);
                repDoc.Database.Tables[1].SetDataSource(dataTable);

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
                FileStream inStr = File.OpenRead(pdfPath);
                while ((inStr.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (Context.Response.IsClientConnected)
                    {
                        Response.Clear();
                        //Response.AddHeader("Accept-Header", buffer.Length.ToString());
                        Response.ContentType = "application/pdf";
                        Context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        Context.Response.Flush();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace,ex.Source,ex.Message);
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
                    return;
                }
                else
                {
                    foreach (FileInfo f in file)
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
                return;
            }
        }

    }
}