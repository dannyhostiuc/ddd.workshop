# Session 4 — Bounded Contexts and Anti-Corruption Layers

## Learning objectives

- Recognise that two parts of "the same business" can have **different
  models** that change for **different reasons** — bounded contexts.
- Use an **anti-corruption layer (ACL)** to keep one context's vocabulary out
  of another's domain code.
- See concretely how the ACL is the *only* place allowed to know about both
  contexts.

## Concept

A **bounded context** is a boundary inside which a single ubiquitous language
applies. Cross that boundary and the same word can mean different things:
in *Catalog* a "Book" is a bibliographic record with subject classification
and publication year; in *Lending* what matters is whether something is a
"loanable item" and whether members may borrow it.

An **Anti-Corruption Layer** sits on the boundary and translates. The Lending
domain depends only on its own interface (`IExternalCatalog`) and types
(`LoanableItem`); the ACL is the one place that knows how to turn a
`Catalog.Book` into a `Lending.LoanableItem`.

This keeps Lending free to evolve without being yanked around every time
Catalog changes a field.

## The exercise

The Domain project contains two sibling namespaces:

- `LibraryLending.BoundedContexts.Catalog` — the "external" published
  language (`Book`, `ICatalogClient`, `FakeCatalogClient`).
- `LibraryLending.BoundedContexts.Lending` — Lending's own language
  (`LoanableItem`, `IExternalCatalog`).

The ACL — `CatalogAntiCorruptionLayer` — lives in the **Api** project. It is
the **only** type that imports both namespaces. Implement its
`FindByReference` method so that:

1. The reference string is treated as an ISBN-13 and looked up in Catalog.
2. If Catalog returns nothing, the ACL returns `null`.
3. The display name is `"{Title} ({first author})"`.
4. `MembersMayBorrow` is the negation of Catalog's `IsRestricted`.

### Steps

1. Open `src/LibraryLending.BoundedContexts.Api/CatalogAntiCorruptionLayer.cs`
   and implement `FindByReference`.
2. Remove the `Skip` argument from
   `tests/LibraryLending.BoundedContexts.Domain.Tests/CatalogAntiCorruptionLayerTests.cs`.
3. Run `dotnet test`.
4. Run the API and try `lending.http` — both happy and `404` cases.

## Acceptance criteria

- `dotnet test` reports all tests as **passed** (no skipped tests left).
- The Lending API handler in `Program.cs` uses **no** types from the
  `LibraryLending.BoundedContexts.Catalog` namespace — verify by searching
  for `using LibraryLending.BoundedContexts.Catalog` outside of
  `CatalogAntiCorruptionLayer.cs` and `Program.cs`'s composition root.
- A request for a known ISBN returns a `LoanableItem` JSON body. A request
  for an unknown ISBN returns `404`.

## Run it

```bash
dotnet build
dotnet test
dotnet run --project src/LibraryLending.BoundedContexts.Api
```

## Discussion

- What would happen if Catalog renamed `IsRestricted` to `AccessLevel`? Which
  files would change?
- Where would the ACL live in your current system? What would break if it
  didn't exist?
