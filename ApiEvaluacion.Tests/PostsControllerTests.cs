using ApiEvaluacion.Application.Interfaces;
using ApiEvaluacion.Domain.Models;
using ApiEvaluacion.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ApiEvaluacion.Application;

namespace ApiEvaluacion.Tests
{
    public class PostsControllerTests
    {
        private readonly PostsController _controller;
        private readonly Mock<IPostsService> _postsServiceMock;

        public PostsControllerTests()
        {
            _postsServiceMock = new Mock<IPostsService>();
            _controller = new PostsController(_postsServiceMock.Object);
        }

        [Fact]
        public async Task GetPosts_ReturnsOk()
        {
            // Arrange
            var mockPosts = new List<Post>
            {
                new Post {
                    Id = 1,
                    UserId = 1,
                    Title = "Publicación de Prueba",
                    Body = "Este es el contenido de una publicación de prueba para verificar la funcionalidad."
                }
            };

            _postsServiceMock.Setup(s => s.GetPostsAsync())
                .ReturnsAsync(mockPosts);

            // Act
            var result = await _controller.GetPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var posts = Assert.IsType<List<Post>>(okResult.Value);
            Assert.Single(posts);
        }

        [Fact]
        public async Task CreatePost_ReturnsOk()
        {
            // Arrange
            var post = new Post
            {
                UserId = 1,
                Title = "Nueva Publicación",
                Body = "Este es el contenido de una nueva publicación creada por el usuario."
            };

            var createdPost = new Post
            {
                Id = 101,
                UserId = 1,
                Title = "Nueva Publicación",
                Body = "Este es el contenido de una nueva publicación creada por el usuario."
            };

            _postsServiceMock.Setup(s => s.CreatePostAsync(post))
                .ReturnsAsync(createdPost);

            // Act
            var result = await _controller.CreatePost(post);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPost = Assert.IsType<Post>(okResult.Value);
            Assert.Equal(101, returnedPost.Id);
        }
    }
}