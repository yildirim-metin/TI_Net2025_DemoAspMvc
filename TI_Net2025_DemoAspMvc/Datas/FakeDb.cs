using TI_Net2025_DemoAspMvc.Models.Entities;

namespace TI_Net2025_DemoAspMvc.Datas
{
    public static class FakeDb
    {
        public static List<Author> Authors { get; } = new List<Author>()
        {
            new Author { Id = 1, Firstname = "Brian W.", Lastname = "Kernighan" },
            new Author { Id = 2, Firstname = "Dennis M.", Lastname = "Ritchie" },
            new Author { Id = 3, Firstname = "Andrew", Lastname = "Hunt" },
            new Author { Id = 4, Firstname = "David", Lastname = "Thomas" },
            new Author { Id = 5, Firstname = "Robert C.", Lastname = "Martin" },
            new Author { Id = 6, Firstname = "Eric", Lastname = "Freeman" },
            new Author { Id = 7, Firstname = "Elisabeth", Lastname = "Robson" },
            new Author { Id = 8, Firstname = "Martin", Lastname = "Fowler" },
            new Author { Id = 9, Firstname = "Eric", Lastname = "Evans" },
            new Author { Id = 10, Firstname = "Kathy", Lastname = "Sierra" },
            new Author { Id = 11, Firstname = "Bert", Lastname = "Bates" },
            new Author { Id = 12, Firstname = "Mark", Lastname = "Lutz" },
        };

        public static List<Book> Books { get; } = new List<Book>()
        {
            new Book
            {
                Isbn = "9780131103627",
                Title = "The C Programming Language",
                Description = "Le livre fondateur sur le langage C, écrit par les créateurs du langage.",
                AuthorId = 1, // Kernighan (co-auteur avec Ritchie)
                Release = new DateTime(1988, 4, 1)
            },
            new Book
            {
                Isbn = "9780201616224",
                Title = "The Pragmatic Programmer",
                Description = "Un guide incontournable pour améliorer ses pratiques de développement.",
                AuthorId = 3, // Andrew Hunt (co-auteur avec David Thomas)
                Release = new DateTime(1999, 10, 30)
            },
            new Book
            {
                Isbn = "9780132350884",
                Title = "Clean Code",
                Description = "Un manuel sur l'écriture de code clair, lisible et maintenable.",
                AuthorId = 5, // Robert C. Martin
                Release = new DateTime(2008, 8, 1)
            },
            new Book
            {
                Isbn = "9780137081073",
                Title = "The Clean Coder",
                Description = "Un guide sur l'éthique et le professionnalisme des développeurs.",
                AuthorId = 5,
                Release = new DateTime(2011, 5, 13)
            },
            new Book
            {
                Isbn = "9780596007126",
                Title = "Head First Design Patterns",
                Description = "Une approche visuelle et ludique pour comprendre les design patterns.",
                AuthorId = 6, // Eric Freeman (co-auteur avec Elisabeth Robson)
                Release = new DateTime(2004, 10, 25)
            },
            new Book
            {
                Isbn = "9780134757599",
                Title = "Clean Architecture",
                Description = "Les principes pour concevoir des logiciels robustes et évolutifs.",
                AuthorId = 5,
                Release = new DateTime(2017, 9, 20)
            },
            new Book
            {
                Isbn = "9780134494166",
                Title = "Refactoring",
                Description = "Techniques pour améliorer progressivement la structure du code.",
                AuthorId = 8, // Martin Fowler
                Release = new DateTime(2018, 11, 19)
            },
            new Book
            {
                Isbn = "9780321125217",
                Title = "Domain-Driven Design",
                Description = "Approche de conception logicielle centrée sur le domaine métier.",
                AuthorId = 9, // Eric Evans
                Release = new DateTime(2003, 8, 30)
            },
            new Book
            {
                Isbn = "9780596009205",
                Title = "Head First Java",
                Description = "Une introduction ludique et pratique à la programmation en Java.",
                AuthorId = 10, // Kathy Sierra (co-auteur avec Bert Bates)
                Release = new DateTime(2005, 2, 9)
            },
            new Book
            {
                Isbn = "9781449331818",
                Title = "Learning Python",
                Description = "Un ouvrage complet pour apprendre et maîtriser Python.",
                AuthorId = 12, // Mark Lutz
                Release = new DateTime(2013, 6, 12)
            }
        };
    }
}
