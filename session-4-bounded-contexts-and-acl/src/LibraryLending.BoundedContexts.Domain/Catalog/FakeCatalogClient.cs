namespace LibraryLending.BoundedContexts.Catalog;

/// <summary>
/// A stand-in for a real Catalog service. Holds a fixed set of books for the
/// workshop.
/// </summary>
public sealed class FakeCatalogClient : ICatalogClient
{
    private readonly Dictionary<string, Book> _books;

    public FakeCatalogClient()
    {
        _books = new[]
        {
            new Book(
                Isbn13: "9780132350884",
                Title: "Clean Code",
                Authors: new[] { "Robert C. Martin" },
                SubjectClassification: "QA76.76.D47",
                PublicationYear: 2008,
                IsRestricted: false),
            new Book(
                Isbn13: "9780321125217",
                Title: "Domain-Driven Design",
                Authors: new[] { "Eric Evans" },
                SubjectClassification: "QA76.9.D26",
                PublicationYear: 2003,
                IsRestricted: false),
            new Book(
                Isbn13: "9780000000001",
                Title: "Restricted Title",
                Authors: new[] { "Anonymous" },
                SubjectClassification: "Z9999",
                PublicationYear: 1999,
                IsRestricted: true),
        }.ToDictionary(b => b.Isbn13);
    }

    public Book? FindByIsbn(string isbn13) =>
        _books.TryGetValue(isbn13, out var book) ? book : null;
}
