using System;
using System.Collections.Generic;
using System.Linq;
using Government.EmergencyDepartment.AddIn.Models;
using Government.EmergencyDepartment.AddIn.Models.WorkSchedule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkScheduleImporter.AddIn.Models;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;

namespace UnitTest
{
    [TestClass]
    public class CheckingWorkScheduleReportValidations
    {
        private const int DIURN_HOUR = 7;
        private const int NOTURN_HOUR = 19;
        private const string VALID_SHIFT_DATE_VALUE1 = "33433";
        private const string VALID_SHIFT_DATE_VALUE2 = "33434";

        private List<WorkShiftModel> availableWorkShiftList = new List<WorkShiftModel>()
            {new WorkShiftModel(DIURN_HOUR, 0, NOTURN_HOUR, 0), new WorkShiftModel(NOTURN_HOUR, 0, DIURN_HOUR, 0) };

        [TestMethod]
        public void IsCheckingRepeatedEmployeeId()
        {
            string messageInfo;

            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();
            workSchedule.Doctor = "1234";
            workSchedule.Nurse = "1234";
            workSchedule.FirstAuxiliar = "3";
            workSchedule.SecondAuxiliar = "4";
            workSchedule.Driver = "5";

            Assert.IsTrue(workSchedule.HasRepeatedEmployeeId(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Doctor = "1234";
            workSchedule.Nurse = "1234";
            workSchedule.FirstAuxiliar = "3";
            workSchedule.SecondAuxiliar = "4";
            workSchedule.Driver = "5";

            Assert.IsTrue(workSchedule.HasRepeatedEmployeeId(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Doctor = "1234";
            workSchedule.Nurse = "3";
            workSchedule.FirstAuxiliar = "1234";
            workSchedule.SecondAuxiliar = "4";
            workSchedule.Driver = "5";

            Assert.IsTrue(workSchedule.HasRepeatedEmployeeId(out messageInfo));
            Assert.IsNotNull(messageInfo);


            workSchedule.Doctor = "1234";
            workSchedule.Nurse = "4";
            workSchedule.FirstAuxiliar = "3";
            workSchedule.SecondAuxiliar = "1234";
            workSchedule.Driver = "5";

            Assert.IsTrue(workSchedule.HasRepeatedEmployeeId(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Doctor = "1234";
            workSchedule.Nurse = "4";
            workSchedule.FirstAuxiliar = "3";
            workSchedule.SecondAuxiliar = "5";
            workSchedule.Driver = "1234";

            Assert.IsTrue(workSchedule.HasRepeatedEmployeeId(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Doctor = "3";
            workSchedule.Nurse = "4";
            workSchedule.FirstAuxiliar = "1234";
            workSchedule.SecondAuxiliar = "5";
            workSchedule.Driver = "1234";

            Assert.IsTrue(workSchedule.HasRepeatedEmployeeId(out messageInfo));
            Assert.IsNotNull(messageInfo);
        }

        [TestMethod]
        public void IsIgnoringNotRepeatedEmployeeId()
        {
            string messageInfo;

            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();
            workSchedule.Doctor = "1234";
            workSchedule.Nurse = "1";
            workSchedule.FirstAuxiliar = "3";
            workSchedule.SecondAuxiliar = "4";
            workSchedule.Driver = "5";

            Assert.IsFalse(workSchedule.HasRepeatedEmployeeId(out messageInfo));
            Assert.IsNull(messageInfo);
        }

        [TestMethod]
        public void IsCheckingOverflowOfCellPhoneLength()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.CellPhone = string.Empty;
            for (int i = 1; i <= ImportableWorkScheduleUnitModel.MAX_CELLPHONE_LENGTH; i++)
            {
                workSchedule.CellPhone += "_";
            }
            workSchedule.CellPhone += "_";

            Assert.IsTrue(workSchedule.HasOverflowOfCellPhoneLength(out messageInfo));
            Assert.IsNotNull(messageInfo);
        }

        [TestMethod]
        public void IsIgnoringNotOverflowOfCellPhoneLength()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.CellPhone = string.Empty;
            for (int i = 1; i <= ImportableWorkScheduleUnitModel.MAX_CELLPHONE_LENGTH; i++)
            {
                workSchedule.CellPhone += "_";
            }

            Assert.IsFalse(workSchedule.HasOverflowOfCellPhoneLength(out messageInfo));
            Assert.IsNull(messageInfo);
        }

        [TestMethod]
        public void IsCheckingOverflowOfRemarkLength()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.Remark = string.Empty;
            for (int i = 1; i <= ImportableWorkScheduleUnitModel.MAX_REMARK_LENGTH; i++)
            {
                workSchedule.Remark += "_";
            }
            workSchedule.Remark += "_";

            Assert.IsTrue(workSchedule.HasOverflowOfRemarkLength(out messageInfo));
            Assert.IsNotNull(messageInfo);
        }

        [TestMethod]
        public void IsIgnoringNotOverflowOfRemarkLength()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.Remark = string.Empty;
            for (int i = 1; i <= ImportableWorkScheduleUnitModel.MAX_REMARK_LENGTH; i++)
            {
                workSchedule.Remark += "_";
            }

            Assert.IsFalse(workSchedule.HasOverflowOfRemarkLength(out messageInfo));
            Assert.IsNull(messageInfo);
        }

        [TestMethod]
        public void IsCheckingIfIsFormatValidOfCrewMember()
        {
            string messageInfo;

            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();
            workSchedule.Doctor = "1234";
            workSchedule.Nurse = "1234 ";
            workSchedule.FirstAuxiliar = " 3";
            workSchedule.SecondAuxiliar = null;
            workSchedule.Driver = "5";

            Assert.IsTrue(workSchedule.IsFormatValidOfDoctor(out messageInfo));
            Assert.IsNull(messageInfo);
            Assert.IsTrue(workSchedule.IsFormatValidOfDriver(out messageInfo));
            Assert.IsNull(messageInfo);
            Assert.IsTrue(workSchedule.IsFormatValidOfFirstAuxiliar(out messageInfo));
            Assert.IsNull(messageInfo);
            Assert.IsTrue(workSchedule.IsFormatValidOfNurse(out messageInfo));
            Assert.IsNull(messageInfo);
            Assert.IsTrue(workSchedule.IsFormatValidOfSecondAuxiliar(out messageInfo));
            Assert.IsNull(messageInfo);
            Assert.IsTrue(workSchedule.IsFormatValidOfThirdAuxiliar(out messageInfo));
            Assert.IsNull(messageInfo);
        }

        [TestMethod]
        public void IsCheckingIfIsFormatInvalidOfCrewMember()
        {
            string messageInfo;

            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();
            workSchedule.Doctor = "1234_";
            workSchedule.Nurse = "1234A";
            workSchedule.FirstAuxiliar = "3.";
            workSchedule.SecondAuxiliar = "4 5";
            workSchedule.ThirdAuxiliar = "9 10";
            workSchedule.Driver = "5-";

            Assert.IsFalse(workSchedule.IsFormatValidOfDoctor(out messageInfo));
            Assert.IsNotNull(messageInfo);
            Assert.IsFalse(workSchedule.IsFormatValidOfDriver(out messageInfo));
            Assert.IsNotNull(messageInfo);
            Assert.IsFalse(workSchedule.IsFormatValidOfFirstAuxiliar(out messageInfo));
            Assert.IsNotNull(messageInfo);
            Assert.IsFalse(workSchedule.IsFormatValidOfNurse(out messageInfo));
            Assert.IsNotNull(messageInfo);
            Assert.IsFalse(workSchedule.IsFormatValidOfSecondAuxiliar(out messageInfo));
            Assert.IsNotNull(messageInfo);
            Assert.IsFalse(workSchedule.IsFormatValidOfThirdAuxiliar(out messageInfo));
            Assert.IsNotNull(messageInfo);
        }

        [TestMethod]
        public void IsCheckingIfRequiredFieldsAreNotFilled()
        {
            string messageInfo;

            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();
            workSchedule.Driver = "1234";
            workSchedule.IsURAM = "SIM";
            workSchedule.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workSchedule.StationId = null;
            workSchedule.UnitId = "235";
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Driver = "1234";
            workSchedule.IsURAM = "SIM";
            workSchedule.ShiftDateCellValue = null;
            workSchedule.StationId = "23";
            workSchedule.UnitId = "235";
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Driver = "1234";
            workSchedule.IsURAM = string.Empty;
            workSchedule.ShiftDateCellValue = "33433";
            workSchedule.StationId = "23";
            workSchedule.UnitId = "235";
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Driver = null;
            workSchedule.IsURAM = "SIM";
            workSchedule.ShiftDateCellValue = "33433";
            workSchedule.StationId = "23";
            workSchedule.UnitId = "235";
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Driver = "1234";
            workSchedule.IsURAM = "SIM";
            workSchedule.ShiftDateCellValue = "33433";
            workSchedule.StationId = "22";
            workSchedule.UnitId = null;
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Driver = "1234";
            workSchedule.IsURAM = "SIM";
            workSchedule.ShiftDateCellValue = "33433";
            workSchedule.StationId = "23";
            workSchedule.UnitId = "235";
            workSchedule.WorkshiftLabel = null;

            Assert.IsTrue(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.Driver = "1234";
            workSchedule.IsURAM = "SIM";
            workSchedule.ShiftDateCellValue = string.Empty;
            workSchedule.StationId = null;
            workSchedule.UnitId = "235";
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNotNull(messageInfo);

        }

        [TestMethod]
        public void IsIgnoringIfRequiredFieldsAreFilled()
        {
            string messageInfo;

            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();
            //workSchedule.Driver = "1234";
            //workSchedule.IsURAM = "SIM";
            workSchedule.ShiftDateCellValue = "33433";
            workSchedule.StationName = "23";
            workSchedule.UnitId = "235";
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsFalse(workSchedule.HasRequiredFieldsNotFilled(out messageInfo));
            Assert.IsNull(messageInfo);


        }

        [TestMethod]
        public void IsCheckingIfInvalidDateFormat()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.ShiftDateCellValue = "-657435";
            Assert.IsTrue(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.ShiftDateCellValue = "A";
            Assert.IsTrue(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.ShiftDateCellValue = "2958466";
            Assert.IsTrue(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.ShiftDateCellValue = null;
            Assert.IsTrue(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNotNull(messageInfo);

            workSchedule.ShiftDateCellValue = string.Empty;
            Assert.IsTrue(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNotNull(messageInfo);
        }

        [TestMethod]
        public void IsIgnoringIfIsValidDateFormat()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.ShiftDateCellValue = "2958465";
            Assert.IsFalse(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNull(messageInfo);

            workSchedule.ShiftDateCellValue = "0";
            Assert.IsFalse(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNull(messageInfo);

            workSchedule.ShiftDateCellValue = "-2";
            Assert.IsFalse(workSchedule.IsInvalidDateFormat(out messageInfo));
            Assert.IsNull(messageInfo);

        }

        [TestMethod]
        public void IsCheckingIfIsForwardShiftTime()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.ShiftDateCellValue = DateTime.Now.AddDays(1.0).ToString("dd/MM/yyyy");
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.IsForwardShiftTime(availableWorkShiftList, out messageInfo));
            Assert.IsNull(messageInfo);

            /*workSchedule.ShiftDateCellValue = DateTime.Now.Date.AddHours(NOTURN_HOUR).AddMinutes(1).ToOADate().ToString();
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsTrue(workSchedule.IsForwardShiftTime(availableWorkShiftList, out messageInfo));
            Assert.IsNull(messageInfo);*/

        }

        [TestMethod]
        public void IsCheckingIfIsNotForwardShiftTime()
        {
            string messageInfo;
            ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

            workSchedule.ShiftDateCellValue = DateTime.Now.AddDays(-1.0).ToString("dd/MM/yyyy");
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsFalse(workSchedule.IsForwardShiftTime(availableWorkShiftList, out messageInfo));
            Assert.IsNotNull(messageInfo);

            //TODO Mock DateTime.Now
            /*
             * using Moq;
             * Mock<WorkScheduleForUnitModel> mockWorkScheduleForUnitModel = new Mock<WorkScheduleForUnitModel>();
            mockWorkScheduleForUnitModel.Setup(ws => ws.GetDateTimeNow()).Returns(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

            mockWorkScheduleForUnitModel.SetupProperty(ws => ws.ShiftDateCellValue, DateTime.Now.AddDays(-1.0).ToOADate().ToString());
            mockWorkScheduleForUnitModel.SetupProperty(ws => ws.WorkshiftLabel, "07-19");

            mockWorkScheduleForUnitModel.Verify(ws => ws.IsForwardShiftTime(availableWorkShiftList, out messageInfo));*/


            /*workSchedule.ShiftDateCellValue = DateTime.Now.Date.AddHours(NOTURN_HOUR-1).ToOADate().ToString();
            workSchedule.WorkshiftLabel = "07-19";

            Assert.IsFalse(workSchedule.IsForwardShiftTime(availableWorkShiftList, out messageInfo));
            Assert.IsNotNull(messageInfo);*/
        }

        [TestMethod]
        public void VerifyIfNotExistsConflictingData()
        {
            string cell = "99";
            string remark = "ok";
            string station = "88";
            string doctor = "1";
            string nurse = "2";
            string firstAux = "3";
            string secondAux = "4";
            string thirdAux = "6";
            string driver = "5";

            string workShiftLabel = "07-19";
            string unitId = "123";

            WorkScheduleUnitModel unitForceMapCellPhone     = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapRemarks       = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapStation       = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapDoctor        = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapNurse         = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapDriver        = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapFirstAuxiliar = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapSecondAuxiliar = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapThirdAuxiliar = new WorkScheduleUnitModel();

            ImportableWorkScheduleUnitModel workScheduleForUnitCellPhone       = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitRemarks         = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitStation         = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitDoctor          = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitNurse           = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitDriver          = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitFirstAuxiliar   = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitSecondAuxiliar  = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitThirdAuxiliar = new ImportableWorkScheduleUnitModel();

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            unitForceMapStation.Station = new CadStationModel();
            unitForceMapDoctor.Doctor = new UnitCrewMember();
            unitForceMapDoctor.Nurse = new UnitCrewMember();
            unitForceMapDoctor.Driver = new UnitCrewMember();
            unitForceMapDoctor.FirstAuxiliar = new UnitCrewMember();
            unitForceMapDoctor.SecondAuxiliar = new UnitCrewMember();
            unitForceMapDoctor.ThirdAuxiliar = new UnitCrewMember();

            unitForceMapCellPhone.CellPhone = workScheduleForUnitCellPhone.CellPhone = cell;
            unitForceMapRemarks.Remarks = workScheduleForUnitRemarks.Remark = remark;
            unitForceMapStation.Station.StationId = workScheduleForUnitStation.StationId = station;
            unitForceMapDoctor.Doctor.EmployeeId = Int32.Parse(workScheduleForUnitDoctor.Doctor = doctor);
            unitForceMapDoctor.Nurse.EmployeeId = Int32.Parse(workScheduleForUnitDoctor.Nurse = nurse);
            unitForceMapDoctor.FirstAuxiliar.EmployeeId = Int32.Parse(workScheduleForUnitDoctor.FirstAuxiliar = firstAux);
            unitForceMapDoctor.SecondAuxiliar.EmployeeId = Int32.Parse(workScheduleForUnitDoctor.SecondAuxiliar = secondAux);
            unitForceMapDoctor.ThirdAuxiliar.EmployeeId = Int32.Parse(workScheduleForUnitDoctor.ThirdAuxiliar = thirdAux);
            unitForceMapDoctor.Driver.EmployeeId = Int32.Parse(workScheduleForUnitDoctor.Driver = driver);

            unitForceMapCellPhone.UnitId = workScheduleForUnitCellPhone.UnitId = "CellPhone";
            unitForceMapRemarks.UnitId = workScheduleForUnitRemarks.UnitId = "Remarks";
            unitForceMapStation.UnitId = workScheduleForUnitStation.UnitId = "Station";
            unitForceMapDoctor.UnitId = workScheduleForUnitDoctor.UnitId = "Doctor";
            unitForceMapNurse.UnitId = workScheduleForUnitNurse.UnitId = "Nurse";
            unitForceMapDriver.UnitId = workScheduleForUnitDriver.UnitId = "Driver";
            unitForceMapFirstAuxiliar.UnitId = workScheduleForUnitFirstAuxiliar.UnitId = "FirstAuxiliar";
            unitForceMapSecondAuxiliar.UnitId = workScheduleForUnitSecondAuxiliar.UnitId = "SecondAuxiliar";
            unitForceMapThirdAuxiliar.UnitId = workScheduleForUnitThirdAuxiliar.UnitId = "ThirdAuxiliar";

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnitCellPhone,
                workScheduleForUnitRemarks,
                workScheduleForUnitStation,
                workScheduleForUnitDoctor,
                workScheduleForUnitNurse,
                workScheduleForUnitDriver,
                workScheduleForUnitFirstAuxiliar,
                workScheduleForUnitSecondAuxiliar,
                workScheduleForUnitThirdAuxiliar

            };

            List<WorkScheduleUnitModel> unitForceMapList = new List<WorkScheduleUnitModel>()
            {
                unitForceMapCellPhone,
                unitForceMapRemarks,
                unitForceMapStation,
                unitForceMapDoctor,
                unitForceMapNurse,
                unitForceMapDriver,
                unitForceMapFirstAuxiliar,
                unitForceMapSecondAuxiliar,
                unitForceMapThirdAuxiliar
            };

            foreach (ImportableWorkScheduleUnitModel ws in template.WorkScheduleForUnitList)
            {
                ws.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
                ws.WorkshiftLabel = workShiftLabel;
            }

            foreach (WorkScheduleUnitModel uf in unitForceMapList)
            {
                uf.ShiftDate = DateTime.FromOADate(Double.Parse(VALID_SHIFT_DATE_VALUE1));
                uf.CurrentWorkShift = availableWorkShiftList.Where(ws => ws.Label == workShiftLabel).FirstOrDefault();
                if (uf.Station == null)
                    uf.Station = new CadStationModel();
            }

            template.UpdateReportValidationAlertingConflictsWithPreviousData(unitForceMapList, availableWorkShiftList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.CONFLICT_WITH_PREVIOUS_DATA).ToList().Count, 0);
        }

        [TestMethod]
        public void VerifyIfExistsConflictingData()
        {
            string cell = "99";
            //string remark = "ok";
            string station = "88";
            string doctor = "1";
            string nurse = "2";
            string firstAux = "3";
            string secondAux = "4";
            string thirdAux = "6";
            string driver = "5";

            string workShiftLabel = "07-19";
            string unitId = "A";

            WorkScheduleUnitModel unitForceMapCellPhone = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapRemarks = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapStation = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapDoctor = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapNurse = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapDriver = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapFirstAuxiliar = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapSecondAuxiliar = new WorkScheduleUnitModel();
            WorkScheduleUnitModel unitForceMapThirdAuxiliar = new WorkScheduleUnitModel();

            ImportableWorkScheduleUnitModel workScheduleForUnitCellPhone = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitRemarks = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitStation = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitDoctor = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitNurse = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitDriver = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitFirstAuxiliar = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitSecondAuxiliar = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnitThirdAuxiliar = new ImportableWorkScheduleUnitModel();

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            unitForceMapStation.Station = new CadStationModel();
            unitForceMapDoctor.Doctor = new UnitCrewMember();
            unitForceMapDoctor.Nurse = new UnitCrewMember();
            unitForceMapDoctor.Driver = new UnitCrewMember();
            unitForceMapDoctor.FirstAuxiliar = new UnitCrewMember();
            unitForceMapDoctor.SecondAuxiliar = new UnitCrewMember();
            unitForceMapDoctor.ThirdAuxiliar = new UnitCrewMember();

            //unitForceMapCellPhone.CellPhone = cell;
            //workScheduleForUnitCellPhone.CellPhone = cell + "a";
            //unitForceMapRemarks.Remarks = remark;
            //workScheduleForUnitRemarks.Remark = remark + "a";
            unitForceMapStation.Station.StationId = station;
            workScheduleForUnitStation.StationId = station + "1";
            unitForceMapDoctor.Doctor.EmployeeId = Int32.Parse(doctor)+10;
            workScheduleForUnitDoctor.Doctor = doctor;
            unitForceMapDoctor.Nurse.EmployeeId = Int32.Parse(nurse)+100;
            workScheduleForUnitDoctor.Nurse = nurse;
            unitForceMapDoctor.FirstAuxiliar.EmployeeId = Int32.Parse(firstAux)+1000;
            workScheduleForUnitDoctor.FirstAuxiliar = firstAux;
            unitForceMapDoctor.SecondAuxiliar.EmployeeId = Int32.Parse(secondAux)+10000;
            workScheduleForUnitDoctor.SecondAuxiliar = secondAux;
            unitForceMapDoctor.ThirdAuxiliar.EmployeeId = Int32.Parse(thirdAux) + 100000;
            workScheduleForUnitDoctor.ThirdAuxiliar = thirdAux;
            unitForceMapDoctor.Driver.EmployeeId = Int32.Parse(driver)+1000000;
            workScheduleForUnitDoctor.Driver = driver;



            //unitForceMapCellPhone.UnitId = workScheduleForUnitCellPhone.UnitId = "CellPhone";
            //unitForceMapRemarks.UnitId = workScheduleForUnitRemarks.UnitId = "Remarks";
            unitForceMapStation.UnitId = workScheduleForUnitStation.UnitId = "Station";
            unitForceMapDoctor.UnitId = workScheduleForUnitDoctor.UnitId = "Doctor";
            unitForceMapNurse.UnitId = workScheduleForUnitNurse.UnitId = "Nurse";
            unitForceMapDriver.UnitId = workScheduleForUnitDriver.UnitId = "Driver";
            unitForceMapFirstAuxiliar.UnitId = workScheduleForUnitFirstAuxiliar.UnitId = "FirstAuxiliar";
            unitForceMapSecondAuxiliar.UnitId = workScheduleForUnitSecondAuxiliar.UnitId = "SecondAuxiliar";
            unitForceMapThirdAuxiliar.UnitId = workScheduleForUnitThirdAuxiliar.UnitId = "ThirdAuxiliar";

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                //workScheduleForUnitCellPhone,
                //workScheduleForUnitRemarks,
                workScheduleForUnitStation,
                workScheduleForUnitDoctor,
                workScheduleForUnitNurse,
                workScheduleForUnitDriver,
                workScheduleForUnitFirstAuxiliar,
                workScheduleForUnitSecondAuxiliar,
                workScheduleForUnitThirdAuxiliar

            };

            List<WorkScheduleUnitModel> unitForceMapList = new List<WorkScheduleUnitModel>()
            {
                //unitForceMapCellPhone,
                //unitForceMapRemarks,
                unitForceMapStation,
                unitForceMapDoctor,
                unitForceMapNurse,
                unitForceMapDriver,
                unitForceMapFirstAuxiliar,
                unitForceMapSecondAuxiliar,
                unitForceMapThirdAuxiliar
            };

            foreach (ImportableWorkScheduleUnitModel ws in template.WorkScheduleForUnitList)
            {
                ws.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
                ws.WorkshiftLabel = workShiftLabel;
            }

            foreach (WorkScheduleUnitModel uf in unitForceMapList)
            {
                uf.ShiftDate = DateTime.FromOADate(Double.Parse(VALID_SHIFT_DATE_VALUE1));
                uf.CurrentWorkShift = availableWorkShiftList.Where(ws => ws.Label == workShiftLabel).FirstOrDefault();
                if (uf.Station == null)
                    uf.Station = new CadStationModel();
            }

            template.UpdateReportValidationAlertingConflictsWithPreviousData(unitForceMapList, availableWorkShiftList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.CONFLICT_WITH_PREVIOUS_DATA).ToList().Count, 7);
        }


        [TestMethod]
        public void VerifyIfExistsInvalidUnits()
        {
            List<string> validUnitList = new List<string>() { "Unid1", "Unid2", "Unid3" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.UnitId = "Unid1";
            workScheduleForUnit2.UnitId = "Unid2";
            workScheduleForUnit3.UnitId = "Unid10";
            workScheduleForUnit4.UnitId = "Unid11";
            workScheduleForUnit5.UnitId = "Unid12";

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5,
            };

            template.UpdateReportValidationWithInvalidUnits(validUnitList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_UNIT).ToList().Count, 3);

        }

        [TestMethod]
        public void VerifyIfNotExistsInvalidUnits()
        {
            List<string> validUnitList = new List<string>() { "Unid1", "Unid2", "Unid3" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.UnitId = "Unid1";
            workScheduleForUnit2.UnitId = "Unid2";
            workScheduleForUnit3.UnitId = "Unid2";
            workScheduleForUnit4.UnitId = "Unid1";
            workScheduleForUnit5.UnitId = "Unid3";

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5,
            };

            template.UpdateReportValidationWithInvalidUnits(validUnitList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_UNIT).ToList().Count, 0);

        }

        [TestMethod]
        public void VerifyIfExistsInvalidEmployees()
        {
            List<string> validEmployeeList = new List<string>() { "1", "2", "3" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.Doctor = "1";
            workScheduleForUnit1.Nurse = "2";
            workScheduleForUnit1.Driver = "11";
            workScheduleForUnit1.FirstAuxiliar = "12";
            workScheduleForUnit1.SecondAuxiliar = "13";

            workScheduleForUnit2.Doctor = "1";
            workScheduleForUnit2.Nurse = "2";
            workScheduleForUnit2.Driver = "3";
            workScheduleForUnit2.FirstAuxiliar = null;
            workScheduleForUnit2.SecondAuxiliar = string.Empty;

            workScheduleForUnit3.Doctor = "14";
            workScheduleForUnit3.Nurse = "24";
            workScheduleForUnit3.Driver = "1";
            workScheduleForUnit3.FirstAuxiliar = "12";
            workScheduleForUnit3.SecondAuxiliar = "13";

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
            };

            template.UpdateReportValidationWithInvalidEmployees(validEmployeeList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_EMPLOYEE_ID).ToList().Count, 2);

        }

        [TestMethod]
        public void VerifyIfItIsShowingListOfInvalidEmployees()
        {
            List<string> validEmployeeList = new List<string>() { "11", "12", "13" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.Doctor = "11";
            workScheduleForUnit1.Nurse = "12";
            workScheduleForUnit1.Driver = "99";
            workScheduleForUnit1.FirstAuxiliar = "98";
            workScheduleForUnit1.SecondAuxiliar = "97";

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1
            };

            template.UpdateReportValidationWithInvalidEmployees(validEmployeeList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_EMPLOYEE_ID).ToList().Count, 1);
            Assert.IsTrue(reportValidation.ReportValidationItemList.FirstOrDefault().DataAffected.Contains("99"));
            Assert.IsTrue(reportValidation.ReportValidationItemList.FirstOrDefault().DataAffected.Contains("98"));
            Assert.IsTrue(reportValidation.ReportValidationItemList.FirstOrDefault().DataAffected.Contains("97"));
            Assert.IsFalse(reportValidation.ReportValidationItemList.FirstOrDefault().DataAffected.Contains("11"));
            Assert.IsFalse(reportValidation.ReportValidationItemList.FirstOrDefault().DataAffected.Contains("12"));
            Assert.IsFalse(reportValidation.ReportValidationItemList.FirstOrDefault().DataAffected.Contains("13"));
        }

        [TestMethod]
        public void VerifyIfNotExistsInvalidEmployees()
        {
            List<string> validEmployeeList = new List<string>() { "1", "2", "3" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.Doctor = "1";
            workScheduleForUnit1.Nurse = "2";
            workScheduleForUnit1.Driver = "1";
            workScheduleForUnit1.FirstAuxiliar = "2";
            workScheduleForUnit1.SecondAuxiliar = "2";

            workScheduleForUnit2.Doctor = "1";
            workScheduleForUnit2.Nurse = "2";
            workScheduleForUnit2.Driver = "3";
            workScheduleForUnit2.FirstAuxiliar = "2";
            workScheduleForUnit2.SecondAuxiliar = "1";

            workScheduleForUnit3.Doctor = "2";
            workScheduleForUnit3.Nurse = "1";
            workScheduleForUnit3.Driver = "1";
            workScheduleForUnit3.FirstAuxiliar = "3";
            workScheduleForUnit3.SecondAuxiliar = "1";

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
            };

            template.UpdateReportValidationWithInvalidEmployees(validEmployeeList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_EMPLOYEE_ID).ToList().Count, 0);


        }

        [TestMethod]
        public void VerifyIfExistsInvalidStations()
        {
            List<string> validStationList = new List<string>() { "Stat1", "Stat2", "Stat3" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.StationName = "Stat1";
            workScheduleForUnit2.StationName = "Stat2";
            workScheduleForUnit3.StationName = "Stat10";
            workScheduleForUnit4.StationName = "Stat11";
            workScheduleForUnit5.StationName = "Stat12";

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5,
            };

            template.UpdateReportValidationWithInvalidStations(validStationList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_STATION).ToList().Count, 3);

        }

        [TestMethod]
        public void VerifyIfNotExistsInvalidStations()
        {
            List<string> validStationList = new List<string>() { "Stat1", "Stat2", "Stat3" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.StationName = "Stat1";
            workScheduleForUnit2.StationName = "Stat2";
            workScheduleForUnit3.StationName = "Stat3";
            workScheduleForUnit4.StationName = "Stat1";
            workScheduleForUnit5.StationName = "Stat3";

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5,
            };

            template.UpdateReportValidationWithInvalidStations(validStationList, ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_STATION).ToList().Count, 0);

        }

        [TestMethod]
        public void VerifyIfExistsRepeatedCrewMembersInWorkSchedule()
        {
            List<string> validEmployeeList = new List<string>() { "1", "2", "3","4","5","6","7","8","9","10","11","12","13","14"};

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit1.WorkshiftLabel = "07-19";
            workScheduleForUnit1.Doctor = "1";
            workScheduleForUnit1.Nurse = "2";
            workScheduleForUnit1.Driver = "3";
            workScheduleForUnit1.FirstAuxiliar = "99";
            workScheduleForUnit1.SecondAuxiliar = String.Empty;
            workScheduleForUnit1.Row = 1;

            workScheduleForUnit2.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit2.WorkshiftLabel = "07-19";
            workScheduleForUnit2.Doctor = "6";
            workScheduleForUnit2.Nurse = "6";
            workScheduleForUnit2.Driver = "8";
            workScheduleForUnit2.FirstAuxiliar = null;
            workScheduleForUnit2.SecondAuxiliar = "10";
            workScheduleForUnit2.Row = 2;

            workScheduleForUnit3.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit3.WorkshiftLabel = "07-19";
            workScheduleForUnit3.Doctor = "8";
            workScheduleForUnit3.Nurse = "12";
            workScheduleForUnit3.Driver = "13";
            workScheduleForUnit3.FirstAuxiliar = "14";
            workScheduleForUnit3.SecondAuxiliar = "99";
            workScheduleForUnit3.Row = 3;

            workScheduleForUnit4.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE2;
            workScheduleForUnit4.WorkshiftLabel = "07-19";
            workScheduleForUnit4.Doctor = "1";
            workScheduleForUnit4.Nurse = "2";
            workScheduleForUnit4.Driver = "3";
            workScheduleForUnit4.FirstAuxiliar = "99";
            workScheduleForUnit4.SecondAuxiliar = String.Empty;
            workScheduleForUnit4.Row = 4;

            workScheduleForUnit5.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit5.WorkshiftLabel = "19-07";
            workScheduleForUnit5.Doctor = "1";
            workScheduleForUnit5.Nurse = "2";
            workScheduleForUnit5.Driver = "3";
            workScheduleForUnit5.FirstAuxiliar = "99";
            workScheduleForUnit5.SecondAuxiliar = String.Empty;
            workScheduleForUnit5.Row = 5;

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5
            };

            template.UpdateReportValidationWithRepeatedCrewMembers(ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.EMPLOYEE_DUPLICATED_AT_DAY).ToList().Count, 2);

        }

        [TestMethod]
        public void VerifyIfNotExistsRepeatedCrewMembersInWorkSchedule()
        {
            List<string> validEmployeeList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit6 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit1.WorkshiftLabel = "07-19";
            workScheduleForUnit1.Doctor = "1";
            workScheduleForUnit1.Nurse = "2";
            workScheduleForUnit1.Driver = "3";
            workScheduleForUnit1.FirstAuxiliar = "99";
            workScheduleForUnit1.SecondAuxiliar = String.Empty;
            workScheduleForUnit1.Row = 1;

            workScheduleForUnit2.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit2.WorkshiftLabel = "07-19";
            workScheduleForUnit2.Doctor = "6";
            workScheduleForUnit2.Nurse = "6";
            workScheduleForUnit2.Driver = "8";
            workScheduleForUnit2.FirstAuxiliar = null;
            workScheduleForUnit2.SecondAuxiliar = "10";
            workScheduleForUnit2.Row = 2;

            workScheduleForUnit3.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE2;
            workScheduleForUnit3.WorkshiftLabel = "07-19";
            workScheduleForUnit3.Doctor = "8";
            workScheduleForUnit3.Nurse = "12";
            workScheduleForUnit3.Driver = "13";
            workScheduleForUnit3.FirstAuxiliar = "14";
            workScheduleForUnit3.SecondAuxiliar = "99";
            workScheduleForUnit3.Row = 3;

            workScheduleForUnit4.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE2;
            workScheduleForUnit4.WorkshiftLabel = "07-19";
            workScheduleForUnit4.Doctor = "1";
            workScheduleForUnit4.Nurse = "2";
            workScheduleForUnit4.Driver = "3";
            workScheduleForUnit4.FirstAuxiliar = "299";
            workScheduleForUnit4.SecondAuxiliar = String.Empty;
            workScheduleForUnit4.Row = 4;

            workScheduleForUnit5.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit5.WorkshiftLabel = "19-07";
            workScheduleForUnit5.Doctor = "1";
            workScheduleForUnit5.Nurse = "2";
            workScheduleForUnit5.Driver = "3";
            workScheduleForUnit5.FirstAuxiliar = "99";
            workScheduleForUnit5.SecondAuxiliar = String.Empty;
            workScheduleForUnit5.Row = 5;

            workScheduleForUnit6.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit6.WorkshiftLabel = "19-07";
            workScheduleForUnit6.Doctor = "4";
            workScheduleForUnit6.Nurse = "5";
            workScheduleForUnit6.Driver = null;
            workScheduleForUnit6.FirstAuxiliar = "199";
            workScheduleForUnit6.SecondAuxiliar = String.Empty;
            workScheduleForUnit6.Row = 6;

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5,
                workScheduleForUnit6
            };

            template.UpdateReportValidationWithRepeatedCrewMembers(ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.EMPLOYEE_DUPLICATED_AT_DAY).ToList().Count, 0);

        }

        [TestMethod]
        public void VerifyIfExistsRepeatedUnitInworkSchedule()
        {
            List<string> validEmployeeList = new List<string>() { "1", "2", "3", "4", "5", "6"};

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit6 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit7 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit1.WorkshiftLabel = "07-19";
            workScheduleForUnit1.UnitId = "1";
            workScheduleForUnit1.Row = 1;

            workScheduleForUnit2.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit2.WorkshiftLabel = "07-19";
            workScheduleForUnit2.UnitId = "3";
            workScheduleForUnit2.Row = 2;

            workScheduleForUnit3.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit3.WorkshiftLabel = "07-19";
            workScheduleForUnit3.UnitId = "3";
            workScheduleForUnit3.Row = 3;

            workScheduleForUnit4.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE2;
            workScheduleForUnit4.WorkshiftLabel = "07-19";
            workScheduleForUnit4.UnitId = "1";
            workScheduleForUnit4.Row = 4;

            workScheduleForUnit5.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit5.WorkshiftLabel = "19-07";
            workScheduleForUnit5.UnitId = "1";
            workScheduleForUnit5.Row = 5;

            workScheduleForUnit6.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit6.WorkshiftLabel = "19-07";
            workScheduleForUnit6.UnitId = String.Empty;
            workScheduleForUnit6.Row = 6;

            workScheduleForUnit7.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit7.WorkshiftLabel = "19-07";
            workScheduleForUnit7.UnitId = null;
            workScheduleForUnit7.Row = 6;

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5,
                workScheduleForUnit6,
                workScheduleForUnit7
            };

            template.UpdateReportValidationWithRepeatedUnits(ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.UNIT_DUPLICATED_AT_DAY).ToList().Count, 1);

            Assert.IsTrue(reportValidation.ReportValidationItemList.Where(r => r.DataAffected.Contains("3")).Count() == 1);

        }

        [TestMethod]
        public void VerifyIfNotExistsRepeatedUnitInworkSchedule()
        {
            List<string> validEmployeeList = new List<string>() { "1", "2", "3", "4", "5", "6" };

            ImportableWorkScheduleUnitModel workScheduleForUnit1 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit2 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit3 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit4 = new ImportableWorkScheduleUnitModel();
            ImportableWorkScheduleUnitModel workScheduleForUnit5 = new ImportableWorkScheduleUnitModel();

            workScheduleForUnit1.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit1.WorkshiftLabel = "07-19";
            workScheduleForUnit1.UnitId = String.Empty;
            workScheduleForUnit1.Row = 1;

            workScheduleForUnit2.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit2.WorkshiftLabel = "07-19";
            workScheduleForUnit2.UnitId = "8";
            workScheduleForUnit2.Row = 2;

            workScheduleForUnit3.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit3.WorkshiftLabel = "07-19";
            workScheduleForUnit3.UnitId = "3";
            workScheduleForUnit3.Row = 3;

            workScheduleForUnit4.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE2;
            workScheduleForUnit4.WorkshiftLabel = "07-19";
            workScheduleForUnit4.UnitId = "1";
            workScheduleForUnit4.Row = 4;

            workScheduleForUnit5.ShiftDateCellValue = VALID_SHIFT_DATE_VALUE1;
            workScheduleForUnit5.WorkshiftLabel = "19-07";
            workScheduleForUnit5.UnitId = null;
            workScheduleForUnit5.Row = 5;

            ReportValidationModel reportValidation = new ReportValidationModel();
            WorkScheduleImportTemplateModel template = new WorkScheduleImportTemplateModel();

            template.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>()
            {
                workScheduleForUnit1,
                workScheduleForUnit2,
                workScheduleForUnit3,
                workScheduleForUnit4,
                workScheduleForUnit5
            };

            template.UpdateReportValidationWithRepeatedUnits(ref reportValidation);

            Assert.AreEqual(reportValidation.ReportValidationItemList.Where
                (r => r.WorkScheduleValidationType == WorkScheduleValidationType.UNIT_DUPLICATED_AT_DAY).ToList().Count, 0);

        }

    }
}
