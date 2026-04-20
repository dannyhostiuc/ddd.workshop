using LibraryLending.ValueObjects.Domain;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Centralized translation: any FormatException raised by a value-object Parse
// method becomes a 400 with the validation reason. This is what makes
// "self-validation at the edge" work without per-endpoint try/catch.
app.UseExceptionHandler(handler => handler.Run(async context =>
{
    var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
    if (feature?.Error is FormatException fx)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = fx.Message });
        return;
    }

    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(new { error = feature?.Error.Message ?? "Unknown error" });
}));

app.MapGet("/", () => Results.Ok(new
{
    lesson = "Session 1 — Value Objects",
    endpoints = new[]
    {
        "POST /isbns       { \"value\": \"978-3-16-148410-0\" }",
        "POST /emails      { \"value\": \"alice@example.com\" }",
        "POST /money       { \"amount\": 1.50, \"currency\": \"EUR\" }",
        "POST /loan-periods{ \"start\": \"2026-04-20\", \"days\": 14 }",
    }
}));

app.MapPost("/isbns", (ParseRequest body) =>
{
    var isbn = Isbn.Parse(body.Value);
    return Results.Ok(new { normalized = isbn.ToString() });
});

app.MapPost("/emails", (ParseRequest body) =>
{
    var email = Email.Parse(body.Value);
    return Results.Ok(new { normalized = email.ToString() });
});

app.MapPost("/money", (MoneyRequest body) =>
{
    var money = new Money(body.Amount, body.Currency);
    return Results.Ok(new { amount = money.Amount, currency = money.Currency });
});

app.MapPost("/loan-periods", (LoanPeriodRequest body) =>
{
    var period = LoanPeriod.Create(body.Start, body.Days);
    return Results.Ok(new { start = period.Start, due = period.Due });
});

app.Run();

internal sealed record ParseRequest(string Value);
internal sealed record MoneyRequest(decimal Amount, string Currency);
internal sealed record LoanPeriodRequest(DateOnly Start, int Days);
