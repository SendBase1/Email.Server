namespace Email.Server.DTOs.Requests.Billing;

public class CancelSubscriptionRequest
{
    /// <summary>
    /// If true, subscription will remain active until the end of the current billing period.
    /// If false, subscription will be canceled immediately.
    /// </summary>
    public bool AtPeriodEnd { get; set; } = true;
}
