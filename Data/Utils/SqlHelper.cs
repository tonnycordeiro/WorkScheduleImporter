using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Data.Utils
{
    public static class SqlHelper
    {
        public const string UNIVERSAL_TIME_TS = "UT";
        public static bool GetBoolean(string columnValue)
        {
            return columnValue == Properties.Settings.Default.DB_TRUE_VALUE ? true : false;
        }

        public static string BooleanToString(bool value)
        {
            return value ? Properties.Settings.Default.DB_TRUE_VALUE : Properties.Settings.Default.DB_FALSE_VALUE;
        }

        public static DateTime GetDateTime(string dts)
        {
            return DateTime.ParseExact(dts.Substring(0, 14), Properties.Settings.Default.DB_DATE_TIME_STAMP_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime GetUniversalDateTime(string dts)
        {
            return DateTime.ParseExact(dts.Substring(0, 14), Properties.Settings.Default.DB_DATE_TIME_STAMP_FORMAT, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public static int GetInt(string number)
        {
            return Int32.Parse(String.IsNullOrEmpty(number) ? "0" : number);
        }

        public static string GetDateTimeStamp(DateTime? dateTime)
        {
            if (dateTime.HasValue)
                return GetDateTimeStamp(dateTime.Value);
            else
                return String.Empty;
        }


        public static string GetDateTimeStamp(DateTime dateTime)
        {
            return String.Format("{0}{1}", dateTime.ToString(Properties.Settings.Default.DB_DATE_TIME_STAMP_FORMAT), UNIVERSAL_TIME_TS);
        }

        public static string GetUtcNowDateTimeStamp()
        {
            return String.Format("{0}{1}", DateTime.UtcNow.ToString(Properties.Settings.Default.DB_DATE_TIME_STAMP_FORMAT), UNIVERSAL_TIME_TS);
        }

        public static void AddParameter(this IDbCommand command, string parameterName, DbType dbType, Object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = String.Format("@{0}", parameterName.Replace("@", ""));
            parameter.Value = value;
            parameter.DbType = dbType;

            command.Parameters.Add(parameter);
        }

    }
}
