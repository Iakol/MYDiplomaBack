namespace Back.Model
{
    public class Author

    {
        public int AuthorId { get; set; }
        public String FirstName { get;set;}
        public String SecondName { get; set; }
        public String FullName {
            get {

                return $"{FirstName} {SecondName}";
            }
        }
        public String Discipline { get; set; }
        public String AboutAuthor { get; set; }
        public ICollection<Book> HasBooks { get; set; }


    }
}
