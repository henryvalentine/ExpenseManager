using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPlug.BusinessObject.CustomizedASPBusinessObject
{
   public class TransactionObject
    {
       public string TransactionTitle { get; set; }
       public string TransactionItem { get; set; }
       public int Quantity { get; set; }
       public double UnitPrice { get; set; }
       public string TransactionApprovalDate { get; set; }
    }
}
