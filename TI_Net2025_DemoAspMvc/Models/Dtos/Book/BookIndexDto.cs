using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Models.Dtos.Book
{
    public class BookIndexDto
    {
        [DisplayName("ISBN")]
        public string Isbn { get; set; }

        [DisplayName("Titre")]
        public string Title { get; set; }

        [DisplayName("Auteur")]
        public string Author { get; set; }

        [DisplayName("Date de sortie")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Release { get; set; }
    }
}
