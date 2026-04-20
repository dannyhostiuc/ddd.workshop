using LibraryLending.AggregatesAndRepositories.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ILoanRepository, InMemoryLoanRepository>();
var app = builder.Build();

// Translate domain validation failures into HTTP 4xx responses. Notice: the
// API handlers below contain no validation logic — the aggregate enforces the
// invariants and the API just relays the result.
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

app.MapGet("/", () => Results.Ok(new
{
    lesson = "Session 2 — Aggregates and Repositories",
    endpoints = new[]
    {
        "POST   /loans              { memberId, copyId, startedOn, dueOn }",
        "POST   /loans/{id}/renew",
        "POST   /loans/{id}/return  { on }",
        "GET    /loans/{id}",
    }
}));

app.MapPost("/loans", async (StartLoanRequest body, ILoanRepository repo) =>
{
    var loan = Loan.Start(new MemberId(body.MemberId), new CopyId(body.CopyId), body.StartedOn, body.DueOn);
    await repo.SaveAsync(loan);
    return Results.Created($"/loans/{loan.Id.Value}", LoanResponse.From(loan));
});

app.MapPost("/loans/{id:guid}/renew", async (Guid id, ILoanRepository repo) =>
{
    var loan = await repo.FindAsync(new LoanId(id));
    if (loan is null) return Results.NotFound();
    loan.Renew();
    await repo.SaveAsync(loan);
    return Results.Ok(LoanResponse.From(loan));
});

app.MapPost("/loans/{id:guid}/return", async (Guid id, ReturnRequest body, ILoanRepository repo) =>
{
    var loan = await repo.FindAsync(new LoanId(id));
    if (loan is null) return Results.NotFound();
    loan.Return(body.On);
    await repo.SaveAsync(loan);
    return Results.Ok(LoanResponse.From(loan));
});

app.MapGet("/loans/{id:guid}", async (Guid id, ILoanRepository repo) =>
{
    var loan = await repo.FindAsync(new LoanId(id));
    return loan is null ? Results.NotFound() : Results.Ok(LoanResponse.From(loan));
});

app.Run();

internal sealed record StartLoanRequest(Guid MemberId, Guid CopyId, DateOnly StartedOn, DateOnly DueOn);
internal sealed record ReturnRequest(DateOnly On);

internal sealed record LoanResponse(
    Guid Id, Guid MemberId, Guid CopyId,
    DateOnly StartedOn, DateOnly DueOn, DateOnly? ReturnedOn,
    int RenewalCount, string State)
{
    public static LoanResponse From(Loan l) =>
        new(l.Id.Value, l.MemberId.Value, l.CopyId.Value,
            l.StartedOn, l.DueOn, l.ReturnedOn, l.RenewalCount, l.State.ToString());
}
