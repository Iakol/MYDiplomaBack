using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Model
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set;}
        public int Quantity { get; set; }
        public string IteamName { get; set; }

        public double Price { get; set; }
    }
}
