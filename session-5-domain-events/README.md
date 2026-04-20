# Session 5 — Domain Events

## Learning objectives

- **Raise** domain events from inside aggregates when state changes.
- **Dispatch** events <em>after</em> persistence so a failed save does not
  leak ghost notifications.
- React to an event by creating a **separate aggregate** (eventual
  consistency between aggregates).

## Concept

A **domain event** is a fact that something meaningful happened in the
domain, expressed in the ubiquitous language: `LoanStarted`,
`LoanReturned`, `LoanOverdue`. Events are **past tense** — they describe
what already happened.

Aggregates raise events as a side effect of their state-changing methods.
Application code pulls those events off the aggregate after persistence and
hands them to a dispatcher, which routes each event to its registered
handlers. Handlers can update read models, send notifications, or — as in
this lesson — create a new aggregate in another part of the model.

The **save-then-dispatch** order matters. If you dispatch first and the save
fails, the rest of the system thinks the change happened when it didn't.

## The exercise

Implement the `Loan` aggregate in
`src/LibraryLending.DomainEvents.Domain/Loan.cs` so that:

| Operation | Event raised |
|---|---|
| `Start` | `LoanStarted` |
| `Return` | `LoanReturned` |
| `CheckOverdue(today)` | `LoanOverdue` — but only **once**, the first time the loan is checked on or after its due date while still active. |

Use `Raise(...)` from the `AggregateRoot` base class.

Everything else is provided:

- `IDomainEvent`, `LoanStarted`, `LoanReturned`, `LoanOverdue` in `Events.cs`.
- `IDomainEventHandler<T>`, `DomainEventDispatcher` in
  `DomainEventDispatcher.cs`.
- `IssueFineOnOverdueHandler` — the handler that creates a `Fine`
  (a separate aggregate) when a `LoanOverdue` is dispatched.
- The minimal-API host that wires it all together and exposes
  `POST /clock/advance` so you can simulate time passing.

### Steps

1. Implement `Loan.Start`, `Loan.Return`, `Loan.CheckOverdue`. Raise the
   appropriate event in each.
2. Remove the `Skip` argument from the tests in
   `tests/LibraryLending.DomainEvents.Domain.Tests/`.
3. Run `dotnet test` until all tests pass.
4. Run the API and walk through `events.http`:
   - Create a loan → `GET /loans` lists it, `GET /fines` is empty.
   - `POST /clock/advance { "days": 10 }` → `GET /fines` now lists a fine.
   - Calling `/clock/advance` again should **not** issue a second fine for
     the same loan (because `CheckOverdue` is idempotent).

## Acceptance criteria

- `dotnet test` reports all tests as **passed** (no skipped tests left).
- After advancing the clock past a loan's due date once, exactly **one**
  fine exists for that loan no matter how many more times the clock is
  advanced.
- Events are dispatched in the API only **after** `repo.SaveAsync(loan)`
  returns — verify by reading `Program.cs`.

## Run it

```bash
dotnet build
dotnet test
dotnet run --project src/LibraryLending.DomainEvents.Api
```

## Discussion

- What would change if `IssueFineOnOverdueHandler` failed half the time?
  Where do retries belong in this design?
- Why is the fine a separate aggregate rather than a property on the loan?
- Which of your current side-effects (e-mails, audit logs, integration
  notifications) are really domain events in disguise?
