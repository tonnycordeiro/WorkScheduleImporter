using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule
{
    public class ReportValidationItemModel
    {
        private WorkScheduleValidationType _workScheduleValidationType;
        private string _dataAffected;
        private List<string> _workScheduleIds;

        public ReportValidationItemModel()
        {
            this._workScheduleIds = new List<string>();
        }

        public ReportValidationItemModel(WorkScheduleValidationType workScheduleValidationType, string dataAffected)
        {
            this._workScheduleIds = new List<string>();
            this._workScheduleValidationType = workScheduleValidationType;
            this._dataAffected = dataAffected;
        }

        public ReportValidationItemModel(WorkScheduleValidationType workScheduleValidationType, string dataAffected, List<string> workScheduleIds)
        {
            this._workScheduleValidationType = workScheduleValidationType;
            this._dataAffected = dataAffected;
            this._workScheduleIds = workScheduleIds;
        }

        public WorkScheduleValidationType WorkScheduleValidationType
        {
            get { return _workScheduleValidationType; }
            set { _workScheduleValidationType = value; }
        }

        public string DataAffected
        {
            get { return _dataAffected; }
            set { _dataAffected = value; }
        }

        public List<string> WorkScheduleIds
        {
            get { return _workScheduleIds; }
            set { _workScheduleIds = value; }
        }

        public override bool Equals(object obj)
        {
            var model = obj as ReportValidationItemModel;
            return model != null &&
                   _workScheduleValidationType == model._workScheduleValidationType &&
                   _dataAffected == model._dataAffected;
        }

        public bool MessageEquals(ReportValidationItemModel model)
        {
            return model != null &&
                   _workScheduleValidationType == model._workScheduleValidationType &&
                   _dataAffected == model._dataAffected;
        }

        public override int GetHashCode()
        {
            var hashCode = 1360223210;
            hashCode = hashCode * -1521134295 + _workScheduleValidationType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_dataAffected);
            return hashCode;
        }

    }
}
