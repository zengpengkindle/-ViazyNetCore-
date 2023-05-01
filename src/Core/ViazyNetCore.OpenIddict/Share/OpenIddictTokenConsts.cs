using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.Share
{
    public class OpenIddictTokenConsts
    {
        public static int ReferenceIdMaxLength { get; set; } = 100;

        public static int StatusMaxLength { get; set; } = 50;

        public static int SubjectMaxLength { get; set; } = 400;

        public static int TypeMaxLength { get; set; } = 50;
    }
}
