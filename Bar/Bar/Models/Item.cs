using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bar.Models
{
    public class Item
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Description")]
        [AllowHtml]
        public string Description { get; set; }

        public Boolean HiddenTag { get; set; }


    }
}