using AutoMapper;
using DentalAppointment.DTOs;
using DentalAppointment.Models;
using DentalAppointment.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalAppointmentTests.ServiceTests
{
    public class AppointmentServiceTest
    {
        private readonly Mock<IAppointmentRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AppointmentService _service;

        public AppointmentServiceTest()
        {
            _mockRepository = new Mock<IAppointmentRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new AppointmentService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAllAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
        {
            new Appointment { PatientName = "John Doe", Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" },
            new Appointment { PatientName = "Jane Doe", Date = DateTime.Now, Dentist = "Dr. Brown", Procedure = "Filling" }
        };
            _mockRepository.Setup(repo => repo.GetAllAppointmentsAsync()).ReturnsAsync(appointments);

            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                PatientName = a.PatientName,
                Date = a.Date,
                Dentist = a.Dentist,
                Procedure = a.Procedure
            }).ToList();

            _mockMapper.Setup(m => m.Map<Appointment, AppointmentDTO>(It.IsAny<Appointment>())).Returns((Appointment a) => new AppointmentDTO
            {
                PatientName = a.PatientName,
                Date = a.Date,
                Dentist = a.Dentist,
                Procedure = a.Procedure
            });

            // Act
            var result = await _service.GetAllAppointmentsAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ReturnsCorrectAppointment()
        {
            // Arrange
            var appointment = new Appointment { Id = 1, PatientName = "John Doe", Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" };
            _mockRepository.Setup(repo => repo.GetAppointmentByIdAsync(1)).ReturnsAsync(appointment);

            // Act
            var result = await _service.GetAppointmentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.PatientName);
        }

        [Fact]
        public async Task GetAppointmentsByPatientNameAsync_ReturnsAppointments()
        {
            // Arrange
            var appointments = new List<Appointment>
        {
            new Appointment { PatientName = "John Doe", Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" }
        };
            _mockRepository.Setup(repo => repo.GetAppointmentsByPatientNameAsync("John Doe")).ReturnsAsync(appointments);

            // Act
            var result = await _service.GetAppointmentsByPatientNameAsync("John Doe");

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().PatientName);
        }

        [Fact]
        public async Task CreateAppointment_AddsAppointment()
        {
            // Arrange
            var appointmentDto = new AppointmentDTO
            {
                PatientName = "John Doe",
                Date = DateTime.Now,
                Dentist = "Dr. Smith",
                Procedure = "Cleaning"
            };

            _mockRepository.Setup(repo => repo.CreateAppointment(appointmentDto)).Returns(Task.CompletedTask);

            // Act
            await _service.CreateAppointment(appointmentDto);

            // Assert
            _mockRepository.Verify(repo => repo.CreateAppointment(It.IsAny<AppointmentDTO>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_UpdatesAppointment()
        {
            // Arrange
            var appointmentDto = new AppointmentDTO
            {
                PatientName = "John Doe",
                Date = DateTime.Now,
                Dentist = "Dr. Smith",
                Procedure = "Cleaning"
            };

            _mockRepository.Setup(repo => repo.UpdateAppointmentAsync(appointmentDto, 1)).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAppointmentAsync(appointmentDto, 1);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAppointmentAsync(It.IsAny<AppointmentDTO>(), 1), Times.Once);
        }
    }
}
