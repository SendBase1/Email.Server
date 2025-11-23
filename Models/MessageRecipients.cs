using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class MessageRecipients
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [ForeignKey("Message")]
    public Guid MessageId { get; set; }

    public byte Kind { get; set; } // 0=To,1=CC,2=BCC

    [MaxLength(320)]
    public required string Email { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    public byte DeliveryStatus { get; set; } = 0; // 0=Pending,1=Delivered,2=Bounced,3=Complained

    [MaxLength(256)]
    public string? SesDeliveryId { get; set; }

    // Navigation properties
    public Messages? Message { get; set; }
}
