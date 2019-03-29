using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseManager.CoreFramework.GridPager
{
    public partial class GridPager : System.Web.UI.UserControl
    {
            private int _totalRecordCount = 0;
            private int _currentPage  = 0;
            private int _pageCount  = 0;
            private int _recordsPerPage  = 0;

        public GridPager()
        {
            GridPageCount = 0;
            GridCurrentPageIndex = 0;
        }

        #region Public Properties
                public int TotalRecordCount
                {
                    get{ return _totalRecordCount;} 
                    set{ _totalRecordCount = value;}
                }
                public int CurrentPage
                {
                    get{ return _currentPage;} 
                    set{ _currentPage = value;}
                }
                public int PageCount
                {
                    get{ return _pageCount;} 
                    set{ _pageCount = value;}
                }
                public int RecordsPerPage
                {
                    get{ return _recordsPerPage;} 
                    set{ _recordsPerPage=value;}
                }

                 public int GridCurrentPageIndex { get; set; }

                public int GridPageCount { get; set; }
        

            #endregion

            //Events
            public delegate void PagerAg(object sender);

            public event PagerAg DoFirst;
            public event PagerAg DoNext;
            public event PagerAg DoPrevious;
            public event PagerAg DoLast;
            public event PagerAg ReLoad;

        public void PagerButtonClick(object sender,  System.Web.UI.ImageClickEventArgs e)
            {
                if(this.Items.Text == ""){return;}
                try
                {
                    int k = int.Parse(this.Items.Text);
                }
                catch{return;}
                if(int.Parse(this.Items.Text) < 1){return;}

                try
                {
                    int p = int.Parse(this.lblTotalItems.Text);
                }
                catch{return;}

               if (int.Parse(this.Items.Text) > int.Parse(this.lblTotalItems.Text)) { return; }

                try
                {
                        string arg = ((ImageButton) sender).CommandArgument;
                        _recordsPerPage = int.Parse(this.Items.Text);
                        switch(arg)
                        {
                            case "next":
                                if(int.Parse(this.lblTotalPages.Text) == int.Parse(this.txt_OfText.Text)){return;}
                                DoNext(this);
                                break;
                            case "prev":
                                 if( int.Parse(this.txt_OfText.Text) ==  1){return;}
                                DoPrevious(this);
                                break;
                            case "last":
                                if( int.Parse(this.txt_OfText.Text) ==  int.Parse(this.lblTotalPages.Text)){return;}
                                DoLast(this);
                                break;
                            case "0":
                                 if( int.Parse(this.txt_OfText.Text) ==  1){return;}
                                DoFirst(this);
                                break;
                            default:
                                return;

                        }
                        this.txt_OfText.Text = _currentPage.ToString(CultureInfo.InvariantCulture);
                        this.lblTotalPages.Text = _pageCount.ToString(CultureInfo.InvariantCulture);
                        this.lblTotalItems.Text = _totalRecordCount.ToString(CultureInfo.InvariantCulture);
                }
                catch{}

            }
       
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) 
            {
                try 
                {
                    if (!string.IsNullOrEmpty(Session["my._currentPage"].ToString()))
                    {
                        try
                        {
                            this.txt_OfText.Text = int.Parse(Session["my._currentPage"].ToString()).ToString(CultureInfo.InvariantCulture);
                        }
                        catch { this.txt_OfText.Text = "1"; }
                    }
                }
                catch 
                {
                    this.txt_OfText.Text = "1";
                }
                try
                {
                    if (!string.IsNullOrEmpty(Session["my._pageCount"].ToString()))
                    {
                        try { this.lblTotalPages.Text = int.Parse((Session["my._pageCount"].ToString())).ToString(CultureInfo.InvariantCulture); }
                        catch { this.lblTotalPages.Text = "1"; }
                    }
                }
                catch
                {
                    this.lblTotalPages.Text = "1";
                }
                try
                {
                    if (!string.IsNullOrEmpty(Session["my._totalRecordCount"].ToString()))
                    {
                        try { this.lblTotalItems.Text = int.Parse((Session["my._totalRecordCount"].ToString())).ToString(CultureInfo.InvariantCulture); }
                        catch { this.lblTotalItems.Text = "1"; }
                    }
                }
                catch
                {
                    this.lblTotalItems.Text = "1";
                }
                try
                {
                    if (!string.IsNullOrEmpty(Session["my._items"].ToString()))
                    {
                        try { _recordsPerPage = int.Parse((Session["my._items"].ToString())); }
                        catch { _recordsPerPage = 1; }
                    }
                }
                catch
                {
                    _recordsPerPage = 1;
                }
                ReLoad(this);
           
            }
        }
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
              if(this.Items.Text == ""){return;}
                try
                {
                    int k = int.Parse(this.Items.Text);
                }
                catch{return;}
                if(int.Parse(this.Items.Text) < 1){return;}

                try
                {
                    int p = int.Parse(this.lblTotalItems.Text);
                }
                catch{return;}

               if (int.Parse(this.Items.Text) > int.Parse(this.lblTotalItems.Text)) { return; }

                _recordsPerPage = int.Parse(this.Items.Text);
                 ReLoad(this);
                this.txt_OfText.Text = _currentPage.ToString(CultureInfo.InvariantCulture);
                this.lblTotalPages.Text = _pageCount.ToString(CultureInfo.InvariantCulture);
                this.lblTotalItems.Text = _totalRecordCount.ToString(CultureInfo.InvariantCulture);
                Session["my._currentPage"] = _currentPage.ToString(CultureInfo.InvariantCulture);
                Session["my._pageCount"] = _pageCount.ToString(CultureInfo.InvariantCulture);
                Session["my._totalRecordCount"] = _totalRecordCount.ToString(CultureInfo.InvariantCulture);
                Session["my._items"] = _recordsPerPage;
        }
         public void InitializeGridSize(int gsize, int totalCount, int pagecount)
         {
                this.txt_OfText.Text = "1";
                this.lblTotalPages.Text = pagecount.ToString(CultureInfo.InvariantCulture);
                this.lblTotalItems.Text = totalCount.ToString(CultureInfo.InvariantCulture);
                _totalRecordCount = totalCount;
                _recordsPerPage = gsize;
                this.Items.Text = gsize.ToString(CultureInfo.InvariantCulture);
                Session["my._currentPage"] = "1";
                Session["my._pageCount"] = pagecount.ToString(CultureInfo.InvariantCulture);
                Session["my._totalRecordCount"] = totalCount.ToString(CultureInfo.InvariantCulture);
                Session["my._items"] = gsize.ToString(CultureInfo.InvariantCulture);
                //ReLoad(this);
         }
 
    }
}