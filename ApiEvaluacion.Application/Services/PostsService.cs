using ApiEvaluacion.Application.Interfaces;
using ApiEvaluacion.Domain.Models;

namespace ApiEvaluacion.Application.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;

        public PostsService(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _postsRepository.GetPostsAsync();
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            return await _postsRepository.CreatePostAsync(post);
        }
    }
}