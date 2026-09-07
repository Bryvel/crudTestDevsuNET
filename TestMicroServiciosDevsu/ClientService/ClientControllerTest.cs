using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Timers;
using UserService.Controllers;
using UserService.Models;
using UserService.Repositories;
using Moq;
using Xunit;

namespace ClientService
{
    public class ClientControllerTest
    {
        [Fact]
        public void Test1Create_ReturnsConflict_WhenDocumentIdAlreadyExists()
        {
            // Arrange: el mock simula que ya existe un cliente con ese documento
            var mockRepo = new Mock<IClientRepository>();
            mockRepo
                .Setup(r => r.ExistsByDocumentIdAsync("0102030405"))
                .ReturnsAsync(true);

            var controller = new ClientsController(mockRepo.Object);
            var dto = new ClientCreateDto("Ana Ruiz", "0102030405", "ana@example.com", null);

            // Act
            var result = await controller.Create(dto);

            // Assert: debe responder 409 Conflict, y NUNCA debe haber intentado guardar
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);

            mockRepo.Verify(r => r.AddAsync(It.IsAny<Client>()), Times.Never);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}