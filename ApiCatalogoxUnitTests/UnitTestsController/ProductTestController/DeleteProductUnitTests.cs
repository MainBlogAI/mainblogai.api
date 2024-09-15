using ApiCatalogo.Controller;
using ApiCatalogo.Models;
using ApiCatalogo.Repository.IRepository;
using ApiCatalogoxUnitTests.UnitTests;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTestsController.ProductTestController
{
    public class DeleteProductUnitTests : IClassFixture<UnitTestController>
    {
        private ProductController _controller;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;

        public DeleteProductUnitTests(UnitTestController produtosUnitTestController)
        {
            _controller = new ProductController(produtosUnitTestController.unitOfWork, produtosUnitTestController.mapper);
        }


        [Fact]
        public async Task DeleteProduct_Return_OkResult()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new ProductController(_mockUnitOfWork.Object);

            int productId = 2;
            _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);
            _mockProductRepository.Setup(repo => repo.GetAsync(p => p.ProductId == productId))
                .ReturnsAsync(new Product { ProductId = productId });

            _mockProductRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Product>()));

            var result = await _controller.DeleteProduct(productId);
            result.Should().BeOfType<NoContentResult>();
            _mockProductRepository.Verify(repo => repo.DeleteAsync(It.Is<Product>(p => p.ProductId == productId)), Times.Once);
        }


        [Fact]
        public async Task DeleteProduct_Return_NotFound()
        {
            int id = 1;
            var result = await _controller.DeleteProduct(2);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
