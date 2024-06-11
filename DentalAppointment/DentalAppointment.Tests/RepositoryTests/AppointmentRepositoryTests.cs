using DentalAppointment.Data;
using DentalAppointment.DTOs;
using DentalAppointment.Models;
using DentalAppointment.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalAppointmentTests.RepositoryTests
{
    public class AppointmentRepositoryTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task CreateAppointment_AddsAppointmentToDatabase()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new AppointmentRepository(context);

            var request = new AppointmentDTO
            {
                PatientName = "John Doe",
                Date = DateTime.Now,
                Dentist = "Dr. Smith",
                Procedure = "Cleaning"
            };

            // Act
            await repository.CreateAppointment(request);

            // Assert
            var appointments = await context.Appointments.ToListAsync();
            Assert.Single(appointments);
            Assert.Equal("John Doe", appointments[0].PatientName);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_RemovesAppointmentFromDatabase()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new AppointmentRepository(context);

            var appointment = new Appointment { Id = 1, PatientName = "John Doe", Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" };
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAppointmentAsync(1);

            // Assert
            var appointments = await context.Appointments.ToListAsync();
            Assert.Empty(appointments);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAllAppointments()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new AppointmentRepository(context);

            var appointments = new List<Appointment>
        {
            new Appointment { PatientName = "John Doe", Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" },
            new Appointment { PatientName = "Jane Doe", Date = DateTime.Now, Dentist = "Dr. Brown", Procedure = "Filling" }
        };

            context.Appointments.AddRange(appointments);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAppointmentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("John Doe", result.First().PatientName);
            Assert.Equal("Jane Doe", result.Last().PatientName);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ReturnsCorrectAppointment()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new AppointmentRepository(context);

            var appointment = new Appointment { Id = 1, PatientName = "John Doe", Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" };
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAppointmentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.PatientName);
        }
    }
}
