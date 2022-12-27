using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_API.Models.DTO.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string CommentUser { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int ProductId { get; set; }
    }
}
