using ApiEvaluacion.Domain.Models;

namespace ApiEvaluacion.Application.Interfaces
{
    public interface IPostsService
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> CreatePostAsync(Post post);
    }
}