using Back.Data;
using Back.Model;
using Back.Model.DTO.AuthorDTO;
using Back.Model.DTO.BookDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/Book")]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ApiResponce _responce;
        public BookController(AppDbContext db) { 
            _db = db;
            _responce = new ApiResponce();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooks() {
            if (!_db.Books.ToList().Any()) {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                return BadRequest(_responce);

            }
            _responce.Result = _db.Books;
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_responce);
        }
        [HttpGet("{id:int}", Name = "GetOneBook") ]
        public async Task<IActionResult> GetOneBook(int id)
        {
           if (id == 0)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);
            }
            Book book = _db.Books.Include(b=> b.HasAuthors).Include(b=>b.RelatedBook).Include(b => b.BookComments).ThenInclude(u=> u.User).Include(b => b.HasSubjects).FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_responce);
            }
            _responce.Result = book;
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_responce);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponce>> AddNewBook([FromForm]BookDTO bookDTO) {
            try
            {
                if (ModelState.IsValid)
                {
                    Book NewBookCreated = new Book
                    {
                        Name = bookDTO.Name,
                        Description = bookDTO.Description,
                        Year = bookDTO.Year,
                        ISBN = bookDTO.ISBN,
                        isEBook = bookDTO.isEBook,
                        Page = bookDTO.Page,
                        BookCost = bookDTO.BookCost,
                        Language = bookDTO.Language,
                        ImgRef = bookDTO.ImgRef,
                        DataAdd = DateTime.UtcNow,
                       
                        RelatedBookId = bookDTO.RelatedBookId,
                        
                        HasAuthors = new List<Author>(),
                        HasSubjects = new List<Subject>()
                    };
                    Book RelatedBook = _db.Books.FirstOrDefault(b => b.Id == bookDTO.RelatedBookId);
                    if (RelatedBook != null) {
                        RelatedBook.RelatedBook = NewBookCreated;
                        NewBookCreated.RelatedBookId = bookDTO.RelatedBookId;
                    }

                    if (bookDTO.HasAuthors != null)
                    {
                        foreach (var authorId in bookDTO.HasAuthors)
                        {
                            var author = _db.Authors.FirstOrDefault(a => a.AuthorId == authorId);
                            if (author != null)
                            {
                                NewBookCreated.HasAuthors.Add(author);
                            }
                            else
                            {
                                _responce.ErrorsMessage.Add("Author  with id " + authorId + " Not found and added");
                            }
                        }
                    }
                    if (bookDTO.HasSubjects != null)
                    {
                        foreach (var SubjectsId in bookDTO.HasSubjects)
                        {
                            var Subject = _db.Subjects.FirstOrDefault(a => a.SubjectId == SubjectsId);
                            if (Subject != null)
                            {
                                NewBookCreated.HasSubjects.Add(Subject);
                            }
                            else
                            {
                                _responce.ErrorsMessage.Add("Subject with id " + SubjectsId + " Not found and added");
                            }
                        }
                    }
                    _db.Books.Add(NewBookCreated);
                    _db.SaveChanges();
                    _responce.IsSuccess = true;
                    _responce.StatusCode = System.Net.HttpStatusCode.Created;
                    return CreatedAtRoute("GetOneBook",new {id = NewBookCreated.Id});
                }
                else {
                    _responce.ErrorsMessage.Add("Bad Model");
                    _responce.IsSuccess = false;
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
            }
            catch (Exception e) {
                _responce.IsSuccess = false;
                _responce.ErrorsMessage = new List<string> { e.Message };
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);
            
            
            }
            
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponce>> UpdateBook(int id, [FromForm] BookDTO bookDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Book book = _db.Books.Include(b=> b.HasSubjects).Include(b => b.HasAuthors).FirstOrDefault(a => a.Id == id);
                    if (book != null)
                    {
                        if (bookDTO.Name != null)
                        {
                            book.Name = bookDTO.Name;
                        }
                        if (bookDTO.Description != null)
                        {
                            book.Description = bookDTO.Description;
                        }
                        if (bookDTO.Year != null)
                        {
                            book.Year = bookDTO.Year;
                        }
                        if (bookDTO.ISBN != null)
                        {
                            book.ISBN = bookDTO.ISBN;
                        }
                        if (bookDTO.isEBook != null)
                        {
                            book.isEBook = bookDTO.isEBook;
                        }
                        if (bookDTO.Page != null)
                        {
                            book.Page = bookDTO.Page;
                        }
                        if (bookDTO.BookCost != null)
                        {
                            book.BookCost = bookDTO.BookCost;
                        }
                        if (bookDTO.RelatedBookId != null)
                        {
                            book.RelatedBookId = bookDTO.RelatedBookId;
                            Book RelatedBook = _db.Books.FirstOrDefault(b => b.Id == bookDTO.RelatedBookId);
                            RelatedBook.RelatedBook = book;

                        }
                        if (bookDTO.Language != null)
                        {
                            book.Language = bookDTO.Language;
                        }
                        if (bookDTO.ImgRef != null)
                        {
                            book.ImgRef = bookDTO.ImgRef;
                        }
                        if (bookDTO.HasAuthors != null)
                        {
                            foreach (var authorId in bookDTO.HasAuthors)
                            {
                                var author = _db.Authors.FirstOrDefault(a => a.AuthorId == authorId);
                                if (author != null)
                                {
                                    book.HasSubjects.Clear();
                                    book.HasAuthors.Add(author);
                                }
                                else
                                {
                                    _responce.ErrorsMessage.Add("Author  with id " + authorId + " Not found and added");
                                }
                            }
                        }
                        if (bookDTO.HasSubjects != null)
                        {
                            foreach (var SubjectsId in bookDTO.HasSubjects)
                            {
                                var Subject = _db.Subjects.FirstOrDefault(a => a.SubjectId == SubjectsId);
                                if (Subject != null)
                                {
                                    book.HasSubjects.Clear();
                                    book.HasSubjects.Add(Subject);
                                }
                                else
                                {
                                    _responce.ErrorsMessage.Add("Subject with id " + SubjectsId + " Not found and added");
                                }
                            }
                        }

                        _db.Books.Update(book);
                        _db.SaveChanges();
                        _responce.IsSuccess = true;
                        _responce.StatusCode = System.Net.HttpStatusCode.OK;
                        return CreatedAtAction(nameof(GetOneBook), new { id = book.Id }, book); 

                    }
                    _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _responce.IsSuccess = false;
                    _responce.ErrorsMessage.Add("Book not find");
                    return NotFound(_responce);

                }
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage.Add("Model not valid");
                return BadRequest(_responce);

            }
            catch (Exception ex)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                return BadRequest(_responce);


            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponce>> DeleteBook(int id)
        {
            Book book = _db.Books.FirstOrDefault(a => a.Id == id);
            if (book == null)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage.Add("book not find");
                return NotFound(_responce);
            }
            _db.Remove(book);
            _db.SaveChanges();
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            _responce.IsSuccess = true;
            _responce.Result = "book delete";
            return Ok(_responce);
        }

        [HttpPost("AddComentar")]
        public async Task<ActionResult<ApiResponce>> AddComentar(int Bookid, string UserId, string Text, int Raiting) {
            try {
                if (!_db.Users.Any(u => u.Id == UserId)) {
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    _responce.ErrorsMessage.Add("Bad user");
                    return BadRequest(_responce);
                }
                if (!_db.Books.Any(b => b.Id == Bookid))
                {
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    _responce.ErrorsMessage.Add("Book Not found");
                    return BadRequest(_responce);
                }

                Book BookNewComment = _db.Books.Include(c => c.BookComments).FirstOrDefault(b => b.Id == Bookid);
                BookComment bookComment = new BookComment
                {
                    BookId = BookNewComment.Id,
                    UserId = UserId,
                    Text = Text,
                    Raiting = Raiting,
                    Book = null,
                    User = null
                };
                BookNewComment.BookComments.Add(bookComment);
                _db.SaveChanges();
                _responce.StatusCode = System.Net.HttpStatusCode.OK;
                _responce.IsSuccess = true;
                return Ok(_responce);

            }
            catch(Exception ex) {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                return BadRequest(_responce);

            }
        
        
        }

        [HttpDelete("DeleteComentar")]
        public async Task<ActionResult<ApiResponce>> DeleteComentar( int Id)
        {
            try
            {
                
                if (!_db.BookComments.Any(b => b.BookCommentId == Id))
                {
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    _responce.ErrorsMessage.Add("Coment Not found");
                    return BadRequest(_responce);
                }

                BookComment commenttoremove = _db.BookComments.FirstOrDefault(b => b.BookCommentId == Id);
                _db.BookComments.Remove(commenttoremove);
                _db.SaveChanges();
                _responce.StatusCode = System.Net.HttpStatusCode.OK;
                _responce.IsSuccess = true;
                return Ok(_responce);

            }
            catch (Exception ex)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                return BadRequest(_responce);

            }


        }

        [HttpGet("GetBookComentar")]
        public async Task<ActionResult<ApiResponce>> GetComentar(int BookId)
        {
            try
            {

                if (!_db.Books.Any(b => b.Id == BookId))
                {
                    _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responce.IsSuccess = false;
                    _responce.ErrorsMessage.Add("Book Not found");
                    return BadRequest(_responce);
                }
                
                if (_db.BookComments.Where(b => b.Book.Id == BookId).ToList().IsNullOrEmpty())
                {
                    _responce.StatusCode = System.Net.HttpStatusCode.OK;
                    _responce.IsSuccess = true;
                    _responce.Result = new List<string> { };
                    return Ok(_responce);
                }

                _responce.StatusCode = System.Net.HttpStatusCode.OK;
                _responce.Result = _db.BookComments.Where(b => b.Book.Id == BookId).ToList();
                _responce.IsSuccess = true;
                return Ok(_responce);

            }
            catch (Exception ex)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage = new List<string> { ex.Message };
                return BadRequest(_responce);

            }


        }

    }
    
}
