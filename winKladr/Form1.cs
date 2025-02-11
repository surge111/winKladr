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
        static string pathDb = Directory.GetCurrentDirectory() + @"\db\kladr.db";
        string connectionStr = $@"Data Source={pathDb};Version=3;Read Only=True;";
        DataTable dtRegion;
        DataTable dtDistrict;
        DataTable dtCity;
        int MaxDropDownItems = 10;
        string RegionCode;
        string DistrictCode;
        string FullCode;

        public Form1()
        {
            InitializeComponent();

            try
            {
                SQLiteConnection con = new SQLiteConnection(connectionStr);
                con.Open();

                SQLiteCommand cmd = new SQLiteCommand(con);
                cmd.CommandText = $@"SELECT * FROM region";

                SQLiteDataReader rdr = cmd.ExecuteReader();
                dtRegion = new DataTable();

                dtRegion.Load(rdr);

                cbRegion.DisplayMember = "name"; // !!!! здесь указываем тот аттрибут (или синоним) который мы прописли в SQL запросе
                cbRegion.DataSource = dtRegion;
                cbRegion.ValueMember = "code";

                // состояние в котором ничего не выбрано
                cbRegion.SelectedIndex = -1;

                /*
                 * When this property is set to true, the control automatically resizes to ensure that an item is not partially displayed. If you want to maintain the original size of the ComboBox based on the space requirements of your form, set this property to false.
                 */
                cbRegion.MaxDropDownItems = MaxDropDownItems;
                cbRegion.IntegralHeight = false;  //

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRegion.SelectedIndex == -1) return;

            RegionCode = cbRegion.SelectedValue.ToString();

            try
            {
                SQLiteConnection con = new SQLiteConnection(connectionStr);
                con.Open();

                SQLiteCommand cmd = new SQLiteCommand(con);
                cmd.CommandText = $@"SELECT concat_ws (' ', name, socr) as name, code FROM kladr 
                                     WHERE code LIKE '{RegionCode}___00000000' AND NOT code='{RegionCode}00000000000';";

                SQLiteDataReader rdr = cmd.ExecuteReader();
                dtDistrict = new DataTable();

                dtDistrict.Load(rdr);

                cbDistrict.DisplayMember = "name"; // !!!! здесь указываем тот аттрибут (или синоним) который мы прописли в SQL запросе
                cbDistrict.DataSource = dtDistrict;
                cbDistrict.ValueMember = "code";

                // состояние в котором ничего не выбрано
                cbDistrict.SelectedIndex = -1;

                /*
                 * When this property is set to true, the control automatically resizes to ensure that an item is not partially displayed. If you want to maintain the original size of the ComboBox based on the space requirements of your form, set this property to false.
                 */
                cbDistrict.MaxDropDownItems = MaxDropDownItems;
                cbDistrict.IntegralHeight = false;  //

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDistrict.SelectedIndex == -1) return;

            DistrictCode = cbDistrict.SelectedValue.ToString().Substring(2,3);

            try
            {
                SQLiteConnection con = new SQLiteConnection(connectionStr);
                con.Open();

                SQLiteCommand cmd = new SQLiteCommand(con);
                cmd.CommandText = $@"SELECT concat_ws (' ', socr, name) as name, code, `index` FROM kladr 
                                     WHERE code LIKE '{RegionCode}{DistrictCode}%00' 
                                           AND NOT 
                                           code='{RegionCode}{DistrictCode}00000000' ORDER BY name;";

                SQLiteDataReader rdr = cmd.ExecuteReader();
                dtCity = new DataTable();

                dtCity.Load(rdr);

                cbCity.DisplayMember = "name"; // !!!! здесь указываем тот аттрибут (или синоним) который мы прописли в SQL запросе
                cbCity.DataSource = dtCity;
                cbCity.ValueMember = "code";

                // состояние в котором ничего не выбрано
                cbCity.SelectedIndex = -1;

                /*
                 * When this property is set to true, the control automatically resizes to ensure that an item is not partially displayed. If you want to maintain the original size of the ComboBox based on the space requirements of your form, set this property to false.
                 */
                cbCity.MaxDropDownItems = MaxDropDownItems;
                cbCity.IntegralHeight = false;  //

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cbCity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FullCode = cbCity.SelectedValue.ToString();
            
            DataRow[] r = dtCity.Select($"code = {FullCode}");

            string path = $"{r[0].ItemArray[2].ToString()}, {cbRegion.Text}, {cbDistrict.Text}, {r[0].ItemArray[0].ToString()}";

            FullAddress.Text = path;
        }
    }
}
