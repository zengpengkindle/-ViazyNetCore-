using AutoMapper;

namespace ViazyNetCore.Ddd
{
    public class ApplicationService
    {
        public IMapper ObjectMapper { get; set; }

        public ApplicationService(IMapper objectMapper)
        {
            this.ObjectMapper = objectMapper;
        }

        protected virtual Task CheckPolicyAsync(string? policyName)
        {
            // TODO 暂未添加 目前根据 Controller 权限控制
            if (string.IsNullOrEmpty(policyName))
            {
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }

    }
}
