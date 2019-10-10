using System.Collections.Generic;

namespace Server_WEB_Programming.Lab2.Dal.Entities
{
    public class Book
    {
        public int IdBook { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Sage> Sages { get; set; }
        public ICollection<BookOrder> BookOrders { get; set; }
    }
}
