using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Model
{
    public class CardItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = new();
        public int Quantity { get; set; }
        public int ShopingCardId { get; set; }
        public ShopingCard ShopingCard { get; set; }
    }
}
