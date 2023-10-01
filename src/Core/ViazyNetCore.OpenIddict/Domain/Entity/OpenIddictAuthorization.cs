using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.Domain.Entity
{
    public class OpenIddictAuthorization:EntityUpdate<long>
    {

        /// <summary>
        /// Gets or sets the application associated with the current authorization.
        /// </summary>
        public virtual long? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the additional properties serialized as a JSON object,
        /// or <c>null</c> if no bag was associated with the current authorization.
        /// </summary>
        public virtual string Properties { get; set; }

        /// <summary>
        /// Gets or sets the scopes associated with the current
        /// authorization, serialized as a JSON array.
        /// </summary>
        public virtual string Scopes { get; set; }

        /// <summary>
        /// Gets or sets the status of the current authorization.
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// Gets or sets the subject associated with the current authorization.
        /// </summary>
        public virtual string Subject { get; set; }

        /// <summary>
        /// Gets or sets the type of the current authorization.
        /// </summary>
        public virtual string Type { get; set; }
    }
}
