using System;
using System.Windows.Data;
using System.Globalization;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;
using WorkScheduleImporter.AddIn.Business.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Views.Converters
{
    public class ReportValidationToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReportValidationItemModel validationItem = (ReportValidationItemModel)value;

            string message = ReportValidationBusiness.ValidationTypeByMessage[validationItem.WorkScheduleValidationType].Message;

            return (message != null) ? message : String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
