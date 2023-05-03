using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.OpenIddict.Domain.Entity;

namespace ViazyNetCore.OpenIddict.Domain
{
    public interface IOpenIddictTokenRepository : IBaseRepository<OpenIddictToken, long>
    {
        Task<List<OpenIddictToken>> FindAsync(string subject, long client, CancellationToken cancellationToken);
        Task<List<OpenIddictToken>> FindAsync(string subject, long client, string status, CancellationToken cancellationToken);
        Task<List<OpenIddictToken>> FindAsync(string subject, long client, string status, string type, CancellationToken cancellationToken);
        Task<List<OpenIddictToken>> FindByApplicationIdAsync(long applicationId, CancellationToken cancellationToken);
        Task<List<OpenIddictToken>> FindByAuthorizationIdAsync(long authorizationId, CancellationToken cancellationToken);
        Task<OpenIddictToken> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken);
        Task<List<OpenIddictToken>> FindBySubjectAsync(string subject, CancellationToken cancellationToken);
        Task<List<OpenIddictToken>> ListAsync(int? count, int? offset, CancellationToken cancellationToken);
        Task PruneAsync(DateTime date, CancellationToken cancellationToken);
    }
}
