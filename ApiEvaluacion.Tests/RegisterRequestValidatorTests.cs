using ApiEvaluacion.Application.DTOs;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Configuration;

namespace ApiEvaluacion.Tests
{
    public class RegisterRequestValidatorTests
    {
        private readonly RegisterRequestValidator _validator;
        private readonly IConfiguration _configuration;

        public RegisterRequestValidatorTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Validation:EmailRegex", "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$"},
                {"Validation:PasswordRegex", "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _validator = new RegisterRequestValidator(_configuration);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new RegisterRequest { Name = "", Email = "juan.perez@example.com", Password = "Password123!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("El nombre es obligatorio.");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new RegisterRequest { Name = "Juan", Email = "correoinvalido", Password = "Password123!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("Formato de correo electrónico inválido.");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Invalid()
        {
            var model = new RegisterRequest { Name = "Juan", Email = "juan.perez@example.com", Password = "password" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password).WithErrorMessage("La contraseña debe tener al menos 8 caracteres e incluir mayúscula, minúscula, número y carácter especial.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Valid()
        {
            var model = new RegisterRequest { Name = "Juan Pérez", Email = "juan.perez@example.com", Password = "Password123!" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}