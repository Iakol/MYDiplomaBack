using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Model
{
    public class ShopingCard
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; } = new();


        public ICollection<CardItem> CardItems { get; set; }

        public string StripePaymantId { get; set; }

        public string ClientSecret { get; set; }

        public double CartTotal { get; set; }
    }
}
