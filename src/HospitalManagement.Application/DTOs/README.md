# Hospital Management System - Data Transfer Objects (DTOs)

This directory contains all the Data Transfer Objects (DTOs) for the Hospital Management System.

## DTO Structure

Each entity has three types of DTOs:

1. **EntityDto.cs** - Complete DTO with all properties including Id, CreatedDate, ModifiedDate
2. **EntityCreateDto.cs** - DTO for creating new records (excludes Id, timestamps, includes Password for user entities)
3. **EntityUpdateDto.cs** - DTO for updating existing records (excludes Id, CreatedDate, includes ModifiedDate handling)

## Entity DTOs

### Patient
- **PatientDto.cs** - Full patient information
- **PatientCreateDto.cs** - Create new patient (includes Password)
- **PatientUpdateDto.cs** - Update patient information

### Doctor
- **DoctorDto.cs** - Full doctor information
- **DoctorCreateDto.cs** - Create new doctor (includes Password)
- **DoctorUpdateDto.cs** - Update doctor information

### Appointment
- **AppointmentDto.cs** - Full appointment information with navigation properties
- **AppointmentCreateDto.cs** - Create new appointment
- **AppointmentUpdateDto.cs** - Update appointment (includes Status)

### Treatment History
- **TreatmentHistoryDto.cs** - Full treatment history with navigation properties
- **TreatmentHistoryCreateDto.cs** - Create new treatment record
- **TreatmentHistoryUpdateDto.cs** - Update treatment record

### Bill
- **BillDto.cs** - Full billing information with calculated totals
- **BillCreateDto.cs** - Create new bill (total calculated automatically)
- **BillUpdateDto.cs** - Update bill information

### Feedback
- **FeedbackDto.cs** - Full feedback information with navigation properties
- **FeedbackCreateDto.cs** - Create new feedback
- **FeedbackUpdateDto.cs** - Update feedback

### Admin
- **AdminDto.cs** - Full admin information
- **AdminCreateDto.cs** - Create new admin (includes Password)
- **AdminUpdateDto.cs** - Update admin information

## Authentication DTOs

### Login
- **LoginDto.cs** - Login credentials (Email, Password)
- **LoginResultDto.cs** - Login result with user information and token

## Validation Attributes Used

- **[Required]** - Field is mandatory
- **[EmailAddress]** - Validates email format
- **[StringLength]** - Maximum and minimum length constraints
- **[Range]** - Numeric range validation
- **[Compare]** - Compares two properties (useful for password confirmation)

## Navigation Properties

Some DTOs include navigation properties (like PatientName, DoctorName) for easier data display without additional queries.

## Usage Example

```csharp
// Creating a new patient
var createDto = new PatientCreateDto
{
    Name = "John Doe",
    Email = "john@example.com",
    Password = "SecurePassword123",
    Phone = "1234567890",
    Age = 30,
    Gender = "Male",
    BloodGroup = "O+"
};

// Updating a patient
var updateDto = new PatientUpdateDto
{
    Name = "John Doe Updated",
    Email = "john.updated@example.com",
    Phone = "9876543210",
    Age = 31,
    Gender = "Male",
    BloodGroup = "O+"
};

// Login
var loginDto = new LoginDto
{
    Email = "user@example.com",
    Password = "password123"
};
```

## Notes

- All DTOs use nullable reference types (`string?`) for optional properties
- CreatedDate and ModifiedDate are handled automatically in the service layer
- Password fields are only included in Create DTOs and Login DTO
- Total amounts in bills are calculated automatically in the service layer
- All DTOs use proper namespaces: `HospitalManagement.Application.DTOs`
