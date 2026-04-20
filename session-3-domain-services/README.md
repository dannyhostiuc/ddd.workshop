# Session 3 — Domain Services

## Learning objectives

- Recognise behavior that **doesn't belong on a single aggregate**.
- Model that behavior as a **stateless domain service**.
- Keep the service free of persistence and I/O — pass everything in.

## Concept

When a piece of business logic is intrinsic to the domain but doesn't have a
natural home on a single aggregate (because it spans several, or because
making one aggregate own it would force it to depend on another), model it as
a **domain service**.

A domain service:

- Is **stateless**.
- Takes **everything it needs as parameters** — including "now".
- Is **named after the operation**, not after a thing
  (`LoanEligibilityService`, not `LoanHelper`).
- Returns a **decision or value**, not a side-effect.

Domain services are easy to spot when you find yourself writing
`if (member.X && copy.Y && reservation.Z) …` inside an API handler — that
condition belongs in a named domain concept.

## The exercise

Implement `LoanEligibilityService.Decide` in
`src/LibraryLending.DomainServices.Domain/LoanEligibilityService.cs` so that
it returns an `EligibilityDecision` listing **every** reason a member is not
eligible to borrow a copy.

The rules are:

1. The member is not suspended.
2. The member has fewer than `MaxConcurrentLoans` (= 5) active loans.
3. The member's outstanding fines do not exceed `MaxOutstandingFinesEur`
   (= €10).
4. The copy is not currently on loan.
5. If a non-expired reservation exists on the copy for **someone else**, the
   request is rejected. A reservation by the requesting member is fine.

Important: when more than one rule fails, return **all** failing reasons —
don't short-circuit on the first one. Librarians want the full picture.

### Steps

1. Read through `LoanEligibilityService.cs` and the test file.
2. Implement `Decide` to satisfy every test in
   `LoanEligibilityServiceTests.cs`.
3. Remove the `Skip` argument from the tests as you go.
4. Run the API and try `eligibility.http` — both happy and ineligible cases.

## Acceptance criteria

- `dotnet test` reports all tests as **passed** (no skipped tests left).
- The "multiple violations" test case returns at least four reasons in a
  single decision.
- The service has **no fields** other than the constants — confirm it stays
  stateless.

## Run it

```bash
dotnet build
dotnet test
dotnet run --project src/LibraryLending.DomainServices.Api
```

## Discussion

- Could `Decide` be moved onto the `Member` aggregate? What would that force
  `Member` to know about?
- Why does `Decide` take `today` as a parameter instead of calling
  `DateOnly.FromDateTime(DateTime.UtcNow)` itself?
