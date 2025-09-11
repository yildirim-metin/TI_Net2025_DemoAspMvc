using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TI_Net2025_DemoAspMvc.Validators;

namespace TI_Net2025_DemoAspMvc.Models.Dtos
{
    public class BookFormDto
    {
        [DisplayName("ISBN")]
        [Required(ErrorMessage = "Champ requis")]
        [Isbn]
        public string Isbn { get; set; }

        [DisplayName("Titre")]
        [Required(ErrorMessage = "Champ requis")]
        [MaxLength(100)]
        public string Title { get; set; }

        [DisplayName("Description")]
        [MaxLength(255)]
        public string Description { get; set; }

        [DisplayName("Auteur")]
        [Required(ErrorMessage = "Champ requis")]
        [MaxLength(100)]
        public string Author { get; set; }

        [DisplayName("Date de sortie")]
        [Required(ErrorMessage = "Champ requis")]
        [DataType(DataType.Date)]
        public DateTime? Release { get; set; } = null;
    }
}
