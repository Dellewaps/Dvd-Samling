using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DVD_Samling.Models
{
    public class RentalCart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int MovieItemId { get; set; }

        [NotMapped]
        [ForeignKey("MovieItemId")]
        public virtual MovieItem MovieItem { get; set; }
    }
}
