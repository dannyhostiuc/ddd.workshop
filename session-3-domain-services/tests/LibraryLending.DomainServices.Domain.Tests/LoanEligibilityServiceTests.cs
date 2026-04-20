using FluentAssertions;
using LibraryLending.DomainServices.Domain;

namespace LibraryLending.DomainServices.Domain.Tests;

public class LoanEligibilityServiceTests
{
    private static readonly DateOnly Today = new(2026, 4, 20);
    private static readonly MemberId Alice = MemberId.New();
    private static readonly CopyId BookA = CopyId.New();

    private static MemberLendingSnapshot HealthyMember(MemberId id) => new(id, ActiveLoanCount: 1, OutstandingFinesEur: 0m, IsSuspended: false);
    private static CopyAvailability AvailableCopy(CopyId id) => new(id, IsOnLoan: false);

    private readonly LoanEligibilityService _service = new();

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void HealthyMember_AvailableCopy_NoReservations_IsEligible()
    {
        var decision = _service.Decide(HealthyMember(Alice), AvailableCopy(BookA), Array.Empty<Reservation>(), Today);

        decision.IsEligible.Should().BeTrue();
        decision.Reasons.Should().BeEmpty();
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void SuspendedMember_IsRejected()
    {
        var member = HealthyMember(Alice) with { IsSuspended = true };

        var decision = _service.Decide(member, AvailableCopy(BookA), Array.Empty<Reservation>(), Today);

        decision.IsEligible.Should().BeFalse();
        decision.Reasons.Should().Contain(r => r.Contains("suspended", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void TooManyActiveLoans_IsRejected()
    {
        var member = HealthyMember(Alice) with { ActiveLoanCount = LoanEligibilityService.MaxConcurrentLoans };

        var decision = _service.Decide(member, AvailableCopy(BookA), Array.Empty<Reservation>(), Today);

        decision.IsEligible.Should().BeFalse();
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void OutstandingFinesAboveLimit_IsRejected()
    {
        var member = HealthyMember(Alice) with { OutstandingFinesEur = LoanEligibilityService.MaxOutstandingFinesEur + 0.01m };

        var decision = _service.Decide(member, AvailableCopy(BookA), Array.Empty<Reservation>(), Today);

        decision.IsEligible.Should().BeFalse();
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void CopyAlreadyOnLoan_IsRejected()
    {
        var copy = AvailableCopy(BookA) with { IsOnLoan = true };

        var decision = _service.Decide(HealthyMember(Alice), copy, Array.Empty<Reservation>(), Today);

        decision.IsEligible.Should().BeFalse();
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void CopyReservedBySomeoneElse_AndNotExpired_IsRejected()
    {
        var bob = MemberId.New();
        var reservation = new Reservation(bob, BookA, Today, Today.AddDays(1));

        var decision = _service.Decide(HealthyMember(Alice), AvailableCopy(BookA), new[] { reservation }, Today);

        decision.IsEligible.Should().BeFalse();
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void CopyReservedByRequestingMember_IsAllowed()
    {
        var reservation = new Reservation(Alice, BookA, Today, Today.AddDays(1));

        var decision = _service.Decide(HealthyMember(Alice), AvailableCopy(BookA), new[] { reservation }, Today);

        decision.IsEligible.Should().BeTrue();
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void ExpiredReservationByOther_IsIgnored()
    {
        var bob = MemberId.New();
        var reservation = new Reservation(bob, BookA, Today.AddDays(-7), Today.AddDays(-1));

        var decision = _service.Decide(HealthyMember(Alice), AvailableCopy(BookA), new[] { reservation }, Today);

        decision.IsEligible.Should().BeTrue();
    }

    [Fact(Skip = "Exercise: implement LoanEligibilityService.Decide")]
    public void MultipleViolations_AreAllReported()
    {
        var member = HealthyMember(Alice) with
        {
            IsSuspended = true,
            OutstandingFinesEur = 100m,
            ActiveLoanCount = LoanEligibilityService.MaxConcurrentLoans + 1,
        };

        var decision = _service.Decide(member, AvailableCopy(BookA) with { IsOnLoan = true }, Array.Empty<Reservation>(), Today);

        decision.IsEligible.Should().BeFalse();
        decision.Reasons.Count.Should().BeGreaterOrEqualTo(4);
    }
}
