# <img src="https://github.com/exentials/re-cache/raw/main/src/re-cache-icon.png" height="48" width="48" /> Exentials.ReCache

Re-Cache is a dockerized in-memory cache key/values data store, build on top of dotnet the client/server communication use the gRPC protocol to maximize performance.

The keys/values can be stored in different structure and can be grouped by a namespace, so you can use the same key with different value in a different group or in different structure of data.

Supported structures are `Set` and `HashSet`.

## HOW TO:

## Re-Cache Server

Re-Cache server is provided by a ready to run docker container:

```
docker run -p 443:443 -p 80:80 -d exentials/re-cache:latest
```

To change the default encription key and authentication account you could add the environment variables:

```
-e Auth__Secretkey=mysecretkey
-e Users__AdminUsername=my_admin_username
-e Users__AdminPassword=my_admin_password
-e Users__ClientUsername=my_client_username
-e Users__ClientPassword=my_client_passowrd
```

gRPC require a secure connection so the container use a self generated certificate.

To test the server use the provided [Re-Cache Cli](https://github.com/exentials/re-cache/releases) console application; the default password is `recachepwd`.

```
recli connect <host> -p recachepwd
```

Re-Cache server expose also a web page, intentions are to provide a dashboard to show the cache statistics and allow the server administration.

The server implements also gRPC transcoding to OpenAPI endpoint.

Re-Cache server implements also an automatic backup/restore to recover stored data during the start process.

## Re-Cache Client

To implement client service communication in your application use the provided NUGET package library.

```
dotnet add package Exentials.ReCache.Client
```

Then add the client configuration programmatically

```csharp
builder.Services.AddReCacheClient(options =>
{
    options.SslUrl = "https://localhost";
    options.Token = "<token>";
    options.KeepAlive = true;
    options.IgnoreSslCertificate = true;  // to allow self generated certificate
});
```

or by the appsettings.json

```json
"ReCache": {
    "SslUrl": "https://localhost",
    "Token": "<token>",
    "KeepAlive": true,
    "IgnoreSslCertificate": true
}
```

and then

```csharp
builder.Services.Configure<ReCacheClientOptions>(builder.Configuration.GetSection(ReCacheClientOptions.ReCache));
builder.Services.AddReCacheClient();
```

To retrieve the token use the server page and post username and password (default/recachepwd).

<br/>

# TODO:

- Add a Queue structure
- Create `@exentials/re-cache-node` for NodeRed node communication.
