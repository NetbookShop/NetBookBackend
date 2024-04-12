using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public UserModel Author { get; set; } = new UserModel();
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}