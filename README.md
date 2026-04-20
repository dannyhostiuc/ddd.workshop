# ddd.workshop

A hands-on workshop for learning **Domain-Driven Design (DDD)** in C#, organised
as five small, self-contained lessons. Each lesson lives in its own folder with
its own solution, so you can open just the lesson you're working on and ignore
the rest.

The shared business domain across all lessons is a **library lending system** —
small enough to fit in your head, rich enough to surface real DDD trade-offs.

## Audience

Developers comfortable with C# who want to learn the practical building blocks
of DDD: value objects, aggregates, repositories, domain services, bounded
contexts, anti-corruption layers, and domain events.

No prior DDD experience is assumed.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- An IDE with C# support (Visual Studio 2026, JetBrains Rider, or VS Code with
  the C# Dev Kit)
- A terminal — every lesson can also be run from the command line

## Lesson index

| # | Folder | Topic |
|---|---|---|
| 1 | [`session-1-value-objects/`](session-1-value-objects/) | Value Objects |
| 2 | [`session-2-aggregates-and-repositories/`](session-2-aggregates-and-repositories/) | Aggregates and Repositories |
| 3 | [`session-3-domain-services/`](session-3-domain-services/) | Domain Services |
| 4 | [`session-4-bounded-contexts-and-acl/`](session-4-bounded-contexts-and-acl/) | Bounded Contexts and Anti-Corruption Layers |
| 5 | [`session-5-domain-events/`](session-5-domain-events/) | Domain Events |

The lessons build conceptually on each other but are **technically
independent** — no project in one lesson references code from another. Each
lesson redeclares the slice of the library-lending domain it needs.

## How to use a lesson

From the lesson folder (e.g. `session-1-value-objects/`):

```bash
# Restore + build the lesson's solution
dotnet build

# Run the lesson's tests
dotnet test

# Run the lesson's minimal-API host
dotnet run --project src/LibraryLending.<Topic>.Api
```

Then open the lesson's `<lesson>.http` file in your IDE (or use `curl`) to hit
the running API. Read the lesson's `README.md` for the learning objectives,
concept explanation, and the exercise.

> Solutions to the exercises are intentionally **not** in the initial drop —
> they will be added later as a follow-up so participants can grapple with the
> problem first.

## The shared domain: library lending

All five lessons model the same business domain so that the DDD concepts —
not the domain — are what changes from lesson to lesson.

### Ubiquitous language

| Term | Meaning |
|---|---|
| **Member** | A registered patron of the library who can borrow copies. |
| **Book** | A bibliographic record (title, author, ISBN). Not borrowable directly. |
| **Copy** | A physical instance of a book, identified by a barcode. This is what gets loaned. |
| **Loan** | A copy in a member's possession for a bounded period. |
| **Reservation** | A member's claim on a copy that is not currently available. Expires if not collected. |
| **Branch** | A physical library location that owns copies and employs librarians. |
| **Librarian** | A staff member who can register loans, returns, fines, and reservations. |
| **Fine** | An amount owed by a member, accrued per day a loan is overdue. |

### Core invariants (used across lessons)

- A member may not have more than a configured number of concurrent loans.
- A copy can be on at most one active loan at a time.
- A reservation expires if not collected within its window.
- An overdue loan accrues a fine per day until returned.

Each lesson uses only the slice of the language and invariants relevant to its
topic.

## Repository layout

```
ddd.workshop/
├── LICENSE                                    MIT
├── README.md                                  this file
├── FACILITATOR.md                             notes for whoever runs the workshop
├── .editorconfig
├── .gitignore
├── session-1-value-objects/
├── session-2-aggregates-and-repositories/
├── session-3-domain-services/
├── session-4-bounded-contexts-and-acl/
└── session-5-domain-events/
```

## License

[MIT](LICENSE).
