namespace Back.Model.DTO.AuthorDTO
{
    public class AuthorDTO

    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Discipline { get; set; }
        public string AboutAuthor { get; set; }
        public List<Book> HasBooks { get; set; }


    }
}
