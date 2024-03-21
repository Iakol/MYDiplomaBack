using Microsoft.AspNetCore.Identity;

namespace Back.Model
{
    public class User: IdentityUser
    {
        public String Name { get; set; }
        public String SecondName { get; set; }
        public String FullName {
            get {

                return $"{Name} {SecondName}";


            }
        }
        public ICollection<BookComment> BookComments { get; set; }

    }
}
