using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduleImporter.AddIn.Data.Provider.Interface;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Data.Provider
{
    public class OleDbExcelDataProvider : IExcelDataProvider
    {
        public const string EXCEL_CONNECTION_CFG_PROVIDER_KEY = "Provider";
        public const string EXCEL_CONNECTION_CFG_EXTENDED_PROPERTIES_KEY = "Extended Properties";
        public const string EXCEL_CONNECTION_CFG_DATA_SOURCE_KEY = "Data Source";
        public const string EXCEL_CONNECTION_CFG_PASSWORD_KEY = "Jet OLEDB:Database Password";
        public const string EXCEL_CONNECTION_CFG_MODE_KEY = "Mode";
        public const string OLE_EXCEL_TABLE_NAME_SCHEMA_KEY = "TABLE_NAME";

        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        string _fullPathFile;
        string _sheetPassword;


        public OleDbExcelDataProvider(string fullPathFile, string sheetPassword) : base(fullPathFile, sheetPassword)
        {
            this.ConnectionString = this.GetConnectionString();
        }

        public override DataTable ReadSheet(string sheetName)
        {
            DataSet ds = new DataSet();
            bool wasSheetFound = false;
            
            using (OleDbConnection conn = new OleDbConnection(this.ConnectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow dr in dtSheet.Rows)
                {
                    string currentSheetName = dr[OLE_EXCEL_TABLE_NAME_SCHEMA_KEY].ToString();

                    if ((currentSheetName ?? String.Empty).ToUpper() != (sheetName + "$" ?? String.Empty).ToUpper())
                        continue;

                    wasSheetFound = true;

                    cmd.CommandText = String.Format("SELECT * FROM [{0}]",currentSheetName);

                    DataTable dt = new DataTable();
                    dt.TableName = currentSheetName.Substring(0, currentSheetName.Length - 1);

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    ds.Tables.Add(dt);

                    break;
                }

                cmd = null;
                conn.Close();
            }

            if (!wasSheetFound)
            {
                throw new SheetNotFoundException();
            }
            
            return ds.Tables[sheetName];
        }



        private string GetConnectionString()
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            props[EXCEL_CONNECTION_CFG_PROVIDER_KEY] = Properties.Settings.Default.EXCEL_PROVIDER ;
            props[EXCEL_CONNECTION_CFG_EXTENDED_PROPERTIES_KEY] = Properties.Settings.Default.EXCEL_EXTENDED_PROPERTIES;
            props[EXCEL_CONNECTION_CFG_DATA_SOURCE_KEY] = this.FullPathFile;
            if (!String.IsNullOrEmpty(this.SheetPassword))
                props[EXCEL_CONNECTION_CFG_EXTENDED_PROPERTIES_KEY] = String.Format("'{0}'", this.SheetPassword);
            props[EXCEL_CONNECTION_CFG_MODE_KEY] = Properties.Settings.Default.EXCEL_MODE;

            return GetConnectionString(props);
        }

        public static string GetConnectionString(Dictionary<string, string> props)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }

        public override void WriteSheet(string sheetName, DataTable dt)
        {
            int columnsQuantity;

            using (OleDbConnection conn = new OleDbConnection(this.ConnectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                columnsQuantity = dt.Columns.Count;
                string currentSheetName = dt.TableName;

                foreach (DataRow dr in dt.Rows)
                {
                    List<string> columNamesList = new List<string>();
                    List<string> valuesList = new List<string>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        columNamesList.Add(dc.ColumnName);
                        valuesList.Add(String.Format("'{0}'", dr[dc].ToString()));
                    }
                    
                    cmd.CommandText = String.Format("INSERT INTO [{0}$]({1}) VALUES({2});",
                                                    sheetName, String.Join(",", columNamesList), String.Join(",", valuesList));
                    cmd.ExecuteNonQuery();

                }
                cmd = null;
                conn.Close();
            }
        }
    }
}
