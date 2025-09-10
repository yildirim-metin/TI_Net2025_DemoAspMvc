using TI_Net2025_DemoAspMvc.Models;

namespace TI_Net2025_DemoAspMvc.Datas
{
    public static class FakeDb
    {
        public static List<Book> Books { get; } = new List<Book>()
        {
            new Book
            {
                Isbn = "978-0131103627",
                Title = "The C Programming Language",
                Description = "Le livre fondateur sur le langage C, écrit par les créateurs du langage.",
                Author = "Brian W. Kernighan, Dennis M. Ritchie",
                Release = new DateTime(1988, 4, 1)
            },
            new Book
            {
                Isbn = "978-0201616224",
                Title = "The Pragmatic Programmer",
                Description = "Un guide incontournable pour améliorer ses pratiques de développement.",
                Author = "Andrew Hunt, David Thomas",
                Release = new DateTime(1999, 10, 30)
            },
            new Book
            {
                Isbn = "978-0132350884",
                Title = "Clean Code",
                Description = "Un manuel sur l'écriture de code clair, lisible et maintenable.",
                Author = "Robert C. Martin",
                Release = new DateTime(2008, 8, 1)
            },
            new Book
            {
                Isbn = "978-0137081073",
                Title = "The Clean Coder",
                Description = "Un guide sur l'éthique et le professionnalisme des développeurs.",
                Author = "Robert C. Martin",
                Release = new DateTime(2011, 5, 13)
            },
            new Book
            {
                Isbn = "978-0596007126",
                Title = "Head First Design Patterns",
                Description = "Une approche visuelle et ludique pour comprendre les design patterns.",
                Author = "Eric Freeman, Elisabeth Robson",
                Release = new DateTime(2004, 10, 25)
            },
            new Book
            {
                Isbn = "978-0134757599",
                Title = "Clean Architecture",
                Description = "Les principes pour concevoir des logiciels robustes et évolutifs.",
                Author = "Robert C. Martin",
                Release = new DateTime(2017, 9, 20)
            },
            new Book
            {
                Isbn = "978-0134494166",
                Title = "Refactoring",
                Description = "Techniques pour améliorer progressivement la structure du code.",
                Author = "Martin Fowler",
                Release = new DateTime(2018, 11, 19)
            },
            new Book
            {
                Isbn = "978-0321125217",
                Title = "Domain-Driven Design",
                Description = "Approche de conception logicielle centrée sur le domaine métier.",
                Author = "Eric Evans",
                Release = new DateTime(2003, 8, 30)
            },
            new Book
            {
                Isbn = "978-0596009205",
                Title = "Head First Java",
                Description = "Une introduction ludique et pratique à la programmation en Java.",
                Author = "Kathy Sierra, Bert Bates",
                Release = new DateTime(2005, 2, 9)
            },
            new Book
            {
                Isbn = "978-1449331818",
                Title = "Learning Python",
                Description = "Un ouvrage complet pour apprendre et maîtriser Python.",
                Author = "Mark Lutz",
                Release = new DateTime(2013, 6, 12)
            }
        };
    }
}
