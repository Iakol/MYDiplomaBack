using Back.Data;
using Back.Model;
using Back.Model.DTO.AuthorDTO;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers
{
    [ApiController]
    [Route("api/Subject")]
    public class SubjectController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ApiResponce _responce;
        public SubjectController(AppDbContext db) {
            _db = db;
            _responce = new ApiResponce();


        }
        [HttpGet]
        public async Task<IActionResult> GetAllSubject()
        {
            if (!_db.Subjects.ToList().Any()) {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_responce);
            }
            _responce.Result = _db.Subjects;
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_responce);
        }


        [HttpGet("{id:int}", Name = "GetOneSubject")]
        public async Task<IActionResult> GetOneSubjects(int id) {
            if (id == 0) {
                _responce.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_responce);

            }
            Subject subject = _db.Subjects.FirstOrDefault(x => x.SubjectId == id);
            if (subject == null)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(_responce);
            }
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            _responce.Result = subject;
            return Ok(_responce);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponce>> AddNewSubject([FromForm] SubjectDTO subjectDTO) {
            try {
                if (ModelState.IsValid) {
                    Subject NewSubjectCreate = new Subject
                    {
                        SubjectName = subjectDTO.SubjectName,
                        HasBooks = subjectDTO.HasBooks

                    };
                    _db.Subjects.Add(NewSubjectCreate);
                    _db.SaveChanges();
                    _responce.StatusCode = System.Net.HttpStatusCode.Created;
                    _responce.Result = NewSubjectCreate;
                    _responce.IsSuccess = true;
                    return CreatedAtRoute("GetOneAuthor", new { id = NewSubjectCreate.SubjectId }, _responce);



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


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponce>> DeleteSubject(int id) {
            Subject subject = _db.Subjects.FirstOrDefault(a => a.SubjectId == id);
            if (subject == null)
            {
                _responce.StatusCode = System.Net.HttpStatusCode.NotFound;
                _responce.IsSuccess = false;
                _responce.ErrorsMessage.Add("subject not find");
                return NotFound(_responce);
            }
            _db.Remove(subject);
            _db.SaveChanges();
            _responce.StatusCode = System.Net.HttpStatusCode.OK;
            _responce.IsSuccess = true;
            _responce.Result = "subject delete";
            return Ok(_responce);
        }
    }
}
