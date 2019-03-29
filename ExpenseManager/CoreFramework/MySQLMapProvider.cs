using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Data;
using MySql.Data.MySqlClient;

namespace ExpenseManager.CoreFramework
{
    public class MySQLMapProvider : StaticSiteMapProvider
    {
        private const string Errmsg1 = "Missing connectionStringName attribute";
        private const string Errmsg2 = "Duplicate node ID";
        private SiteMapNode _root = null;
        private string _connect = String.Empty;
        private string _relConnect = String.Empty;
        private readonly Object _mobjLock = new Object();

        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {

            if (!base.SecurityTrimmingEnabled)
            {
                return true;
            }
            return node.Roles.Cast<string>().Any(item => context.User.IsInRole(item) || item == "*");
        }


        public override void Initialize(string name, NameValueCollection attributes)
        {
            base.Initialize(name, attributes);
            if (attributes == null)
            {
                throw new ConfigurationErrorsException(Errmsg1);
            }
            _connect = attributes["connectionStringName"];

            if (string.IsNullOrEmpty(_connect))
            {
                throw new ConfigurationErrorsException(Errmsg1);
            }
            _relConnect = ConfigurationManager.ConnectionStrings[_connect].ConnectionString;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override SiteMapNode BuildSiteMap()
        {
            //' Return immediately if this method has been called before
            if (_root != null)
            {
                return _root;
            }

            lock (_mobjLock)
            {
                Clear();
            }

            //' Create a dictionary for temporary node storage and lookup
            var nodes = new Dictionary<int, SiteMapNode>(16);


            //' Query the database for site map nodes
            try
            {

                using (var connection = new MySqlConnection(_relConnect))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    using (var command = new MySqlCommand("SELECT `ID`, `Title`, `Description`, `Url`, `Roles`, `Parent`, `TabType`, `TabOrder` FROM `SiteMap` ORDER BY `TabType`, `TabOrder`", connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            int id = reader.GetOrdinal("ID");
                            int url = reader.GetOrdinal("Url");
                            int title = reader.GetOrdinal("Title");
                            int desc = reader.GetOrdinal("Description");
                            int roles = reader.GetOrdinal("Roles");
                            int parent = reader.GetOrdinal("Parent");
                            int tabType = reader.GetOrdinal("TabType");
                            int tabOrder = reader.GetOrdinal("TabOrder");

                            if (reader.Read())
                            {
                                //' Create the root SiteMapNode
                                var advUrl = string.Format("XpenseManager.aspx?tabParentId={0}&tabId={1}&tabType={2}&tabOrder={3}", reader.GetInt32(parent).ToString(CultureInfo.InvariantCulture), reader.GetInt32(id).ToString(CultureInfo.InvariantCulture), reader.GetInt32(tabType).ToString(CultureInfo.InvariantCulture), reader.GetInt32(tabOrder).ToString(CultureInfo.InvariantCulture));
                                _root = new SiteMapNode(this, reader.GetInt32(id).ToString(CultureInfo.InvariantCulture), advUrl, reader.IsDBNull(title) ? null : reader.GetString(title), reader.IsDBNull(desc) ? null : reader.GetString(desc));

                                if (!reader.IsDBNull(roles))
                                {
                                    string rolenames = reader.GetString(roles).Trim();
                                    if (!string.IsNullOrEmpty(rolenames))
                                    {
                                        string[] rolelist = rolenames.Split(new char[] { ',', ';' }, 512);
                                        _root.Roles = rolelist;
                                    }
                                }
                                // ' Add "*" to the roles list if no roles are specified
                                if (_root.Roles == null)
                                {
                                    _root.Roles = new String[] { "*" };
                                }

                                // Add custom attributes to parent
                                _root["moduleSource"] = reader.IsDBNull(url) ? null : reader.GetString(url);
                                _root["tabParentId"] = reader.GetInt32(parent).ToString(CultureInfo.InvariantCulture);

                                //  ' Record the root node in the dictionary
                                if (nodes.ContainsKey(reader.GetInt32(id)))
                                {
                                    throw new ConfigurationErrorsException(Errmsg2);
                                }

                                // ' ConfigurationException pre-Beta 2
                                nodes.Add(reader.GetInt32(id), _root);

                                // ' Add the node to the site map
                                AddNode(_root, null);

                                //' Build a tree of SiteMapNodes underneath the root node
                                while (reader.Read())
                                {

                                    var advUrl2 = string.Format("XpenseManager.aspx?tabParentId={0}&tabId={1}&tabtype={2}&tabOrder={3}", reader.GetInt32(parent).ToString(CultureInfo.InvariantCulture), reader.GetInt32(id).ToString(CultureInfo.InvariantCulture), reader.GetInt32(tabType).ToString(CultureInfo.InvariantCulture), reader.GetInt32(tabOrder).ToString(CultureInfo.InvariantCulture));
                                    var node = new SiteMapNode(this,
                                                                 reader.GetInt32(id).ToString(CultureInfo.InvariantCulture),
                                                                 advUrl2,
                                                                 reader.IsDBNull(title) ? null : reader.GetString(title),
                                                                 reader.IsDBNull(desc) ? null : reader.GetString(desc));

                                    if (!reader.IsDBNull(roles))
                                    {
                                        string rolenames = reader.GetString(roles).Trim();
                                        if (!string.IsNullOrEmpty(rolenames))
                                        {
                                            string[] rolelist = rolenames.Split(new char[] { ',', ';' }, 512);
                                            node.Roles = rolelist;
                                        }

                                    }

                                    // Add custom attributes to children
                                    node["moduleSource"] = reader.IsDBNull(url) ? "" : reader.GetString(url);
                                    node["tabParentId"] = reader.GetInt32(parent).ToString(CultureInfo.InvariantCulture);
                                    //' If the node lacks roles information, "inherit" that
                                    //' information from its parent

                                    if (nodes.ContainsKey(reader.GetInt32(parent)))
                                    {
                                        SiteMapNode parentnode = nodes[reader.GetInt32(parent)];
                                        if (node.Roles == null)
                                        {
                                            node.Roles = parentnode.Roles;
                                        }
                                        //' Record the node in the dictionary
                                        if (nodes.ContainsKey(reader.GetInt32(id)))
                                        {
                                            throw new ConfigurationErrorsException(Errmsg2);
                                        }

                                        nodes.Add(reader.GetInt32(id), node);

                                        //' Add the node to the site map
                                        AddNode(node, parentnode);
                                    }

                                }
                            }
                        }
                        
                        
                    }
                 
                }

            }
            catch (Exception ex)
            {
                //AppException.LogError(ex.Message, ex.StackTrace.ToString);
                string mx = ex.Message;
                return null;
            }
            //' Return the root SiteMapNode
            return _root;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            BuildSiteMap();
            return _root;
        }
    }
}