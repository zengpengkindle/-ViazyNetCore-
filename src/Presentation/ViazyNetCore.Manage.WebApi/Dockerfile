#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Presentation/ViazyNetCore.Manage.WebApi/ViazyNetCore.Manage.WebApi.csproj", "Presentation/ViazyNetCore.Manage.WebApi/"]
COPY ["Core/ViazyNetCore.AttachmentProvider/ViazyNetCore.AttachmentProvider.csproj", "Core/ViazyNetCore.AttachmentProvider/"]
COPY ["Core/ViazyNetCore.Core/ViazyNetCore.Core.csproj", "Core/ViazyNetCore.Core/"]
COPY ["Infrastructure/ViazyNetCore.Annotations/ViazyNetCore.Annotations.csproj", "Infrastructure/ViazyNetCore.Annotations/"]
COPY ["Core/ViazyNetCore.Authorization/ViazyNetCore.Authorization.csproj", "Core/ViazyNetCore.Authorization/"]
COPY ["Infrastructure/ViazyNetCore.Swagger/ViazyNetCore.Swagger.csproj", "Infrastructure/ViazyNetCore.Swagger/"]
COPY ["Core/ViazyNetCore.Auth/ViazyNetCore.Auth.csproj", "Core/ViazyNetCore.Auth/"]
COPY ["Infrastructure/ViazyNetCore.DI/ViazyNetCore.DI.csproj", "Infrastructure/ViazyNetCore.DI/"]
COPY ["Infrastructure/ViazyNetCore.EventBus/ViazyNetCore.EventBus.csproj", "Infrastructure/ViazyNetCore.EventBus/"]
COPY ["Core/ViazyNetCore.Caching/ViazyNetCore.Caching.csproj", "Core/ViazyNetCore.Caching/"]
COPY ["Core/ViazyNetCore.Data/ViazyNetCore.Data.FreeSql.csproj", "Core/ViazyNetCore.Data/"]
COPY ["Core/ViazyNetCore.Identity/ViazyNetCore.Identity.csproj", "Core/ViazyNetCore.Identity/"]
COPY ["Infrastructure/ViazyNetCore.AutoMapper/ViazyNetCore.AutoMapper.csproj", "Infrastructure/ViazyNetCore.AutoMapper/"]
COPY ["Infrastructure/ViazyNetCore.MultiTenancy/ViazyNetCore.MultiTenancy.csproj", "Infrastructure/ViazyNetCore.MultiTenancy/"]
COPY ["Core/ViazyNetCore.Filter/ViazyNetCore.Filter.csproj", "Core/ViazyNetCore.Filter/"]
COPY ["Infrastructure/ViazyNetCore.AspNetCore/ViazyNetCore.AspNetCore.csproj", "Infrastructure/ViazyNetCore.AspNetCore/"]
COPY ["Infrastructure/ViazyNetCore.EventBus.RabbitMQ/ViazyNetCore.EventBus.RabbitMQ.csproj", "Infrastructure/ViazyNetCore.EventBus.RabbitMQ/"]
COPY ["Infrastructure/ViazyNetCore.RabbitMQ/ViazyNetCore.RabbitMQ.csproj", "Infrastructure/ViazyNetCore.RabbitMQ/"]
COPY ["Infrastructure/ViazyNetCore.Formatter.Response/ViazyNetCore.Formatter.Response.csproj", "Infrastructure/ViazyNetCore.Formatter.Response/"]
COPY ["Infrastructure/ViazyNetCore.NLog/ViazyNetCore.NLog.csproj", "Infrastructure/ViazyNetCore.NLog/"]
COPY ["Infrastructure/ViazyNetCore.Redis/ViazyNetCore.Redis.csproj", "Infrastructure/ViazyNetCore.Redis/"]
COPY ["Infrastructure/ViazyNetCore.TaskScheduler.RabbitMQ/ViazyNetCore.TaskScheduler.RabbitMQ.csproj", "Infrastructure/ViazyNetCore.TaskScheduler.RabbitMQ/"]
COPY ["Infrastructure/ViazyNetCore.TaskScheduler/ViazyNetCore.TaskScheduler.csproj", "Infrastructure/ViazyNetCore.TaskScheduler/"]
COPY ["Infrastructure/ViazyNetCore.Web.DevServer/ViazyNetCore.Web.DevServer.csproj", "Infrastructure/ViazyNetCore.Web.DevServer/"]
COPY ["src/ShopMall/ViazyNetCore.ShopMall.Manage.Application/ViazyNetCore.ShopMall.Manage.Application.csproj", "src/ShopMall/ViazyNetCore.ShopMall.Manage.Application/"]
COPY ["src/ViazyNetCore.ShopMall.Modules/ViazyNetCore.ShopMall.Modules.csproj", "src/ViazyNetCore.ShopMall.Modules/"]
COPY ["sdks/payment/Essensoft.AspNetCore.Payment.Alipay/Essensoft.AspNetCore.Payment.Alipay.csproj", "sdks/payment/Essensoft.AspNetCore.Payment.Alipay/"]
COPY ["sdks/payment/Essensoft.AspNetCore.Payment.Security/Essensoft.AspNetCore.Payment.Security.csproj", "sdks/payment/Essensoft.AspNetCore.Payment.Security/"]
COPY ["sdks/payment/Essensoft.Paylink.WeChatPay/Essensoft.Paylink.WeChatPay.csproj", "sdks/payment/Essensoft.Paylink.WeChatPay/"]
COPY ["src/ViazyNetCore.Business.Core/ViazyNetCore.Business.Core.csproj", "src/ViazyNetCore.Business.Core/"]
COPY ["src/ViazyNetCore.Domain/ViazyNetCore.Domain.csproj", "src/ViazyNetCore.Domain/"]
COPY ["src/ViazyNetCore.Model/ViazyNetCore.Model.csproj", "src/ViazyNetCore.Model/"]
RUN dotnet restore "Presentation/ViazyNetCore.Manage.WebApi/ViazyNetCore.Manage.WebApi.csproj"
COPY . .
WORKDIR "/src/Presentation/ViazyNetCore.Manage.WebApi"
RUN dotnet build "ViazyNetCore.Manage.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ViazyNetCore.Manage.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ViazyNetCore.Manage.WebApi.dll"]