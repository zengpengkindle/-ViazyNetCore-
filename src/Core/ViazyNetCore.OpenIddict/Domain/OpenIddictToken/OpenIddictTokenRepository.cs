using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using OpenIddict.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class OpenIddictTokenRepository : DefaultRepository<OpenIddictToken, long>, IOpenIddictTokenRepository
    {
        public OpenIddictTokenRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task<List<OpenIddictToken>> FindAsync(string subject, long client, CancellationToken cancellationToken)
        {
            return this.Select.Where(x => x.Subject == subject && x.ApplicationId == client).ToListAsync(cancellationToken);
        }

        public Task<List<OpenIddictToken>> FindAsync(string subject, long client, string status, CancellationToken cancellationToken)
        {
            return this.Select.Where(x => x.Subject == subject && x.ApplicationId == client && x.Status == status).ToListAsync(cancellationToken);
        }

        public Task<List<OpenIddictToken>> FindAsync(string subject, long client, string status, string type, CancellationToken cancellationToken)
        {
            return this.Select.Where(x => x.Subject == subject && x.ApplicationId == client && x.Status == status && x.Type == type).ToListAsync(cancellationToken);
        }

        public Task<List<OpenIddictToken>> FindByApplicationIdAsync(long applicationId, CancellationToken cancellationToken)
        {
            return this.Select.Where(x => x.ApplicationId == applicationId).ToListAsync(cancellationToken);
        }

        public Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(long authorizationId, CancellationToken cancellationToken)
        {
            return this.Select.Where(x => x.AuthorizationId == authorizationId).ToListAsync(cancellationToken);
        }

        public Task<OpenIddictToken> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken)
        {
            return this.Select.Where(x => x.ReferenceId == identifier).FirstAsync(cancellationToken);
        }

        public Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
        {
            return this.Select.Where(x => x.Subject == subject).ToListAsync(cancellationToken);

        }

        public Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
        {
            return this.Select
            .OrderBy(x => x.Id)
            .Skip(offset ?? 0)
            .Take(count ?? 0)
            .ToListAsync(cancellationToken);
        }

        public async Task PruneAsync(DateTime date, CancellationToken cancellationToken)
        {
           await this.Select.From<OpenIddictAuthorization>()
                .LeftJoin((t, auth) => t.AuthorizationId == auth.Id)
                .Where((t, auth) => t.CreationDate < date)
                .Where((token, auth) => (token.Status != OpenIddictConstants.Statuses.Inactive && token.Status != OpenIddictConstants.Statuses.Valid) ||
                      (auth != null && auth.Status != OpenIddictConstants.Statuses.Valid) ||
                      token.ExpirationDate < DateTime.UtcNow)
                .ToDelete()
                .ExecuteAffrowsAsync(cancellationToken);
        }
    }
}
