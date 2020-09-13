using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Data.Provider.Interface
{
    public abstract class IExcelDataProvider
    {
        string _fullPathFile;

        public string FullPathFile
        {
            get { return _fullPathFile; }
            set { _fullPathFile = value; }
        }
        string _sheetPassword;

        public string SheetPassword
        {
            get { return _sheetPassword; }
            set { _sheetPassword = value; }
        }

        public IExcelDataProvider(string fullPathFile, string sheetPassword)
        {
            FullPathFile = fullPathFile;
            SheetPassword = sheetPassword;
        }

        public abstract DataTable ReadSheet(string sheetName);
        public abstract void WriteSheet(string sheetName, DataTable dt);
    }
}
