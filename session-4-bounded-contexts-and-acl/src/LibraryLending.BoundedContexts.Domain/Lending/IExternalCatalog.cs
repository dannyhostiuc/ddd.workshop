namespace LibraryLending.BoundedContexts.Lending;

/// <summary>
/// The contract the <em>Lending</em> bounded context needs from the outside
/// world. It is defined entirely in Lending's language — references and
/// loanable items — and knows nothing about Catalog.
/// </summary>
/// <remarks>
/// Implementations of this interface live <strong>outside</strong> the Lending
/// domain, in the ACL (anti-corruption layer). The ACL is the only place
/// allowed to mention <c>Catalog.*</c> types.
/// </remarks>
public interface IExternalCatalog
{
    LoanableItem? FindByReference(string reference);
}
