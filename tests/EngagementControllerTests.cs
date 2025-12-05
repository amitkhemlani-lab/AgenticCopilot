using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AgenticCopilot.Controllers;
using AgenticCopilot.Models;
using AgenticCopilot.DTOs;
using AgenticCopilot.Services;

namespace AgenticCopilot.Tests
{
    /// <summary>
    /// Unit tests for EngagementController
    /// </summary>
    public class EngagementControllerTests
    {
        private readonly Mock<IEngagementService> _mockService;
        private readonly Mock<ILogger<EngagementController>> _mockLogger;
        private readonly EngagementController _controller;

        public EngagementControllerTests()
        {
            _mockService = new Mock<IEngagementService>();
            _mockLogger = new Mock<ILogger<EngagementController>>();
            _controller = new EngagementController(_mockService.Object, _mockLogger.Object);
        }

        #region GetAllEngagements Tests

        [Fact]
        public async Task GetAllEngagements_ReturnsOkResult_WithListOfEngagements()
        {
            // Arrange
            var engagements = new List<Engagement>
            {
                new Engagement { Id = Guid.NewGuid(), Name = "Engagement1", ClientName = "Client1", PartnerName = "Partner1" },
                new Engagement { Id = Guid.NewGuid(), Name = "Engagement2", ClientName = "Client2", PartnerName = "Partner2" }
            };

            _mockService.Setup(s => s.GetAllEngagementsAsync())
                .ReturnsAsync(engagements);

            // Act
            var result = await _controller.GetAllEngagements();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEngagements = Assert.IsAssignableFrom<IEnumerable<Engagement>>(okResult.Value);
            Assert.Equal(2, returnedEngagements.Count());
        }

        [Fact]
        public async Task GetAllEngagements_ReturnsOkResult_WithEmptyList_WhenNoEngagements()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllEngagementsAsync())
                .ReturnsAsync(new List<Engagement>());

            // Act
            var result = await _controller.GetAllEngagements();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEngagements = Assert.IsAssignableFrom<IEnumerable<Engagement>>(okResult.Value);
            Assert.Empty(returnedEngagements);
        }

        [Fact]
        public async Task GetAllEngagements_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllEngagementsAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAllEngagements();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region GetEngagementById Tests

        [Fact]
        public async Task GetEngagementById_ReturnsOkResult_WithEngagement()
        {
            // Arrange
            var id = Guid.NewGuid();
            var engagement = new Engagement
            {
                Id = id,
                Name = "Test Engagement",
                ClientName = "Test Client",
                PartnerName = "Test Partner"
            };

            _mockService.Setup(s => s.GetEngagementByIdAsync(id))
                .ReturnsAsync(engagement);

            // Act
            var result = await _controller.GetEngagementById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEngagement = Assert.IsType<Engagement>(okResult.Value);
            Assert.Equal(id, returnedEngagement.Id);
        }

        [Fact]
        public async Task GetEngagementById_ReturnsNotFound_WhenEngagementDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetEngagementByIdAsync(id))
                .ReturnsAsync((Engagement)null);

            // Act
            var result = await _controller.GetEngagementById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetEngagementById_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.GetEngagementByIdAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetEngagementById(id);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region CreateEngagement Tests

        [Fact]
        public async Task CreateEngagement_ReturnsCreatedAtAction_WithEngagement()
        {
            // Arrange
            var createDto = new CreateEngagementDto
            {
                Name = "New Engagement",
                ClientName = "New Client",
                PartnerName = "New Partner"
            };

            var createdEngagement = new Engagement
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                ClientName = createDto.ClientName,
                PartnerName = createDto.PartnerName
            };

            _mockService.Setup(s => s.CreateEngagementAsync(createDto))
                .ReturnsAsync(createdEngagement);

            // Act
            var result = await _controller.CreateEngagement(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedEngagement = Assert.IsType<Engagement>(createdAtActionResult.Value);
            Assert.Equal(createDto.Name, returnedEngagement.Name);
        }

        [Fact]
        public async Task CreateEngagement_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var createDto = new CreateEngagementDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.CreateEngagement(createDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateEngagement_ReturnsBadRequest_WhenArgumentNullException()
        {
            // Arrange
            var createDto = new CreateEngagementDto
            {
                Name = "Test",
                ClientName = "Client",
                PartnerName = "Partner"
            };

            _mockService.Setup(s => s.CreateEngagementAsync(createDto))
                .ThrowsAsync(new ArgumentNullException());

            // Act
            var result = await _controller.CreateEngagement(createDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateEngagement_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var createDto = new CreateEngagementDto
            {
                Name = "Test",
                ClientName = "Client",
                PartnerName = "Partner"
            };

            _mockService.Setup(s => s.CreateEngagementAsync(createDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateEngagement(createDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region UpdateEngagement Tests

        [Fact]
        public async Task UpdateEngagement_ReturnsOkResult_WithUpdatedEngagement()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UpdateEngagementDto
            {
                Name = "Updated Engagement",
                ClientName = "Updated Client",
                PartnerName = "Updated Partner"
            };

            var updatedEngagement = new Engagement
            {
                Id = id,
                Name = updateDto.Name,
                ClientName = updateDto.ClientName,
                PartnerName = updateDto.PartnerName
            };

            _mockService.Setup(s => s.UpdateEngagementAsync(id, updateDto))
                .ReturnsAsync(updatedEngagement);

            // Act
            var result = await _controller.UpdateEngagement(id, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEngagement = Assert.IsType<Engagement>(okResult.Value);
            Assert.Equal(updateDto.Name, returnedEngagement.Name);
        }

        [Fact]
        public async Task UpdateEngagement_ReturnsNotFound_WhenEngagementDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UpdateEngagementDto
            {
                Name = "Test",
                ClientName = "Client",
                PartnerName = "Partner"
            };

            _mockService.Setup(s => s.UpdateEngagementAsync(id, updateDto))
                .ReturnsAsync((Engagement)null);

            // Act
            var result = await _controller.UpdateEngagement(id, updateDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEngagement_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UpdateEngagementDto();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.UpdateEngagement(id, updateDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEngagement_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UpdateEngagementDto
            {
                Name = "Test",
                ClientName = "Client",
                PartnerName = "Partner"
            };

            _mockService.Setup(s => s.UpdateEngagementAsync(id, updateDto))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateEngagement(id, updateDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region DeleteEngagement Tests

        [Fact]
        public async Task DeleteEngagement_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteEngagementAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteEngagement(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEngagement_ReturnsNotFound_WhenEngagementDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteEngagementAsync(id))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteEngagement(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteEngagement_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteEngagementAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.DeleteEngagement(id);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region EngagementExists Tests

        [Fact]
        public async Task EngagementExists_ReturnsOk_WhenEngagementExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.EngagementExistsAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.EngagementExists(id);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task EngagementExists_ReturnsNotFound_WhenEngagementDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.EngagementExistsAsync(id))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.EngagementExists(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EngagementExists_ReturnsServerError_WhenExceptionOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.EngagementExistsAsync(id))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.EngagementExists(id);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        #endregion
    }
}
