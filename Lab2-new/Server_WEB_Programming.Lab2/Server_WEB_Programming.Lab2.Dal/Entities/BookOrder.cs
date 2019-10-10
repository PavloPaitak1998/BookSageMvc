using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_WEB_Programming.Lab2.Dal.Entities
{
    public class BookOrder
    {
        public int BookOrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public Book Book { get; set; }
    }
}
