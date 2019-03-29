using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManager.EF;
using ExpenseManager.EF.Helpers;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject.CustomizedASPBusinessObject;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:42
	///*******************************************************************************


	public partial class ExpenseTransactionPaymentHistoryManager
	{
        public BusinessObject.ExpenseTransactionPaymentHistory GetRecentExpenseTransactionPaymentHistories(BusinessObject.ExpenseTransactionPayment expenseTransactionPayment)
        {
            try
            {
                var paymentHistoryList = expenseTransactionPayment.ExpenseTransactionPaymentHistories.ToList();

                if(!paymentHistoryList.Any())
                {
                    return new BusinessObject.ExpenseTransactionPaymentHistory();
                }

                var paymentHistory = paymentHistoryList.OrderByDescending(m => DateTime.Parse(m.PaymentDate)).ThenByDescending(m => DateTime.Parse(m.PaymentTime)).Take(1).ToList();
                
                if(!paymentHistory.Any())
                {
                    return new BusinessObject.ExpenseTransactionPaymentHistory();
                }

                return paymentHistory.ElementAt(0);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.ExpenseTransactionPaymentHistory();
            }
        }
        public DictObject GetMyGenericVoucherObject(long transactionPaymentHistoryId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myEntityObj = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransactionPaymentHistoryId == transactionPaymentHistoryId).ToList();
                    var dictObject = new DictObject();

                    if (!myEntityObj.Any())
                    {
                        return dictObject;
                    }

                    //Re-Map Entity Object to Business Object
                    var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(myEntityObj.ElementAt(0));

                    if (myBusinessObj == null || myBusinessObj.ExpenseTransactionPaymentHistoryId < 1)
                    {
                        return dictObject;
                    }

                    var transactionItems = db.TransactionItems.Where(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId && m.ApprovedQuantity > 0 && m.ApprovedUnitPrice > 0 && m.ApprovedTotalPrice > 0).ToList();
                    if (!transactionItems.Any())
                    {
                        return dictObject;
                    }

                    var transactionItemList = transactionItems.Select(TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>).Where(obj => obj != null).ToList();

                    dictObject.TransactionpaymentHistoryId = myBusinessObj.ExpenseTransactionPaymentHistoryId;
                    dictObject.TransactionTitle = myBusinessObj.ExpenseTransaction.ExpenseTitle;
                    dictObject.ReceivedBy = myBusinessObj.Beneficiary.FullName;
                    dictObject.RequestedById = myBusinessObj.ExpenseTransaction.RegisteredById;
                    dictObject.AmmountPaid = myBusinessObj.AmountPaid;
                    dictObject.ApproverId = myBusinessObj.ExpenseTransaction.ApproverId;
                    dictObject.PaymentMode = myBusinessObj.PaymentMode;
                    dictObject.DatePaid = DateMap.ReverseToGeneralDate(myBusinessObj.PaymentDate);
                    dictObject.TransactionItems = transactionItemList.OrderBy(m => m.ExpenseItem.Title).ToList();
                    dictObject.TotalApprovedAmmount = myBusinessObj.ExpenseTransaction.TotalApprovedAmount;
                    dictObject.DepartmentId = myBusinessObj.ExpenseTransactionPayment.DepartmentId;

                    var pcvEntity = db.PaymentVoucherNumbers.SingleOrDefault(m => m.TransactionId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                    if (pcvEntity == null || pcvEntity.PaymentVoucherNumberId < 1)
                    {
                        return null;
                    }

                    var pcvObject = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, BusinessObject.PaymentVoucherNumber>(pcvEntity);
                    
                    if (pcvObject == null || pcvObject.PaymentVoucherNumberId < 1)
                    {
                        return null;
                    }

                    dictObject.PcvId = pcvObject.PaymentVoucherNumberId;
                    var beneficiaryEntity = db.Beneficiaries.SingleOrDefault(m => m.BeneficiaryId == myBusinessObj.ExpenseTransaction.BeneficiaryId);
                    if (beneficiaryEntity == null || beneficiaryEntity.BeneficiaryId < 1)
                    {
                        return null;
                    }
                    var beneficiaryObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(beneficiaryEntity);
                    if (beneficiaryObj == null || beneficiaryObj.BeneficiaryId < 1)
                    {
                        return null;
                    }
                   
                   var chequePayment = db.Cheques.SingleOrDefault(m => m.ExpenseTransactionPaymentHistoryId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                   if (chequePayment == null || chequePayment.ChequePaymentId < 1)
                    {
                        dictObject.ChequeNo = "";
                    }

                   else
                   {
                       var chequePaymentObj = ChequeMapper.Map<Cheque, BusinessObject.Cheque>(chequePayment);

                       dictObject.ChequeNo = chequePaymentObj == null ? "" : chequePaymentObj.ChequeNo;
                   }

                    return dictObject;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new DictObject();
            }
        }
        public long AddTransactionPaymentHistoryAndPcv(BusinessObject.ExpenseTransactionPaymentHistory expenseTransactionPaymentHistory)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = ExpenseTransactionPaymentHistoryMapper.Map<BusinessObject.ExpenseTransactionPaymentHistory, ExpenseTransactionPaymentHistory>(expenseTransactionPaymentHistory);
                if (myEntityObj == null)
                { return -2; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    db.AddToExpenseTransactionPaymentHistories(myEntityObj);
                    db.SaveChanges();
                    expenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId = myEntityObj.ExpenseTransactionPaymentHistoryId;
                    var transaction = myEntityObj.ExpenseTransaction;
                    var paymentVoucher = new PaymentVoucherNumber
                                             {
                                                 TransactionId =
                                                     expenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId,
                                                 TransactionTotalAmount = transaction.TotalApprovedAmount,
                                                 PaymentDate = expenseTransactionPaymentHistory.PaymentDate,
                                                 DateSubmitted = DateMap.GetLocalDate()
                                             };

                    //var pcvEntityObj = PaymentVoucherNumberMapper.Map<BusinessObject.PaymentVoucherNumber, PaymentVoucherNumber>(paymentVoucher);
                    db.AddToPaymentVoucherNumbers(paymentVoucher);
                    db.SaveChanges();

                    return expenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public List<DictObject> GetMyGenericVoucherObjectsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myEntityObjList = db.ExpenseTransactionPaymentHistories.ToList().FindAll(m => (String.CompareOrdinal(m.PaymentDate, startDate.ToString("yyyy/MM/dd")) >= 0) &&
                        (String.CompareOrdinal(m.PaymentDate, endDate.ToString("yyyy/MM/dd")) <= 0)).ToList();

                    if (!myEntityObjList.Any())
                    {
                        return new List<DictObject>();
                    }
                    
                    var dictObjList = new List<DictObject>();

                    var transactionItems = db.TransactionItems.ToList();

                    if (!transactionItems.Any())
                    {
                        return new List<DictObject>();
                    }
                   
                    //Re-Map each Entity Object to Business Object 'newTransactionPaymentList'

                    foreach (var entityObj in myEntityObjList)
                    {
                        if (dictObjList.All(m => m.TransactionpaymentHistoryId != entityObj.ExpenseTransactionPaymentHistoryId))
                        {
                            var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(entityObj);

                            if (myBusinessObj == null || myBusinessObj.ExpenseTransactionPaymentHistoryId < 1)
                            {
                                return new List<DictObject>();
                            }
                            
                            var newItems = transactionItems.FindAll(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId && m.Status == 1).ToList();
                            
                            if (!newItems.Any())
                            {
                                return new List<DictObject>();
                            }

                            var transactionItemList = new List<BusinessObject.TransactionItem>();

                            foreach (var item in newItems)
                            {
                                if (transactionItemList.All(m => m.TransactionItemId != item.TransactionItemId))
                                {
                                    var obj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                                    if (obj == null)
                                    {
                                        continue;
                                    }
                                    transactionItemList.Add(obj);
                                }
                            }
                            var dictObject = new DictObject
                                                 {
                                                     TransactionId = myBusinessObj.ExpenseTransactionPaymentHistoryId,
                                                     TransactionpaymentHistoryId = myBusinessObj.ExpenseTransactionPaymentHistoryId,
                                                     TransactionTitle = myBusinessObj.ExpenseTransaction.ExpenseTitle,
                                                     ReceivedBy = myBusinessObj.Beneficiary.FullName,
                                                     RequestedById = myBusinessObj.ExpenseTransaction.RegisteredById,
                                                     AmmountPaid = myBusinessObj.AmountPaid,
                                                     ApproverId = myBusinessObj.ExpenseTransaction.ApproverId,
                                                     PaymentMode = myBusinessObj.PaymentMode,
                                                     TimePaid = myBusinessObj.PaymentTime,
                                                     DatePaid = DateMap.ReverseToGeneralDate(myBusinessObj.PaymentDate),
                                                     TransactionItems = transactionItemList.OrderBy(m => m.ExpenseItem.Title).ToList(),
                                                     TotalApprovedAmmount = myBusinessObj.ExpenseTransaction.TotalApprovedAmount,
                                                     DepartmentId = myBusinessObj.ExpenseTransactionPayment.DepartmentId
                                                 };

                            var pcvEntity = db.PaymentVoucherNumbers.SingleOrDefault(m => m.TransactionId == myBusinessObj.ExpenseTransactionPaymentHistoryId);

                            if (pcvEntity == null || pcvEntity.PaymentVoucherNumberId < 1)
                            {
                                return null;
                            }

                            var pcvObject = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, BusinessObject.PaymentVoucherNumber>(pcvEntity);

                            if (pcvObject == null || pcvObject.PaymentVoucherNumberId < 1)
                            {
                                return new List<DictObject>();
                            }

                            dictObject.PcvId = pcvObject.PaymentVoucherNumberId;

                            var beneficiaryEntity = db.Beneficiaries.SingleOrDefault(m => m.BeneficiaryId == myBusinessObj.ExpenseTransaction.BeneficiaryId);
                            if (beneficiaryEntity == null || beneficiaryEntity.BeneficiaryId < 1)
                            {
                                return new List<DictObject>();
                            }
                            var beneficiaryObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(beneficiaryEntity);
                            if (beneficiaryObj == null || beneficiaryObj.BeneficiaryId < 1)
                            {
                                return new List<DictObject>();
                            }
                            
                            var chequePayment =
                                db.Cheques.SingleOrDefault(m => m.ExpenseTransactionPaymentHistoryId == myBusinessObj.ExpenseTransactionPaymentHistoryId);

                            if (chequePayment == null || chequePayment.ChequePaymentId < 1)
                            {
                                dictObject.ChequeNo = "";
                            }

                            else
                            {
                                var chequePaymentObj = ChequeMapper.Map<Cheque, BusinessObject.Cheque>(chequePayment);

                                dictObject.ChequeNo = chequePaymentObj == null ? "" : chequePaymentObj.ChequeNo;
                            }

                            dictObjList.Add(dictObject);
                        }
                    }

                    myEntityObjList.Clear();
                    return dictObjList.OrderBy(m => m.DatePaid).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<DictObject>();
            }
        }
        public List<DictObject> GetApprovedTransactionPaymentVoucherObjectsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    //var myEntityObjList = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransaction.Status == 1).ToList();
                    var myEntityObjList = db.ExpenseTransactionPaymentHistories.ToList().FindAll(m => (String.CompareOrdinal(m.PaymentDate, startDate.ToString("yyyy/MM/dd")) >= 0) &&
                        (String.CompareOrdinal(m.PaymentDate, endDate.ToString("yyyy/MM/dd")) <= 0) && m.ExpenseTransaction.Status == 1).ToList();

                    if (!myEntityObjList.Any())
                    {
                        return new List<DictObject>();
                    }
                    
                    var dictObjList = new List<DictObject>();
                  
                    var transactionItems = db.TransactionItems.ToList();

                    if (!transactionItems.Any())
                    {
                        return new List<DictObject>();
                    }

                    //Re-Map each Entity Object to Business Object 'newTransactionPaymentList'

                    foreach (var entityObj in myEntityObjList)
                    {
                        if (dictObjList.All(m => m.TransactionpaymentHistoryId != entityObj.ExpenseTransactionPaymentHistoryId))
                        {
                            var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(entityObj);

                            if (myBusinessObj == null || myBusinessObj.ExpenseTransactionPaymentHistoryId < 1)
                            {
                                return new List<DictObject>();
                            }
                           
                            var newItems = transactionItems.FindAll(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId && m.Status == 1).ToList();

                            if (!newItems.Any())
                            {
                                return new List<DictObject>();
                            }

                            var transactionItemList = new List<BusinessObject.TransactionItem>();

                            foreach (var item in newItems)
                            {
                                if (transactionItemList.All(m => m.TransactionItemId != item.TransactionItemId))
                                {
                                    var obj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                                    if (obj == null)
                                    {
                                        continue;
                                    }
                                    transactionItemList.Add(obj);
                                }
                            }
                            var dictObject = new DictObject
                            {
                                TransactionpaymentHistoryId = myBusinessObj.ExpenseTransactionPaymentHistoryId,
                                TransactionTitle = myBusinessObj.ExpenseTransaction.ExpenseTitle,
                                ReceivedBy = myBusinessObj.Beneficiary.FullName,
                                RequestedById = myBusinessObj.ExpenseTransaction.RegisteredById,
                                AmmountPaid = myBusinessObj.AmountPaid,
                                ApproverId = myBusinessObj.ExpenseTransaction.ApproverId,
                                PaymentMode = myBusinessObj.PaymentMode,
                                TimePaid = myBusinessObj.PaymentTime,
                                DatePaid = DateMap.ReverseToGeneralDate(myBusinessObj.PaymentDate),
                                TransactionItems = transactionItemList.OrderBy(m => m.ExpenseItem.Title).ToList(),
                                TotalApprovedAmmount = myBusinessObj.ExpenseTransaction.TotalApprovedAmount,
                                DepartmentId = myBusinessObj.ExpenseTransactionPayment.DepartmentId
                            };

                            var pcvEntity = db.PaymentVoucherNumbers.SingleOrDefault(m => m.TransactionId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                            if (pcvEntity == null || pcvEntity.PaymentVoucherNumberId < 1)
                            {
                                return null;
                            }

                            var pcvObject = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, BusinessObject.PaymentVoucherNumber>(
                                    pcvEntity);

                            if (pcvObject == null || pcvObject.PaymentVoucherNumberId < 1)
                            {
                                return new List<DictObject>();
                            }

                            dictObject.PcvId = pcvObject.PaymentVoucherNumberId;
                            var beneficiaryEntity =
                                db.Beneficiaries.SingleOrDefault(m => m.BeneficiaryId == myBusinessObj.ExpenseTransaction.BeneficiaryId);
                            if (beneficiaryEntity == null || beneficiaryEntity.BeneficiaryId < 1)
                            {
                                return new List<DictObject>();
                            }
                            var beneficiaryObj =
                                BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(beneficiaryEntity);
                            if (beneficiaryObj == null || beneficiaryObj.BeneficiaryId < 1)
                            {
                                return new List<DictObject>();
                            }
                            
                            var chequePayment =
                                db.Cheques.SingleOrDefault(
                                    m =>
                                    m.ExpenseTransactionPaymentHistoryId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                            if (chequePayment == null || chequePayment.ChequePaymentId < 1)
                            {
                                dictObject.ChequeNo = "";
                            }

                            else
                            {
                                var chequePaymentObj = ChequeMapper.Map<Cheque, BusinessObject.Cheque>(chequePayment);

                                dictObject.ChequeNo = chequePaymentObj == null ? "" : chequePaymentObj.ChequeNo;
                            }

                            dictObjList.Add(dictObject);
                        }
                    }

                    myEntityObjList.Clear();
                    return dictObjList.OrderBy(m => m.DatePaid).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<DictObject>();
            }
        }
        public List<DictObject> GetVoidedTransactionPaymentVoucherObjectsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    //var myEntityObjList = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransaction.Status == 3).ToList();
                    var myEntityObjList = db.ExpenseTransactionPaymentHistories.ToList().FindAll(m => (String.CompareOrdinal(m.PaymentDate, startDate.ToString("yyyy/MM/dd")) >= 0) &&
                        (String.CompareOrdinal(m.PaymentDate, endDate.ToString("yyyy/MM/dd")) <= 0) && m.ExpenseTransaction.Status == 3).ToList();

                    if (!myEntityObjList.Any())
                    {
                        return new List<DictObject>();
                    }

                    var dictObjList = new List<DictObject>();
                    var transactionItems = db.TransactionItems.ToList();

                    if (!transactionItems.Any())
                    {
                        return new List<DictObject>();
                    }
                    //if (!newTransactionPaymentList.Any())
                    //{
                    //    return dictObjList;
                    //}

                    //Re-Map each Entity Object to Business Object 'newTransactionPaymentList'

                    foreach (var entityObj in myEntityObjList)
                    {
                        if (dictObjList.All(m => m.TransactionpaymentHistoryId != entityObj.ExpenseTransactionPaymentHistoryId))
                        {
                            var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(entityObj);

                            if (myBusinessObj == null || myBusinessObj.ExpenseTransactionPaymentHistoryId < 1)
                            {
                                return new List<DictObject>();
                            }

                            var newItems = transactionItems.FindAll(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId && m.Status == 1).ToList();

                            if (!newItems.Any())
                            {
                                return new List<DictObject>();
                            }

                            var transactionItemList = new List<BusinessObject.TransactionItem>();

                            foreach (var item in newItems)
                            {
                                if (transactionItemList.All(m => m.TransactionItemId != item.TransactionItemId))
                                {
                                    var obj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                                    if (obj == null)
                                    {
                                        continue;
                                    }
                                    transactionItemList.Add(obj);
                                }
                            }
                            var dictObject = new DictObject
                            {
                                TransactionpaymentHistoryId = myBusinessObj.ExpenseTransactionPaymentHistoryId,
                                TransactionTitle = myBusinessObj.ExpenseTransaction.ExpenseTitle,
                                ReceivedBy = myBusinessObj.Beneficiary.FullName,
                                RequestedById = myBusinessObj.ExpenseTransaction.RegisteredById,
                                AmmountPaid = myBusinessObj.AmountPaid,
                                ApproverId = myBusinessObj.ExpenseTransaction.ApproverId,
                                PaymentMode = myBusinessObj.PaymentMode,
                                TimePaid = myBusinessObj.PaymentTime,
                                DatePaid = DateMap.ReverseToGeneralDate(myBusinessObj.PaymentDate),
                                TransactionItems = transactionItemList.OrderBy(m => m.ExpenseItem.Title).ToList(),
                                TotalApprovedAmmount = myBusinessObj.ExpenseTransaction.TotalApprovedAmount,
                                DepartmentId = myBusinessObj.ExpenseTransactionPayment.DepartmentId
                            };

                            var pcvEntity = db.PaymentVoucherNumbers.SingleOrDefault(m => m.TransactionId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                            if (pcvEntity == null || pcvEntity.PaymentVoucherNumberId < 1)
                            {
                                return null;
                            }

                            var pcvObject = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, BusinessObject.PaymentVoucherNumber>(pcvEntity);

                            if (pcvObject == null || pcvObject.PaymentVoucherNumberId < 1)
                            {
                                return new List<DictObject>();
                            }

                            dictObject.PcvId = pcvObject.PaymentVoucherNumberId;
                            var beneficiaryEntity = db.Beneficiaries.SingleOrDefault(m => m.BeneficiaryId == myBusinessObj.ExpenseTransaction.BeneficiaryId);
                            if (beneficiaryEntity == null || beneficiaryEntity.BeneficiaryId < 1)
                            {
                                return new List<DictObject>();
                            }
                            var beneficiaryObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(beneficiaryEntity);
                            if (beneficiaryObj == null || beneficiaryObj.BeneficiaryId < 1)
                            {
                                return new List<DictObject>();
                            }
                            
                            var chequePayment =db.Cheques.SingleOrDefault(m =>m.ExpenseTransactionPaymentHistoryId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                            if (chequePayment == null || chequePayment.ChequePaymentId < 1)
                            {
                                dictObject.ChequeNo = "";
                            }

                            else
                            {
                                var chequePaymentObj = ChequeMapper.Map<Cheque, BusinessObject.Cheque>(chequePayment);

                                dictObject.ChequeNo = chequePaymentObj == null ? "" : chequePaymentObj.ChequeNo;
                            }

                            dictObjList.Add(dictObject);
                        }
                    }

                    myEntityObjList.Clear();
                    return dictObjList.OrderBy(m => m.DatePaid).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<DictObject>();
            }
        }
        public List<BusinessObject.ExpenseTransactionPaymentHistory> GetTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate, int dept)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myEntityObjs = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransactionPayment.DepartmentId == dept).ToList();

                    var tx = startDate.ToString("yyyy/MM/dd");
                    var txy = endDate.ToString("yyyy/MM/dd");
                  
                    var myEntityObjList = (from hist in myEntityObjs
                                          where  String.CompareOrdinal(hist.PaymentDate, tx) >= 0 &&
                                                 String.CompareOrdinal(hist.PaymentDate, txy) <= 0
                                               select hist).ToList();

                    if (!myEntityObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPaymentHistory>();
                    }

                    var historyObjList = new List<BusinessObject.ExpenseTransactionPaymentHistory>();
                
                    //Re-Map each Entity Object to Business Object 'newTransactionPaymentList'

                    foreach (var entityObj in myEntityObjList)
                    {
                        
                        var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(entityObj);
                        
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObj.Receiver = myBusinessObj.Beneficiary.FullName;
                        historyObjList.Add(myBusinessObj);
                        
                    }

                    return historyObjList.OrderBy(m => DateTime.Parse(m.PaymentDate)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPaymentHistory>();
            }
        }
        public List<BusinessObject.ExpenseTransactionPaymentHistory> GetApprovedTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate, int dept)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    //var myEntityObjList = db.ExpenseTransactionPaymentHistories.ToList();
                    var myEntityObjs = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransactionPayment.DepartmentId == dept && m.ExpenseTransaction.Status == 1);
                        
                    var tx = startDate.ToString("yyyy/MM/dd");
                    var txy = endDate.ToString("yyyy/MM/dd");

                    var myEntityObjList = (from hist in myEntityObjs
                                           where String.CompareOrdinal(hist.PaymentDate, tx) >= 0 &&
                                                  String.CompareOrdinal(hist.PaymentDate, txy) <= 0
                                           select hist).ToList();
                    
                    
                    if (!myEntityObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPaymentHistory>();
                    }

                    var historyObjList = new List<BusinessObject.ExpenseTransactionPaymentHistory>();

                    //Re-Map each Entity Object to Business Object 'newTransactionPaymentList'

                    foreach (var entityObj in myEntityObjList)
                    {

                        var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(entityObj);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObj.Receiver = myBusinessObj.Beneficiary.FullName;
                        historyObjList.Add(myBusinessObj);

                    }

                    return historyObjList.OrderBy(m => DateTime.Parse(m.PaymentDate)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPaymentHistory>();
            }
        }
        public List<BusinessObject.ExpenseTransactionPaymentHistory> GetVoidedTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate, int dept)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    //var myEntityObjList = db.ExpenseTransactionPaymentHistories.ToList();
                    var myEntityObjs = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransactionPayment.DepartmentId == dept && m.ExpenseTransaction.Status == 3);

                    var tx = startDate.ToString("yyyy/MM/dd");
                    var txy = endDate.ToString("yyyy/MM/dd");

                    var myEntityObjList = (from hist in myEntityObjs
                                           where String.CompareOrdinal(hist.PaymentDate, tx) >= 0 &&
                                                  String.CompareOrdinal(hist.PaymentDate, txy) <= 0
                                           select hist).ToList();

                    if (!myEntityObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPaymentHistory>();
                    }

                    var historyObjList = new List<BusinessObject.ExpenseTransactionPaymentHistory>();

                    //Re-Map each Entity Object to Business Object 'newTransactionPaymentList'

                    foreach (var entityObj in myEntityObjList)
                    {

                        var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(entityObj);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObj.Receiver = myBusinessObj.Beneficiary.FullName;
                        historyObjList.Add(myBusinessObj);

                    }

                    return historyObjList.OrderBy(m => DateTime.Parse(m.PaymentDate)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPaymentHistory>();
            }
        }
        public List<DictObject> GetMyGenericVoucherObjectsByIds(List<long> paymentHistoryIds )
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                  
                    var myEntityObjList = new List<ExpenseTransactionPaymentHistory>();
                    foreach (var paymentHistoryId in paymentHistoryIds)
                    {
                        var id = paymentHistoryId;
                        var historyEntityObj = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransactionPaymentHistoryId == id).ToList();

                        if (!historyEntityObj.Any())
                        {
                            return null;
                        }

                        myEntityObjList.Add(historyEntityObj.ElementAt(0));
                    }

                    if (!myEntityObjList.Any())
                    {
                        return new List<DictObject>();
                    }
                    
                    var dictObjList = new List<DictObject>();

                    var transactionItems = db.TransactionItems.ToList();

                    if (!transactionItems.Any())
                    {
                        return new List<DictObject>();
                    }
                 
                    foreach (var entityObj in myEntityObjList)
                    {
                        var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(entityObj);

                        if (myBusinessObj == null || myBusinessObj.ExpenseTransactionPaymentHistoryId < 1)
                        {
                            return new List<DictObject>();
                        }

                        var newItems = transactionItems.FindAll(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId && m.Status == 1).ToList();

                        if (!newItems.Any())
                        {
                            continue;
                            //return new List<DictObject>();
                        }

                        var transactionItemList = new List<BusinessObject.TransactionItem>();

                        foreach (var item in newItems)
                        {
                            if (transactionItemList.All(m => m.TransactionItemId != item.TransactionItemId))
                            {
                                var obj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                                if (obj == null)
                                {
                                    continue;
                                }
                                transactionItemList.Add(obj);
                            }
                        }
                        var dictObject = new DictObject
                                                {
                                                    TransactionpaymentHistoryId = myBusinessObj.ExpenseTransactionPaymentHistoryId,
                                                    TransactionTitle = myBusinessObj.ExpenseTransaction.ExpenseTitle,
                                                    ReceivedBy = myBusinessObj.Beneficiary.FullName,
                                                    RequestedById = myBusinessObj.ExpenseTransaction.RegisteredById,
                                                    AmmountPaid = myBusinessObj.AmountPaid,
                                                    ApproverId = myBusinessObj.ExpenseTransaction.ApproverId,
                                                    PaymentMode = myBusinessObj.PaymentMode,
                                                    TimePaid = myBusinessObj.PaymentTime,
                                                    DatePaid = DateMap.ReverseToGeneralDate(myBusinessObj.PaymentDate),
                                                    TransactionItems = transactionItemList.OrderBy(m => m.ExpenseItem.Title).ToList(),
                                                    TotalApprovedAmmount = myBusinessObj.ExpenseTransaction.TotalApprovedAmount,
                                                    DepartmentId = myBusinessObj.ExpenseTransactionPayment.DepartmentId
                                                };

                        var pcvEntity = db.PaymentVoucherNumbers.SingleOrDefault(m => m.TransactionId == myBusinessObj.ExpenseTransactionPaymentHistoryId);

                        if (pcvEntity == null || pcvEntity.PaymentVoucherNumberId < 1)
                        {
                            return null;
                        }

                        var pcvObject = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, BusinessObject.PaymentVoucherNumber>(pcvEntity);

                        if (pcvObject == null || pcvObject.PaymentVoucherNumberId < 1)
                        {
                            return new List<DictObject>();
                        }

                        dictObject.PcvId = pcvObject.PaymentVoucherNumberId;

                        var beneficiaryEntity = db.Beneficiaries.SingleOrDefault(m => m.BeneficiaryId == myBusinessObj.ExpenseTransaction.BeneficiaryId);
                        if (beneficiaryEntity == null || beneficiaryEntity.BeneficiaryId < 1)
                        {
                            return new List<DictObject>();
                        }
                        var beneficiaryObj =
                            BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(beneficiaryEntity);
                        if (beneficiaryObj == null || beneficiaryObj.BeneficiaryId < 1)
                        {
                            return new List<DictObject>();
                        }

                        var chequePayment = db.Cheques.SingleOrDefault(m =>m.ExpenseTransactionPaymentHistoryId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                        if (chequePayment == null || chequePayment.ChequePaymentId < 1)
                        {
                            dictObject.ChequeNo = "";
                        }

                        else
                        {
                            var chequePaymentObj = ChequeMapper.Map<Cheque, BusinessObject.Cheque>(chequePayment);

                            dictObject.ChequeNo = chequePaymentObj == null ? "" : chequePaymentObj.ChequeNo;
                        }

                        dictObjList.Add(dictObject);
                        
                    }

                    myEntityObjList.Clear();
                    return dictObjList.OrderBy(m => m.DatePaid).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<DictObject>();
            }
        }
        public List<DictObject> GetVoucherObjects(Dictionary<long, string> dictCollection)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var myObjList = new List<BusinessObject.ExpenseTransactionPaymentHistory>();
                  
                    foreach (var keyVal in dictCollection)
                    {
                        var id = keyVal.Key;
                        var historyEntityObj = db.ExpenseTransactionPaymentHistories.Where(m => m.ExpenseTransactionPaymentHistoryId == id).ToList();

                        if (!historyEntityObj.Any())
                        {
                            return null;
                        }

                        var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, BusinessObject.ExpenseTransactionPaymentHistory>(historyEntityObj.ElementAt(0));

                        if (myBusinessObj == null || myBusinessObj.ExpenseTransactionPaymentHistoryId < 1)
                        {
                            return new List<DictObject>();
                        }
                        myBusinessObj.WordValue = keyVal.Value;
                        myObjList.Add(myBusinessObj);
                    }

                    if (!myObjList.Any())
                    {
                        return new List<DictObject>();
                    }

                    var dictObjList = new List<DictObject>();

                    var transactionItems = db.TransactionItems.ToList();

                    if (!transactionItems.Any())
                    {
                        return new List<DictObject>();
                    }

                    foreach (var myBusinessObj in myObjList)
                    {
                        var newItems = transactionItems.FindAll(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId && m.Status == 1).ToList();

                        if (!newItems.Any())
                        {
                            return new List<DictObject>();
                        }

                        var transactionItemList = new List<BusinessObject.TransactionItem>();

                        foreach (var obj in newItems.Where(item => transactionItemList.All(m => m.TransactionItemId != item.TransactionItemId)).Select(TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>).Where(obj => obj != null))
                        {
                            transactionItemList.Add(obj);
                        }
                        var dictObject = new DictObject
                                            {
                                                TransactionpaymentHistoryId = myBusinessObj.ExpenseTransactionPaymentHistoryId,
                                                TransactionTitle = myBusinessObj.ExpenseTransaction.ExpenseTitle,
                                                ReceivedBy = myBusinessObj.Beneficiary.FullName,
                                                RequestedById = myBusinessObj.ExpenseTransaction.RegisteredById,
                                                AmmountPaid = myBusinessObj.AmountPaid,
                                                ApproverId = myBusinessObj.ExpenseTransaction.ApproverId,
                                                PaymentMode = myBusinessObj.PaymentMode,
                                                TimePaid = myBusinessObj.PaymentTime,
                                                DatePaid = DateMap.ReverseToGeneralDate(myBusinessObj.PaymentDate),
                                                TransactionItems = transactionItemList.OrderBy(m => m.ExpenseItem.Title).ToList(),
                                                TotalApprovedAmmount = myBusinessObj.ExpenseTransaction.TotalApprovedAmount,
                                                WordValue = myBusinessObj.WordValue,
                                                DepartmentId = myBusinessObj.ExpenseTransactionPayment.DepartmentId
                                            };

                        var pcvEntity = db.PaymentVoucherNumbers.SingleOrDefault(m => m.TransactionId == myBusinessObj.ExpenseTransactionPaymentHistoryId);

                        if (pcvEntity == null || pcvEntity.PaymentVoucherNumberId < 1)
                        {
                            return null;
                        }

                        var pcvObject = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, BusinessObject.PaymentVoucherNumber>(pcvEntity);

                        if (pcvObject == null || pcvObject.PaymentVoucherNumberId < 1)
                        {
                            return new List<DictObject>();
                        }

                        dictObject.PcvId = pcvObject.PaymentVoucherNumberId;

                        var beneficiaryEntity = db.Beneficiaries.SingleOrDefault(m => m.BeneficiaryId == myBusinessObj.ExpenseTransaction.BeneficiaryId);
                        if (beneficiaryEntity == null || beneficiaryEntity.BeneficiaryId < 1)
                        {
                            return new List<DictObject>();
                        }
                        var beneficiaryObj =
                            BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(beneficiaryEntity);
                        if (beneficiaryObj == null || beneficiaryObj.BeneficiaryId < 1)
                        {
                            return new List<DictObject>();
                        }
                        
                        var chequePayment = db.Cheques.SingleOrDefault(m =>m.ExpenseTransactionPaymentHistoryId == myBusinessObj.ExpenseTransactionPaymentHistoryId);
                        if (chequePayment == null || chequePayment.ChequePaymentId < 1)
                        {
                            dictObject.ChequeNo = "";
                        }

                        else
                        {
                            var chequePaymentObj = ChequeMapper.Map<Cheque, BusinessObject.Cheque>(chequePayment);

                            dictObject.ChequeNo = chequePaymentObj == null ? "" : chequePaymentObj.ChequeNo;
                        }

                        dictObjList.Add(dictObject);

                    }

                    myObjList.Clear();
                    return dictObjList.OrderByDescending(m => m.DepartmentId).ThenBy(m => m.DatePaid).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<DictObject>();
            }
        }

       
	}

    //Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
