using System.ComponentModel.DataAnnotations;

namespace WebApi.Model
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(10,1000,ErrorMessage ="Price shoul between 10 to 1000")]
        public decimal Price { get; set; }
    }
}
