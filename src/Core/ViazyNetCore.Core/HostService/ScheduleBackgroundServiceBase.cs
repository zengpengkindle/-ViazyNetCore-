using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public abstract class ScheduleBackgroundServiceBase : BackgroundService
    {
        private readonly ILogger _logger;

        public ScheduleBackgroundServiceBase(ILogger logger)
        {
            this._logger = logger;
        }

        public abstract TimeSpan Timeout { get; }

        protected abstract Task<TimeSpan> OnExecuteAsync(ScheduleServiceState state);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }

    public class ScheduleServiceState
    {
        public CancellationToken CancellationToken;
    }
}
