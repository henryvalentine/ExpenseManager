using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;



namespace xPlug.BusinessObject
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:26
	///*******************************************************************************


	public partial class FixedAsset
	{
		#region Main Properties
		public int FixedAssetId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public double CostOfPurchase { get; set; }

		public double CostOfTransportationAndInstallation { get; set; }

		public string DatePurchased { get; set; }

		public byte[] ScannedReceipt { get; set; }

		public string Model { get; set; }

		public string Make { get; set; }

		public string Brand { get; set; }

		public long Code { get; set; }

		public int Status { get; set; }

		public int Quantity { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 AssetTypeId {get; set;}
		[Required(AllowEmptyStrings = false, ErrorMessage="* Required")]
		public Int32 AssetCategoryId {get; set;}
		#endregion
		#region Navigation Properties
		public AssetType AssetType {get; set;}
		public AssetCategory AssetCategory {get; set;}
		#endregion
		#region Navigation Collections
		#endregion
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
