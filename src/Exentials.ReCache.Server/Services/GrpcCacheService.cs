using Exentials.ReCache.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace Exentials.ReCache.Server.Services;

/// <summary>
/// Service for gRPC client communication 
/// </summary>
[Authorize(Roles = "Admin, Client")]
public sealed class GrpcCacheService(ReCacheProvider cacheProvider) : RpcCacheService.RpcCacheServiceBase
{
    private readonly ReCacheProvider _cache = cacheProvider;

    private static Task<Response> TaskResult(int code, string message, string? data = null)
    {
        return Task.FromResult(new Response { Code = code, Message = message, Data = data ?? string.Empty });
    }

    public static Task<Response> NoResult()
    {
        return TaskResult(0, "No result");
    }

    public static Task<Response> Ok()
    {
        return Ok(string.Empty);
    }

    public static Task<Response> Ok(string data)
    {
        return TaskResult(1, "OK", data);
    }

    public static Task<Response> Ok(HashSet<string>? data)
    {
        string json = JsonSerializer.Serialize(data);
        return Ok(json);
    }

    public override Task<KeysResponse> ListDictionary(KeysRequest request, ServerCallContext context)
    {
        string nameSpace = request.HasNameSpace ? request.NameSpace : string.Empty;
        KeysResponse response = new();
        var keys = _cache.Keys(nameSpace).Where(k => k.KeyType == KeyType.Set).Select(k => new KeyItem { Key = k.Key, NameSpace = k.Namespace });
        response.Items.AddRange(keys);
        return Task.FromResult(response);
    }

    public override Task<NamespacesResponse> ListDictionaryNamespaces(Empty request, ServerCallContext context)
    {
        NamespacesResponse response = new();
        response.Items.AddRange(_cache.Namespaces(KeyType.Set));
        return Task.FromResult(response);
    }

    public override Task<Response> SetDictionary(RpcCacheData request, ServerCallContext context)
    {
        ReCacheKey key = new(request.Key, request.NameSpace, KeyType.Set);
        _cache.Set(
            key,
            request.Value,
            request.AbsoluteExpire?.ToDateTimeOffset(),
            request.SlidingExpire?.ToTimeSpan()
        );
        return Ok();
    }

    public override Task<Response> GetDictionary(RpcKey request, ServerCallContext context)
    {
        ReCacheKey key = new(request.Key, request.NameSpace, KeyType.Set);
        string? value = _cache.Get<string>(key);

        return value is null
            ? NoResult()
            : Ok(value);
    }

    public override Task<Response> DelDictionary(RpcKey request, ServerCallContext context)
    {
        ReCacheKey key = new(request.Key, request.NameSpace, KeyType.Set);
        string? value = _cache.Del<string>(key);
        return value is null
            ? NoResult()
            : Ok(value);
    }

    public override Task<KeysResponse> ListHashSet(KeysRequest request, ServerCallContext context)
    {
        string nameSpace = request.HasNameSpace ? request.NameSpace : string.Empty;
        KeysResponse response = new();
        var keys = _cache.Keys(nameSpace).Where(k => k.KeyType == KeyType.HashSet).Select(k => new KeyItem { Key = k.Key, NameSpace = k.Namespace });
        response.Items.AddRange(keys);
        return Task.FromResult(response);
    }

    public override Task<NamespacesResponse> ListHashSetNamespaces(Empty request, ServerCallContext context)
    {
        NamespacesResponse response = new();
        response.Items.AddRange(_cache.Namespaces(KeyType.HashSet));
        return Task.FromResult(response);
    }

    public override Task<Response> SetHashSet(RpcCacheData request, ServerCallContext context)
    {
        ReCacheKey key = new(request.Key, request.NameSpace, KeyType.HashSet);
        _cache.SetHashSet(
            key,
            request.Value,
            request.AbsoluteExpire?.ToDateTimeOffset(),
            request.SlidingExpire?.ToTimeSpan()
        );
        return Ok();
    }

    public override Task<Response> GetHashSet(RpcKey request, ServerCallContext context)
    {
        ReCacheKey key = new(request.Key, request.NameSpace, KeyType.HashSet);
        HashSet<string>? hashSet = _cache.GetHashSet<string>(key);
        return Ok(hashSet);
    }

    public override Task<Response> DelHashSet(RpcCacheData request, ServerCallContext context)
    {
        ReCacheKey key = new(request.Key, request.NameSpace, KeyType.HashSet);
        _cache.DelHashSet(key, request.Value);
        return Ok();
    }

    public override Task<Response> RemoveHashSet(RpcKey request, ServerCallContext context)
    {
        ReCacheKey key = new(request.Key, request.NameSpace, KeyType.HashSet);
        _cache.RemoveEntry(key);
        return Ok();
    }

    public override Task<Response> Clear(KeysRequest request, ServerCallContext context)
    {
        _cache.Clear(request.NameSpace);
        return Ok();
    }

    //public override Task<Response> QueuePush(QueueData request, ServerCallContext context)
    //{
    //    Queue.Enqueue(request.Message);
    //    return Ok();
    //}

    //public override Task<Response> QueuePop(QueueRequest request, ServerCallContext context)
    //{
    //    if (Queue.TryDequeue(out string? message))
    //    {
    //        return Ok(ByteString.CopyFrom(message, Encoding.Default));
    //    }
    //    return Ok();
    //}

    //public override Task<Response> QueuePeek(QueueRequest request, ServerCallContext context)
    //{
    //    if (Queue.TryPeek(out string? message))
    //    {
    //        return Ok(ByteString.CopyFrom(message, Encoding.Default));
    //    }
    //    return Ok();
    //}
}
