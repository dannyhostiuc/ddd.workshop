// CATALOG bounded context.
//
// This is the *external* world from Lending's point of view. Catalog has its
// own ubiquitous language ("Book", "Edition", "Subject classification") and
// changes for its own reasons. The Lending domain MUST NOT depend on these
// types directly — see Lending/IExternalCatalog.cs and the ACL implementation
// in the Api project.

namespace LibraryLending.BoundedContexts.Catalog;

public sealed record Book(
    string Isbn13,
    string Title,
    IReadOnlyCollection<string> Authors,
    string SubjectClassification,
    int PublicationYear,
    bool IsRestricted);
