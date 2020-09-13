using System;
using System.Windows.Data;
using System.Globalization;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;
using WorkScheduleImporter.AddIn.Business.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Views.Converters
{
    public class ValidationLevelImageConverter : IValueConverter
    {
        private const string DELETE_IMAGE_PATH = "/WorkScheduleImporter.AddIn.Views;component/Images/Delete.png";
        private const string WARNING_IMAGE_PATH = "/WorkScheduleImporter.AddIn.Views;component/Images/Warning.png";
        private const string VALIDATED_IMAGE_PATH = "/WorkScheduleImporter.AddIn.Views;component/Images/Check.png";
        private const string DEFAULT_IMAGE_PATH = "/WorkScheduleImporter.AddIn.Views;component/Images/Warning.png";


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReportValidationItemModel validationItem = (ReportValidationItemModel)value;

            ValidationLevelType level = ReportValidationBusiness.ValidationTypeByMessage[validationItem.WorkScheduleValidationType].Level;

            switch (level)
            {
                case ValidationLevelType.ERROR:
                    return DELETE_IMAGE_PATH;
                case ValidationLevelType.WARNING:
                    return WARNING_IMAGE_PATH;
                case ValidationLevelType.VALIDATED:
                    return VALIDATED_IMAGE_PATH;
                default:
                    return DEFAULT_IMAGE_PATH;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
