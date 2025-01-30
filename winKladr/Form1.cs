using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SQLite;
using System.IO;

// Sqlite dll взял на платформу x64, по этому необходимо настроить проект
// Project Properties -> Build -> Platform target -> x64

namespace winKladr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string pathDb = Directory.GetCurrentDirectory() + @"\db\kladr.db";
            string connectionStr = $@"Data Source={pathDb};Version=3;Read Only=True;";

            SQLiteConnection con = new SQLiteConnection(connectionStr);
            con.Open();


            con.Close();
        }
    }
}
