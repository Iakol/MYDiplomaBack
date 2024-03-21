using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Back.Model.DTO.OrderDTO
{
    public class OrderHeaderDTO
    {
        
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpPhone { get; set; }
        [Required]
        public string PickUpEmail { get; set; }

        public string UserId { get; set; }

        public double OrderTotal { get; set; }
        public string StripePaymentId { get; set; }
        public string OrderStatus { get; set; }
        public int OrderTotalItem { get; set; }
        public IEnumerable<CreateOrderDetailDTO> OrderDetailsDTO { get; set; }
    }
}
