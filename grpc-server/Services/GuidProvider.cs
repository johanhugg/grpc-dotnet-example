namespace grpc.Services;

public interface IGuidProvider
{
    string Guid36 { get; }
}

public class GuidProvider : IGuidProvider
{
    public string Guid36 { get; } = Guid.NewGuid().ToString("D");
}