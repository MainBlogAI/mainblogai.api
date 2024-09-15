using ApiCatalogo.Controller;
using ApiCatalogo.DTOs;
using ApiCatalogoxUnitTests.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTestsController.ProductTestController
{
    public class GetProductsUnitTest : IClassFixture<UnitTestController>
    {
        private ProductController _controller;

        public GetProductsUnitTest(UnitTestController produtosUnitTestController)
        {
            _controller = new ProductController(produtosUnitTestController.unitOfWork, produtosUnitTestController.mapper);
        }

        [Fact]
        public async Task GetProductsById_OkResult()
        {
            //Arrange
            int prodId = 1;
            //Act
            var data = await _controller.GetProductByIdAsync(prodId);

            //Assert (xunit)
            var okResult = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal(200, okResult.StatusCode);

            //Assert (FluentAssertions)
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);

        }

        [Fact]
        public async Task GetProductsById_NotFoundResult()
        {
            // Arrange
            int prodId = 10300;

            // Act
            var result = await _controller.GetProductByIdAsync(prodId);

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be($"O produto com id não foi encontrado");
        }

        [Fact]
        public async Task GetProducts_BadRequestResult()
        {
            int prodId = -1;

            var data = await _controller.GetProductByIdAsync(prodId);

            var badRequestResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult.Subject.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetProducts_Return_ListOfProductsDTO()
        {
            var data = await _controller.GetProductsList();

            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<ProductDTO>>()
                .And.NotBeNull();
        }
    }
}
