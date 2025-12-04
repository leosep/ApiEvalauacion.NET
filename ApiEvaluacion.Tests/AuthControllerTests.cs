using ApiEvaluacion.Application.DTOs;
using ApiEvaluacion.Application.Interfaces;
using ApiEvaluacion.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiEvaluacion.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IAuthService> _authServiceMock;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "Juan Pérez",
                Email = "juan.perez@example.com",
                Password = "Contraseña123!"
            };

            var expectedResponse = new RegisterResponse
            {
                Name = request.Name,
                Email = request.Email,
                Id = Guid.NewGuid(),
                Token = "token-simulado"
            };

            _authServiceMock.Setup(s => s.RegisterAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<RegisterResponse>(okResult.Value);
            Assert.Equal(request.Name, response.Name);
            Assert.Equal(request.Email, response.Email);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.NotNull(response.Token);
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "María García",
                Email = "maria.garcia@example.com",
                Password = "Contraseña123!"
            };

            _authServiceMock.Setup(s => s.RegisterAsync(request))
                .ThrowsAsync(new InvalidOperationException("El correo electrónico ya está registrado."));

            // Act
            var result = await _controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("El correo electrónico ya está registrado.", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "carlos.rodriguez@example.com",
                Password = "Contraseña123!"
            };

            var expectedResponse = new LoginResponse
            {
                Token = "token-login-simulado"
            };

            _authServiceMock.Setup(s => s.LoginAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.NotNull(response.Token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "usuario.inexistente@example.com",
                Password = "ContraseñaIncorrecta"
            };

            _authServiceMock.Setup(s => s.LoginAsync(request))
                .ThrowsAsync(new UnauthorizedAccessException("Credenciales inválidas."));

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}