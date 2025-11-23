using AutoMapper;
using Email.Server.Data;
using Email.Server.DTOs.Responses;
using Email.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Email.Server.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantContextService _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageService> _logger;

    public MessageService(
        ApplicationDbContext context,
        ITenantContextService tenantContext,
        IMapper mapper,
        ILogger<MessageService> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MessageResponse> GetMessageAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var tenantId = _tenantContext.GetTenantId();

        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId && m.TenantId == tenantId, cancellationToken);

        if (message == null)
        {
            throw new KeyNotFoundException($"Message {messageId} not found");
        }

        var recipients = await _context.MessageRecipients
            .Where(r => r.MessageId == messageId)
            .ToListAsync(cancellationToken);

        var events = await _context.MessageEvents
            .Where(e => e.MessageId == messageId)
            .OrderBy(e => e.OccurredAtUtc)
            .ToListAsync(cancellationToken);

        var tags = await _context.MessageTags
            .Where(t => t.MessageId == messageId)
            .ToListAsync(cancellationToken);

        var response = _mapper.Map<MessageResponse>(message);
        response.Recipients = _mapper.Map<List<MessageRecipientResponse>>(recipients);
        response.Events = _mapper.Map<List<MessageEventResponse>>(events);
        response.Tags = tags.ToDictionary(t => t.Name, t => t.Value);

        return response;
    }

    public async Task<IEnumerable<MessageResponse>> GetMessagesAsync(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var tenantId = _tenantContext.GetTenantId();

        var messages = await _context.Messages
            .Where(m => m.TenantId == tenantId)
            .OrderByDescending(m => m.RequestedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<MessageResponse>>(messages);
    }
}
