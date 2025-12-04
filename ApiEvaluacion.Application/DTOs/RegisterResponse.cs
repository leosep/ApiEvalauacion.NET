namespace ApiEvaluacion.Application.DTOs
{
    public class RegisterResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Token { get; set; }
    }
}