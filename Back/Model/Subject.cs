namespace Back.Model
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public ICollection<Book> HasBooks { get; set; }


    }
}
