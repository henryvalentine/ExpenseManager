
namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:16
	///*******************************************************************************


	public class StandardServices : IServices
	{
        public PaymentVoucherNumberService GetPaymentVoucherNumberServices()
        {
            return new PaymentVoucherNumberService();
        }

        public BankService GetBankServices()
        {
            return new BankService();
        }
        
		public ExpenseTransactionPaymentHistoryService GetExpenseTransactionPaymentHistoryServices()
		{
			return new ExpenseTransactionPaymentHistoryService();
		}

		public AssetCategoryService GetAssetCategoryServices()
		{
			return new AssetCategoryService();
		}

		public DepartmentService GetDepartmentServices()
		{
			return new DepartmentService();
		}

		public PaymentModeService GetPaymentModeServices()
		{
			return new PaymentModeService();
		}

		public UnitService GetUnitServices()
		{
			return new UnitService();
		}

		public ChequeService GetChequeServices()
		{
			return new ChequeService();
		}

		public AssetTypeService GetAssetTypeServices()
		{
			return new AssetTypeService();
		}

		public AccountsHeadService GetAccountsHeadServices()
		{
			return new AccountsHeadService();
		}

		public LiquidAssetService GetLiquidAssetServices()
		{
			return new LiquidAssetService();
		}

		public ExpenseTransactionService GetExpenseTransactionServices()
		{
			return new ExpenseTransactionService();
		}

		public FixedAssetService GetFixedAssetServices()
		{
			return new FixedAssetService();
		}

		public ExpenseItemService GetExpenseItemServices()
		{
			return new ExpenseItemService();
		}

		public StaffUserService GetStaffUserServices()
		{
			return new StaffUserService();
		}

		public BeneficiaryService GetBeneficiaryServices()
		{
			return new BeneficiaryService();
		}

		public TransactionItemService GetTransactionItemServices()
		{
			return new TransactionItemService();
		}

		public BeneficiaryTypeService GetBeneficiaryTypeServices()
		{
			return new BeneficiaryTypeService();
		}

		public ExpenseTypeService GetExpenseTypeServices()
		{
			return new ExpenseTypeService();
		}

		public ExpenseTransactionPaymentService GetExpenseTransactionPaymentServices()
		{
			return new ExpenseTransactionPaymentService();
		}

		public ExpenseCategoryService GetExpenseCategoryServices()
		{
			return new ExpenseCategoryService();
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
