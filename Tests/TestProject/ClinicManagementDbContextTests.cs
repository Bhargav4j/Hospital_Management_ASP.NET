using Xunit;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Tests.Data
{
    public class ClinicManagementDbContextTests
    {
        private ClinicManagementDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new ClinicManagementDbContext(options);
        }

        [Fact]
        public void DbContext_Constructor_ShouldInitialize()
        {
            // Act
            using var context = GetInMemoryDbContext();

            // Assert
            Assert.NotNull(context);
            Assert.NotNull(context.Patients);
            Assert.NotNull(context.Doctors);
            Assert.NotNull(context.Departments);
            Assert.NotNull(context.Appointments);
            Assert.NotNull(context.OtherStaff);
        }

        [Fact]
        public async Task DbContext_CanAddPatient()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Test Patient", Email = "test@test.com" };

            // Act
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            // Assert
            var savedPatient = await context.Patients.FirstOrDefaultAsync();
            Assert.NotNull(savedPatient);
            Assert.Equal("Test Patient", savedPatient.Name);
        }

        [Fact]
        public async Task DbContext_CanAddDoctor()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var department = new Department { DeptName = "Cardiology" };
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            var doctor = new Doctor { Name = "Dr. Smith", DeptNo = department.DeptNo };

            // Act
            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();

            // Assert
            var savedDoctor = await context.Doctors.FirstOrDefaultAsync();
            Assert.NotNull(savedDoctor);
            Assert.Equal("Dr. Smith", savedDoctor.Name);
        }

        [Fact]
        public async Task DbContext_CanAddDepartment()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var department = new Department { DeptName = "Neurology", Description = "Brain care" };

            // Act
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            // Assert
            var savedDepartment = await context.Departments.FirstOrDefaultAsync();
            Assert.NotNull(savedDepartment);
            Assert.Equal("Neurology", savedDepartment.DeptName);
        }

        [Fact]
        public async Task DbContext_CanAddAppointment()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Test Patient", Email = "test@test.com" };
            var department = new Department { DeptName = "Cardiology" };
            await context.Patients.AddAsync(patient);
            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();

            var doctor = new Doctor { Name = "Dr. Smith", DeptNo = department.DeptNo };
            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();

            var appointment = new Appointment { PatientID = patient.PatientID, DoctorID = doctor.DoctorID };

            // Act
            await context.Appointments.AddAsync(appointment);
            await context.SaveChangesAsync();

            // Assert
            var savedAppointment = await context.Appointments.FirstOrDefaultAsync();
            Assert.NotNull(savedAppointment);
        }

        [Fact]
        public async Task DbContext_CanAddOtherStaff()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var staff = new OtherStaff { Name = "Jane Nurse", Designation = "Nurse" };

            // Act
            await context.OtherStaff.AddAsync(staff);
            await context.SaveChangesAsync();

            // Assert
            var savedStaff = await context.OtherStaff.FirstOrDefaultAsync();
            Assert.NotNull(savedStaff);
            Assert.Equal("Jane Nurse", savedStaff.Name);
        }

        [Fact]
        public async Task DbContext_CanQueryPatients()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            await context.Patients.AddAsync(new Patient { Name = "Patient1", Email = "p1@test.com" });
            await context.Patients.AddAsync(new Patient { Name = "Patient2", Email = "p2@test.com" });
            await context.SaveChangesAsync();

            // Act
            var patients = await context.Patients.ToListAsync();

            // Assert
            Assert.Equal(2, patients.Count);
        }

        [Fact]
        public async Task DbContext_CanUpdatePatient()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Original", Email = "test@test.com" };
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            // Act
            patient.Name = "Updated";
            await context.SaveChangesAsync();

            // Assert
            var updated = await context.Patients.FindAsync(patient.PatientID);
            Assert.Equal("Updated", updated?.Name);
        }

        [Fact]
        public async Task DbContext_CanDeletePatient()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "To Delete", Email = "delete@test.com" };
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
            var patientId = patient.PatientID;

            // Act
            context.Patients.Remove(patient);
            await context.SaveChangesAsync();

            // Assert
            var deleted = await context.Patients.FindAsync(patientId);
            Assert.Null(deleted);
        }
    }
}
