namespace TaskManager.Database.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; }

        public FileModel? Photo { get; set; } = new FileModel();
        // Поля для комментариев
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}

    // Модель комментария
  