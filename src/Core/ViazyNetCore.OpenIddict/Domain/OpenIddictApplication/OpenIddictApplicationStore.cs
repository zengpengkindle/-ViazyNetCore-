using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FreeSql;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using ViazyNetCore.Data.FreeSql;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictApplicationStore : IOpenIdApplicationStore
    {
        private readonly IOpenIddictApplicationRepository _applicationRepository;
        private readonly IOpenIddictTokenRepository _tokenRepository;
        private readonly IOptions<OpenIddictOptions> _options;
        private readonly IMapper _mapper;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public OpenIddictApplicationStore(IOpenIddictApplicationRepository applicationRepository
            , IOpenIddictTokenRepository tokenRepository
            , IOptions<OpenIddictOptions> options
            , UnitOfWorkManagerCloud unitOfWorkManagerCloud
            , IMapper mapper)
        {
            this._applicationRepository = applicationRepository;
            this._tokenRepository = tokenRepository;
            this._options = options;
            this._mapper = mapper;
            this._unitOfWorkManager = unitOfWorkManagerCloud.GetUnitOfWorkManager(options.Value.DbKey);
        }
        public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            return await this._applicationRepository.Select.CountAsync(cancellationToken);
        }

        public ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictApplicationDto>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public async ValueTask CreateAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            var entity = this._mapper.Map<OpenIddictApplicationDto, OpenIddictApplication>(application);
            await this._applicationRepository.InsertAsync(entity, cancellationToken);
        }

        public async ValueTask DeleteAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            var unow = this._unitOfWorkManager.Begin();
            await this._tokenRepository.DeleteCascadeByDatabaseAsync(p => p.ApplicationId == application.Id, cancellationToken);
            await this._applicationRepository.DeleteAsync(application.Id, cancellationToken);
            unow.Commit();
        }

        public async ValueTask<OpenIddictApplicationDto?> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var entity = await this._applicationRepository.Select.Where(p => p.ClientId == identifier).ToOneAsync(cancellationToken);
            return this._mapper.Map<OpenIddictApplication, OpenIddictApplicationDto>(entity);
        }

        public async ValueTask<OpenIddictApplicationDto?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var entity = await this._applicationRepository.Select.Where(p => p.Id == identifier.ParseTo<long>()).ToOneAsync(cancellationToken);
            return this._mapper.Map<OpenIddictApplication, OpenIddictApplicationDto>(entity);
        }

        public async IAsyncEnumerable<OpenIddictApplicationDto> FindByPostLogoutRedirectUriAsync(string uri, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var applications = await this._applicationRepository
                .Where(x => x.PostLogoutRedirectUris.Contains(uri)).ToListAsync(cancellationToken);
            foreach (var application in applications)
            {
                var dto = this._mapper.Map<OpenIddictApplication, OpenIddictApplicationDto>(application);
                var addresses = await GetPostLogoutRedirectUrisAsync(dto, cancellationToken);
                if (addresses.Contains(uri, StringComparer.Ordinal))
                {
                    yield return dto;
                }
            }

        }

        public async IAsyncEnumerable<OpenIddictApplicationDto> FindByRedirectUriAsync(string uri, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var applications = await this._applicationRepository
                .Where(x => x.RedirectUris.Contains(uri)).ToListAsync(cancellationToken); ;
            foreach (var application in applications)
            {
                var dto = this._mapper.Map<OpenIddictApplication, OpenIddictApplicationDto>(application);
                var addresses = await GetRedirectUrisAsync(dto, cancellationToken);
                if (addresses.Contains(uri, StringComparer.Ordinal))
                {
                    yield return dto;
                }
            }
        }

        public ValueTask<TResult?> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictApplicationDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public ValueTask<string?> GetClientIdAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));
            return new ValueTask<string?>(application.ClientId);
        }

        public ValueTask<string?> GetClientSecretAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));
            return new ValueTask<string?>(application.ClientSecret);
        }

        public ValueTask<string?> GetClientTypeAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            Check.NotNull(application, nameof(application));
            return new ValueTask<string?>(application.Type);
        }

        public ValueTask<string> GetClientUriAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken = default)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.ClientUri);
        }

        public ValueTask<string?> GetConsentTypeAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(application.ConsentType);
        }

        public ValueTask<string?> GetDisplayNameAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(application.DisplayName);
        }

        public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(application.DisplayNames))
            {
                return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
            }
            using (var document = JsonDocument.Parse(application.DisplayNames))
            {
                var builder = ImmutableDictionary.CreateBuilder<CultureInfo, string>();

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    var value = property.Value.GetString();
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    builder[CultureInfo.GetCultureInfo(property.Name)] = value;
                }

                return new ValueTask<ImmutableDictionary<CultureInfo, string>>(builder.ToImmutable());
            }
        }

        public ValueTask<string?> GetIdAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>((application.Id).ToString());
        }

        public ValueTask<string> GetLogoUriAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken = default)
        {
            Check.NotNull(application, nameof(application));

            return new ValueTask<string>(application.LogoUri);
        }

        public ValueTask<ImmutableArray<string>> GetPermissionsAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(application.Permissions))
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            using (var document = JsonDocument.Parse(application.Permissions))
            {
                var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

                foreach (var element in document.RootElement.EnumerateArray())
                {
                    var value = element.GetString();
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    builder.Add(value);
                }

                return new ValueTask<ImmutableArray<string>>(builder.ToImmutable());
            }
        }

        public ValueTask<ImmutableArray<string>> GetPostLogoutRedirectUrisAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(application.PostLogoutRedirectUris))
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            using (var document = JsonDocument.Parse(application.PostLogoutRedirectUris))
            {
                var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

                foreach (var element in document.RootElement.EnumerateArray())
                {
                    var value = element.GetString();
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    builder.Add(value);
                }

                return new ValueTask<ImmutableArray<string>>(builder.ToImmutable());
            };
        }

        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(application.Properties))
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            using (var document = JsonDocument.Parse(application.Properties))
            {
                var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    builder[property.Name] = property.Value.Clone();
                }
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(builder.ToImmutable());
            }
        }

        public ValueTask<ImmutableArray<string>> GetRedirectUrisAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(application.RedirectUris))
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            using (var document = JsonDocument.Parse(application.RedirectUris))
            {
                var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

                foreach (var element in document.RootElement.EnumerateArray())
                {
                    var value = element.GetString();
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    builder.Add(value);
                }

                return new ValueTask<ImmutableArray<string>>(builder.ToImmutable());
            }
        }

        public ValueTask<ImmutableArray<string>> GetRequirementsAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(application.Requirements))
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            using (var document = JsonDocument.Parse(application.Requirements))
            {
                var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

                foreach (var element in document.RootElement.EnumerateArray())
                {
                    var value = element.GetString();
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    builder.Add(value);
                }

                return new ValueTask<ImmutableArray<string>>(builder.ToImmutable());
            }
        }

        public ValueTask<OpenIddictApplicationDto> InstantiateAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<OpenIddictApplicationDto>(new OpenIddictApplicationDto
            {
                Id = Snowflake.NextId()
            });
        }

        public async IAsyncEnumerable<OpenIddictApplicationDto> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
        {
            var applications = await this._applicationRepository.Select.Skip(offset ?? 0).Take(count ?? 0).ToListAsync(cancellationToken);
            foreach (var application in applications)
            {
                var dto = this._mapper.Map<OpenIddictApplication, OpenIddictApplicationDto>(application);
                yield return dto;
            }
        }

        public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictApplicationDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public ValueTask SetClientIdAsync(OpenIddictApplicationDto application, string? identifier, CancellationToken cancellationToken)
        {
            application.ClientId = identifier;
            return default;
        }

        public ValueTask SetClientSecretAsync(OpenIddictApplicationDto application, string? secret, CancellationToken cancellationToken)
        {
            application.ClientSecret = secret;
            return default;
        }

        public ValueTask SetClientTypeAsync(OpenIddictApplicationDto application, string? type, CancellationToken cancellationToken)
        {
            application.Type = type;
            return default;
        }

        public ValueTask SetConsentTypeAsync(OpenIddictApplicationDto application, string? type, CancellationToken cancellationToken)
        {
            application.ConsentType = type;
            return default;
        }

        public ValueTask SetDisplayNameAsync(OpenIddictApplicationDto application, string? name, CancellationToken cancellationToken)
        {
            application.DisplayName = name;
            return default;
        }

        public ValueTask SetDisplayNamesAsync(OpenIddictApplicationDto application, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
        {
            if (names is null || names.IsEmpty)
            {
                application.DisplayNames = null;
                return default;
            }

            application.DisplayNames = WriteStream(writer =>
            {
                writer.WriteStartObject();
                foreach (var pair in names)
                {
                    writer.WritePropertyName(pair.Key.Name);
                    writer.WriteStringValue(pair.Value);
                }
                writer.WriteEndObject();
            });

            return default;
        }

        public ValueTask SetPermissionsAsync(OpenIddictApplicationDto application, ImmutableArray<string> permissions, CancellationToken cancellationToken)
        {
            if (permissions.IsDefaultOrEmpty)
            {
                application.Permissions = null;
                return default;
            }

            application.Permissions = WriteStream(writer =>
            {
                writer.WriteStartArray();
                foreach (var permission in permissions)
                {
                    writer.WriteStringValue(permission);
                }
                writer.WriteEndArray();
            });

            return default;
        }

        public ValueTask SetPostLogoutRedirectUrisAsync(OpenIddictApplicationDto application, ImmutableArray<string> uris, CancellationToken cancellationToken)
        {
            if (uris.IsDefaultOrEmpty)
            {
                application.PostLogoutRedirectUris = null;
                return default;
            }

            application.PostLogoutRedirectUris = WriteStream(writer =>
            {
                writer.WriteStartArray();
                foreach (var uri in uris)
                {
                    writer.WriteStringValue(uri);
                }
                writer.WriteEndArray();
            });

            return default;
        }

        public ValueTask SetPropertiesAsync(OpenIddictApplicationDto application, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            if (properties is null || properties.IsEmpty)
            {
                application.Properties = null;
                return default;
            }

            application.Properties = WriteStream(writer =>
            {
                writer.WriteStartObject();
                foreach (var property in properties)
                {
                    writer.WritePropertyName(property.Key);
                    property.Value.WriteTo(writer);
                }
                writer.WriteEndObject();
            });

            return default;
        }

        public ValueTask SetRedirectUrisAsync(OpenIddictApplicationDto application, ImmutableArray<string> uris, CancellationToken cancellationToken)
        {
            if (uris.IsDefaultOrEmpty)
            {
                application.RedirectUris = null;
                return default;
            }

            application.RedirectUris = WriteStream(writer =>
            {
                writer.WriteStartArray();
                foreach (var uri in uris)
                {
                    writer.WriteStringValue(uri);
                }
                writer.WriteEndArray();
            });

            return default;
        }

        public ValueTask SetRequirementsAsync(OpenIddictApplicationDto application, ImmutableArray<string> requirements, CancellationToken cancellationToken)
        {
            if (requirements.IsDefaultOrEmpty)
            {
                application.Requirements = null;
                return default;
            }

            application.Requirements = WriteStream(writer =>
            {
                writer.WriteStartArray();
                foreach (var requirement in requirements)
                {
                    writer.WriteStringValue(requirement);
                }
                writer.WriteEndArray();
            });

            return default;
        }

        public async ValueTask UpdateAsync(OpenIddictApplicationDto application, CancellationToken cancellationToken)
        {
            var entity = await this._applicationRepository.GetAsync(application.Id, cancellationToken: cancellationToken);

            try
            {
                var updateEntity = this._mapper.Map<OpenIddictApplicationDto, OpenIddictApplication>(application);
                await this._applicationRepository.UpdateAsync(updateEntity, cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
            }

            var result = await this._applicationRepository.FindAsync(entity.Id, cancellationToken: cancellationToken);

            application = this._mapper.Map<OpenIddictApplication, OpenIddictApplicationDto>(result);

        }

        protected virtual string WriteStream(Action<Utf8JsonWriter> action)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Indented = false
                }))
                {
                    action(writer);
                    writer.Flush();
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }
    }
}
