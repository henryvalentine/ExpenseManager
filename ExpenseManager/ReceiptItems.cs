using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace megPayPortalPortal
{
    public class ReceiptItems
    {
        public string CustomerName { get; set; }
        public string Purpose { get; set; }
        public string Amount { get; set; }
        public string NairaAmountInWords { get; set; }
        public string KoboAmountInWords { get; set; }
    }
}