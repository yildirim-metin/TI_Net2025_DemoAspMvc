namespace TI_Net2025_DemoAspMvc.Models.Entities
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public DateTime Release {  get; set; }
    }
}
