using System.Threading.RateLimiting;

namespace RealEstateManager.Infrastructure;

public class ApiRateLimiter : IDisposable
{
    private readonly RateLimiter _rateLimiter;

    public ApiRateLimiter(int permitLimit = 90, int windowSeconds = 60)
    {
        _rateLimiter = new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,
            Window = TimeSpan.FromSeconds(windowSeconds),
            SegmentsPerWindow = 6, // Divides window into 6 segments for smoother limiting
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 100 // Max requests that can wait in queue
        });
    }

    public async Task WaitAsync(CancellationToken cancellationToken = default)
    {
        using var lease = await _rateLimiter.AcquireAsync(permitCount: 1, cancellationToken);
            
        if (!lease.IsAcquired)
        {
            throw new InvalidOperationException("Failed to acquire rate limit lease");
        }
    }

    public void Dispose()
    {
        _rateLimiter.Dispose();
    }
}