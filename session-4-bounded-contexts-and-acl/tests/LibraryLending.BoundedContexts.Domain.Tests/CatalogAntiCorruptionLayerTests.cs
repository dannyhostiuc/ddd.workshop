using FluentAssertions;
using LibraryLending.BoundedContexts.Api;
using LibraryLending.BoundedContexts.Catalog;
using LibraryLending.BoundedContexts.Lending;

namespace LibraryLending.BoundedContexts.Domain.Tests;

public class CatalogAntiCorruptionLayerTests
{
    private sealed class StubCatalog : ICatalogClient
    {
        private readonly Book? _book;
        public StubCatalog(Book? book) => _book = book;
        public Book? FindByIsbn(string isbn13) =>
            _book is not null && _book.Isbn13 == isbn13 ? _book : null;
    }

    [Fact(Skip = "Exercise: implement the ACL translation")]
    public void TranslatesBook_ToLoanableItem()
    {
        var catalog = new StubCatalog(new Book(
            Isbn13: "9780321125217",
            Title: "Domain-Driven Design",
            Authors: new[] { "Eric Evans" },
            SubjectClassification: "QA76.9.D26",
            PublicationYear: 2003,
            IsRestricted: false));

        IExternalCatalog acl = new CatalogAntiCorruptionLayer(catalog);

        var item = acl.FindByReference("9780321125217");

        item.Should().NotBeNull();
        item!.Reference.Should().Be("9780321125217");
        item.DisplayName.Should().Be("Domain-Driven Design (Eric Evans)");
        item.MembersMayBorrow.Should().BeTrue();
    }

    [Fact(Skip = "Exercise: implement the ACL translation")]
    public void RestrictedBook_IsNotBorrowable()
    {
        var catalog = new StubCatalog(new Book(
            Isbn13: "9780000000001",
            Title: "Restricted Title",
            Authors: new[] { "Anonymous" },
            SubjectClassification: "Z9999",
            PublicationYear: 1999,
            IsRestricted: true));

        IExternalCatalog acl = new CatalogAntiCorruptionLayer(catalog);

        var item = acl.FindByReference("9780000000001");

        item.Should().NotBeNull();
        item!.MembersMayBorrow.Should().BeFalse();
    }

    [Fact(Skip = "Exercise: implement the ACL translation")]
    public void UnknownBook_ReturnsNull()
    {
        IExternalCatalog acl = new CatalogAntiCorruptionLayer(new StubCatalog(null));

        acl.FindByReference("0000000000000").Should().BeNull();
    }
}
