using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OSS
{
    public interface IMinioOSSService : IObjectStorageService
    {
        Task<bool> RemoveIncompleteUploadAsync(string bucketName, string objectName);

        Task<List<ItemUploadInfo>> ListIncompleteUploads(string bucketName);

        Task<PolicyInfo> GetPolicyAsync(string bucketName);

        Task<bool> SetPolicyAsync(string bucketName, List<StatementItem> statements);

        Task<bool> RemovePolicyAsync(string bucketName);

        Task<bool> PolicyExistsAsync(string bucketName, StatementItem statement);
    }
}
