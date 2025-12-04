using System.Text.Json;
using Microsoft.Extensions.Options;
using ApiEvaluacion.Application;
using ApiEvaluacion.Application.Interfaces;
using ApiEvaluacion.Domain.Models;

namespace ApiEvaluacion.Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ExternalApiSettings _apiSettings;

        public PostsRepository(HttpClient httpClient, IOptions<ExternalApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var response = await _httpClient.GetAsync(_apiSettings.JsonPlaceholder.PostsUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Post>>(json, _jsonOptions) ?? new List<Post>();
            }
            throw new HttpRequestException($"Error al obtener las publicaciones: {response.StatusCode}");
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(post, _jsonOptions), System.Text.Encoding.UTF8, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            var response = await _httpClient.PostAsync(_apiSettings.JsonPlaceholder.PostsUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Post>(json, _jsonOptions) ?? post;
            }
            throw new HttpRequestException($"Error al crear la publicación: {response.StatusCode}");
        }
    }
}