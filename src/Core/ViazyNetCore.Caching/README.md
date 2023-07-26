### About
ViazyNetCore.Caching 是基于缓存的应用,支持二级缓存;

### How to Use
```csharp
builder.Services.AddCaching()
    .UseDistributedMemoryCache();
```
 
使用 StackExchange.Redis
```
dotnet add package ViazyNetCore.Redis
```
```
builder.Services.AddCaching()
  .UseStackExchangeRedisCaching(options =>
    {
        options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
        {
            EndPoints =
            {
                { redisConfig.Host, redisConfig.Port }
            },
            Password = redisConfig.Password,
        };
    })

```