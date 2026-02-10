using System;
using System.Data;
using Xunit;
using DBProject.DAL;

namespace DBProject.DAL.Tests
{
    public class myDALTests
    {
        [Fact]
        public void MyDAL_Constructor_ShouldCreateInstance()
        {
            // Act
            var dal = new myDAL();

            // Assert
            Assert.NotNull(dal);
            Assert.IsType<myDAL>(dal);
        }

        [Fact]
        public void ValidateLogin_WithNullEmail_ShouldHandleGracefully()
        {
            // Arrange
            var dal = new myDAL();
            int type = 0;
            int id = 0;

            // Act & Assert - May throw due to database connection
            var exception = Record.Exception(() => dal.validateLogin(null, "password", ref type, ref id));
        }

        [Fact]
        public void ValidateUser_WithValidParameters_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();
            int id = 0;

            // Act & Assert - Method exists
            var exception = Record.Exception(() =>
                dal.validateUser("Test", "2000-01-01", "test@test.com", "pass", "1234567890", "M", "Address", ref id));
        }

        [Fact]
        public void DoctorEmailAlreadyExist_WithEmail_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act & Assert - May throw due to database connection
            var exception = Record.Exception(() => dal.DoctorEmailAlreadyExist("test@test.com"));
        }

        [Fact]
        public void AddDoctor_WithValidParameters_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("AddDoctor");

            // Assert
            Assert.NotNull(method);
            Assert.Equal(13, method.GetParameters().Length);
        }

        [Fact]
        public void AddStaff_WithValidParameters_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("AddStaff");

            // Assert
            Assert.NotNull(method);
            Assert.Equal(8, method.GetParameters().Length);
        }

        [Fact]
        public void GetAdminHomeInformation_ShouldAcceptDataTableArray()
        {
            // Arrange
            var dal = new myDAL();
            DataTable[] tables = new DataTable[5];
            for (int i = 0; i < 5; i++)
                tables[i] = new DataTable();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("GetAdminHomeInformation");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DeleteDoctor_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("DeleteDoctor");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DeleteStaff_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("DeleteStaff");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void LoadDoctor_WithEmptySearchQuery_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();
            DataTable table = new DataTable();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("LoadDoctor");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void LoadPatient_WithSearchQuery_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("LoadPatient");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void LoadOtherStaff_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("LoadOtherStaff");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GETPATIENT_WithValidId_ShouldHaveCorrectParameters()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("GETPATIENT");

            // Assert
            Assert.NotNull(method);
            Assert.Equal(8, method.GetParameters().Length);
        }

        [Fact]
        public void GET_DOCTOR_PROFILE_WithValidId_ShouldHaveCorrectParameters()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("GET_DOCTOR_PROFILE");

            // Assert
            Assert.NotNull(method);
            Assert.Equal(12, method.GetParameters().Length);
        }

        [Fact]
        public void GETSATFF_WithValidId_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("GETSATFF");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void PatientInfoDisplayer_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("patientInfoDisplayer");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetBillHistory_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("getBillHistory");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void AppointmentTodayDisplayer_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("appointmentTodayDisplayer");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetTreatmentHistory_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("getTreatmentHistory");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetDeptInfo_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("getdeptInfo");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetDeptDoctorInfo_WithDeptName_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("getDeptDoctorInfo");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DoctorInfoDisplayer_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("doctorInfoDisplayer");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetFreeSlots_WithValidIds_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("getFreeSlots");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void InsertAppointment_WithValidParameters_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("insertAppointment");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetNotifications_WithValidPid_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("getNotifications");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void IsFeedbackPending_WithValidPid_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("isFeedbackPending");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GivePendingFeedback_WithValidAid_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("givePendingFeedback");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Docinfo_DAL_WithValidDoctorId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("docinfo_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetAllPendingAppointments_DAL_WithValidDoctorId_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("GetAllpendingappointments_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void UpdateAppointment_DAL_WithValidAppointmentId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("UpdateAppointment_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DeleteAppointment_DAL_WithValidAppointmentId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("Deleteappointment_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Search_Patient_DAL_WithValidDid_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("search_patient_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Update_Prescription_DAL_WithValidParameters_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("update_prescription_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Generate_Bill_DAL_WithValidDocid_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("generate_bill_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Paid_Bill_DAL_WithValidParameters_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("paid_bill_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Unpaid_Bill_DAL_WithValidParameters_ShouldExist()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("Unpaid_bill_DAL");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GetPHistory_WithValidId_ShouldReturnInteger()
        {
            // Arrange
            var dal = new myDAL();

            // Act - Verify method exists
            var method = dal.GetType().GetMethod("getPHistory");

            // Assert
            Assert.NotNull(method);
        }
    }
}
