using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Logging;
using ViazyNetCore.IdentityService4.FreeSql.Entities;

namespace ViazyNetCore.IdentityService4.FreeSql.Stores
{
    public class DeviceFlowStore : IDeviceFlowStore
    {
        private readonly ILogger<DeviceFlowStore> _logger;

        public IPersistedGrantDbContext Context { get; }
        public IPersistentGrantSerializer Serializer { get; }

        public DeviceFlowStore(
           IPersistedGrantDbContext context,
           IPersistentGrantSerializer serializer,
           ILogger<DeviceFlowStore> logger)
        {
            Context = context;
            Serializer = serializer;
            this._logger = logger;
        }

        public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = await Context.DeviceFlowCodes
                .Where(x => x.DeviceCode == deviceCode)
                .NoTracking()
                .ToOneAsync();
            var model = ToModel(deviceFlowCodes?.Data);

            this._logger.LogDebug("{deviceCode} found in database: {deviceCodeFound}", deviceCode, model != null);

            return model;
        }

        public async Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            var deviceFlowCodes = await Context.DeviceFlowCodes
                .Where(x => x.UserCode == userCode)
                .NoTracking()
                .FirstAsync();
            var model = ToModel(deviceFlowCodes?.Data);

            this._logger.LogDebug("{userCode} found in database: {userCodeFound}", userCode, model != null);

            return model;
        }

        public async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            var deviceFlowCodes = await Context.DeviceFlowCodes
                .Where(x => x.DeviceCode == deviceCode)
                .ToOneAsync();
            if(deviceFlowCodes != null)
            {
                this._logger.LogDebug("removing {deviceCode} device code from database", deviceCode);

                Context.DeviceFlowCodes.Remove(deviceFlowCodes);

                try
                {
                    await Context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    this._logger.LogInformation("exception removing {deviceCode} device code from database: {error}", deviceCode, ex.Message);
                }
            }
            else
            {
                this._logger.LogDebug("no {deviceCode} device code found in database", deviceCode);
            }
        }

        public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
        {
            Context.DeviceFlowCodes.Add(ToEntity(data, deviceCode, userCode));
            await Context.SaveChangesAsync();
        }

        public async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            var existing = await Context.DeviceFlowCodes
                .Where(x => x.UserCode == userCode)
                .ToOneAsync();
            if(existing == null)
            {
                this._logger.LogError("{userCode} not found in database", userCode);
                throw new InvalidOperationException("Could not update device code");
            }

            var entity = ToEntity(data, existing.DeviceCode, userCode);
            this._logger.LogDebug("{userCode} found in database", userCode);

            existing.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject).Value;
            existing.Data = entity.Data;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                this._logger.LogWarning("exception updating {userCode} user code in database: {error}", userCode, ex.Message);
            }
        }

        protected DeviceFlowCodes ToEntity(DeviceCode model, string deviceCode, string userCode)
        {
            if(model == null || deviceCode == null || userCode == null)
                return null;

            return new DeviceFlowCodes
            {
                DeviceCode = deviceCode,
                UserCode = userCode,
                ClientId = model.ClientId,
                SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                CreationTime = model.CreationTime,
                Expiration = model.CreationTime.AddSeconds(model.Lifetime),
                Data = Serializer.Serialize(model)
            };
        }

        protected DeviceCode ToModel(string entity)
        {
            if(entity == null)
                return null;
            return Serializer.Deserialize<DeviceCode>(entity);
        }
    }
}
