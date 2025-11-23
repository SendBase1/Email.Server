using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class MessageTags
{
    [ForeignKey("Message")]
    public Guid MessageId { get; set; }

    [MaxLength(128)]
    public required string Name { get; set; }

    [MaxLength(256)]
    public required string Value { get; set; }

    // Navigation properties
    public Messages? Message { get; set; }
}
