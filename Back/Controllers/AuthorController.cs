using Back.Data;
using Back.Model;
using Back.Model.DTO.AuthorDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/Author")]
    public class AuthorController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ApiResponce _responce;
        public AuthorController(AppDbContext db) {
            _db = db;
            _responce = new ApiResponce();


        }
        [HttpGet]
        public async Task<IActionResult> GetAllAuthor()
        {
            if (!_db.Authors.ToList().Any()) {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_responce);
            }
            _responce.Result = _db.Authors.Include(a=>a.HasBooks);
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_responce);
        }


        [HttpGet("{id:int}", Name = "GetOneAuthor")]
        public async Task<IActionResult> GetOneAuthor(int id) {
            if (id == 0) {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);

            }
            Author author = _db.Authors.FirstOrDefault(x => x.AuthorId == id);
            if (author == null)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_responce);
            }
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            _responce.Result = author;
            return Ok(_responce);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponce>> AddNewAuthor([FromForm] AuthorDTO authordto) {
            try {
                if (ModelState.IsValid) {
                    Author NewAuthorCreate = new Author
                    {
                        FirstName = authordto.FirstName,
                        SecondName = authordto.SecondName,
                        Discipline = authordto.Discipline,
                        AboutAuthor = authordto.AboutAuthor,
                        HasBooks = authordto.HasBooks
                    };
                    _db.Authors.Add(NewAuthorCreate);
                    _db.SaveChanges();
                    _responce.StatusCode = System.Net.HttpStatusCode.Created;
                    _responce.Result = NewAuthorCreate;
                    _responce.IsSuccess = true;
                    return CreatedAtRoute("GetOneAuthor", new { id = NewAuthorCreate.AuthorId }, _responce);



                }
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage.Add("Bad Model");
                return BadRequest(_responce);
            }
            catch (Exception ex) {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                _responce.IsSuccess = false;
                return BadRequest(_responce);


            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponce>> UpdateAuthor(int id, [FromForm] AuthorDTO authorDTO) {
            try
            {
                if (ModelState.IsValid) {
                    Author author = _db.Authors.FirstOrDefault(a => a.AuthorId == id);
                    if (author != null) 
                    {
                        if (authorDTO.FirstName != null) {
                            author.FirstName = authorDTO.FirstName;
                        }
                        if (authorDTO.SecondName != null)
                        {
                            author.SecondName = authorDTO.SecondName;
                        }
                        if (authorDTO.Discipline != null)
                        {
                            author.Discipline = authorDTO.Discipline;
                        }
                        if (authorDTO.AboutAuthor != null)
                        {
                            author.AboutAuthor = authorDTO.AboutAuthor;
                        }
                        _db.Authors.Update(author);
                        _db.SaveChanges();
                        _responce.IsSuccess = true;
                        _responce.Result = author;
                        _responce.StatusCode=System.Net.HttpStatusCode.OK;
                        return Ok(_responce);

                    }
                    _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _responce.IsSuccess=false;
                    _responce.ErrorsMessage.Add("Author not find");
                    return NotFound(_responce);

                }
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage.Add("Model not valid");
                return BadRequest(_responce);

            }
            catch (Exception ex){
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                return BadRequest(_responce);


            } 
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponce>> DeleteAuthor(int id) {
            Author author = _db.Authors.FirstOrDefault(a => a.AuthorId == id);
            if (author == null)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage.Add("Author not find");
                return NotFound(_responce);
            }
            _db.Remove(author);
            _db.SaveChanges();
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            _responce.IsSuccess = true;
            _responce.Result = "Author delete";
            return Ok(_responce);
        }
    }
}
