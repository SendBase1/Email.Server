using System.Text.Json.Serialization;

namespace Email.Server.DTOs.Sns;

/// <summary>
/// Base SNS message wrapper
/// </summary>
public class SnsMessage
{
    [JsonPropertyName("Type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("MessageId")]
    public string MessageId { get; set; } = string.Empty;

    [JsonPropertyName("TopicArn")]
    public string TopicArn { get; set; } = string.Empty;

    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("Timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("SignatureVersion")]
    public string? SignatureVersion { get; set; }

    [JsonPropertyName("Signature")]
    public string? Signature { get; set; }

    [JsonPropertyName("SigningCertURL")]
    public string? SigningCertUrl { get; set; }

    [JsonPropertyName("SubscribeURL")]
    public string? SubscribeUrl { get; set; }

    [JsonPropertyName("UnsubscribeURL")]
    public string? UnsubscribeUrl { get; set; }
}

/// <summary>
/// SES notification contained within SNS message
/// </summary>
public class SesNotification
{
    [JsonPropertyName("notificationType")]
    public string NotificationType { get; set; } = string.Empty;

    // Event notifications (Open, Click, etc.) use eventType instead of notificationType
    [JsonPropertyName("eventType")]
    public string EventType { get; set; } = string.Empty;

    // Helper to get the actual event type regardless of which field is populated
    public string GetEventType() => !string.IsNullOrEmpty(EventType) ? EventType : NotificationType;

    [JsonPropertyName("mail")]
    public SesMail Mail { get; set; } = new();

    [JsonPropertyName("bounce")]
    public SesBounce? Bounce { get; set; }

    [JsonPropertyName("complaint")]
    public SesComplaint? Complaint { get; set; }

    [JsonPropertyName("delivery")]
    public SesDelivery? Delivery { get; set; }

    [JsonPropertyName("open")]
    public SesOpen? Open { get; set; }

    [JsonPropertyName("click")]
    public SesClick? Click { get; set; }

    [JsonPropertyName("send")]
    public SesSend? Send { get; set; }

    [JsonPropertyName("reject")]
    public SesReject? Reject { get; set; }

    [JsonPropertyName("deliveryDelay")]
    public SesDeliveryDelay? DeliveryDelay { get; set; }

    [JsonPropertyName("renderingFailure")]
    public SesRenderingFailure? RenderingFailure { get; set; }

    [JsonPropertyName("subscription")]
    public SesSubscription? Subscription { get; set; }
}

public class SesMail
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("messageId")]
    public string MessageId { get; set; } = string.Empty;

    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;

    [JsonPropertyName("sourceArn")]
    public string? SourceArn { get; set; }

    [JsonPropertyName("destination")]
    public List<string> Destination { get; set; } = new();

    [JsonPropertyName("headersTruncated")]
    public bool HeadersTruncated { get; set; }

    [JsonPropertyName("headers")]
    public List<SesHeader>? Headers { get; set; }

    [JsonPropertyName("commonHeaders")]
    public SesCommonHeaders? CommonHeaders { get; set; }
}

public class SesHeader
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

public class SesCommonHeaders
{
    [JsonPropertyName("from")]
    public List<string>? From { get; set; }

    [JsonPropertyName("to")]
    public List<string>? To { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }
}

public class SesBounce
{
    [JsonPropertyName("bounceType")]
    public string BounceType { get; set; } = string.Empty;

    [JsonPropertyName("bounceSubType")]
    public string BounceSubType { get; set; } = string.Empty;

    [JsonPropertyName("bouncedRecipients")]
    public List<SesBouncedRecipient> BouncedRecipients { get; set; } = new();

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("feedbackId")]
    public string FeedbackId { get; set; } = string.Empty;

    [JsonPropertyName("reportingMTA")]
    public string? ReportingMta { get; set; }
}

public class SesBouncedRecipient
{
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("diagnosticCode")]
    public string? DiagnosticCode { get; set; }
}

public class SesComplaint
{
    [JsonPropertyName("complainedRecipients")]
    public List<SesComplainedRecipient> ComplainedRecipients { get; set; } = new();

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("feedbackId")]
    public string FeedbackId { get; set; } = string.Empty;

    [JsonPropertyName("complaintSubType")]
    public string? ComplaintSubType { get; set; }

    [JsonPropertyName("complaintFeedbackType")]
    public string? ComplaintFeedbackType { get; set; }
}

public class SesComplainedRecipient
{
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;
}

public class SesDelivery
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("processingTimeMillis")]
    public int ProcessingTimeMillis { get; set; }

    [JsonPropertyName("recipients")]
    public List<string> Recipients { get; set; } = new();

    [JsonPropertyName("smtpResponse")]
    public string? SmtpResponse { get; set; }

    [JsonPropertyName("reportingMTA")]
    public string? ReportingMta { get; set; }
}

public class SesOpen
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("userAgent")]
    public string? UserAgent { get; set; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; set; }
}

public class SesClick
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("userAgent")]
    public string? UserAgent { get; set; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; set; }

    [JsonPropertyName("link")]
    public string? Link { get; set; }

    [JsonPropertyName("linkTags")]
    public Dictionary<string, List<string>>? LinkTags { get; set; }
}

public class SesSend
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

public class SesReject
{
    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}

public class SesDeliveryDelay
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("delayType")]
    public string DelayType { get; set; } = string.Empty;

    [JsonPropertyName("expirationTime")]
    public DateTime? ExpirationTime { get; set; }

    [JsonPropertyName("delayedRecipients")]
    public List<SesDelayedRecipient> DelayedRecipients { get; set; } = new();
}

public class SesDelayedRecipient
{
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("diagnosticCode")]
    public string? DiagnosticCode { get; set; }
}

public class SesRenderingFailure
{
    [JsonPropertyName("templateName")]
    public string? TemplateName { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}

public class SesSubscription
{
    [JsonPropertyName("contactList")]
    public string? ContactList { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("newTopicPreferences")]
    public SesTopicPreferences? NewTopicPreferences { get; set; }

    [JsonPropertyName("oldTopicPreferences")]
    public SesTopicPreferences? OldTopicPreferences { get; set; }
}

public class SesTopicPreferences
{
    [JsonPropertyName("unsubscribeAll")]
    public bool UnsubscribeAll { get; set; }

    [JsonPropertyName("topicSubscriptionStatus")]
    public List<SesTopicSubscriptionStatus>? TopicSubscriptionStatus { get; set; }
}

public class SesTopicSubscriptionStatus
{
    [JsonPropertyName("topicName")]
    public string TopicName { get; set; } = string.Empty;

    [JsonPropertyName("subscriptionStatus")]
    public string SubscriptionStatus { get; set; } = string.Empty;
}
