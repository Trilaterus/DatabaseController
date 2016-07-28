using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Xml.Linq;

namespace EmployeeDatabase
{
    public partial class ConnectForm : Form
    {
        private String m_sServer;
        private String m_sDatabase;
        private String m_sUID;
        private XDocument m_XMLDoc = XDocument.Load("..//..//LoginInformation.xml");
        private DataSet m_dsTableNames;
        private MySqlDataAdapter m_daDataAdapter;
        private String m_sConnection;
        private List<String>[] m_vsTableColumnNames = new List<String>[2];
        private String m_sPrimaryKeyColumn;

        private List<String> m_asTableNames;
        private List<String> m_asColumnNames;

        public ConnectForm()
        {
            InitializeComponent();
            m_vsTableColumnNames[0] = new List<String>();
            m_vsTableColumnNames[1] = new List<String>();
            initLoginTab();
        }

        private void initLoginTab()
        {
            // Run query for info
            var info = from r in m_XMLDoc.Descendants("Login")
                       select new
                       {
                           Server = r.Element("server").Value,
                           DatabaseName = r.Element("database").Value,
                           UserID = r.Element("userid").Value,
                           ServerCheck = r.Element("servercheck").Value,
                           DatabaseNameCheck = r.Element("databasecheck").Value,
                           UserIDCheck = r.Element("useridcheck").Value
                       };
            foreach (var r in info)
            {
                m_sServer = r.Server;
                m_sDatabase = r.DatabaseName;
                m_sUID = r.UserID;

                if (r.ServerCheck == "True")
                    cbServer.Checked = true;
                if (r.DatabaseNameCheck == "True")
                    cbDatabaseName.Checked = true;
                if (r.UserIDCheck == "True")
                    cbUID.Checked = true;
            }

            tbServer.Text = m_sServer;
            tbDatabaseName.Text = m_sDatabase;
            tbUID.Text = m_sUID;
        }

        private void initColumnTab()
        {
            treeView2.Nodes.Clear();

            m_dsTableNames = new DataSet();
            string sComGetTableName = "select table_name, column_name from information_schema.columns where table_schema = '" + m_sDatabase + "';";

            using (MySqlConnection cnConnection = new MySqlConnection(m_sConnection))
            {
                cnConnection.Open();
                m_daDataAdapter = new MySqlDataAdapter(sComGetTableName, cnConnection);
                m_daDataAdapter.Fill(m_dsTableNames);
            }

            TreeNode tnRoot = treeView2.Nodes.Add(m_sDatabase);
            tnRoot.Name = m_sDatabase;
            AutoIncrementer aiTables = new AutoIncrementer();
            AutoIncrementer aiColumns = new AutoIncrementer();
            string sPreviousTable = "";
            foreach (DataRow row in m_dsTableNames.Tables[0].Rows)
            {
                string sCurrentTable = row["table_name"].ToString();
                if (sPreviousTable != sCurrentTable)
                {
                    string sTableName = row["table_name"].ToString();
                    TreeNode tnTable = tnRoot.Nodes.Add(sTableName);
                    tnTable.Name = sTableName;
                    aiColumns.Reset();
                    uint uiCurrentTableIndex = aiTables.Next();
                    m_vsTableColumnNames[0].Add(sTableName);

                    string sColumnName = row["column_name"].ToString();
                    TreeNode tnColumn = tnTable.Nodes.Add(sColumnName);
                    tnColumn.Name = sColumnName;
                    m_vsTableColumnNames[1].Add(sTableName + "." + sColumnName);
                }
                else
                {
                    TreeNode[] tnTable = tnRoot.Nodes.Find(sCurrentTable, true);
                    string sColumnName = row["column_name"].ToString();
                    tnTable.ElementAt(0).Nodes.Add(sColumnName);
                    m_vsTableColumnNames[1].Add(sPreviousTable + "." + sColumnName);
                }
                sPreviousTable = sCurrentTable;
            }
            treeView2.ExpandAll();

            Copy(treeView2, myTreeView1);
            treeView2.ExpandAll();
            myTreeView1.ExpandAll();
        }

        public void Copy(TreeView treeview1, TreeView treeview2)
        {
            TreeNode newTn;
            foreach (TreeNode tn in treeview1.Nodes)
            {
                newTn = new TreeNode(tn.Text);
                CopyChilds(newTn, tn);
                treeview2.Nodes.Add(newTn);
            }
        }

        public void CopyChilds(TreeNode parent, TreeNode willCopied)
        {
            TreeNode newTn;
            foreach (TreeNode tn in willCopied.Nodes)
            {
                newTn = new TreeNode(tn.Text);
                parent.Nodes.Add(newTn);
                CopyChilds(newTn, tn);
            }
        }

        /// <summary>
        /// Fill m_sGeneratedSelectStatement with information based on the selected nodes in the treeview
        /// using a special algorithm to change the statement to convey the foreign key content rather
        /// than the raw foreign key values
        /// </summary>
        private String UpdateSelectStatement/*WithForeignKeys*/()
        {
            // Clear the statement
            String sStatementInnerJoins = "";
            String sStatementColumnNames = "";
            String sStatementTableNames = "";
            List<String> lsTableNames = new List<String>();
            List<String> lsColumnNames = new List<String>();
            // Iterate through each of the tables
            foreach (TreeNode root in myTreeView1.Nodes)
            {
                foreach (TreeNode tables in root.Nodes)
                {
                    if (tables.Checked)
                    {
                        lsTableNames.Add(tables.Text);
                        foreach (TreeNode columns in tables.Nodes)
                        {
                            if (columns.Checked)
                            {
                                lsColumnNames.Add(tables.Text + "." + columns.Text);
                            }
                        }
                    }
                }
            }

            // Foreign key checking and column name finalization
            // System of generating SELECT statements that have foreign key auto detection
            foreach (String sColumnName in lsColumnNames)
            {
                String sTableName = sColumnName.Substring(0, sColumnName.IndexOf('.'));

                if (sColumnName.EndsWith("_id"))
                {
                    // Get the "foreign" out of "table.foreign_id"
                    sTableName = sColumnName.Substring(sColumnName.IndexOf('.') + 1, sColumnName.IndexOf('_') - sColumnName.IndexOf('.') - 1);
                    if (lsTableNames.Contains(sTableName))
                    {
                        sStatementInnerJoins += " INNER JOIN " + sTableName + " ON " + sColumnName + " = " + sTableName + ".id";
                    }
                    else
                    {
                        sStatementColumnNames += sColumnName + ", ";
                    }
                }
                else
                {
                    sStatementColumnNames += sColumnName + ", ";
                    if (!sStatementInnerJoins.Contains(sTableName) &&
                        m_sPrimaryKeyColumn.Substring(0, m_sPrimaryKeyColumn.IndexOf('.')) != sTableName)
                    {
                        sStatementTableNames += sTableName + ", ";
                    }
                }
            }

            // checking for duplicate tables part
            String sTableNames = sStatementTableNames;
            String sInnerJoins = sStatementInnerJoins;
            sTableNames = sTableNames.Replace(",", "");
            sInnerJoins = sInnerJoins.Replace(",", "");
            String sProperTableNames = "";
            foreach (String sTableName in lsTableNames)
            {
                if (!sInnerJoins.Contains(sTableName) &&
                    sTableName != m_sPrimaryKeyColumn.Substring(0, m_sPrimaryKeyColumn.IndexOf('.')))
                {
                    sProperTableNames += sTableName + ", ";
                }
            }
            if (sStatementColumnNames.Count() <= 0)
            {
                sStatementColumnNames += "  ";
            }

            String sStatement = "SELECT " + m_sPrimaryKeyColumn + "," + sStatementColumnNames.Substring(0, sStatementColumnNames.Length - 2) + " FROM " + sProperTableNames + " " + m_sPrimaryKeyColumn.Substring(0, m_sPrimaryKeyColumn.IndexOf('.')) + sStatementInnerJoins;

            m_asTableNames = lsTableNames;
            m_asColumnNames = lsColumnNames;

            return sStatement;
        }

        /// <summary>
        /// Fill the m_sGeneratedStatement with information based on the selected nodes in the treeview
        /// </summary>
        /// <returns></returns>
        /*
        private String UpdateSelectStatement()
        {


            return "";
        }*/

        private void btLogin_Click(object sender, EventArgs e)
        {
            bool bSuccess = false;
            String sConnection = "";
            try
            {
                String sServer = tbServer.Text;
                String sDatabaseName = tbDatabaseName.Text;
                String sUID = tbUID.Text;
                String sPassword = tbPassword.Text;
                sConnection = "Server = " + sServer + "; Database = " + sDatabaseName + "; Uid = " + sUID + "; Pwd = " + sPassword + ";";
                using (MySqlConnection cnConnection = new MySqlConnection(sConnection))
                {
                    cnConnection.Open();
                    bSuccess = true;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                bSuccess = false;
            }
            if (bSuccess)
            {
                m_sConnection = sConnection;
                String sServer = "";
                String sDatabase = "";
                String sUID = "";
                if (cbServer.Checked)
                    sServer = tbServer.Text;
                if (cbDatabaseName.Checked)
                    sDatabase = tbDatabaseName.Text;
                if (cbUID.Checked)
                    sUID = tbUID.Text;
                // Write remember me values to XML
                m_XMLDoc = new XDocument(); // Overwrite current XML
                m_XMLDoc = new XDocument(
                    new XElement("Doc",
                        new XElement("Login",
                            new XElement("server", sServer),
                            new XElement("database", sDatabase),
                            new XElement("userid", sUID),
                            new XElement("servercheck", cbServer.Checked.ToString()),
                            new XElement("databasecheck", cbDatabaseName.Checked.ToString()),
                            new XElement("useridcheck", cbUID.Checked.ToString()))));
                m_XMLDoc.Save("..//..//LoginInformation.xml");

                // Change tabs
                initColumnTab();
                tablessTabControl1.SelectedIndex++;
            }
        }

        private void btGenerate_Click(object sender, EventArgs e)
        {
            // Switch WinForms
            this.Hide();
            var form = new Form1();
            form.m_sConnection = m_sConnection;
            form.m_sSelectStatement = textBox1.Text;
            //form.m_sIndexColumnName = m_sPrimaryKeyColumn;
            form.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Checks if the selected node is viable for being a primary key
        /// by checking if it is an unsigned int
        /// </summary>
        private void btPKey_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                try
                {
                    DataSet dsIDTableType = new DataSet();
                    MySqlDataAdapter daDataAdapter;
                    string sComGetColType = "SELECT " + treeView2.SelectedNode.Text + " FROM " + treeView2.SelectedNode.Parent.Text;
                    using (MySqlConnection cnConnection = new MySqlConnection(m_sConnection))
                    {
                        cnConnection.Open();
                        daDataAdapter = new MySqlDataAdapter(sComGetColType, cnConnection);
                        daDataAdapter.Fill(dsIDTableType);
                    }
                    Type tType = dsIDTableType.Tables[0].Columns[0].DataType;
                    if (tType != typeof(uint))
                    {
                        throw new SystemException("Primary key type is not 'uint'");
                    }
                    else
                    {
                        m_sPrimaryKeyColumn = treeView2.SelectedNode.Parent.Text + "." + treeView2.SelectedNode.Text;
                        tablessTabControl1.SelectedIndex++;
                    }
                }
                catch (SystemException eException)
                {
                    MessageBox.Show("Error: " + eException.Message);
                }
            }
            else
            {
                MessageBox.Show("No tag selected to be primary key!");
            }
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            tablessTabControl1.SelectedIndex++;
        }

        private void btBack_Click(object sender, EventArgs e)
        {
            tablessTabControl1.SelectedIndex--;
        }
        
        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // If the selected node is NOT a parent
            if (e.Node.Nodes.Count == 0)
            {
                textBox2.Text = treeView2.SelectedNode.Text;
            }
            else
            {
                textBox2.Text = "";
            }
        }

        // Updates all child tree nodes recursively.
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // If the node has been changed to checked
            if (e.Node.Checked)
            {
                if (e.Node.Parent != null)
                {
                    if (!e.Node.Parent.Checked)
                    {
                        e.Node.Parent.Checked = true;
                    }
                }

                if (e.Node.Nodes.Count > 0)
                {
                    // Calls the CheckAllChildNodes method, passing in the current 
                    // Checked value of the TreeNode whose checked state changed.
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
            else // If the node has been changed to unchecked
            {
                if (e.Node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(e.Node, false);
                }
            }

            textBox1.Text = UpdateSelectStatement();
        }
    }

    /// <summary>
    /// http://stackoverflow.com/questions/13602824/c-sharp-multiple-screen-view-single-form
    /// </summary>
    public class TablessTabControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            // Hide tabs by trapping the TCM_ADJUSTRECT message
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }
    }

    public class AutoIncrementer
    {
        private uint m_uiValue;

        public AutoIncrementer()
        {
            m_uiValue = 0;
        }

        /// <summary>
        /// Gets the current index and increments the counter
        /// </summary>
        /// <returns>The current index value</returns>
        public uint Next()
        {
            uint uiValue = m_uiValue;
            m_uiValue++;
            return uiValue;
        }

        /// <summary>
        /// Starts the index back at zero again,
        /// can be dangerous, only use if sure.
        /// </summary>
        public void Reset()
        {
            m_uiValue = 0;
        }
    }

    /// <summary>
    /// http://stackoverflow.com/questions/14647216/c-sharp-treeview-ignore-double-click-only-at-checkbox
    /// tl;dr make double click event handle as two single click events
    /// </summary>
    public class MyTreeView : TreeView
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0203)
            {
                m.Msg = 0x0201;
            }
            base.WndProc(ref m);
        }
    }
}
