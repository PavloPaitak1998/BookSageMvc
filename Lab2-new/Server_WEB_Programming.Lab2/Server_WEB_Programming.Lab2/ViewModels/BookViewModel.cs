using Server_WEB_Programming.Lab2.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Server_WEB_Programming.Lab2.ViewModels
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public IList<SelectListItem> AllSages { get; set; }

        private IList<int> _selectedSages;

        public IList<int> SelectedSages
        {
            get { return _selectedSages ?? (_selectedSages = Book?.Sages?.Select(m => m.IdSage).ToList() ?? new List<int>()); }
            set => _selectedSages = value;
        }
    }
}