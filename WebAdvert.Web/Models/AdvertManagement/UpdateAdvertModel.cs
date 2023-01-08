using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.AdvertManagement
{
    public class UpdateAdvertModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
    }
}
