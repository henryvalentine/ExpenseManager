using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPlug.BusinessObject.CustomizedASPBusinessObject
{
   public class LineObject
    {
       public  double TotalRequestedUnitPrice { get; set; }
       public double TotalApprovedUnitPrice { get; set; }
       public int TotalRequestedQuantity { get; set; }
       public int TotalApprovedQuantity { get; set; }
       public double TotalApprovedPrice { get; set; }
       public double GrandTotalApprovedPrice { get; set; }
       public bool Ismultiple { get; set; }
    }
}
