using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPlug.BusinessObject.CustomizedASPBusinessObject
{
  public class GenericObject
    {
        public string ExpenseTitle { get; set; }
        public string  ExpenseCategoryTitle { get; set; }
        public string ExpenseItemTitle  { get; set; }
        public double ApprovedUnitAmmount  { get; set; }
        public int  ApprovedQuantity { get; set; }
        public string Approver  { get; set; }
        public double ApprovedTotalAmmount  { get; set; }
        public string DateApproved  { get; set; }
        public string  TimeApproved { get; set; }
    
    }
}
