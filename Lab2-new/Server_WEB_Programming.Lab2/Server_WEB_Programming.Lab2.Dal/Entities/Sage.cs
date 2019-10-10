using System.Collections.Generic;

namespace Server_WEB_Programming.Lab2.Dal.Entities
{
    public class Sage
    {
        public int IdSage { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public byte[] Photo { get; set; }
        public string City { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
