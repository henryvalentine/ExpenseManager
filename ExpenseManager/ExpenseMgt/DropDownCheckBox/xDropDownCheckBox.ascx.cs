using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using XPLUG.WEBTOOLS;

namespace megPayPayroll.CoreFramework.DropDownCheckBox
{
    public partial class xDropDownCheckBox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Set the Width of the CheckBoxList
        /// </summary>
        public int CheckedListBoxWidth
        {
            set
            {
                chkList.Width = value;
                pnlContainer.Width = value + 20;
            }
        }

        /// <summary>
        /// Set the Width of the Combo
        /// </summary>
        public int Width
        {
            set { chkListtxtCombo.Width = value; }
            get { return (Int32)chkListtxtCombo.Width.Value; }
        }

        public bool Enabled
        {
            set { chkListtxtCombo.Enabled = value; }
        }

        /// <summary>
        /// Set the CheckBoxList font Size
        /// </summary>
        /// 
        public FontUnit CheckedBoxListFontSize
        {
            set { chkList.Font.Size = value; }
            get { return chkList.Font.Size; }
        }

        /// <summary>
        /// Set the ComboBox font Size
        /// </summary>
        public FontUnit TextBoxFontSize
        {
            set { chkListtxtCombo.Font.Size = value; }
        }

        /// <summary>
        /// Add Items to the CheckBoxList.
        /// </summary>
        /// <param name="array">ArrayList to be added to the CheckBoxList</param>
        public void AddItems(ArrayList array)
        {
            foreach (DropDownNameValue t in array)
            {
                chkList.Items.Add(new ListItem(t.Text, t.Name));
            }
        }

        public void AddXItems(List<NameAndValueObject> xclientItems)
        {
            foreach (NameAndValueObject t in xclientItems)
            {
                chkList.Items.Add(new ListItem(t.Name,t.Id.ToString(CultureInfo.InvariantCulture)));
            }
        }


        /// <summary>
        /// Uncheck of the Items of the CheckBox
        /// </summary>
        public void UnselectAllItems()
        {
            for (var i = 0; i < chkList.Items.Count; i++)
            {
                chkList.Items[i].Selected = false;
            }
        }

        /// <summary>
        /// Delete all the Items of the CheckBox;
        /// </summary>
        public void ClearAll()
        {
            chkListtxtCombo.Text = "";
            chkList.Items.Clear();
        }

        /// <summary>
        /// Get or Set the Text shown in the Combo
        /// </summary>
        public string Text
        {
            get { return chkListhidVal.Value; }
            set { chkListtxtCombo.Text = value; }
        }

        /// <summary>
        /// Get All Selected Items of the CheckBox
        /// </summary>
        public ArrayList GetSelectedItems()
        {
            var myList = new ArrayList();
            for (var i = 0; i < chkList.Items.Count; i++)
            {
               if(chkList.Items[i].Selected)
               {
                   myList.Add(chkList.Items[i]);
               }
            }
            return myList;
        }


       
    }
}