using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVD_Samling.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<MovieItem> MovieItem { get; set; }
        public IEnumerable<Genre> Genre { get; set; }
    }
}
