using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.Payment.Events
{
    public interface IPayNotifyHandler
    {
        string TradeType { get; }

        Task PaySuccessNotify(string outId);
    }
}
