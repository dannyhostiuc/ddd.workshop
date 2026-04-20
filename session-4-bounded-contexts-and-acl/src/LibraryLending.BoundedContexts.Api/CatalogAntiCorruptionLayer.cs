using LibraryLending.BoundedContexts.Catalog;
using LibraryLending.BoundedContexts.Lending;

namespace LibraryLending.BoundedContexts.Api;

/// <summary>
/// The Anti-Corruption Layer. The <strong>only</strong> place in the codebase
/// allowed to <c>using</c> both <see cref="LibraryLending.BoundedContexts.Catalog"/>
/// and <see cref="LibraryLending.BoundedContexts.Lending"/>.
/// </summary>
/// <remarks>
/// The ACL implements Lending's <see cref="IExternalCatalog"/> by translating
/// from Catalog's <see cref="Book"/> to Lending's <see cref="LoanableItem"/>.
/// All of the translation rules — which Catalog field maps to which Lending
/// field, what "may borrow" means in Lending terms — live here and only here.
/// <para>
/// Exercise: implement <see cref="FindByReference"/> so that:
/// </para>
/// <list type="number">
///   <item>The Lending <c>reference</c> string is treated as an ISBN-13 and
///         used to look the book up in Catalog.</item>
///   <item>If the book is missing, return <c>null</c>.</item>
///   <item>The display name is <c>"{Title} ({first author})"</c>.</item>
///   <item><c>MembersMayBorrow</c> is the negation of Catalog's
///         <c>IsRestricted</c>.</item>
/// </list>
/// </remarks>
public sealed class CatalogAntiCorruptionLayer : IExternalCatalog
{
    private readonly ICatalogClient _catalog;

    public CatalogAntiCorruptionLayer(ICatalogClient catalog) => _catalog = catalog;

    public LoanableItem? FindByReference(string reference) =>
        throw new NotImplementedException("Exercise: implement the ACL translation");
}
