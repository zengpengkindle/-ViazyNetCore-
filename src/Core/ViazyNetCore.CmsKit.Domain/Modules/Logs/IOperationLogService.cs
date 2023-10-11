using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.CmsKit.Models;

namespace ViazyNetCore.CmsKit.Modules
{
    [Injection]
    public interface IOperationLogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorIp">操作人ip</param>
        /// <param name="operateUserId">操作人用户编号</param>
        /// <param name="operatorNickname">操作人名（昵称）</param>
        /// <param name="operatorType">用户类型（BMS用户，Merchant用户，子商户）</param>
        /// <param name="operationType">操作类型(一般直接写事件，一个单词，如新增，删除，字符型)</param>
        /// <param name="objectId">操作对象ID</param>
        /// <param name="objectName">操作对象名称</param>
        /// <param name="descripation">描述</param>
        /// <returns></returns>
        Task<bool> AddOperationLog(string operatorIp, string operateUserId, string operatorNickname, OperatorType operatorType, string operationType, string objectId, string objectName, string descripation);
        Task<bool> AddOperationLog(OperationLog operationLog);
        PageData<OperationLog> GetOperationLog(int page, int limit, OperationLogSearchType searchType = OperationLogSearchType.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information);
        PageData<OperationLog> GetOperationLog(DateTime beginTime, DateTime endTime, int page, int limit, OperationLogSearchType searchType = OperationLogSearchType.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information);

        /// <summary>
        /// 获取商户端操作日志
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="merchantId"></param>
        /// <param name="searchType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        PageData<OperationLog> GetMerchantOperationLog(DateTime beginTime, DateTime endTime, int page, int limit, string merchantId = "", OperationLogSearchType searchType = OperationLogSearchType.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information);
    }
}
