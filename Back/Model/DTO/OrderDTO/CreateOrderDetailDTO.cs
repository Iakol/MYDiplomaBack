using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Back.Model.DTO.OrderDTO
{
    public class CreateOrderDetailDTO
    {
        
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string IteamName { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
