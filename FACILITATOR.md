# Facilitator notes

Notes for whoever is running the workshop. Participants don't need to read
this file.

## Format

- 5 lessons, each ~60–90 minutes.
- Each lesson is self-contained: open `session-N-*/session-N-*.sln`, no
  cross-lesson references.
- Suggested cadence per lesson:
  1. **5–10 min** — frame the concept with a short whiteboard/slide.
  2. **5 min** — walk through the lesson's `README.md` together.
  3. **30–60 min** — pair / mob on the exercise.
  4. **10–15 min** — group debrief: what trade-offs did you hit?

## What "done" looks like for each lesson

| # | Lesson | Done when… |
|---|---|---|
| 1 | Value Objects | All `*.Domain.Tests` pass; the API rejects malformed input with 400 + reason; equality is by value, not reference. |
| 2 | Aggregates and Repositories | Loan invariants are enforced inside the aggregate (not in the API handler); the repository is an interface in the Domain project. |
| 3 | Domain Services | The eligibility check is a stateless service that takes everything it needs as parameters; aggregates do not call the service. |
| 4 | Bounded Contexts and ACL | No `Catalog` type appears in the `Lending` API handlers; the ACL is the only translation point. |
| 5 | Domain Events | Events are raised inside aggregates and dispatched **after** persistence; the fine-on-overdue handler creates a separate aggregate. |

## Common pitfalls to watch for

- **Anaemic domain model.** Putting all logic in the API handler and leaving
  the domain types as bags of properties. Push back.
- **Leaky aggregates.** Returning internal collections by reference. Prefer
  `IReadOnlyCollection` and copy-on-read.
- **Repository per entity.** A repository belongs to an *aggregate root*, not
  every entity. Watch for `ICopyRepository` sneaking in alongside
  `ILoanRepository`.
- **Domain service as god object.** If a "domain service" is doing five
  unrelated things, it's probably hiding a missing aggregate.
- **ACL bypass.** In Session 4, learners often `using LibraryLending.Catalog;`
  inside the Lending handler "just for the DTO". That defeats the exercise.
- **Events before persistence.** Dispatching events before the aggregate is
  saved leads to ghost notifications when the save fails.

## Discussion prompts

- Session 1: *Where in your real codebase do you have primitives that should
  be value objects? What stops you from introducing them?*
- Session 2: *What's the smallest aggregate that still protects this
  invariant? What's the largest you'd tolerate?*
- Session 3: *Could this domain service be moved onto an aggregate? If yes,
  which one and why didn't you?*
- Session 4: *Where would the ACL live in your current system? What would
  break if it didn't exist?*
- Session 5: *Which of your current side-effects are really domain events in
  disguise?*

## Timing notes

- Sessions 1 and 2 take the longest because participants are also getting used
  to the lesson layout and tooling. Budget extra time for those.
- Session 4 needs the most facilitator presence — the temptation to share
  types across contexts is strong.

## Solutions

Worked solutions (`SOLUTION.md` per lesson) will be added as a **follow-up**
once exercises have been play-tested. Until then, facilitators should write
their own reference solutions while preparing.
