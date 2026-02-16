using Xunit;
using ClinicManagement.Domain.Enums;
using System;
using System.Linq;

namespace ClinicManagement.Domain.Tests.Enums
{
    public class AppointmentStatusTests
    {
        [Fact]
        public void AppointmentStatus_Pending_ShouldBeDefined()
        {
            // Act
            var status = AppointmentStatus.Pending;

            // Assert
            Assert.Equal(AppointmentStatus.Pending, status);
        }

        [Fact]
        public void AppointmentStatus_Confirmed_ShouldBeDefined()
        {
            // Act
            var status = AppointmentStatus.Confirmed;

            // Assert
            Assert.Equal(AppointmentStatus.Confirmed, status);
        }

        [Fact]
        public void AppointmentStatus_InProgress_ShouldBeDefined()
        {
            // Act
            var status = AppointmentStatus.InProgress;

            // Assert
            Assert.Equal(AppointmentStatus.InProgress, status);
        }

        [Fact]
        public void AppointmentStatus_Completed_ShouldBeDefined()
        {
            // Act
            var status = AppointmentStatus.Completed;

            // Assert
            Assert.Equal(AppointmentStatus.Completed, status);
        }

        [Fact]
        public void AppointmentStatus_Cancelled_ShouldBeDefined()
        {
            // Act
            var status = AppointmentStatus.Cancelled;

            // Assert
            Assert.Equal(AppointmentStatus.Cancelled, status);
        }

        [Fact]
        public void AppointmentStatus_AllValues_ShouldBeAccessible()
        {
            // Arrange & Act
            var values = Enum.GetValues(typeof(AppointmentStatus));

            // Assert
            Assert.Equal(5, values.Length);
            Assert.Contains(AppointmentStatus.Pending, values.Cast<AppointmentStatus>());
            Assert.Contains(AppointmentStatus.Confirmed, values.Cast<AppointmentStatus>());
            Assert.Contains(AppointmentStatus.InProgress, values.Cast<AppointmentStatus>());
            Assert.Contains(AppointmentStatus.Completed, values.Cast<AppointmentStatus>());
            Assert.Contains(AppointmentStatus.Cancelled, values.Cast<AppointmentStatus>());
        }

        [Theory]
        [InlineData(AppointmentStatus.Pending, "Pending")]
        [InlineData(AppointmentStatus.Confirmed, "Confirmed")]
        [InlineData(AppointmentStatus.InProgress, "InProgress")]
        [InlineData(AppointmentStatus.Completed, "Completed")]
        [InlineData(AppointmentStatus.Cancelled, "Cancelled")]
        public void AppointmentStatus_ToString_ShouldReturnCorrectName(AppointmentStatus status, string expected)
        {
            // Act
            var result = status.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, AppointmentStatus.Pending)]
        [InlineData(1, AppointmentStatus.Confirmed)]
        [InlineData(2, AppointmentStatus.InProgress)]
        [InlineData(3, AppointmentStatus.Completed)]
        [InlineData(4, AppointmentStatus.Cancelled)]
        public void AppointmentStatus_CastFromInt_ShouldReturnCorrectEnum(int value, AppointmentStatus expected)
        {
            // Act
            var status = (AppointmentStatus)value;

            // Assert
            Assert.Equal(expected, status);
        }
    }
}
