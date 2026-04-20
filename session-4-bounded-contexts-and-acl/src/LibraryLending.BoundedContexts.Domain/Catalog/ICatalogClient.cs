namespace LibraryLending.BoundedContexts.Catalog;

/// <summary>
/// The contract Catalog exposes to its consumers. In a real system this would
/// be an HTTP client or message broker; here it's a fake adapter wired up by
/// the API host.
/// </summary>
/// <remarks>
/// This interface is part of <strong>Catalog's published language</strong>. It
/// returns Catalog types. Lending must not depend on this interface directly —
/// it depends on its own <c>IExternalCatalog</c> in the Lending namespace, and
/// the ACL is what bridges the two.
/// </remarks>
public interface ICatalogClient
{
    Book? FindByIsbn(string isbn13);
}
