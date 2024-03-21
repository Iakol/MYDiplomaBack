using Back.Data;
using Back.Model;
using Back.Model.DTO.IdentityDTO;
using Back.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Back.Controllers
{
    [Route("api/Identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _UserManger;
        private readonly RoleManager<IdentityRole> _RoleManger;
        private ApiResponce _responce;
        private string SecretKey;

        public IdentityController(AppDbContext db, IConfiguration configuration, UserManager<User> UserManger, RoleManager<IdentityRole> RoleManger) {
            _db = db;
            _UserManger = UserManger;
            _RoleManger = RoleManger;
            _responce = new ApiResponce();
            SecretKey = configuration.GetValue<string>("ApiSettings:TokenKEY");

        }

        [HttpPost("registerUser")]
        public async Task<ActionResult<ApiResponce>> RegisterUser([FromBody] RegisterDTO model)
        {
            try
            {

                if (!await _RoleManger.RoleExistsAsync(SD.Admin) || !await _RoleManger.RoleExistsAsync(SD.User))
                {
                    await _RoleManger.CreateAsync(new IdentityRole { Name = SD.Admin });
                    await _RoleManger.CreateAsync(new IdentityRole { Name = SD.User });
                }
                if (ModelState.IsValid)
                {
                    if (_UserManger.FindByNameAsync(model.UserName) != null || _UserManger.FindByNameAsync(model.Email) != null)
                    {
                        User newUserCreate = new User
                        {

                            UserName = model.UserName,
                            Email = model.Email,
                            NormalizedEmail = model.Email.ToUpper(),
                            NormalizedUserName = model.UserName.ToUpper()

                        };

                        var result = await _UserManger.CreateAsync(newUserCreate, model.Password);

                        if (result.Succeeded)
                        {
                            await _UserManger.AddToRoleAsync(newUserCreate, SD.User);
                            _responce.StatusCode = System.Net.HttpStatusCode.OK;
                            _responce.Result = newUserCreate;
                            _responce.IsSuccess = true;
                            return Ok(_responce);
                        }
                        _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _responce.ErrorsMessage.Add(result.ToString());
                        _responce.IsSuccess = false;
                        return BadRequest(_responce);
                    }
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.ErrorsMessage.Add("UserName or Email alredy is exist");
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);

                }
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage.Add("Not valid Model");
                _responce.IsSuccess = false;
                return BadRequest(_responce);
            }
            catch (Exception ex) {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                _responce.ErrorsMessage.Add("Catch report");
                _responce.IsSuccess = false;
                return BadRequest(_responce);

            }
        }

        [HttpPost("registerAdmin")]
        public async Task<ActionResult<ApiResponce>> RegisterAdmin([FromBody] RegisterDTO model)
        {
            try
            {

                if (!await _RoleManger.RoleExistsAsync(SD.Admin) || !await _RoleManger.RoleExistsAsync(SD.User))
                {
                    await _RoleManger.CreateAsync(new IdentityRole { Name = SD.Admin });
                    await _RoleManger.CreateAsync(new IdentityRole { Name = SD.User });
                }
                if (ModelState.IsValid)
                {
                    if (_UserManger.FindByNameAsync(model.UserName) != null || _UserManger.FindByNameAsync(model.Email) != null)
                    {
                        User newUserCreate = new User
                        {

                            UserName = model.UserName,
                            Email = model.Email,
                            NormalizedEmail = model.Email.ToUpper(),
                            NormalizedUserName = model.UserName.ToUpper()

                        };

                        var result = await _UserManger.CreateAsync(newUserCreate, model.Password);

                        if (result.Succeeded)
                        {
                            await _UserManger.AddToRoleAsync(newUserCreate, SD.Admin);
                            _responce.StatusCode = System.Net.HttpStatusCode.OK;
                            _responce.Result = newUserCreate;
                            _responce.IsSuccess = true;
                            return Ok(_responce);
                        }
                        _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _responce.ErrorsMessage.Add(result.ToString());
                        _responce.IsSuccess = false;
                        return BadRequest(_responce);
                    }
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.ErrorsMessage.Add("UserName or Email alredy is exist");
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);

                }
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage.Add("Not valid Model");
                _responce.IsSuccess = false;
                return BadRequest(_responce);
            }
            catch (Exception ex)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                _responce.ErrorsMessage.Add("Catch report");
                _responce.IsSuccess = false;
                return BadRequest(_responce);

            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponce>> login([FromBody] LoginRequestDTO model) {
            try
            {
                if (await _UserManger.FindByEmailAsync(model.userName) != null || await _UserManger.FindByNameAsync(model.userName) != null) {
                    User logingUser =  _db.Users.FirstOrDefault(u => (u.UserName == model.userName || u.Email == model.userName));
                    if (await _UserManger.CheckPasswordAsync(logingUser, model.password)) {
                        //add JWT token
                        var roles = await _UserManger.GetRolesAsync(logingUser);
                        JwtSecurityTokenHandler tokenhendler = new();
                        byte[] key = Encoding.ASCII.GetBytes(SecretKey);
                        SecurityTokenDescriptor tokenDescriptor = new()
                        {
                            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                                new Claim("UserName", logingUser.UserName),
                                new Claim("id", logingUser.Id),
                                new Claim(ClaimTypes.Email, logingUser.Email),
                                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                            }),
                            Expires = DateTime.UtcNow.AddDays(SD.TokenDay),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        SecurityToken token = tokenhendler.CreateJwtSecurityToken(tokenDescriptor);
                        LoginResponceDTO responeDTO = new LoginResponceDTO
                        {
                            Email = logingUser.Email,
                            Token = tokenhendler.WriteToken(token)
                        };
                        if(responeDTO.Email != null || string.IsNullOrEmpty(responeDTO.Token))
                        {
                            _responce.StatusCode = System.Net.HttpStatusCode.OK;
                            _responce.IsSuccess = true;
                            _responce.Result = responeDTO;
                            return Ok(_responce);
                        }
                        _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _responce.ErrorsMessage.Add("Bad Tokin creating");
                        _responce.IsSuccess = false;
                        return BadRequest(_responce);


                    }
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.ErrorsMessage.Add("Login or password not valid");
                    _responce.IsSuccess = false;
                    return BadRequest(_responce);
                }
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage.Add("Login or password not valid");
                _responce.IsSuccess = false;
                return BadRequest(_responce);


            }
            catch (Exception ex)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                _responce.ErrorsMessage.Add("Catch report");
                _responce.IsSuccess = false;
                return BadRequest(_responce);


            }
        
        
        
        }




    }
}
