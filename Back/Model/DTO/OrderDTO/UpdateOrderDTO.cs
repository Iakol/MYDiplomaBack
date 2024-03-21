using System.ComponentModel.DataAnnotations;

namespace Back.Model.DTO.OrderDTO
{
    public class UpdateOrderDTO
    {
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpPhone { get; set; }
        [Required]
        public string PickUpEmail { get; set; }


        public string StripePaymentId { get; set; }
        public string OrderStatus { get; set; }
    }
}
