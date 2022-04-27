using Grpc.Core;

namespace grpc.Services;

public class GreeterService : Greeter.GreeterBase
{
    public IGuidProvider GuidProvider { get; }
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(
        ILogger<GreeterService> logger,
        IGuidProvider guidProvider)
    {
        GuidProvider = guidProvider;
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(
        HelloRequest request,
        ServerCallContext context)
    {
        var message = "Hello " + request.Name;
        _logger.LogInformation($"[{GuidProvider.Guid36}] Responding {message}");
        return Task.FromResult(
            new HelloReply
            {
                Message = message
            });
    }
}