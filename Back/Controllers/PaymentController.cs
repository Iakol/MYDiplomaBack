using Back.Data;
using Back.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private ApiResponce _responce;

        public PaymentController(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _responce = new ApiResponce();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponce>> MakePayment(string UserId)
        {
            try
            {
                ShopingCard SchopingCardToPay = _db.shopingCards.Include(sc => sc.CardItems).ThenInclude(ci => ci.Book).FirstOrDefault(sc => sc.UserId == UserId);
                if (SchopingCardToPay == null || SchopingCardToPay.CardItems.Count() == 0 || SchopingCardToPay.CardItems == null)
                {
                    _responce.ErrorsMessage.Add("Bad ShopingCard or Card is Empty");
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);

                }
                StripeConfiguration.ApiKey = _configuration.GetValue<string>("StripeSettings:SecretKey");
                SchopingCardToPay.CartTotal = SchopingCardToPay.CardItems.Sum(u => u.Quantity * u.Book.BookCost);

                PaymentIntentCreateOptions options = new() {
                    Amount = (int)(SchopingCardToPay.CartTotal * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>
                        {
                            "card",
                        },
                };
                PaymentIntentService servise = new();
                PaymentIntent response = servise.Create(options);
                SchopingCardToPay.StripePaymantId = response.Id;
                SchopingCardToPay.ClientSecret = response.ClientSecret;


                _responce.Result = SchopingCardToPay;
                _responce.StatusCode = System.Net.HttpStatusCode.OK;
                _responce.IsSuccess = true;
                return Ok(_responce);


            }
            catch (Exception ex) {
                _responce.ErrorsMessage.Add(ex.Message);
                _responce.ErrorsMessage.Add("Catch Ex");
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                return BadRequest(_responce);
            }


        
        }

    }
}
