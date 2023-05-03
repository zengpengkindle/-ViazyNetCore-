using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Common;
using System.Globalization;
using System.Linq;
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

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictScopeStore : IOpenIddictScopeStore<OpenIddictScopeDto>
    {
        private readonly IOpenIddictScopeRepository _scopeRepository;
        private readonly IOptions<OpenIddictOptions> _options;
        private readonly IMapper _mapper;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public OpenIddictScopeStore(IOpenIddictScopeRepository scopeRepository
            , IOptions<OpenIddictOptions> options
            , UnitOfWorkManagerCloud unitOfWorkManagerCloud
            , IMapper mapper)
        {
            this._scopeRepository = scopeRepository;
            this._options = options;
            this._mapper = mapper;

        }
        public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            return await this._scopeRepository.Select.CountAsync(cancellationToken);
        }

        public virtual ValueTask<long> CountAsync<TResult>(Func<IQueryable<OpenIddictScopeDto>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public virtual async ValueTask CreateAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            var entity = this._mapper.Map<OpenIddictScopeDto, OpenIddictScope>(scope);
            await this._scopeRepository.InsertAsync(entity, cancellationToken: cancellationToken);

            //scope = (await this._scopeRepository.FindAsync(scope.Id, cancellationToken: cancellationToken)).ToModel();
        }

        public virtual async ValueTask DeleteAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            try
            {
                await this._scopeRepository.DeleteAsync(scope.Id, cancellationToken: cancellationToken);
            }
            catch (DbException e)
            {
                throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
            }
        }

        public virtual async ValueTask<OpenIddictScopeDto> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(identifier, nameof(identifier));

            var entity = await this._scopeRepository.GetAsync(identifier.ParseTo<long>(), cancellationToken);

            return this._mapper.Map<OpenIddictScope, OpenIddictScopeDto>(entity);
        }

        public virtual async ValueTask<OpenIddictScopeDto> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(name, nameof(name));

            var entity = await this._scopeRepository.Where(p => p.Name == name).FirstAsync(cancellationToken);

            return this._mapper.Map<OpenIddictScope, OpenIddictScopeDto>(entity);
        }

        public virtual async IAsyncEnumerable<OpenIddictScopeDto> FindByNamesAsync(ImmutableArray<string> names, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNull(names, nameof(names));

            foreach (var name in names)
            {
                Check.NotNullOrEmpty(name, nameof(name));
            }

            var scopes = await this._scopeRepository.Where(p => names.Contains(p.Name)).ToListAsync(cancellationToken);
            foreach (var scope in scopes)
            {
                var scopeDto = this._mapper.Map<OpenIddictScope, OpenIddictScopeDto>(scope);
                yield return scopeDto;
            }
        }

        public virtual async IAsyncEnumerable<OpenIddictScopeDto> FindByResourceAsync(string resource, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Check.NotNullOrEmpty(resource, nameof(resource));

            var scopes = await this._scopeRepository.Where(p=>p.Resources.Contains(resource)).ToListAsync( cancellationToken);
            foreach (var scope in scopes)
            {
                var scopeDto = this._mapper.Map<OpenIddictScope, OpenIddictScopeDto>(scope);
                var resources = await GetResourcesAsync(scopeDto, cancellationToken);
                if (resources.Contains(resource, StringComparer.Ordinal))
                {
                    yield return scopeDto;
                }
            }
        }

        public virtual ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<OpenIddictScopeDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public virtual ValueTask<string> GetDescriptionAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.Description);
        }

        public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDescriptionsAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (string.IsNullOrEmpty(scope.Descriptions))
            {
                return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
            }

            using (var document = JsonDocument.Parse(scope.Descriptions))
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

        public virtual ValueTask<string> GetDisplayNameAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.DisplayName);
        }

        public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (string.IsNullOrEmpty(scope.DisplayNames))
            {
                return new ValueTask<ImmutableDictionary<CultureInfo, string>>(ImmutableDictionary.Create<CultureInfo, string>());
            }

            using (var document = JsonDocument.Parse(scope.DisplayNames))
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

        public virtual ValueTask<string> GetIdAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.Id.ToString());
        }

        public virtual ValueTask<string> GetNameAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            return new ValueTask<string>(scope.Name);
        }

        public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (string.IsNullOrEmpty(scope.Properties))
            {
                return new ValueTask<ImmutableDictionary<string, JsonElement>>(ImmutableDictionary.Create<string, JsonElement>());
            }

            using (var document = JsonDocument.Parse(scope.Properties))
            {
                var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    builder[property.Name] = property.Value.Clone();
                }

                return new ValueTask<ImmutableDictionary<string, JsonElement>>(builder.ToImmutable());
            }
        }

        public virtual ValueTask<ImmutableArray<string>> GetResourcesAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (string.IsNullOrEmpty(scope.Resources))
            {
                return new ValueTask<ImmutableArray<string>>(ImmutableArray.Create<string>());
            }

            using (var document = JsonDocument.Parse(scope.Resources))
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

        public virtual ValueTask<OpenIddictScopeDto> InstantiateAsync(CancellationToken cancellationToken)
        {
            return new ValueTask<OpenIddictScopeDto>(new OpenIddictScopeDto
            {
                Id = Snowflake.NextId()
            });
        }

        public virtual async IAsyncEnumerable<OpenIddictScopeDto> ListAsync(int? count, int? offset, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var scopes = await this._scopeRepository.Select.Skip(offset??0).Take(count??0).ToListAsync( cancellationToken);
            foreach (var scope in scopes)
            {
                var scopeDto = this._mapper.Map<OpenIddictScope, OpenIddictScopeDto>(scope);
                yield return scopeDto;
            }
        }

        public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<OpenIddictScopeDto>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public virtual ValueTask SetDescriptionAsync(OpenIddictScopeDto scope, string description, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.Description = description;

            return default;
        }

        public virtual ValueTask SetDescriptionsAsync(OpenIddictScopeDto scope, ImmutableDictionary<CultureInfo, string> descriptions, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (descriptions is null || descriptions.IsEmpty)
            {
                scope.Descriptions = null;
                return default;
            }

            scope.Descriptions = WriteStream(writer =>
            {
                writer.WriteStartObject();
                foreach (var description in descriptions)
                {
                    writer.WritePropertyName(description.Key.Name);
                    writer.WriteStringValue(description.Value);
                }
                writer.WriteEndObject();
            });

            return default;
        }

        public virtual ValueTask SetDisplayNameAsync(OpenIddictScopeDto scope, string name, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.DisplayName = name;

            return default;
        }

        public virtual ValueTask SetDisplayNamesAsync(OpenIddictScopeDto scope, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (names is null || names.IsEmpty)
            {
                scope.DisplayNames = null;
                return default;
            }

            scope.DisplayNames = WriteStream(writer =>
            {
                writer.WriteStartObject();
                foreach (var name in names)
                {
                    writer.WritePropertyName(name.Key.Name);
                    writer.WriteStringValue(name.Value);
                }
                writer.WriteEndObject();
            });

            return default;
        }

        public virtual ValueTask SetNameAsync(OpenIddictScopeDto scope, string name, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            scope.Name = name;

            return default;
        }

        public virtual ValueTask SetPropertiesAsync(OpenIddictScopeDto scope, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (properties is null || properties.IsEmpty)
            {
                scope.Properties = null;
                return default;
            }

            scope.Properties = WriteStream(writer =>
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

        public virtual ValueTask SetResourcesAsync(OpenIddictScopeDto scope, ImmutableArray<string> resources, CancellationToken cancellationToken)
        {
            Check.NotNull(scope, nameof(scope));

            if (resources.IsDefaultOrEmpty)
            {
                scope.Resources = null;
                return default;
            }

            scope.Resources = WriteStream(writer =>
            {
                writer.WriteStartArray();
                foreach (var resource in resources)
                {
                    writer.WriteStringValue(resource);
                }
                writer.WriteEndArray();
            });

            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OpenIddictExceptions.ConcurrencyException"></exception>
        public virtual async ValueTask UpdateAsync(OpenIddictScopeDto scope, CancellationToken cancellationToken)
        {

            var entity = await this._scopeRepository.GetAsync(scope.Id, cancellationToken: cancellationToken);

            try
            {
                var scopeEntity = this._mapper.Map<OpenIddictScopeDto, OpenIddictScope>(scope);
                await this._scopeRepository.UpdateAsync(scopeEntity, cancellationToken: cancellationToken);
            }
            catch (DbException e)
            {
                throw new OpenIddictExceptions.ConcurrencyException(e.Message, e.InnerException);
            }

            //scope = (await this._scopeRepository.FindAsync(entity.Id, cancellationToken: cancellationToken)).ToModel();
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
