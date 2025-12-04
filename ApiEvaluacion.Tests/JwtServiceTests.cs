using ApiEvaluacion.Application.Services;
using ApiEvaluacion.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace ApiEvaluacion.Tests
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public JwtServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Key", "sdfasdfgdgsdgsdfgsdfgdasasdasdasdasdasdagvdfghfhdfgdfsdfsdfsdfsd"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtService = new JwtService(_configuration);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidToken()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Ana López",
                Email = "ana.lopez@example.com"
            };

            // Act
            var token = _jwtService.GenerateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }
    }
}