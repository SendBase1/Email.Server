using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class WebhookDeliveries
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [ForeignKey("Endpoint")]
    public Guid EndpointId { get; set; }

    [ForeignKey("Event")]
    public long EventId { get; set; }

    public byte Status { get; set; } // 0=Pending,1=Sent,2=Retry,3=Failed

    public int AttemptCount { get; set; } = 0;

    public DateTime? LastAttemptUtc { get; set; }

    // Navigation properties
    public WebhookEndpoints? Endpoint { get; set; }
    public MessageEvents? Event { get; set; }
}
