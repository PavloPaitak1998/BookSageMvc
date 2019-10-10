using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Server_WEB_Programming.Lab2.Dal.Entities;

namespace Server_WEB_Programming.Lab2.ViewModels
{
    public class SageViewModel
    {
        public Sage Sage { get; set; }
        public IList<SelectListItem> AllBooks { get; set; }

        private IList<int> _selectedBooks;

        public IList<int> SelectedBooks
        {
            get { return _selectedBooks ?? (_selectedBooks = Sage?.Books?.Select(m => m.IdBook).ToList() ?? new List<int>()); }
            set => _selectedBooks = value;
        }
    }
}