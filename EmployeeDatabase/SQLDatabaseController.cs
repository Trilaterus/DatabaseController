using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace EmployeeDatabase
{
    public partial class SQLDatabaseController : UserControl
    {
        // Database information variables
        private List<String> m_lsTableNames;
        private List<String> m_lsColumnNames;
        private String m_sConnectionInformation;
        public Tuple<String, String> m_sPrimaryKeyInformation; // Item1 is table and Item2 is columns
        private String m_sSelectStatement;
        private bool m_bNewRowSelected; // Since the new row changes itself when it gets unselected this bool tracks if it was really the last thing selected
        private Color m_ForeignHighlightColour = Color.PowderBlue;
        private Control m_SearchControl;

        // DatabaseController variables
        private DataSet m_dsDataset;
        private MySqlDataAdapter m_daDataAdapter;
        private List<Type> m_ltDatabaseTypes = new List<Type>();

        public SQLDatabaseController()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialises the SQLDatabaseController with necessary information.
        /// </summary>
        /// <param name="sConnectionInformation">The Server, Database, UserId and Password information needed to connect</param>
        /// <param name="sSelectStatement">The MySQL SELECT statement string that will be used to populate the grid</param>
        /// <param name="sPrimaryKeyInfo">A double Tuple of strings, the first is the table name of the Primary Key and the second is the column name. The Primary Key must be an unsigned int column</param>
        /// <param name="iSelectedIndex">The index id that should appear initially selected</param>
        public void Init(String sConnectionInformation, String sSelectStatement, Tuple<String, String> sPrimaryKeyInfo, int iSelectedIndex = 1)
        {
            m_SearchControl = new Control();
            this.Controls.Add(m_SearchControl);
            m_bNewRowSelected = false;
            m_sPrimaryKeyInformation = sPrimaryKeyInfo;
            m_sSelectStatement = sSelectStatement;
            ParseSelectStatement(sSelectStatement);
            SetConnection(sConnectionInformation);
            FillDataGrid(sSelectStatement);
            FillComboBox(m_lsColumnNames);
            dataGridView1.CurrentCell = dataGridView1.Rows[iSelectedIndex - 1].Cells[0];
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                int iValue = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                if (iValue == iSelectedIndex)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                }
            }
        }

        /// <summary>
        /// Fills the search combo box with a list of values
        /// </summary>
        private void FillComboBox(List<String> lsList)
        {
            foreach (String sString in lsList)
            {
                String sSubstring = sString.Substring(sString.IndexOf('.') + 1, sString.Length - sString.IndexOf('.') - 1);
                comboBox1.Items.Add(sSubstring);
            }

            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// Parse the passed SELECT statement to gather information about the referenced tables and columns.
        /// </summary>
        /// <param name="sSelectStatement">The SELECT statement String to parse into tables and columns</param>
        private void ParseSelectStatement(String sSelectStatement)
        {
            // The entire string between SELECT and FROM
            String sAllColumnNames = sSelectStatement.Substring(sSelectStatement.IndexOf("SELECT") + 6, sSelectStatement.IndexOf("FROM") - sSelectStatement.IndexOf("SELECT") - 6);
            sAllColumnNames = sAllColumnNames.Replace(" ", ""); // Remove all whitespace so that words are only split by commas
            bool bCommaLoop = true;
            int iPreviousCommaIndex = -1;
            List<String> lsColumnNames = new List<String>();
            List<String> lsTableNames = new List<String>();
            while (bCommaLoop)
            {
                // String.IndexOfAny returns -1 when it can't find the requested char anymore
                int iCurrentCommaIndex = sAllColumnNames.IndexOfAny(new char[] { ',' }, iPreviousCommaIndex + 1); // + 1 to hop over the previously found comma

                if (iCurrentCommaIndex > 0)
                {
                    String sColumnName = sAllColumnNames.Substring(iPreviousCommaIndex + 1, iCurrentCommaIndex - iPreviousCommaIndex - 1);
                    lsColumnNames.Add(sColumnName);

                    iPreviousCommaIndex = iCurrentCommaIndex;
                }
                else
                {
                    String sColumnName = sAllColumnNames.Substring(iPreviousCommaIndex + 1, sAllColumnNames.Length - iPreviousCommaIndex - 1);
                    lsColumnNames.Add(sColumnName);

                    bCommaLoop = false;
                }
            }

            // Column names retrieved from select statement and stored in lsColumnNames
            // Next need to get foreign key column names from INNER JOIN section
            // The entire string from FROM onwards

            String sForeignColumnNames = sSelectStatement.Substring(sSelectStatement.IndexOf("FROM") + 4, sSelectStatement.Length - sSelectStatement.IndexOf("FROM") - 4);

            bool bLoop = true;
            int iPreviousIndex = -1;
            while (bLoop)
            {
                int iCurrentIndex = sForeignColumnNames.IndexOfAny(new char[] { ' ', '=' }, iPreviousIndex + 1);

                if (iCurrentIndex >= 0)
                {
                    String sWord = sForeignColumnNames.Substring(iPreviousIndex + 1, iCurrentIndex - iPreviousIndex - 1);
                    if (sWord.Contains("_id"))
                    {
                        // Also record the table name to allow for foreign key referencing
                        String sTableName = sWord.Substring(sWord.IndexOf('.') + 1, sWord.IndexOf('_') - sWord.IndexOf('.') - 1);
                        lsTableNames.Add(sTableName);
                    }

                    iPreviousIndex = iCurrentIndex;
                }
                else
                {
                    /*
                    String sWord = sForeignColumnNames.Substring(iPreviousIndex + 1, sForeignColumnNames.Length - iPreviousIndex - 1);
                    if (sWord.Contains("_id"))
                    {
                        //lsColumnNames.Add(sWord);
                    }
                    */
                    bLoop = false;
                }
            }


            // Next is to get table names
            foreach (String sColumnName in lsColumnNames)
            {
                String sTableName = sColumnName.Substring(0, sColumnName.IndexOf('.'));
                if (!lsTableNames.Contains(sTableName))
                {
                    lsTableNames.Add(sTableName);
                }
            }

            // Copy generated names to member values
            m_lsColumnNames = lsColumnNames;
            m_lsTableNames = lsTableNames;
        }

        /// <summary>
        /// Set the connection string for the MySQLDatabase.
        /// </summary>
        /// <param name="sConnectionInformation">The Server, Database, UserId and Password information needed to connect</param>
        /// <returns>True if the connection can be established</returns>
        public bool SetConnection(String sConnectionInformation)
        {
            MySqlConnection sqlConnection = new MySqlConnection(sConnectionInformation);
            try
            {
                sqlConnection.Open();
            }
            catch
            {
                return false;
            }

            sqlConnection.Close();
            m_sConnectionInformation = sConnectionInformation;
            return true;
        }

        /// <summary>
        /// Fills the data grid with information based on an SQL select statement and the current connection,
        /// whilst also instantiating the controls based on the information loaded.
        /// </summary>
        /// <param name="sSelectStatement">The SQL select statement to run. It will also be saved if no other select statement has been made</param>
        /// <param name="sConnectionInformation">Leave blank if SetConnection() has been called or fill in to connect to a different database</param>
        /// <returns>True if the connection and statement were valid and executed properly</returns>
        public bool FillDataGrid(String sSelectStatement, String sConnectionInformation = "")
        {
            try
            {
                m_dsDataset = new DataSet();
                //dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

                if (sConnectionInformation != "")
                {
                    m_sConnectionInformation = sConnectionInformation;
                }
                if (m_sSelectStatement == "")
                {
                    m_sSelectStatement = sSelectStatement;
                }

                using (MySqlConnection cnConnection = new MySqlConnection(m_sConnectionInformation))
                {
                    cnConnection.Open();
                    m_dsDataset.Clear();
                    m_daDataAdapter = new MySqlDataAdapter(sSelectStatement, cnConnection);
                    m_daDataAdapter.Fill(m_dsDataset);
                    dataGridView1.DataSource = m_dsDataset.Tables[0];

                    // For each column add a label and specialised control in the floating panel
                    for (int i = 0; i < dataGridView1.ColumnCount; ++i)
                    {
                        m_ltDatabaseTypes.Add(dataGridView1.Columns[i].ValueType);

                        Label lbLabel = new Label();
                        lbLabel.Parent = flowLayoutPanel1;
                        lbLabel.Text = dataGridView1.Columns[i].Name;

                        Control ctrl = new Control();
                        ctrl.Parent = flowLayoutPanel1;

                        if (m_ltDatabaseTypes.Last() == typeof(String))
                        {
                            ctrl = new TextBox();
                        }
                        else if (m_ltDatabaseTypes.Last() == typeof(DateTime))
                        {
                            ctrl = new DateTimePicker();
                        }
                        else if (m_ltDatabaseTypes.Last() == typeof(uint) ||
                                 m_ltDatabaseTypes.Last() == typeof(int))
                        {
                            ctrl = new NumericTextBox();
                        }
                        else
                        {
                            // Add no specialised control for unsupported column
                        }

                        ctrl.Width = flowLayoutPanel1.Width - 30;

                        flowLayoutPanel1.Controls.Add(lbLabel);
                        flowLayoutPanel1.Controls.Add(ctrl);
                    }
                }

                // Set the first control (the primary key control) to read only/uneditable
                // Since it has to be an unsigned int we can bind it to a numeric textbox
                NumericTextBox ntbPrimaryKey = (NumericTextBox)flowLayoutPanel1.Controls[2];
                ntbPrimaryKey.ReadOnly = true;

                // Freeze the primary key column
                dataGridView1.Columns[0].Frozen = true;

                // Append a delete button column
                DataGridViewButtonColumn ButtonColumn = new DataGridViewButtonColumn();
                dataGridView1.Columns.Insert(dataGridView1.Columns.Count, ButtonColumn);
                ButtonColumn.HeaderText = "Delete";
                ButtonColumn.Text = "X";
                ButtonColumn.Name = "btn";
                ButtonColumn.UseColumnTextForButtonValue = true;

                // Change the colour of the foreign id columns
                foreach (String sColumnName in m_lsColumnNames)
                {
                    String sTableName = sColumnName.Substring(0, sColumnName.IndexOf('.'));

                    if (sColumnName.EndsWith("_id"))
                    {
                        // Get the "foreign" out of "table.foreign_id"
                        sTableName = sColumnName.Substring(sColumnName.IndexOf('.') + 1, sColumnName.IndexOf('_') - sColumnName.IndexOf('.') - 1);
                        if (m_lsTableNames.Contains(sTableName))
                        {
                            // First parse the right name of the column
                            String sColumnDisplayName = sColumnName.Substring(sColumnName.IndexOf('.') + 1, sColumnName.Length - sColumnName.IndexOf('.') - 1);
                            // Set the colour of that column
                            dataGridView1.Columns[sColumnDisplayName].DefaultCellStyle.BackColor = m_ForeignHighlightColour;
                        }
                    }
                }
            }
            catch (SystemException seError)
            {
                MessageBox.Show("Error: " + seError.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update the control panel controllers values based on the cell that has been clicked.
        /// </summary>
        private void DataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            // If the clear 'new' row is selected make all the controllers empty
            if (dataGridView1.CurrentRow.IsNewRow)
            {
                m_bNewRowSelected = true;
                for (int i = 0; i < m_ltDatabaseTypes.Count; ++i)
                {
                    if (m_ltDatabaseTypes[i] == typeof(String))
                    {
                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = "";
                    }
                    else if (m_ltDatabaseTypes[i] == typeof(DateTime))
                    {
                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = DateTime.Today.ToShortDateString();
                    }
                    else if (m_ltDatabaseTypes[i] == typeof(int))
                    {
                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = "";
                    }
                    else if (m_ltDatabaseTypes[i] == typeof(uint))
                    {
                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = "";
                    }
                    else
                    {
                        // If the type cannot be identified do not put anything in the editor tool
                    }
                }
            }
            else
            {
                m_bNewRowSelected = false;
                try
                {
                    uint uiIndexId = (uint)dataGridView1.CurrentRow.Cells[1].Value; // Bug that button column repositions itself to the start fo the column array

                    using (MySqlConnection cnConnection = new MySqlConnection(m_sConnectionInformation))
                    {
                        cnConnection.Open();
                        String sComSelectRow = m_sSelectStatement + " WHERE " + m_sPrimaryKeyInformation.Item1 + '.' + m_sPrimaryKeyInformation.Item2 + " = " + uiIndexId.ToString();
                        MySqlCommand cmdSelectRow = new MySqlCommand(sComSelectRow, cnConnection);
                        MySqlDataReader rdrDataReader = cmdSelectRow.ExecuteReader();

                        while (rdrDataReader.Read())
                        {
                            for (int i = 0; i < m_ltDatabaseTypes.Count; ++i)
                            {
                                // If the reader does not read a null cell
                                if (!rdrDataReader.IsDBNull(i))
                                {
                                    if (m_ltDatabaseTypes[i] == typeof(String))
                                    {
                                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = rdrDataReader.GetString(i);
                                    }
                                    else if (m_ltDatabaseTypes[i] == typeof(DateTime))
                                    {
                                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = rdrDataReader.GetDateTime(i).ToShortDateString();
                                    }
                                    else if (m_ltDatabaseTypes[i] == typeof(int))
                                    {
                                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = rdrDataReader.GetInt16(i).ToString();
                                    }
                                    else if (m_ltDatabaseTypes[i] == typeof(uint))
                                    {
                                        flowLayoutPanel1.Controls[(i * 3) + 2].Text = rdrDataReader.GetUInt16(i).ToString();
                                    }
                                    else
                                    {
                                        // If the type cannot be identified do not put anything in the editor tool
                                    }
                                }
                                else
                                {
                                    // If it is a null cell, fill the control with an empty string
                                    flowLayoutPanel1.Controls[(i * 3) + 2].Text = "";
                                }
                            }
                        }
                    }
                }
                catch (SystemException seError)
                {
                    MessageBox.Show("Error: " + seError.Message);
                }
            }
        }

        /// <summary>
        /// Handling for when a cells content is clicked within the datagridview,
        /// in this case it's handling for the delete button column.
        /// </summary>
        private void DataGridViewButtonClick(object sender, DataGridViewCellEventArgs e)
        {
            var sendergrid = (DataGridView)sender;

            if (sendergrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                // Triggers when the selected button is the new row button
                if (dataGridView1.CurrentRow.Cells[1].Value == null ||
                    dataGridView1.CurrentRow.Cells[1].Value.ToString() == "") // Since the button value text should equal 'X' anyway
                {
                    return;
                }
                uint uiIndexId = (uint)dataGridView1.CurrentRow.Cells[1].Value;
                // Ask whether the user still wants to delete the row
                DialogResult dialogResult = MessageBox.Show("Are you sure you would like to delete row " + uiIndexId + "?\n\nThis change is irreversible.", "Delete row", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        // Delete the row of the pressed button
                        using (MySqlConnection cnConnection = new MySqlConnection(m_sConnectionInformation))
                        {
                            cnConnection.Open();
                            String sComDeleteRow = "DELETE FROM " + m_sPrimaryKeyInformation.Item1 + " WHERE " + m_sPrimaryKeyInformation.Item1 + "." + m_sPrimaryKeyInformation.Item2 + " = " + uiIndexId;
                            MySqlCommand cmdDeleteRow = new MySqlCommand(sComDeleteRow, cnConnection);

                            int iRowsAffected = cmdDeleteRow.ExecuteNonQuery();

                            m_dsDataset.Clear();
                            m_daDataAdapter.Fill(m_dsDataset);
                            dataGridView1.DataSource = m_dsDataset.Tables[0];
                            MessageBox.Show("Row " + uiIndexId + " deleted");
                        }
                    }
                    catch (SystemException seError)
                    {
                        if (seError.GetType() == typeof(MySqlException))
                        {
                            MySqlException sqlError = (MySqlException)seError;
                            if (sqlError.Number == 1451) // Deleting a foreign key item that is referenced elsewhere
                            {
                                MessageBox.Show("SQL Error: This item cannot be deleted.\nIt is being referenced by another entry.\n\nChange or delete the entry before trying again.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: " + seError.Message);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Generate an update statement and execute it if there are no issues.
        /// If the current cell is within the new row section then create
        /// a new row with the filled in sections.
        /// </summary>
        private void SubmitButtonOnClick(object sender, EventArgs e)
        {
            MySqlCommand cmdUpdate = new MySqlCommand();
            if (m_bNewRowSelected)
            {
                // Insert rather than update
                try
                {
                    // Do the new push an empty one on and update that method
                    String sComInsertBlank = "INSERT INTO " + m_sPrimaryKeyInformation.Item1 + " () VALUES ();";

                    using (MySqlConnection cnConnection = new MySqlConnection(m_sConnectionInformation))
                    {
                        cnConnection.Open();
                        cmdUpdate = new MySqlCommand(sComInsertBlank, cnConnection);

                        int iRowsAffected = cmdUpdate.ExecuteNonQuery();

                        m_dsDataset.Clear();
                        m_daDataAdapter.Fill(m_dsDataset);
                        dataGridView1.DataSource = m_dsDataset.Tables[0];
                    }
                }
                catch (SystemException seError)
                {
                    MessageBox.Show("Error: " + seError.Message);
                }
            }
            try
            {
                String sComUpdate = "UPDATE ";
                sComUpdate += m_sPrimaryKeyInformation.Item1;

                // Push on column names
                sComUpdate += " SET ";
                bool bAnyEmptyControls = false;
                for (int i = 1; i < m_ltDatabaseTypes.Count; ++i) // Start at one to avoid updating indexes
                {
                    if (String.IsNullOrEmpty(flowLayoutPanel1.Controls[(i * 3) + 2].Text))
                    {
                        sComUpdate += m_lsColumnNames[i] + " = null, ";
                        bAnyEmptyControls = true;
                    }
                    else
                    {
                        if (m_ltDatabaseTypes[i] == typeof(String))
                        {
                            sComUpdate += m_lsColumnNames[i] + " = '" + flowLayoutPanel1.Controls[(i * 3) + 2].Text + "', ";
                        }
                        else if (m_ltDatabaseTypes[i] == typeof(DateTime))
                        {
                            DateTime dt = DateTime.ParseExact(flowLayoutPanel1.Controls[(i * 3) + 2].Text, "dd MMMM yyyy", CultureInfo.InvariantCulture);
                            String sDt = dt.ToString("yyyy-MM-dd");
                            sComUpdate += m_lsColumnNames[i] + " = '" + sDt + "', ";
                        }
                        else if (m_ltDatabaseTypes[i] == typeof(int) ||
                                 m_ltDatabaseTypes[i] == typeof(uint))
                        {
                            sComUpdate += m_lsColumnNames[i] + " = '" + flowLayoutPanel1.Controls[(i * 3) + 2].Text + "', ";
                        }
                    }
                }

                if (bAnyEmptyControls)
                {
                    DialogResult dialogResult = MessageBox.Show("Some parts of the editor were not filled in.\nFailing to fill in the entire editor could lead to entries not appearing.\n\nWould you like to submit anyway?", "Submit entry", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }

                // Trim final comma
                sComUpdate = sComUpdate.Substring(0, sComUpdate.Length - 2);

                // Push on Index WHERE statement (and other foreign key links)
                uint uiIndexId = (uint)dataGridView1.CurrentRow.Cells[1].Value; // Will only work if the index is in the first row (which it should always be as that column is frozen)
                if (m_bNewRowSelected)
                {
                    sComUpdate += " WHERE " + m_sPrimaryKeyInformation.Item1 + "." + m_sPrimaryKeyInformation.Item2 + " = " + cmdUpdate.LastInsertedId;
                }
                else
                {
                    sComUpdate += " WHERE " + m_sPrimaryKeyInformation.Item1 + "." + m_sPrimaryKeyInformation.Item2 + " = " + uiIndexId;
                }

                using (MySqlConnection cnConnection = new MySqlConnection(m_sConnectionInformation))
                {
                    cnConnection.Open();
                    cmdUpdate = new MySqlCommand(sComUpdate, cnConnection);

                    int iRowsAffected = cmdUpdate.ExecuteNonQuery();

                    if (m_bNewRowSelected)
                    {
                        MySqlCommand cmdSelectAgain = new MySqlCommand(m_sSelectStatement, cnConnection);
                        cmdSelectAgain.ExecuteNonQuery();
                        m_dsDataset.Clear();
                        m_daDataAdapter.Fill(m_dsDataset);
                        dataGridView1.DataSource = m_dsDataset.Tables[0];
                        MessageBox.Show("New row added!");
                    }
                    else
                    {
                        m_dsDataset.Clear();
                        m_daDataAdapter.Fill(m_dsDataset);
                        dataGridView1.DataSource = m_dsDataset.Tables[0];

                        MessageBox.Show("Changes submitted to row with index: " + uiIndexId.ToString());
                    }
                }
            }
            catch (SystemException seError)
            {
                MessageBox.Show("Error: " + seError.Message);
            }
        }

        /// <summary>
        /// If a cell is double clicked, check if the cell is a foreign key,
        /// if it is open a modal window with another database controller
        /// with the initial highlighted row to be the same as the id of the
        /// cell that was clicked.
        /// </summary>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Checking whether the cell is a foreign key is simplified by just checking for colour
            if (dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.BackColor == m_ForeignHighlightColour)
            {
                String sColumnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
                String sTableName = sColumnName.Substring(0, sColumnName.IndexOf("_id"));
                List<String> lsColumnNames = new List<String>();

                DataTable dsDataTable = null;
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(m_sConnectionInformation))
                {
                    using (var schemaCommand = new MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM " + sTableName, con))
                    {
                        con.Open();
                        using (var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly))
                        {
                            dsDataTable = reader.GetSchemaTable();
                        }
                    }
                }
                String sAllColumnNames = "";
                foreach (DataRow col in dsDataTable.Rows)
                {
                    sAllColumnNames += sTableName + '.' + col.Field<String>("ColumnName") + ", ";
                }
                sAllColumnNames = sAllColumnNames.Substring(0, sAllColumnNames.Length - 2);

                var form = new Form1();
                form.m_sSelectStatement = "SELECT " + sAllColumnNames + " FROM " + sTableName;
                form.m_sPrimaryColumn = new Tuple<String, String>(sTableName, "id");
                form.m_iSelectedRow = Convert.ToInt32(dataGridView1.CurrentCell.Value);
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Handling for when the searching combo box's value has been changed.
        /// Mostly updating the search controller to match the column type selected.
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear previous control
            m_SearchControl.Dispose();

            // Assign new control tool based on column type
            if (m_ltDatabaseTypes[comboBox1.SelectedIndex] == typeof(String))
            {
                m_SearchControl = new TextBox();
            }
            else if (m_ltDatabaseTypes[comboBox1.SelectedIndex] == typeof(DateTime))
            {
                m_SearchControl = new DateTimePicker();
            }
            else if (m_ltDatabaseTypes[comboBox1.SelectedIndex] == typeof(int)
                || m_ltDatabaseTypes[comboBox1.SelectedIndex] == typeof(uint))
            {
                m_SearchControl = new NumericTextBox();
            }
            else
            {
                // Don't show a controller for an unsupported type
                m_SearchControl = new Control();
            }

            // Set the location to be on the right of the combo box with the correct width and anchoring
            m_SearchControl.Location = new Point(comboBox1.Location.X + comboBox1.Width + 10, comboBox1.Location.Y);
            m_SearchControl.Width = btSearch.Location.X - m_SearchControl.Location.X - 10;
            m_SearchControl.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
            m_SearchControl.KeyUp += SearchControlOnEnterPressed;

            this.Controls.Add(m_SearchControl);
        }

        /// <summary>
        /// Create and execute a SELECT statement based on the column selected and the value submitted.
        /// </summary>
        private void btSearch_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0)
            {
                String sColumnName = Convert.ToString(comboBox1.SelectedItem);
                String sSearchValue = m_SearchControl.Text;
                if (sSearchValue != "")
                {
                    llbClearSearch.Visible = true;
                }

                // Add handling for types that need specific parsing
                if (m_ltDatabaseTypes[comboBox1.SelectedIndex] == typeof(DateTime))
                {
                    DateTime dt = DateTime.ParseExact(m_SearchControl.Text, "dd MMMM yyyy", CultureInfo.InvariantCulture);
                    sSearchValue = dt.ToString("yyyy-MM-dd");
                }

                String sColumnNamesConcat = "";
                foreach (String sColName in m_lsColumnNames)
                {
                    sColumnNamesConcat += sColName + ", ";
                }
                sColumnNamesConcat = sColumnNamesConcat.Substring(0, sColumnNamesConcat.Length - 2);

                // This search statement returns entries that 'contain' the data searched for.
                String sSearchStatement = "SELECT " + sColumnNamesConcat + " FROM " + m_sPrimaryKeyInformation.Item1 + " WHERE " + sColumnName + " LIKE '%" + sSearchValue + "%';";

                using (MySqlConnection cnConnection = new MySqlConnection(m_sConnectionInformation))
                {
                    cnConnection.Open();
                    m_dsDataset.Clear();
                    m_daDataAdapter = new MySqlDataAdapter(sSearchStatement, cnConnection);
                    m_daDataAdapter.Fill(m_dsDataset);
                    dataGridView1.DataSource = m_dsDataset.Tables[0];
                }
            }

        }

        /// <summary>
        /// Clears the SELECT statement and search column. It also returns the datagridview back to the
        /// original SELECT statment requested.
        /// </summary>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llbClearSearch.Visible = false;
            comboBox1.SelectedIndex = 0;
            m_SearchControl.Text = "";

            using (MySqlConnection cnConnection = new MySqlConnection(m_sConnectionInformation))
            {
                cnConnection.Open();
                m_dsDataset.Clear();
                m_daDataAdapter = new MySqlDataAdapter(m_sSelectStatement, cnConnection);
                m_daDataAdapter.Fill(m_dsDataset);
                dataGridView1.DataSource = m_dsDataset.Tables[0];
            }
        }

        /// <summary>
        /// Update and execute the search SELECT statement whenever a key is pressed.
        /// </summary>
        private void SearchControlOnEnterPressed(object sender, KeyEventArgs e)
        {
            btSearch_Click(this, new EventArgs());
        }
    }

    /// <summary>
    /// Modified version of MSDNs provided NumericTextBox class: https://msdn.microsoft.com/en-gb/library/ms229644(v=vs.100).aspx
    /// Modifications include:
    /// - Changing input recognition to only allow one decimal seperator
    /// </summary>
    public class NumericTextBox : TextBox
    {
        bool allowSpace = false;

        // Restricts the entry of characters to digits (including hex), the negative sign,
        // the decimal point, and editing keystrokes (backspace).
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            string currentInput = this.Text;

            if (Char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (keyInput.Equals(groupSeparator) || keyInput.Equals(negativeSign))
            {
                // Negative and large number (like commas) separators are OK
            }
            else if (keyInput.Equals(decimalSeparator) && !currentInput.Contains(decimalSeparator))
            {
                // Allow 1 decimal point seperator
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            //    {
            //     // Let the edit control handle control and alt key combinations
            //    }
            else if (this.allowSpace && e.KeyChar == ' ')
            {

            }
            else
            {
                // Consume this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }

        public int IntValue
        {
            get
            {
                return Int32.Parse(this.Text);
            }
        }

        public decimal DecimalValue
        {
            get
            {
                return Decimal.Parse(this.Text);
            }
        }

        public bool AllowSpace
        {
            set
            {
                this.allowSpace = value;
            }

            get
            {
                return this.allowSpace;
            }
        }
    }
}
