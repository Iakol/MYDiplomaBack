using Back.Data;
using Back.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back.Controllers
{
    [Route("api/ShopingCard")]
    [ApiController]
    public class ShopingCardController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ApiResponce _responce;

        public ShopingCardController(AppDbContext db)
        {
            _db = db;
            _responce = new ApiResponce();
        }

        [HttpPost("AddItemInCard")]
        public async Task<ActionResult<ApiResponce>> AddAndUpdateItem(string UserId, int BookId, int UpdateQuantity)
        {
            try
            {
                if (!_db.Users.Any(u => u.Id == UserId))
                {
                    _responce.ErrorsMessage.Add("User not Find");
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }
                ShopingCard shopingCard = _db.shopingCards.FirstOrDefault(s => s.UserId == UserId);
                if ((shopingCard == null))
                {
                    shopingCard = new ShopingCard { UserId = UserId, User = null };
                    _db.shopingCards.Add(shopingCard);
                    _db.SaveChanges();
                }


                shopingCard = _db.shopingCards.Include(s => s.CardItems).FirstOrDefault(s => s.UserId == UserId);
                Book Getbook = _db.Books.FirstOrDefault(b => b.Id == BookId);
                if ((shopingCard.CardItems.FirstOrDefault(c => c.BookId == BookId) == null) && UpdateQuantity > 0)
                {
                    CardItem newCartBook = new CardItem
                    {
                        BookId = BookId,
                        Quantity = Getbook.isEBook ? 1 : UpdateQuantity,
                        ShopingCardId = shopingCard.Id,
                        Book = null
                    };
                    shopingCard.CardItems.Add(newCartBook);
                    _db.SaveChanges();
                    _responce.StatusCode = System.Net.HttpStatusCode.OK;
                    _responce.IsSuccess = true;
                    return Ok(_responce);
                }
                else if ((shopingCard.CardItems.FirstOrDefault(c => c.BookId == BookId) == null) && UpdateQuantity <= 0)
                {
                    _responce.ErrorsMessage.Add("Error adding in cart");
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }
                else if (shopingCard.CardItems.FirstOrDefault(c => c.BookId == BookId) != null)
                {
                    CardItem ItemInCartToUpdate = shopingCard.CardItems.FirstOrDefault(c => c.BookId == BookId);
                    if (!Getbook.isEBook)
                    {
                        ItemInCartToUpdate.Quantity += UpdateQuantity;
                        if (ItemInCartToUpdate.Quantity <= 0)
                        {
                            shopingCard.CardItems.Remove(ItemInCartToUpdate);
                            _db.SaveChanges();
                            _responce.StatusCode = System.Net.HttpStatusCode.OK;
                            _responce.ErrorsMessage.Add("The item remove");
                            _responce.IsSuccess = true;
                            return Ok(_responce);

                        }
                        else
                        {
                            _db.SaveChanges();
                            _responce.StatusCode = System.Net.HttpStatusCode.OK;
                            _responce.ErrorsMessage.Add("The item update");
                            _responce.IsSuccess = true;
                            return Ok(_responce);
                        }
                    }
                    else {
                        if(UpdateQuantity <= 0) {
                            shopingCard.CardItems.Remove(ItemInCartToUpdate);
                            _db.SaveChanges();
                            _responce.StatusCode = System.Net.HttpStatusCode.OK;
                            _responce.ErrorsMessage.Add("The item remove");
                            _responce.IsSuccess = true;
                            return Ok(_responce);
                        }else
                        {
                            if (ItemInCartToUpdate.Quantity > 1)
                            {
                                ItemInCartToUpdate.Quantity = 1;
                                _db.SaveChanges();
                            }
                            
                            _responce.StatusCode = System.Net.HttpStatusCode.OK;
                            _responce.ErrorsMessage.Add("Item not update because this is Ebook and need only 1");
                            _responce.IsSuccess = true;
                            return Ok(_responce);
                        }
                        

                    }
                }
                else
                {
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.ErrorsMessage.Add("Some Error");
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }

            }
            catch (Exception ex)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage.Add(ex.Message);
                _responce.ErrorsMessage.Add("Cath error");
                _responce.IsSuccess = false;
                return BadRequest(_responce);
            }
        }


        [HttpGet("GetCart")]
        public async Task<ActionResult<ApiResponce>> GetCart(string UserId) {
            try { 
                if(!_db.Users.Any(u => u.Id == UserId))
                {
                    _responce.StatusCode=System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    _responce.ErrorsMessage.Add("User not found");
                    return BadRequest(_responce);
                }
                

                ShopingCard ReturnShopingCard = _db.shopingCards.Include(u => u.CardItems).ThenInclude(c => c.Book).FirstOrDefault(s => s.UserId == UserId);
                if (ReturnShopingCard.CardItems != null && ReturnShopingCard.CardItems.Count > 0) {
                    ReturnShopingCard.CartTotal = ReturnShopingCard.CardItems.Sum(u => u.Quantity * u.Book.BookCost);
                }
                _responce.StatusCode = System.Net.HttpStatusCode.OK;
                _responce.IsSuccess = true;
                _responce.Result = ReturnShopingCard;
                return Ok(_responce);

            }
            catch (Exception e){
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage.Add($"{e.Message}");
                return Ok(_responce);

            }
        }
    }



    
}
