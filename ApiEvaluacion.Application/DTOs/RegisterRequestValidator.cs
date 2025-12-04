using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace ApiEvaluacion.Application.DTOs
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        private readonly IConfiguration _configuration;

        public RegisterRequestValidator(IConfiguration configuration)
        {
            _configuration = configuration;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .Must(BeValidEmail).WithMessage("Formato de correo electrónico inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .Must(BeValidPassword).WithMessage("La contraseña debe tener al menos 8 caracteres e incluir mayúscula, minúscula, número y carácter especial.");
        }

        private bool BeValidEmail(string email)
        {
            var regex = new Regex(_configuration["Validation:EmailRegex"]);
            return regex.IsMatch(email);
        }

        private bool BeValidPassword(string password)
        {
            var regex = new Regex(_configuration["Validation:PasswordRegex"]);
            return regex.IsMatch(password);
        }
    }
}