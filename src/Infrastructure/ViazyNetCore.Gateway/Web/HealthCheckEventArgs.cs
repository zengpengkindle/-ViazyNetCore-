using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Web
{
    public class HealthCheckEventArgs
    {
        public HealthCheckEventArgs(AddressModel address)
        {
            Address = address;
        }

        public HealthCheckEventArgs(AddressModel address, bool health)
        {
            Address = address;
            Health = health;
        }

        public AddressModel Address { get; private set; }

        public bool Health { get; private set; }
    }
}
