using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Exentials.ReCache.Server.Interceptors;

public class ServerLoggingInterceptor(ILogger<ServerLoggingInterceptor> logger) : Interceptor
{
    private readonly ILogger _logger = logger;

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var methodType = MethodType.Unary;
        var method = context.Method;

        _logger.LogDebug("Starting receiving call. Type: {methodType}. Method: {method}, Parameters: {request}.", methodType, method, request);

        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error thrown by {method}.", method);
            throw;
        }
    }
}
