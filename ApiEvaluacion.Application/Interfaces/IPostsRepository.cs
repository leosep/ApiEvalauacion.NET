using ApiEvaluacion.Domain.Models;

namespace ApiEvaluacion.Application.Interfaces
{
    public interface IPostsRepository
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> CreatePostAsync(Post post);
    }
}