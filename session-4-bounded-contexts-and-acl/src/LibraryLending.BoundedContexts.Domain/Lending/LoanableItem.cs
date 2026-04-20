// LENDING bounded context.
//
// Lending speaks its own language. A "Book" in Catalog is not the same thing
// as what Lending cares about; Lending only cares whether something is a
// "loanable item" with a display name and a flag for whether members can
// borrow it. This is the type the Lending API and any Lending domain logic
// consume.

namespace LibraryLending.BoundedContexts.Lending;

public sealed record LoanableItem(
    string Reference,    // a Lending-shaped identifier; how Catalog represents it is not Lending's concern
    string DisplayName,
    bool MembersMayBorrow);
