using Email.Server.DTOs.Responses;

namespace Email.Server.Services.Interfaces;

public interface IMessageService
{
    Task<MessageResponse> GetMessageAsync(Guid messageId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MessageResponse>> GetMessagesAsync(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
}
