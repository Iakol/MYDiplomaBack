using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public bool isEBook { get; set; }
        public int Page { get; set; }
        public int BookCost { get; set; }
        public string Language { get; set; }
        public string ImgRef { get; set; }
        public int? RelatedBookId { get; set; }
        public Book RelatedBook { get; set; }
        public DateTime DataAdd { get; set; }
        public ICollection<Author> HasAuthors { get; set; }
        public ICollection<Subject> HasSubjects { get; set; }
        public ICollection<BookComment> BookComments { get; set; }
        public ICollection<CardItem> CardItems { get; set; }


    }
}
