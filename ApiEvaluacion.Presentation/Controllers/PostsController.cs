using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiEvaluacion.Application.Interfaces;
using ApiEvaluacion.Domain.Models;

namespace ApiEvaluacion.Presentation.Controllers
{
    [ApiController]
    [Route("api/posts")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var posts = await _postsService.GetPostsAsync();
                return Ok(posts);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al obtener las publicaciones: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            try
            {
                var createdPost = await _postsService.CreatePostAsync(post);
                return Ok(createdPost);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al crear la publicación: {ex.Message}");
            }
        }
    }
}