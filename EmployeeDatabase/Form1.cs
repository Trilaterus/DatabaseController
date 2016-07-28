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
using System.Globalization;

namespace EmployeeDatabase
{
    public partial class Form1 : Form
    {
        public String m_sConnection = "Server=localhost;Database=people;Uid=root;Pwd=12345yes;";
        public String m_sSelectStatement = "SELECT person.id, person.name, person.address_id, person.employment_id, person.telephone_id, person.dob, person.national_insurance FROM  person INNER JOIN address ON person.address_id = address.id INNER JOIN telephone ON person.telephone_id = telephone.id INNER JOIN employment ON person.employment_id = employment.id";
        public Tuple<String, String> m_sPrimaryColumn = new Tuple<String, String>("person", "id");
        public int m_iSelectedRow = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void sqlDatabaseController1_Load(object sender, EventArgs e)
        {
            sqlDatabaseController1.Init(m_sConnection, m_sSelectStatement, m_sPrimaryColumn, m_iSelectedRow);
        }
    }
}
