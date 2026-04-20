using LibraryLending.DomainEvents.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ILoanRepository, InMemoryLoanRepository>();
builder.Services.AddSingleton<IFineRepository, InMemoryFineRepository>();
builder.Services.AddSingleton<IDomainEventHandler<LoanOverdue>, IssueFineOnOverdueHandler>();
builder.Services.AddSingleton<DomainEventDispatcher>(sp => new DomainEventDispatcher(handlerType =>
    (IEnumerable<object>)sp.GetServices(handlerType)!));

// A tiny mutable clock so the workshop can simulate time passing without
// waiting for the calendar.
builder.Services.AddSingleton<MutableClock>();

var app = builder.Build();

app.UseExceptionHandler(handler => handler.Run(async context =>
{
    var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
    var status = feature?.Error switch
    {
        ArgumentException => StatusCodes.Status400BadRequest,
        InvalidOperationException => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError,
    };
    context.Response.StatusCode = status;
    await context.Response.WriteAsJsonAsync(new { error = feature?.Error.Message ?? "Unknown error" });
}));

app.MapGet("/", (MutableClock clock) => Results.Ok(new
{
    lesson = "Session 5 — Domain Events",
    today = clock.Today,
    endpoints = new[]
    {
        "POST /loans                 { memberId, copyId, days }",
        "POST /loans/{id}/return",
        "POST /clock/advance         { days }",
        "GET  /loans",
        "GET  /fines",
    }
}));

app.MapPost("/loans", async (StartLoanRequest body, ILoanRepository loans, DomainEventDispatcher dispatcher, MutableClock clock) =>
{
    var loan = Loan.Start(new MemberId(body.MemberId), new CopyId(body.CopyId), clock.Today, clock.Today.AddDays(body.Days));

    // Save FIRST, then dispatch. If save throws, no events leak out.
    await loans.SaveAsync(loan);
    await dispatcher.DispatchAsync(loan.DequeueEvents());

    return Results.Created($"/loans/{loan.Id.Value}", LoanResponse.From(loan));
});

app.MapPost("/loans/{id:guid}/return", async (Guid id, ILoanRepository loans, DomainEventDispatcher dispatcher, MutableClock clock) =>
{
    var loan = await loans.FindAsync(new LoanId(id));
    if (loan is null) return Results.NotFound();

    loan.Return(clock.Today);

    await loans.SaveAsync(loan);
    await dispatcher.DispatchAsync(loan.DequeueEvents());

    return Results.Ok(LoanResponse.From(loan));
});

app.MapPost("/clock/advance", async (AdvanceRequest body, MutableClock clock, ILoanRepository loans, DomainEventDispatcher dispatcher) =>
{
    clock.Advance(body.Days);

    // Give every active loan a chance to notice it has gone overdue.
    var active = await loans.ListActiveAsync();
    foreach (var loan in active)
    {
        loan.CheckOverdue(clock.Today);
        await loans.SaveAsync(loan);
        await dispatcher.DispatchAsync(loan.DequeueEvents());
    }

    return Results.Ok(new { today = clock.Today });
});

app.MapGet("/loans", async (ILoanRepository loans) =>
{
    var active = await loans.ListActiveAsync();
    return Results.Ok(active.Select(LoanResponse.From));
});

app.MapGet("/fines", async (IFineRepository fines) =>
{
    var all = await fines.ListAsync();
    return Results.Ok(all.Select(f => new
    {
        id = f.Id.Value,
        memberId = f.MemberId.Value,
        loanId = f.LoanId.Value,
        issuedOn = f.IssuedOn,
        amountEur = f.AmountEur,
    }));
});

app.Run();

internal sealed class MutableClock
{
    public DateOnly Today { get; private set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public void Advance(int days) => Today = Today.AddDays(days);
}

internal sealed record StartLoanRequest(Guid MemberId, Guid CopyId, int Days);
internal sealed record AdvanceRequest(int Days);

internal sealed record LoanResponse(Guid Id, Guid MemberId, Guid CopyId, DateOnly StartedOn, DateOnly DueOn, DateOnly? ReturnedOn)
{
    public static LoanResponse From(Loan l) =>
        new(l.Id.Value, l.MemberId.Value, l.CopyId.Value, l.StartedOn, l.DueOn, l.ReturnedOn);
}
