using System.ComponentModel;

namespace TI_Net2025_DemoAspMvc.Models.Dtos.Author
{
    public class AuthorDto
    {
        public int Id { get; set; }

        [DisplayName("Prénom")]
        public string Firstname { get; set; }

        [DisplayName("Nom")]
        public string Lastname { get; set; }
    }
}
