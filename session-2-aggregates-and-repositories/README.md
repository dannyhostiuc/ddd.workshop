# Session 2 — Aggregates and Repositories

## Learning objectives

- Identify an **aggregate root** and the invariants it protects.
- Keep all behavior that maintains an invariant **inside** the aggregate.
- Define a **repository interface** in the Domain layer that exposes only the
  operations the aggregate needs.

## Concept

An **aggregate** is a cluster of objects treated as a single unit for the
purposes of consistency. The **aggregate root** is the only object outside
code is allowed to hold a reference to; all changes inside the aggregate go
through the root, which guarantees the aggregate's invariants.

A **repository** provides collection-like access to aggregate roots. There is
exactly **one repository per aggregate root** — never one per entity. The
repository interface lives in the Domain layer; implementations live in
infrastructure.

In this lesson the aggregate is `Loan` and the repository is `ILoanRepository`.

## The exercise

Implement the `Loan` aggregate in
`src/LibraryLending.AggregatesAndRepositories.Domain/Loan.cs` so that:

| Operation | Allowed when… | Effect |
|---|---|---|
| `Start` | always | Creates an `Active` loan with the given dates. Throws `ArgumentException` if the due date is not after the start date. |
| `Renew` | state is `Active` and `RenewalCount < MaxRenewals` (= 2) | Extends `DueOn` by `RenewalDays` (= 7) and increments `RenewalCount`. Otherwise throws `InvalidOperationException`. |
| `Return` | state is `Active` | Sets `State = Returned` and `ReturnedOn = on`. Otherwise throws `InvalidOperationException`. |

The repository (`ILoanRepository`) and its in-memory implementation are
already provided. Notice how the API handlers in
`src/LibraryLending.AggregatesAndRepositories.Api/Program.cs` contain
**no business validation** — they call into the aggregate and let it enforce
the rules.

### Steps

1. Open `Loan.cs` and implement `Start`, `Renew`, and `Return`.
2. Remove the `Skip` argument from the tests in
   `tests/LibraryLending.AggregatesAndRepositories.Domain.Tests/`.
3. Run `dotnet test` until everything is green.
4. Run the API and walk through `loans.http` end-to-end.

## Acceptance criteria

- `dotnet test` reports all tests as **passed** (no skipped tests left).
- The API handlers contain no `if`/throw based on loan state — the aggregate
  is the only thing that enforces invariants.
- Three `POST /loans/{id}/renew` calls in a row return `200, 200, 409`.
- `POST /loans/{id}/return` followed by `POST /loans/{id}/renew` returns
  `409 Conflict`.

## Run it

```bash
dotnet build
dotnet test
dotnet run --project src/LibraryLending.AggregatesAndRepositories.Api
```

## Discussion

- What's the smallest aggregate that still protects this invariant? What's
  the largest you'd tolerate?
- Why is there no `ICopyRepository` in this lesson? What would change if
  copies were also an aggregate root?
