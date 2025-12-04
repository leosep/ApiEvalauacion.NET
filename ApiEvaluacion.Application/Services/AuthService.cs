using ApiEvaluacion.Application.DTOs;
using ApiEvaluacion.Application.Interfaces;
using ApiEvaluacion.Domain.Models;

namespace ApiEvaluacion.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("El correo electrónico ya está registrado.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddUserAsync(user);

            var token = _jwtService.GenerateToken(user);
            user.Token = token;
            await _userRepository.UpdateUserAsync(user);

            return new RegisterResponse
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                Token = token
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            var token = _jwtService.GenerateToken(user);
            user.Token = token;
            await _userRepository.UpdateUserAsync(user);

            return new LoginResponse
            {
                Token = token
            };
        }
    }
}