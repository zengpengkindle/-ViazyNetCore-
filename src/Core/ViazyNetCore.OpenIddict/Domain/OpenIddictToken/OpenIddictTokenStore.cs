using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FreeSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using ViazyNetCore.Data.FreeSql;
using ViazyNetCore.OpenIddict.Domain.Entity;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictTokenStore : IOpenIddictTokenStore<OpenIddictTokenDto>
    {
        private readonly IOpenIddictTokenRepository _tokenRepository;
        private readonly IOpenIddictApplicationRepository _applicationRepository;
        private readonly IOpenIddictAuthorizationRepository _authorizationRepository;
        private readonly IOptions<OpenIddictOptions> _options;
        private readonly IMapper _mapper;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public OpenIddictTokenStore(IOpenIddictTokenRepository tokenRepository
            , IOpenIddictApplicationRepository applicationRepository
            , IOpenIddictAuthorizationRepository authorizationRepository
            , IOptions<OpenIddictOptions> options
            , UnitOfWorkManagerCloud unitOfWorkManagerCloud
            , IMapper mapper)
        {
            this._tokenRepository = tokenRepository;
            this._applicationRepository = applicationRepository;
            this._authorizationRepository = authorizationRepository;
            this._options = options;
            this._mapper = mapper;
            this._unitOfWorkManager = unitOfWorkManagerCloud.GetUnitOfWorkManager(options.Value.DbKey);
        }


        public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            return await this._tokenRepository.Select.CountAsync(cancellationToken);
        }

        public virtual async ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictTokenDto>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            return query(CreateQuaryableFilter()).LongCount();
        }

        protected virtual IQueryable<OpenIddictTokenDto> CreateQuaryableFilter()
        {
            return this._tokenRepository.Orm.Select<OpenIddictTokenDto>().AsTable((type, oldname) => "OpenIddictToken").AsQueryable();
        }

        public virtual async ValueTask CreateAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));
            var entity = this._mapper.Map<OpenIddictTokenDto, OpenIddictToken>(token);
            await this._tokenRepository.InsertAsync(entity, cancellationToken: cancellationToken);

            //var tokenEnity = await this._tokenRepository.FindAsync(token.Id, cancellationToken: cancellationToken);
        }

        public virtual async ValueTask DeleteAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            try
            {
                var entity = this._mapper.Map<OpenIddictTokenDto, OpenIddictToken>(token);
                await this._tokenRepository.DeleteAsync(entity, cancellationToken: cancellationToken);
            }
            catch (DbException e)
            {
                throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
            }
        }

        public virtual async IAsyncEnumerable<OpenIddictTokenDto> FindAsync(string subject, string client, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(subject, nameof(subject));
            Check.NotNullOrEmpty(client, nameof(client));

            var tokens = await this._tokenRepository.FindAsync(subject, client.ParseTo<long>(), cancellationToken);
            foreach (var token in tokens)
            {
                var tokenDto = this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
                yield return tokenDto;
            }
        }

        public virtual async IAsyncEnumerable<OpenIddictTokenDto> FindAsync(string subject, string client, string status, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(subject, nameof(subject));
            Check.NotNullOrEmpty(client, nameof(client));
            Check.NotNullOrEmpty(status, nameof(status));

            var tokens = await this._tokenRepository.FindAsync(subject, client.ParseTo<long>(), status, cancellationToken);
            foreach (var token in tokens)
            {
                var tokenDto = this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
                yield return tokenDto;
            }
        }

        public virtual async IAsyncEnumerable<OpenIddictTokenDto> FindAsync(string subject, string client, string status, string type, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(subject, nameof(subject));
            Check.NotNullOrEmpty(client, nameof(client));
            Check.NotNullOrEmpty(status, nameof(status));
            Check.NotNullOrEmpty(type, nameof(type));

            var tokens = await this._tokenRepository.FindAsync(subject, client.ParseTo<long>(), status, type, cancellationToken);
            foreach (var token in tokens)
            {
                var tokenDto = this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
                yield return tokenDto;
            }
        }

        public virtual async IAsyncEnumerable<OpenIddictTokenDto> FindByApplicationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(identifier, nameof(identifier));

            var tokens = await this._tokenRepository.FindByApplicationIdAsync(identifier.ParseTo<long>(), cancellationToken);
            foreach (var token in tokens)
            {
                var tokenDto = this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
                yield return tokenDto;
            }
        }

        public virtual async IAsyncEnumerable<OpenIddictTokenDto> FindByAuthorizationIdAsync(string identifier, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(identifier, nameof(identifier));

            var tokens = await this._tokenRepository.FindByAuthorizationIdAsync(identifier.ParseTo<long>(), cancellationToken);
            foreach (var token in tokens)
            {
                var tokenDto = this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
                yield return tokenDto;
            }
        }

        public virtual async ValueTask<OpenIddictTokenDto?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(identifier, nameof(identifier));

            var token = await this._tokenRepository.GetAsync(identifier.ParseTo<long>(), cancellationToken);
            return this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
        }

        public virtual async ValueTask<OpenIddictTokenDto?> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(identifier, nameof(identifier));

            var token = await this._tokenRepository.FindByReferenceIdAsync(identifier, cancellationToken);
            return this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
        }

        public virtual async IAsyncEnumerable<OpenIddictTokenDto> FindBySubjectAsync(string subject, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(subject, nameof(subject));

            var tokens = await this._tokenRepository.FindBySubjectAsync(subject, cancellationToken);
            foreach (var token in tokens)
            {
                var tokenDto = this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
                yield return tokenDto;
            }
        }

        public virtual ValueTask<string?> GetApplicationIdAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.ApplicationId.HasValue
                ? token.ApplicationId.Value.ToString()
                : null);
        }

        public virtual async ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictTokenDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return query(CreateQuaryableFilter(), state).FirstOrDefault();
        }

        public virtual ValueTask<string?> GetAuthorizationIdAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.AuthorizationId.HasValue
                ? token.AuthorizationId.Value.ToString()
                : null);
        }

        public virtual ValueTask<DateTimeOffset?> GetCreationDateAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.CreationDate is null)
            {
                return new ValueTask<DateTimeOffset?>(result: null);
            }

            return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.CreationDate.Value, DateTimeKind.Utc));
        }

        public virtual ValueTask<DateTimeOffset?> GetExpirationDateAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.ExpirationDate is null)
            {
                return new ValueTask<DateTimeOffset?>(result: null);
            }

            return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.ExpirationDate.Value, DateTimeKind.Utc));
        }

        public virtual ValueTask<string?> GetIdAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.Id.ToString());
        }

        public virtual ValueTask<string?> GetPayloadAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.Payload);
        }

        public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (string.IsNullOrEmpty(token.Properties))
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            using (var document = JsonDocument.Parse(token.Properties))
            {
                var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    builder[property.Name] = property.Value.Clone();
                }

                return new ValueTask<ImmutableDictionary<string, JsonElement>>(builder.ToImmutable());
            }
        }

        public virtual ValueTask<DateTimeOffset?> GetRedemptionDateAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (token.RedemptionDate is null)
            {
                return new ValueTask<DateTimeOffset?>(result: null);
            }

            return new ValueTask<DateTimeOffset?>(DateTime.SpecifyKind(token.RedemptionDate.Value, DateTimeKind.Utc));
        }

        public virtual ValueTask<string?> GetReferenceIdAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.ReferenceId);
        }

        public virtual ValueTask<string?> GetStatusAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.Status);
        }

        public virtual ValueTask<string?> GetSubjectAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.Subject);
        }

        public virtual ValueTask<string?> GetTypeAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            return new ValueTask<string?>(token.Type);
        }

        public virtual ValueTask<OpenIddictTokenDto> InstantiateAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<OpenIddictTokenDto>(new OpenIddictTokenDto
            {
                Id = Snowflake.NextId(),
            });
        }

        public virtual async IAsyncEnumerable<OpenIddictTokenDto> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var tokens = await this._tokenRepository.ListAsync(count, offset, cancellationToken);
            foreach (var token in tokens)
            {
                var tokenDto = this._mapper.Map<OpenIddictToken, OpenIddictTokenDto>(token);
                yield return tokenDto;
            }
        }

        public virtual IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictTokenDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public virtual async ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
        {
            var uow = this._unitOfWorkManager.Begin();

            var date = threshold.UtcDateTime;
            await this._tokenRepository.PruneAsync(date, cancellationToken: cancellationToken);
            uow.Commit();

        }

        public virtual async ValueTask SetApplicationIdAsync(OpenIddictTokenDto token, string identifier, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (!string.IsNullOrEmpty(identifier))
            {
                var application = await this._applicationRepository.GetAsync(identifier.ParseTo<long>(), cancellationToken: cancellationToken);
                token.ApplicationId = application.Id;
            }
            else
            {
                token.ApplicationId = null;
            }
        }

        public virtual async ValueTask SetAuthorizationIdAsync(OpenIddictTokenDto token, string identifier, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (!string.IsNullOrEmpty(identifier))
            {
                var authorization = await this._authorizationRepository.GetAsync(identifier.ParseTo<long>(), cancellationToken: cancellationToken);
                token.AuthorizationId = authorization.Id;
            }
            else
            {
                token.AuthorizationId = null;
            }
        }

        public virtual ValueTask SetCreationDateAsync(OpenIddictTokenDto token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.CreationDate = date?.UtcDateTime;

            return default;
        }

        public virtual ValueTask SetExpirationDateAsync(OpenIddictTokenDto token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.ExpirationDate = date?.UtcDateTime;

            return default;
        }

        public virtual ValueTask SetPayloadAsync(OpenIddictTokenDto token, string payload, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.Payload = payload;

            return default;
        }

        public virtual ValueTask SetPropertiesAsync(OpenIddictTokenDto token, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            if (properties is null || properties.IsEmpty)
            {
                token.Properties = null;
                return default;
            }

            token.Properties = WriteStream(writer =>
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

        public virtual ValueTask SetRedemptionDateAsync(OpenIddictTokenDto token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.RedemptionDate = date?.UtcDateTime;

            return default;
        }

        public virtual ValueTask SetReferenceIdAsync(OpenIddictTokenDto token, string identifier, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.ReferenceId = identifier;

            return default;
        }

        public virtual ValueTask SetStatusAsync(OpenIddictTokenDto token, string status, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.Status = status;

            return default;
        }

        public virtual ValueTask SetSubjectAsync(OpenIddictTokenDto token, string subject, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.Subject = subject;

            return default;
        }

        public virtual ValueTask SetTypeAsync(OpenIddictTokenDto token, string type, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            token.Type = type;

            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OpenIddictExceptions.ConcurrencyException"></exception>
        public virtual async ValueTask UpdateAsync(OpenIddictTokenDto token, CancellationToken cancellationToken)
        {
            Check.NotNull(token, nameof(token));

            var entity = await this._tokenRepository.GetAsync(token.Id, cancellationToken: cancellationToken);

            try
            {
                var tokenEntity = this._mapper.Map<OpenIddictTokenDto, OpenIddictToken>(token);
                await this._tokenRepository.UpdateAsync(tokenEntity, cancellationToken: cancellationToken);
            }
            catch (DbException e)
            {
                throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
            }

            //token = (await this._tokenRepository.FindAsync(entity.Id, cancellationToken: cancellationToken)).ToModel();
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
