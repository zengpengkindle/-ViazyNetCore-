using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization
{
    public class OperationLogEventData : CommonEventData<OperationLog>
    {
        public OperationLogEventData(OperationLog eventdata) : base(eventdata)
        {
        }
    }
}
