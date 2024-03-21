namespace Back.Model.DTO.BookDTO
{
    public class BookDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public bool isEBook { get; set; }
        public int Page { get; set; }
        public int BookCost { get; set; }
        public int? RelatedBookId { get; set; }
         public string Language { get; set; }
        public string ImgRef { get; set; }
        public ICollection<int> HasAuthors { get; set; }
        public ICollection<int> HasSubjects { get; set; }







    }
}
