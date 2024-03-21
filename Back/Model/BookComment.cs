using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Model
{
    public class BookComment
    {
            public int BookCommentId { get; set; }
            public int BookId { get; set; }
            public string UserId { get; set; }
            public string Text { get; set; }
            [Range(1, 5)]
            public int Raiting { get; set; }
            public Book Book { get; set; }
            public User User { get; set; }
        
    }
}
