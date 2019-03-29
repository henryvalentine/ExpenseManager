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
	///* Date Generated:	25-11-2013 09:26:23
	///*******************************************************************************


	public partial class LiquidAsset
	{
		#region Main Properties
		public int LiquidAssetId { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public int Status { get; set; }

		public double Amount { get; set; }

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
