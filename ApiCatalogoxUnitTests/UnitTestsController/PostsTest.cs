using ApiCatalogoxUnitTests.UnitTests;
using AutoMapper;
using MainBlog.Controller;
using MainBlog.DTOs.Response;
using MainBlog.IRepository;
using MainBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainBlogxUnitTests.UnitTestsController
{
    public class PostsTest : IClassFixture<UnitTestController>
    {
        private readonly PostController _controller;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;

        public PostsTest(UnitTestController controller)
        {
            // Inicializando os mocks
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            // Instanciando o controller com os mocks
            _controller = new PostController(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        // Teste para GetPosts
        [Fact]
        public async Task GetPosts_ShouldReturnOk_WhenPostsExist()
        {
            // Arrange
            var postList = new List<Posts> { new Posts { Id = 1, Title = "Test Post" } };
            var postDtoList = new List<PostAllResponseDTO> { new PostAllResponseDTO { Id = 1, Title = "Test Post" } };

            _unitOfWorkMock.Setup(u => u.PostService.GetAllPostsAsync()).ReturnsAsync(postList);
            _mapperMock.Setup(m => m.Map<IList<PostAllResponseDTO>>(postList)).Returns(postDtoList);

            // Act
            var result = await _controller.GetPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPosts = Assert.IsType<List<PostAllResponseDTO>>(okResult.Value);
            Assert.Equal(1, returnPosts.Count);
            Assert.Equal("Test Post", returnPosts[0].Title);
        }

        // Teste para PostCreatePosts
        [Fact]
        public async Task PostCreatePosts_ShouldReturnOk_WhenPostIsCreated()
        {
            // Arrange
            var postRequest = new PostCreateRequestDTO
            {
                Id = 1,
                Title = "New Post",
                Description = "Description",
                Author = "Author",
                IsPublished = true,
                BlogId = 1
            };

            var newPost = new Posts { Id = 1, Title = "New Post" };
            var postResponse = new PostPageResponseDTO { Id = 1, Title = "New Post" };

            _mapperMock.Setup(m => m.Map<Posts>(postRequest)).Returns(newPost);
            _unitOfWorkMock.Setup(u => u.PostService.PostCreateAsync(newPost)).ReturnsAsync(newPost);
            _mapperMock.Setup(m => m.Map<PostPageResponseDTO>(newPost)).Returns(postResponse);

            // Act
            var result = await _controller.PostCreatePosts(postRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var createdPost = Assert.IsType<PostPageResponseDTO>(okResult.Value);
            Assert.Equal("New Post", createdPost.Title);
        }

        // Teste para GetPostById
        [Fact]
        public async Task GetPostById_ShouldReturnOk_WhenValidIdProvided()
        {
            // Arrange
            var post = new Posts { Id = 1, Title = "Existing Post" };
            var postResponse = new PostPageResponseDTO { Id = 1, Title = "Existing Post" };

            _unitOfWorkMock.Setup(u => u.PostService.GetPostByIdAsync(1)).ReturnsAsync(post);
            _mapperMock.Setup(m => m.Map<PostPageResponseDTO>(post)).Returns(postResponse);

            // Act
            var result = await _controller.GetPostById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var foundPost = Assert.IsType<PostPageResponseDTO>(okResult.Value);
            Assert.Equal("Existing Post", foundPost.Title);
        }

        [Fact]
        public async Task GetPostById_ShouldReturnBadRequest_WhenInvalidIdProvided()
        {
            // Act
            var result = await _controller.GetPostById(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }

}
