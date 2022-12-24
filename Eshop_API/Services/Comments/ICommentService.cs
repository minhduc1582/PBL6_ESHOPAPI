using eshop_api.Entities;
using Eshop_API.Models.DTO.Comments;

namespace Eshop_API.Services.Comments
{
    public interface ICommentService
    {
        Task<List<Comment>> GetProductComment(int idProduct);
        Task<Comment> AddComment(CreateUpdateComment createUpdateComment, int userId);
        Task<Comment> EditComment(CreateUpdateComment createUpdateComment, int idComment);
        Task<bool> DeleteComment(int idComment);
    }
}
