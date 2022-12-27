using AutoMapper;
using eshop_api.Entities;
using eshop_api.Helpers;
using Eshop_API.Models.DTO.Comments;

namespace Eshop_API.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CommentService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Comment> AddComment(CreateUpdateComment createUpdateComment, int userId)
        {
            Comment comment = new Comment();
            comment.UserId = userId;
            comment.CommentUser = createUpdateComment.Comment;
            comment.ProductId = createUpdateComment.ProductId;
            await _context.AddAsync(comment);
            _context.SaveChanges();
            return comment;
        }

        public async Task<bool> DeleteComment(int idComment, int idUser)
        {
            var comment = _context.Comments.FirstOrDefault(x => x.Id == idComment);
            if(comment!=null)
            {
                if (idUser != comment.UserId) return false;
                var result = _context.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Comment> EditComment(CreateUpdateComment createUpdateComment, int idComment)
        {
            var comments = _context.Comments.FirstOrDefault(x => x.Id == idComment);
            if(comments!=null)
            {
                comments.CommentUser = createUpdateComment.Comment;
                comments.ProductId = createUpdateComment.ProductId;
                var result = _context.Comments.Update(comments);
                await _context.SaveChangesAsync();
                return comments;
            }
            return comments;
        }

        public async Task<List<CommentDto>> GetProductComment(int idProduct)
        {
            var comments = _context.Comments.Where(x => x.ProductId == idProduct).ToList();
            List<CommentDto> list = new List<CommentDto>();
            foreach(var i in comments)
            {
                CommentDto comment = _mapper.Map<Comment, CommentDto>(i);
                string firstName = _context.AppUsers.FirstOrDefault(x => x.Id == i.UserId).FirstName ?? _context.AppUsers.FirstOrDefault(x => x.Id == i.UserId).Username;
                comment.Username = firstName;
                list.Add(comment);
            }    
            return list;
        }
    }
}
