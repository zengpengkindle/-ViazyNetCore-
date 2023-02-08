using System.Collections.Generic;

namespace ViazyNetCore.Domain
{
    public class RouterModel
    {
        public List<PageGroupModel> Groups { get; set; }

        public List<PageSimpleModel> Pages { get; set; }
    }
}