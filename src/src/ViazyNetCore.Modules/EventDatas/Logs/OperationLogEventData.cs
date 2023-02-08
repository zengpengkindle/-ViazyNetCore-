using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    public class OperationLogEventData : CommonEventData<OperationLog>
    {
        public OperationLogEventData()
        {

        }

        public OperationLogEventData(OperationLog operationLog)
        {
            this.Data = operationLog;
            this.EventTime = DateTime.Now;
        }
    }
}
