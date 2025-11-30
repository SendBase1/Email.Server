namespace Email.Server.DTOs.Responses.Billing;

public class PlanLimitsResponse
{
    public required string PlanName { get; set; }

    // Current counts
    public int CurrentApiKeys { get; set; }
    public int CurrentDomains { get; set; }
    public int CurrentTeamMembers { get; set; }
    public int CurrentWebhooks { get; set; }
    public int CurrentTemplates { get; set; }

    // Plan limits (-1 = unlimited)
    public int MaxApiKeys { get; set; }
    public int MaxDomains { get; set; }
    public int MaxTeamMembers { get; set; }
    public int MaxWebhooks { get; set; }
    public int MaxTemplates { get; set; }

    // Remaining capacity (-1 = unlimited)
    public int RemainingApiKeys { get; set; }
    public int RemainingDomains { get; set; }
    public int RemainingTeamMembers { get; set; }
    public int RemainingWebhooks { get; set; }
    public int RemainingTemplates { get; set; }

    // Usage
    public long EmailsSentThisPeriod { get; set; }
    public long IncludedEmailsLimit { get; set; }
    public bool AllowsOverage { get; set; }
    public int OverageRateCentsPer1K { get; set; }
}
