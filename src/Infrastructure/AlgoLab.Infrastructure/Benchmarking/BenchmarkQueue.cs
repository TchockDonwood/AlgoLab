using AlgoLab.Application.Common.Interfaces;
using System.Threading.Channels;

namespace AlgoLab.Infrastructure.Benchmarking;
public class BenchmarkQueue : IBenchmarkQueue
{
    private readonly Channel<Guid> _channel;

    public BenchmarkQueue()
    {
        _channel = Channel.CreateUnbounded<Guid>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
    }

    public ValueTask EnqueueAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        return _channel.Writer.WriteAsync(
            sessionId,
            cancellationToken);
    }

    public ValueTask<Guid> DequeueAsync(
        CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAsync(
            cancellationToken);
    }
}