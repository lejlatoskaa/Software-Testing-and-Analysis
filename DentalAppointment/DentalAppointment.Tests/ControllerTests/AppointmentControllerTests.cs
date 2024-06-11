using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using DentalAppointment.Controllers; // Ensure this matches your project
using DentalAppointment.Services.Interfaces;
using DentalAppointment.DTOs;
using DentalAppointment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace DentalAppointmentTests.ControllerTests // Ensure this matches your project structure
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _mockService;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _mockService = new Mock<IAppointmentService>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _controller = new AppointmentController(_mockService.Object, _mockMemoryCache.Object);
        }

        [Fact]
        public async Task CreateAppointment_ReturnsOkResult()
        {
            // Arrange
            var request = new AppointmentDTO
            {
                PatientName = "John Doe",
                Date = DateTime.Now,
                Dentist = "Dr. Smith",
                Procedure = "Cleaning"
            };

            _mockService.Setup(service => service.CreateAppointment(request)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateAppointment(request);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockService.Verify(service => service.CreateAppointment(It.IsAny<AppointmentDTO>()), Times.Once);
        }

        [Fact]
        public async Task GetAppointment_ReturnsOkResult_WithAppointmentDTO()
        {
            // Arrange
            var appointmentId = 1;
            var appointment = new Appointment
            {
                Id = appointmentId,
                PatientName = "John Doe",
                Date = DateTime.Now,
                Dentist = "Dr. Smith",
                Procedure = "Cleaning"
            };

            var appointmentDto = new AppointmentDTO
            {
                PatientName = appointment.PatientName,
                Date = appointment.Date,
                Dentist = appointment.Dentist,
                Procedure = appointment.Procedure
            };

            object cacheEntry = null;
            _mockService.Setup(service => service.GetAppointmentByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _mockMemoryCache.Setup(cache => cache.TryGetValue("myData", out cacheEntry)).Returns(false);
            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _controller.GetAppointment(appointmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<AppointmentDTO>(okResult.Value);
            Assert.Equal("John Doe", returnValue.PatientName);
            Assert.Equal("Dr. Smith", returnValue.Dentist);
        }

        [Fact]
        public async Task GetAllAppointments_ReturnsOkResult_WithListOfAppointmentDTOs()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment { PatientName = "John Doe", Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" },
                new Appointment { PatientName = "Jane Doe", Date = DateTime.Now, Dentist = "Dr. Brown", Procedure = "Filling" }
            };

            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                PatientName = a.PatientName,
                Date = a.Date,
                Dentist = a.Dentist,
                Procedure = a.Procedure
            }).ToList();

            object cacheEntry = null;
            _mockService.Setup(service => service.GetAllAppointmentsAsync()).ReturnsAsync(appointmentDtos);
            _mockMemoryCache.Setup(cache => cache.TryGetValue("myData", out cacheEntry)).Returns(false);
            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _controller.GetAllAppointments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<AppointmentDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAppointmentsByPatientNameAsync_ReturnsOkResult_WithListOfAppointments()
        {
            // Arrange
            var patientName = "John Doe";
            var appointments = new List<Appointment>
            {
                new Appointment { PatientName = patientName, Date = DateTime.Now, Dentist = "Dr. Smith", Procedure = "Cleaning" }
            };

            object cacheEntry = null;
            _mockService.Setup(service => service.GetAppointmentsByPatientNameAsync(patientName)).ReturnsAsync(appointments);
            _mockMemoryCache.Setup(cache => cache.TryGetValue("myData", out cacheEntry)).Returns(false);
            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _controller.GetAppointmentsByPatientNameAsync(patientName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Appointment>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal(patientName, returnValue.First().PatientName);
        }

        [Fact]
        public async Task GetAppointmentsByDentistAsync_ReturnsOkResult_WithListOfAppointments()
        {
            // Arrange
            var dentistName = "Dr. Smith";
            var appointments = new List<Appointment>
            {
                new Appointment { PatientName = "John Doe", Date = DateTime.Now, Dentist = dentistName, Procedure = "Cleaning" }
            };

            object cacheEntry = null;
            _mockService.Setup(service => service.GetAppointmentsByDentistAsync(dentistName)).ReturnsAsync(appointments);
            _mockMemoryCache.Setup(cache => cache.TryGetValue("myData", out cacheEntry)).Returns(false);
            _mockMemoryCache.Setup(cache => cache.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _controller.GetAppointmentsByDentistAsync(dentistName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Appointment>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal(dentistName, returnValue.First().Dentist);
        }

        [Fact]
        public async Task UpdateAppointment_ReturnsOkResult()
        {
            // Arrange
            var appointmentId = 1;
            var request = new AppointmentDTO
            {
                PatientName = "John Doe",
                Date = DateTime.Now,
                Dentist = "Dr. Smith",
                Procedure = "Cleaning"
            };

            _mockService.Setup(service => service.UpdateAppointmentAsync(request, appointmentId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAppointment(request, appointmentId);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockService.Verify(service => service.UpdateAppointmentAsync(It.IsAny<AppointmentDTO>(), appointmentId), Times.Once);
        }

        [Fact]
        public async Task DeleteAppointment_ReturnsOkResult()
        {
            // Arrange
            var appointmentId = 1;

            _mockService.Setup(service => service.DeleteAppointmentAsync(appointmentId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAppointment(appointmentId);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockService.Verify(service => service.DeleteAppointmentAsync(appointmentId), Times.Once);
        }
    }
}
