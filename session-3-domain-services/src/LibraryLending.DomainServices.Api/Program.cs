using LibraryLending.DomainServices.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<LoanEligibilityService>();
var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    lesson = "Session 3 — Domain Services",
    endpoints = new[]
    {
        "POST /eligibility-checks  { member, copy, reservations[], today }",
    }
}));

app.MapPost("/eligibility-checks", (EligibilityRequest body, LoanEligibilityService service) =>
{
    var member = new MemberLendingSnapshot(
        new MemberId(body.Member.Id),
        body.Member.ActiveLoanCount,
        body.Member.OutstandingFinesEur,
        body.Member.IsSuspended);

    var copy = new CopyAvailability(new CopyId(body.Copy.Id), body.Copy.IsOnLoan);

    var reservations = body.Reservations
        .Select(r => new Reservation(new MemberId(r.MemberId), new CopyId(r.CopyId), r.ReservedOn, r.ExpiresOn))
        .ToList();

    var decision = service.Decide(member, copy, reservations, body.Today);

    return Results.Ok(new { decision.IsEligible, decision.Reasons });
});

app.Run();

internal sealed record EligibilityRequest(
    MemberDto Member,
    CopyDto Copy,
    IReadOnlyCollection<ReservationDto> Reservations,
    DateOnly Today);

internal sealed record MemberDto(Guid Id, int ActiveLoanCount, decimal OutstandingFinesEur, bool IsSuspended);
internal sealed record CopyDto(Guid Id, bool IsOnLoan);
internal sealed record ReservationDto(Guid MemberId, Guid CopyId, DateOnly ReservedOn, DateOnly ExpiresOn);
