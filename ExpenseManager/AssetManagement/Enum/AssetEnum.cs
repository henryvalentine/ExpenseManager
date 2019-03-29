using System.ComponentModel;

namespace ExpenseManager.AssetManagement.Enum
{
    public class AssetCategoryIdStore
    {
        public enum AssetCategories
        {
            [Description("Fixed Asset")]FixedAsset = 1,
            [Description("Liquid Asset")]LiquidAsset = 2,
        }
    }
}