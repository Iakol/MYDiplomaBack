using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Model
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public string PickUpName {get; set; }
        public string PickUpPhone { get; set; }
        public string PickUpEmail { get; set; }

        public string UserId { get; set; }
        public User user { get; set; }

        public double OrderTotal { get; set; }
        public string StripePaymentId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public int OrderTotalItem{ get; set;}
        public IEnumerable<OrderDetail> OrderDetails { get; set; }


    }
}
