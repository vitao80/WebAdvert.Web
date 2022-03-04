using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models
{
    public class CreateAdvertViewModel
    {
        [Required(ErrorMessage = "Digitar o titulo.")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Digitar o preço")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
    }
}
