using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace megPayPayroll.CoreFramework.DropDownCheckBox
{
    public class xDropDownCheckBoxHelper
    {
        public static ArrayList ConvertEnumToArrayList(Type enumType)
        {
            if (enumType == null)
            {
                return null;
            }

            var allValues = (int[]) Enum.GetValues(enumType);
            var enumNames = Enum.GetNames(enumType);

            var myCollector = new ArrayList();

            try
            {
                for (var i = 0; i < allValues.GetLength(0); i++)
                {
                    var myObj = new DropDownNameValue ();

                    if (enumNames[i].IndexOf("_", StringComparison.Ordinal) > -1)
                    {
                        myObj.Name = enumNames[i].Replace("_", "");
                        myObj.Text = enumNames[i].Replace("_", " ");
                    }
                    else
                    {
                        myObj.Name = enumNames[i];
                        myObj.Text = enumNames[i];
                    }

                    myCollector.Add(myObj);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return myCollector;
        }
    }
    public class DropDownNameValue
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }

    //public class XyDropDownNAmeValue
    //{
    //    public string Name { get; set; }
    //    public string Text { get; set; }

    //}
}