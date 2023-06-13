using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Zookeeper.Internal
{
    internal static class Invariant
    {
        [Conditional("DEBUG")]
        public static void Require(bool condition, string? message = null)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message ?? "invariant violated");
            }
        }
    }
}
