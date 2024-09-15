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
    public class PostProductsUnitTests : IClassFixture<UnitTestController>
    {
        private ProductController _controller;

        public PostProductsUnitTests(UnitTestController unitTestController)
        {
            _controller = new ProductController(unitTestController.unitOfWork, unitTestController.mapper);
        }

        [Fact]
        public async Task PostProduct_Return_CreatedStatusCode()
        { 
        ProductDTO produtoDTO = new ProductDTO
        {
            Name = "Produto Teste",
            Description = "Descrição do produto teste",
            Price = 10,
            ImgUrl = "http://teste.com.br/imagem.jpg",
            Inventory = 10,
            CategoryId = 1
        };
          var response = await _controller.PostProductAsync(produtoDTO);
          var result = response.Result.Should().BeOfType<CreatedAtRouteResult>();
          result.Subject.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task PostProduct_Return_BadRequestStatusCode()
        {
            ProductDTO productDTO = null;

            var response = await _controller.PostProductAsync(productDTO);
            var result = response.Result.Should().BeOfType<BadRequestResult>();
            result.Subject.StatusCode.Should().Be(400);
        }
    }
}
