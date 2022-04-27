using grpc_client;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;

var request = new HelloRequest
{
    Name = "Johan"
};
var url = "dns:///grpc-server:5000";
Console.WriteLine($"Connecting to URL {url}");
var defaultMethodConfig = new MethodConfig
{
    Names = {MethodName.Default},
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = {StatusCode.Unavailable}
    }
};
using var channel = GrpcChannel.ForAddress(
    url,
    new GrpcChannelOptions
    {
        Credentials = ChannelCredentials.Insecure,
        ServiceConfig = new ServiceConfig
            {LoadBalancingConfigs = {new RoundRobinConfig()}, MethodConfigs = {defaultMethodConfig}}
    });
var client = new Greeter.GreeterClient(channel);
var serverLoop = async () =>
{
    for (;;)
    {
        Thread.Sleep(5000);
        var reply = await client.SayHelloAsync(request);
        Console.WriteLine($"[{DateTime.Now}] Got reply {reply.Message}");
    }
};

try
{
    await serverLoop();
}
catch (Exception e)
{
    Console.WriteLine(e);
    await serverLoop();
}