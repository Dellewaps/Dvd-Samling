using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVD_Samling.Models.ViewModels
{
    public class MovieItemViewModel
    {
        public MovieItem MovieItem { get; set; }
        public IEnumerable<Genre> genre { get; set; }
    }
}
