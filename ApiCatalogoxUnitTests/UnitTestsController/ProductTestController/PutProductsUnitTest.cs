using ApiCatalogo.Context;
using ApiCatalogo.Controller;
using ApiCatalogo.DTOs;
using ApiCatalogo.Repository.IRepository;
using ApiCatalogoxUnitTests.UnitTests;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTestsController.ProductTestController
{
    public class PutProductsUnitTest : IClassFixture<UnitTestController>
    {
        public IUnitOfWork unitOfWork;
        public IMapper mapper;
        private ProductController _controller;
        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "server=localhost;port=3307;database=CatalogoDB;user=root;password=S3cure!P4ssw0rd#2024";

        public PutProductsUnitTest(UnitTestController unitTestController)
        {
            _controller = new ProductController(unitTestController.unitOfWork, unitTestController.mapper);
        }

        [Fact]
        public async Task PutProduct_Update_Return_OkResult()
        {
            // Arrange
            var prodId = 4;
            var updateProductDto = new ProductDTO
            {
                ProductId = prodId,
                Name = "Produto Teste",
                Description = "Descrição do produto teste",
                Price = 20,
                ImgUrl = "http://teste.com.br/imagem.jpg",
                Inventory = 20,
                CategoryId = 1
            };

            var result = await _controller.PutProductAsync(prodId, updateProductDto);

            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject; 
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task PutProduct_Update_Return_BadRequest() {
            var prodId = 5;
            var updateProductDto = new ProductDTO
            {
            };

            var result = await _controller.PutProductAsync(prodId, updateProductDto);

            result.Should().NotBeNull();
            var okResult = result.Result.Should().BeOfType<BadRequestResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

    }
}
