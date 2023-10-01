using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
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
using ViazyNetCore.OpenIddict.Domain.Entity;
using static System.Net.Mime.MediaTypeNames;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictAuthorizationStore : IOpenIddictAuthorizationStore<OpenIddictAuthorizationDto>
    {
        private readonly IOpenIddictAuthorizationRepository _authorizationRepository;
        private readonly IOpenIddictTokenRepository _tokenRepository;
        private readonly IOpenIddictApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private UnitOfWorkManager _unitOfWorkManager;

        public OpenIddictAuthorizationStore(IOpenIddictAuthorizationRepository authorizationRepository
            , IOpenIddictTokenRepository tokenRepository
            , IOpenIddictApplicationRepository applicationRepository
            , UnitOfWorkManagerCloud unitOfWorkManagerCloud
            , IOptions<OpenIddictOptions> options
            , IMapper mapper)
        {
            this._authorizationRepository = authorizationRepository;
            this._tokenRepository = tokenRepository;
            this._applicationRepository = applicationRepository;
            this._mapper = mapper;
            this._unitOfWorkManager = unitOfWorkManagerCloud.GetUnitOfWorkManager(options.Value.DbKey);
        }

        public async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            return await this._authorizationRepository.Select.CountAsync(cancellationToken);
        }

        public ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictAuthorizationDto>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public async ValueTask CreateAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            var entity = this._mapper.Map<OpenIddictAuthorizationDto, OpenIddictAuthorization>(authorization);
            await this._authorizationRepository.InsertAsync(entity, cancellationToken);
            authorization.Id = entity.Id;
        }

        public async ValueTask DeleteAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            var unow = this._unitOfWorkManager.Begin();
            await this._tokenRepository.DeleteCascadeByDatabaseAsync(p => p.AuthorizationId == authorization.Id, cancellationToken);
            await this._authorizationRepository.DeleteAsync(authorization.Id, cancellationToken);
            unow.Commit();
        }

        public async IAsyncEnumerable<OpenIddictAuthorizationDto> FindAsync(string subject, string client, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await this._authorizationRepository.Select.Where(p => p.Subject == subject && p.ApplicationId == client.ParseTo<long>()).ToListAsync(cancellationToken);
            foreach (var authorization in authorizations)
            {
                var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
                yield return authorizationDto;
            }
        }

        public async IAsyncEnumerable<OpenIddictAuthorizationDto> FindAsync(string subject, string client, string status, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await this._authorizationRepository.Select
                .Where(p => p.Subject == subject
                && p.Status == status
                && p.ApplicationId == client.ParseTo<long>())
                .ToListAsync(cancellationToken);
            foreach (var authorization in authorizations)
            {
                var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
                yield return authorizationDto;
            }
        }

        public async IAsyncEnumerable<OpenIddictAuthorizationDto> FindAsync(string subject, string client, string status, string type, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await this._authorizationRepository.Select
                .Where(p => p.Subject == subject
                && p.Status == status && p.Type == type
                && p.ApplicationId == client.ParseTo<long>())
                .ToListAsync(cancellationToken);
            foreach (var authorization in authorizations)
            {
                var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
                yield return authorizationDto;
            }
        }

        public async IAsyncEnumerable<OpenIddictAuthorizationDto> FindAsync(string subject, string client, string status, string type, ImmutableArray<string> scopes, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await this._authorizationRepository.Select
                .Where(p => p.Subject == subject
                && p.Status == status && p.Type == type
                && p.ApplicationId == client.ParseTo<long>())
                .ToListAsync(cancellationToken);
            foreach (var authorization in authorizations)
            {
                var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
                if (new HashSet<string>(await GetScopesAsync(authorizationDto, cancellationToken), StringComparer.Ordinal).IsSupersetOf(scopes))
                {
                    yield return authorizationDto;
                }
            }
        }

        public async IAsyncEnumerable<OpenIddictAuthorizationDto> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await this._authorizationRepository.Select
                .Where(p => p.ApplicationId == identifier.ParseTo<long>())
                .ToListAsync(cancellationToken);
            foreach (var authorization in authorizations)
            {
                var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
                yield return authorizationDto;
            }
        }

        public async ValueTask<OpenIddictAuthorizationDto?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var authorization = await this._authorizationRepository.Select
                .Where(p => p.ApplicationId == identifier.ParseTo<long>())
                .FirstAsync(cancellationToken);
            var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
            return authorizationDto;
        }

        public async IAsyncEnumerable<OpenIddictAuthorizationDto> FindBySubjectAsync(string subject, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await this._authorizationRepository.Select
                .Where(p => p.Subject == subject)
                .ToListAsync(cancellationToken);
            foreach (var authorization in authorizations)
            {
                var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
                yield return authorizationDto;
            }
        }

        public ValueTask<string?> GetApplicationIdAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(authorization.ApplicationId.HasValue
                ? authorization.ApplicationId.Value.ToString()
                : null);
        }

        public ValueTask<TResult?> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorizationDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public ValueTask<DateTimeOffset?> GetCreationDateAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            return authorization.CreateTime is null
            ? new ValueTask<DateTimeOffset?>(result: null)
            : new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(authorization.CreateTime.Value, DateTimeKind.Utc));
        }

        public ValueTask<string?> GetIdAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(authorization.Id.ToString());
        }

        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(authorization.Properties))
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            using (var document = JsonDocument.Parse(authorization.Properties))
            {
                var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    builder[property.Name] = property.Value.Clone();
                }

                return new ValueTask<ImmutableDictionary<string, JsonElement>>(builder.ToImmutable());
            }
        }

        public ValueTask<ImmutableArray<string>> GetScopesAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(authorization.Scopes))
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            using (var document = JsonDocument.Parse(authorization.Scopes))
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

        public ValueTask<string?> GetStatusAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(authorization.Status);
        }

        public ValueTask<string?> GetSubjectAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(authorization.Subject);
        }

        public ValueTask<string?> GetTypeAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            return new ValueTask<string?>(authorization.Type);
        }

        public ValueTask<OpenIddictAuthorizationDto> InstantiateAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<OpenIddictAuthorizationDto>(new OpenIddictAuthorizationDto
            {
                Id = Snowflake.NextId()
            });
        }

        public async IAsyncEnumerable<OpenIddictAuthorizationDto> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await this._authorizationRepository.Select.Skip(offset ?? 0).Take(count ?? 0).ToListAsync(cancellationToken);
            foreach (var authorization in authorizations)
            {
                var authorizationDto = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(authorization);
                yield return authorizationDto;
            }
        }

        public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictAuthorizationDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public async ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
        {
            var unow = this._unitOfWorkManager.Begin();
            var date = threshold.UtcDateTime;
            await this._authorizationRepository.PruneAsync(date, cancellationToken);
            unow.Commit();
        }

        public async ValueTask SetApplicationIdAsync(OpenIddictAuthorizationDto authorization, string? identifier, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(identifier))
            {
                var application = await this._applicationRepository.GetAsync(identifier.ParseTo<long>(), cancellationToken: cancellationToken);
                authorization.ApplicationId = application.Id;
            }
            else
            {
                authorization.ApplicationId = null;
            }
        }

        public ValueTask SetCreationDateAsync(OpenIddictAuthorizationDto authorization, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            authorization.CreateTime = date?.UtcDateTime;

            return default;
        }

        public ValueTask SetPropertiesAsync(OpenIddictAuthorizationDto authorization, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            if (properties is null || properties.IsEmpty)
            {
                authorization.Properties = null;
                return default;
            }

            authorization.Properties = WriteStream(writer =>
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

        public ValueTask SetScopesAsync(OpenIddictAuthorizationDto authorization, ImmutableArray<string> scopes, CancellationToken cancellationToken)
        {
            if (scopes.IsDefaultOrEmpty)
            {
                authorization.Scopes = null;
                return default;
            }

            authorization.Scopes = WriteStream(writer =>
            {
                writer.WriteStartArray();
                foreach (var scope in scopes)
                {
                    writer.WriteStringValue(scope);
                }
                writer.WriteEndArray();
            });

            return default;
        }

        public ValueTask SetStatusAsync(OpenIddictAuthorizationDto authorization, string? status, CancellationToken cancellationToken)
        {
            authorization.Status = status;

            return default;
        }

        public ValueTask SetSubjectAsync(OpenIddictAuthorizationDto authorization, string? subject, CancellationToken cancellationToken)
        {
            authorization.Subject = subject;

            return default;
        }

        public ValueTask SetTypeAsync(OpenIddictAuthorizationDto authorization, string? type, CancellationToken cancellationToken)
        {
            authorization.Type = type;

            return default;
        }

        public async ValueTask UpdateAsync(OpenIddictAuthorizationDto authorization, CancellationToken cancellationToken)
        {
            var entity = await this._authorizationRepository.GetAsync(authorization.Id, cancellationToken: cancellationToken);

            try
            {
                var updateEntity = this._mapper.Map<OpenIddictAuthorizationDto, OpenIddictAuthorization>(authorization);

                await this._authorizationRepository.UpdateAsync(updateEntity, cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
            }
            var result = await this._authorizationRepository.FindAsync(entity.Id, cancellationToken: cancellationToken);

            authorization = this._mapper.Map<OpenIddictAuthorization, OpenIddictAuthorizationDto>(result);
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
