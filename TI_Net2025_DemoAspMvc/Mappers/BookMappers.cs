using TI_Net2025_DemoAspMvc.Models.Dtos.Book;
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
                Author = b.Author.Lastname + " " + b.Author.Firstname,
                Release = b.Release,
            };
        }

        public static BookDetailDto ToBookDetailDto(this Book b)
        {
            return new BookDetailDto()
            {
                Isbn = b.Isbn,
                Title = b.Title,
                Author = b.Author.ToAuthorDto(),
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
                AuthorId = form.AuthorId,
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
                AuthorId = book.AuthorId,
                Release = book.Release,
                Description = book.Description,
            };
        }
    }
}
