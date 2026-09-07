using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Controllers;
using UserService.Models;
using UserService.Repositories;
using Xunit;
namespace UserServiceTest
{
    public class ClientControllerTest
    {
        [Fact]
        public async Task Create_ReturnsConflict_WhenDocumentIdAlreadyExist()
        {
            // Arrange: el mock simula que ya existe un cliente con ese documento
            var mockRepo = new Mock<IClientRepository>();
            mockRepo
                .Setup(r => r.ExistsByCI("0102030405"))
                .ReturnsAsync(true);

            var controller = new ClientsController(mockRepo.Object);
            var dto = new ClientCreateDto("Ana Ruiz","Female" ,25,"0102030405", "La Gatazo", "0999270707","16464644",true);

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