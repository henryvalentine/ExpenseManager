using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ExpenseManager.Helpers
{
    public class NumberToEnglishHelper
    {
        public static string NumbersToWord(double s, string naira, string kobo)
        {

            try
            {
                var groupedMapping = new string[] {"", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion",
            "Septillion", "Octillion", "Nonillion", "Decillion", "Undecillion", "Duodecillion", "Tredecillion",
            "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septendecillion", "Octodecillion", "Novemdecillion",
            "Vigintillion", "Unvigintillion", "Duovigintillion", "10^72", "10^75", "10^78", "10^81", "10^84", "10^87",
            "Vigintinonillion", "10^93", "10^96", "Duotrigintillion", "Trestrigintillion"};
                var onesMapping = new string[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
                var tensMapping = new string[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var twentiesMapping = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                var c = s.ToString(CultureInfo.InvariantCulture);
                c = c.Replace(".", string.Empty);
                if (s != float.Parse(c))
                    return "not a number";
                var x = c.IndexOf('.');
                if (x == -1) x = c.Length;
                //if (x > 15) return 'too big';
                var n = c.Select(digit => digit.ToString(CultureInfo.InvariantCulture)).ToArray();
                var str = "";
                var sk = 0;

                for (var i = 0; i < x; i++)
                {
                    var xInt = int.Parse(n[i]);
                    if ((x - i) % 3 == 2)
                    {
                        if (xInt == 1)
                        {
                            str += tensMapping[int.Parse(n[xInt + 1])] + " ";
                            i++; sk = 1;
                        }
                        else if (n[i] != "0")
                        {
                            str += twentiesMapping[xInt - 2] + " ";
                            sk = 1;
                        }
                    }

                    else if (xInt != 0)
                    {
                        str += onesMapping[xInt] + " ";
                        if ((x - i) % 3 == 0) str += "Hundred and ";
                        sk = 1;
                    }

                    if ((x - i) % 3 == 1)
                    {
                        if (sk) str += groupedMapping[(x - i - 1) / 3] + ' ';
                        sk = 0;
                    }

                }

                if (x != c.Length)
                {
                    var y = c.Length;
                    str += naira + ' ';
                    for (var j = x + 1; j < y; j++)
                        str += onesMapping[int.Parse(n[j])] + ' ' + kobo + " ";
                }

                if (x == c.Length)
                {
                    var r = c.Length;
                    str += ' ';
                    for (var k = x + 1; k < r; k++) str += onesMapping[int.Parse(n[k])] + " ";
                }

                var newString = "";

                if (str.Length > 6)
                {
                    if (str.ElementAt(str.Length - 7) == ' ' && str.ElementAt(str.Length - 6) == 'a'
                    && str.ElementAt(str.Length - 5) == 'n' && str.ElementAt(str.Length - 4) == 'd' && str.ElementAt(str.Length - 3) == ' ')
                    {
                        var substring = str.Substring(0, str.Length - 6);
                        return substring;
                    }
                    else
                    {

                        if (newString.ElementAt(newString.Length - 5) == ' ' && newString.ElementAt(newString.Length - 4) == 'a'
                        && newString.ElementAt(newString.Length - 3) == 'n' && newString.ElementAt(newString.Length - 2) == 'd' && newString.ElementAt(newString.Length - 1) == ' ')
                        {
                            var substring1 = newString.Substring(0, str.Length - 6);
                            return substring1;
                        }
                        else
                        {
                            return str;
                        }

                    }

                }

                return GetFormatedAmountInWords(str, naira);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string GetFormatedAmountInWords(string totalPrice, string naira) 
       {

           if (!totalPrice.Contains("Naira")) 
           {
              return totalPrice;
           }

           return totalPrice + "  " + naira;
           
       }
    }
      
}
