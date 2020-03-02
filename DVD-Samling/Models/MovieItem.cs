using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DVD_Samling.Models
{
    public class MovieItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Subtitels { get; set; }

        public string Agelimit { get; set; }
        public enum EAge { All=0, Age7=1, Age11=2, Age15=3}

        public string Image { get; set; }
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }
    }
}
