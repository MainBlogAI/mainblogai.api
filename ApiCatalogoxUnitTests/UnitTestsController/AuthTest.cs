using System.Threading.Tasks;
using FluentAssertions;
using MainBlog.Controller;
using MainBlog.DTOs.AuthenticationsDTO;
using MainBlog.DTOs.Request;
using MainBlog.DTOs.Response;
using MainBlog.Services.AuthenticationsServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MainBlogxUnitTests.UnitTestsController
{
    public class AuthTest
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthTest()
        {
            // Mocka a dependência IAuthService
            _mockAuthService = new Mock<IAuthService>();

            // Injeta o mock do IAuthService no AuthController
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task ForgotPassword_ShouldReturnOk_WhenStatusIsNotError()
        {
            var email = new RequestEmailDTO("rennanbaccili@gmail.com");
            var responseModel = new ResponseModel
            {
                Status = "Success", 
                Message = "Password reset link sent"
            };

            // Act
            var result = await _controller.ForgotPassword(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);  
            var actualResponse = okResult.Value as ResponseModel;
            actualResponse.Should().BeEquivalentTo(responseModel);  
        }
    }
}
