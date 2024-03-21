using Back.Data;
using Back.Model;
using Back.Model.DTO.OrderDTO;
using Back.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ApiResponce _responce;
        public OrderController(AppDbContext db)
        {
            _db = db;
            _responce = new ApiResponce();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponce>> GetOrders(string? UserId)
        {
            try
            {
                var Orderheaders = _db.OrderHeaders.Include(oh => oh.OrderDetails)
                    .ThenInclude(od => od.Book)
                    .OrderByDescending(od => od.OrderHeaderId);
                if (!string.IsNullOrEmpty(UserId))
                {
                    _responce.Result = Orderheaders.Where(o => o.UserId == UserId);

                }
                else
                {
                    _responce.Result = Orderheaders;
                }

                _responce.StatusCode = System.Net.HttpStatusCode.OK;
                _responce.IsSuccess = true;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.ErrorsMessage.Add(ex.Message);
                _responce.ErrorsMessage.Add("Catch Error");
                _responce.IsSuccess = false;
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponce>> GetOrder(int id)
        {
            try
            {
                var Orderheader = _db.OrderHeaders.Include(oh => oh.OrderDetails)
                    .ThenInclude(od => od.Book)
                    .Where(oh => oh.OrderHeaderId == id);
                if (Orderheader != null)
                {
                    _responce.Result = Orderheader;
                    _responce.StatusCode = System.Net.HttpStatusCode.OK;
                    _responce.IsSuccess = true;
                    return Ok(_responce);

                }
                else
                {
                    _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _responce.IsSuccess = true;
                    return NotFound(_responce);
                }


            }
            catch (Exception ex)
            {
                _responce.ErrorsMessage.Add(ex.Message);
                _responce.ErrorsMessage.Add("Catch Error");
                _responce.IsSuccess = false;
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponce>> CreateOrder([FromBody] OrderHeaderDTO model) {
            try
            {
                if (ModelState.IsValid)
                {
                    OrderHeader NewHeader = new()
                    {
                        UserId = model.UserId,
                        PickUpName = model.PickUpName,
                        PickUpPhone = model.PickUpPhone,
                        PickUpEmail = model.PickUpEmail,
                        OrderTotal = model.OrderTotal,
                        StripePaymentId = model.StripePaymentId,
                        OrderStatus = !string.IsNullOrEmpty(model.OrderStatus) ? SD.status_pending : model.OrderStatus,
                        OrderTotalItem = model.OrderTotalItem,
                        OrderDate = DateTime.Now

                    };
                    if (ModelState.IsValid)
                    {
                        _db.OrderHeaders.Add(NewHeader);
                        _db.SaveChanges();
                        foreach (var OrderDetailDTO in model.OrderDetailsDTO)
                        {
                            OrderDetail NewOrderDetail = new()
                            {
                                OrderHeaderId = NewHeader.OrderHeaderId,
                                BookId = OrderDetailDTO.BookId,
                                Quantity = OrderDetailDTO.Quantity,
                                IteamName = OrderDetailDTO.IteamName,
                                Price = OrderDetailDTO.Price
                            };
                            _db.orderDetails.Add(NewOrderDetail);

                        }
                        _db.SaveChanges();
                        _responce.IsSuccess = true;
                        _responce.Result = NewHeader;
                        _responce.StatusCode = System.Net.HttpStatusCode.Created;
                        return Ok(_responce);


                    }
                    else
                    {
                        _responce.ErrorsMessage.Add("Invalid Model");
                        _responce.IsSuccess = false;
                        _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        return BadRequest(_responce);

                    }
                } else
                {
                    _responce.ErrorsMessage.Add("Invalid Model");
                    _responce.IsSuccess = false;
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_responce);

                }
            }
            catch (Exception ex)
            {
                _responce.ErrorsMessage.Add(ex.Message);
                _responce.ErrorsMessage.Add("Catch Error");
                _responce.IsSuccess = false;
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);

            }
        }

        [HttpPut("id:int")]
        public async Task<ActionResult<ApiResponce>> UpdateOrder([FromBody] UpdateOrderDTO model, int id) {
            try { 
                if (ModelState.IsValid)
                {
                    OrderHeader OrderToUPD = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderId == id);
                    if (OrderToUPD != null)
                    {
                        if (!string.IsNullOrEmpty(model.PickUpName))
                        {
                            OrderToUPD.PickUpName = model.PickUpName;
                        }
                        if (!string.IsNullOrEmpty(model.PickUpPhone))
                        {
                            OrderToUPD.PickUpPhone = model.PickUpPhone;
                        }
                        if (!string.IsNullOrEmpty(model.PickUpEmail))
                        {
                            OrderToUPD.PickUpEmail = model.PickUpEmail;
                        }
                        if (!string.IsNullOrEmpty(model.StripePaymentId))
                        {
                            OrderToUPD.StripePaymentId = model.StripePaymentId;
                        }
                        if (!string.IsNullOrEmpty(model.OrderStatus))
                        {
                            OrderToUPD.OrderStatus = model.OrderStatus;
                        }
                        _db.OrderHeaders.Update(OrderToUPD);
                        _responce.IsSuccess = true;
                        _responce.StatusCode = System.Net.HttpStatusCode.OK;
                        return Ok(_responce);
                    }
                    _responce.ErrorsMessage.Add("Not Found");
                    _responce.IsSuccess = false;
                    _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_responce);

                }
                _responce.ErrorsMessage.Add("Model Error");
                _responce.IsSuccess = false;
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);
            }
            catch (Exception ex)
            {
                _responce.ErrorsMessage.Add(ex.Message);
                _responce.ErrorsMessage.Add("Catch Error");
                _responce.IsSuccess = false;
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);
            }
        
        }
    }
}

