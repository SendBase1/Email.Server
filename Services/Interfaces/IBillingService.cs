using Email.Server.DTOs.Responses.Billing;

namespace Email.Server.Services.Interfaces;

public interface IBillingService
{
    Task<IEnumerable<BillingPlanResponse>> GetAvailablePlansAsync(CancellationToken ct = default);
    Task<SubscriptionResponse?> GetCurrentSubscriptionAsync(Guid tenantId, CancellationToken ct = default);
    Task<CheckoutSessionResponse> CreateCheckoutSessionAsync(Guid tenantId, Guid planId, string successUrl, string cancelUrl, CancellationToken ct = default);
    Task<CustomerPortalResponse> CreateCustomerPortalSessionAsync(Guid tenantId, string returnUrl, CancellationToken ct = default);
    Task<SubscriptionResponse> ChangePlanAsync(Guid tenantId, Guid newPlanId, CancellationToken ct = default);
    Task<SubscriptionResponse> CancelSubscriptionAsync(Guid tenantId, bool atPeriodEnd, CancellationToken ct = default);
    Task<SubscriptionResponse> ReactivateSubscriptionAsync(Guid tenantId, CancellationToken ct = default);
    Task<UsageSummaryResponse> GetCurrentUsageAsync(Guid tenantId, CancellationToken ct = default);
    Task<IEnumerable<UsageSummaryResponse>> GetUsageHistoryAsync(Guid tenantId, int months = 6, CancellationToken ct = default);
    Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync(Guid tenantId, CancellationToken ct = default);
}
