using TaskManager.Database.Models;

namespace TaskManager.Schemas
{
    public class CreateProductScheme
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Guid Fileid { get; set; }
    }
}
