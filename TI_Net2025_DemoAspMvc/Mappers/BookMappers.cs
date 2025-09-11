using TI_Net2025_DemoAspMvc.Models.Dtos;
using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Mappers
{
    public static class BookMappers
    {

        public static BookIndexDto ToBookIndexDto(this Book b)
        {
            return new BookIndexDto()
            {
                Isbn = b.Isbn,
                Title = b.Title,
                Author = b.Author,
                Release = b.Release,
            };
        }

        public static BookDetailDto ToBookDetailDto(this Book b)
        {
            return new BookDetailDto()
            {
                Isbn = b.Isbn,
                Title = b.Title,
                Author = b.Author,
                Release = b.Release,
                Description = b.Description,
            };
        }

        public static Book ToBook(this BookFormDto form)
        {
            return new Book()
            {
                Isbn = form.Isbn,
                Title = form.Title,
                Author = form.Author,
                Release = (DateTime) form.Release,
                Description = form.Description,
            };
        }

        public static BookFormDto ToBookFormDto(this Book book)
        {
            return new BookFormDto()
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Author = book.Author,
                Release = book.Release,
                Description = book.Description,
            };
        }
    }
}
