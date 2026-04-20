# Session 1 — Value Objects

## Learning objectives

By the end of this lesson you should be able to:

- Recognise primitives that are really value objects in disguise.
- Implement a value object in C# with **immutability**, **value equality**, and
  **self-validation**.
- Push validation to the **edge of the domain** so handlers don't carry
  defensive checks.

## Concept

A **value object** is a type that:

1. Is defined entirely by its attributes (no identity).
2. Is **immutable** — once constructed, its state cannot change.
3. Implements **value equality** — two instances with equal attributes are
   equal, and have equal hash codes.
4. Validates itself at construction — it is impossible to obtain an instance
   in an invalid state.

Replacing primitives (`string`, `decimal`, `int`) with value objects pushes
correctness into the type system: a method that takes an `Email` cannot be
called with a `string` that happens to be a customer's name.

## The exercise

You will implement four value objects in `src/LibraryLending.ValueObjects.Domain/`:

| Type | Concern |
|---|---|
| `Isbn` | Parsing, normalization, ISBN-13 checksum, value equality regardless of formatting. |
| `Email` | Parsing, structural validation, case-insensitive domain part for equality. |
| `Money` | Non-negative amounts, currency-aware arithmetic, addition across currencies throws. |
| `LoanPeriod` | Bounded length (1 .. `MaxLoanDays`), `Contains`, `IsOverdueOn`, non-mutating `Renew`. |

Each type ships as a stub that throws `NotImplementedException`. The tests in
`tests/LibraryLending.ValueObjects.Domain.Tests/` describe the expected
behavior and are currently marked `Skip = "Exercise: implement …"`.

### Steps

1. Open `Isbn.cs`, implement `Parse`, `TryParse`, value equality, and
   `GetHashCode`.
2. Remove the `Skip` argument from the corresponding tests in `IsbnTests.cs`
   and run them.
3. Repeat for `Email`, `Money`, and `LoanPeriod`.
4. Run the API and exercise the endpoints with `value-objects.http` (see
   below). Notice that bad input returns `400` with the validation reason —
   without any per-endpoint try/catch.

## Acceptance criteria

- `dotnet test` reports all tests as **passed** (no skipped tests left).
- `POST /isbns` with a malformed ISBN returns `400` and a meaningful message.
- `POST /money` with a negative amount returns `400`.
- Two `Isbn` instances parsed from `"978-3-16-148410-0"` and `"9783161484100"`
  are equal and have equal hash codes.

## Run it

From this folder:

```bash
# Build
dotnet build

# Test (initially all skipped — implement the exercises to enable them)
dotnet test

# Run the API on http://localhost:5000
dotnet run --project src/LibraryLending.ValueObjects.Api
```

Then open `src/LibraryLending.ValueObjects.Api/value-objects.http` in your
IDE and send the requests, or use `curl`:

```bash
curl -X POST http://localhost:5000/isbns \
  -H 'Content-Type: application/json' \
  -d '{"value":"978-3-16-148410-0"}'
```

## Discussion

- Where in your day job do you have a `string` that should really be a value
  object? What stops you from introducing one?
- Should `LoanPeriod` know about overdue-ness, or should that be on the
  aggregate that owns the period? (We'll come back to this in Session 2.)
