using Amazon.S3;
using Amazon.S3.Model;
using Email.Server.Services.Interfaces;

namespace Email.Server.Services.Implementations;

public class AttachmentStorageService : IAttachmentStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<AttachmentStorageService> _logger;
    private readonly string _bucketName;
    private static readonly TimeSpan PresignedUrlExpiry = TimeSpan.FromHours(1);

    public AttachmentStorageService(
        IAmazonS3 s3Client,
        IConfiguration configuration,
        ILogger<AttachmentStorageService> logger)
    {
        _s3Client = s3Client;
        _logger = logger;
        _bucketName = configuration["S3:AttachmentsBucket"]
            ?? throw new InvalidOperationException("S3:AttachmentsBucket configuration is required");
    }

    public async Task<string> UploadAttachmentAsync(
        Guid tenantId,
        Guid messageId,
        string fileName,
        string contentType,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        // Organize by tenant/message for easy cleanup
        var s3Key = $"attachments/{tenantId}/{messageId}/{Guid.NewGuid()}/{fileName}";

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = s3Key,
            InputStream = stream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);

        _logger.LogInformation(
            "Uploaded attachment {FileName} for message {MessageId}, key {S3Key}",
            fileName, messageId, s3Key);

        return s3Key;
    }

    public Task<(string Url, DateTime ExpiresAt)> GetSignedDownloadUrlAsync(
        string s3Key,
        CancellationToken cancellationToken = default)
    {
        var expiresAt = DateTime.UtcNow.Add(PresignedUrlExpiry);

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = s3Key,
            Expires = expiresAt,
            Verb = HttpVerb.GET
        };

        var url = _s3Client.GetPreSignedURL(request);

        return Task.FromResult((url, expiresAt));
    }

    public async Task DeleteAttachmentAsync(string s3Key, CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = s3Key
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);

        _logger.LogInformation("Deleted attachment {S3Key}", s3Key);
    }
}
