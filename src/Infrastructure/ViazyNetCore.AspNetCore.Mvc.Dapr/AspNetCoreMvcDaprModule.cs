
using ViazyNetCore.Dapr;

namespace ViazyNetCore.AspNetCore.Mvc.Dapr;

[DependsOn(
    typeof(AspNetCoreMvcModule),
    typeof(DaprModule)
)]
public class AspNetCoreMvcDaprModule : InjectionModule
{

}
