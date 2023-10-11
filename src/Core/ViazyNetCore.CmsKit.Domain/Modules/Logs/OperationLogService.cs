using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.CmsKit.Modules.Repository;

namespace ViazyNetCore.CmsKit.Modules
{
    [Injection]
    public class OperationLogService : IOperationLogService
    {
        private readonly IOperationLogRepository _repository;

        public OperationLogService(IOperationLogRepository repository)
        {
            this._repository = repository;
        }
        private bool CheckArgs(OperationLog operationLog)
        {
            if (operationLog == null)
                return false;
            if (string.IsNullOrWhiteSpace(operationLog.OperateUserId))
                throw new Exception("OperateUserId can not be null!");
            return true;
        }
        public Task<bool> AddOperationLog(string operatorIp, string operateUserId, string operatorNickname, OperatorType operatorType, string operationType, string objectId, string objectName, string descripation)
        {
            OperationLog log = new OperationLog(operatorIp, operateUserId, operatorNickname, operatorType, operationType, objectId, objectName, descripation);
            return this.AddOperationLog(log);
        }

        public async Task<bool> AddOperationLog(OperationLog operationLog)
        {
            //截取长度，防止异常
            if (operationLog.Description.IsNotNull() && operationLog.Description.Length > 1000)
                operationLog.Description = operationLog.Description.Substring(0, 1000);

            if (CheckArgs(operationLog))
            {
                await _repository.InsertAsync(operationLog);
                return true;
            }
            else
                return false;
        }

        public PageData<OperationLog> GetOperationLog(int page, int limit, OperationLogSearchType searchType = OperationLogSearchType.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information)
        {
            var query = _repository.Select.Where(a => a.LogLevel == logRecordLevel).OrderByDescending(a => a.CreateTime);
            if (!string.IsNullOrWhiteSpace(keyword))
                switch (searchType)
                {
                    case OperationLogSearchType.Default:
                        break;
                    case OperationLogSearchType.Ip:
                        query = query.Where(a => a.OperatorIP == keyword);
                        break;
                    case OperationLogSearchType.UserId:
                        query = query.Where(a => a.OperateUserId == keyword);
                        break;
                    case OperationLogSearchType.OperatorNickname:
                        query = query.Where(a => a.Operator == keyword);
                        break;
                    case OperationLogSearchType.OperationType:
                        query = query.Where(a => a.OperationType == keyword);
                        break;
                    case OperationLogSearchType.ObjectId:
                        query = query.Where(a => a.ObjectId == keyword);
                        break;
                    case OperationLogSearchType.ObjectName:
                        query = query.Where(a => a.ObjectName.Contains(keyword));
                        break;
                    case OperationLogSearchType.Description:
                        query = query.Where(a => a.Description.Contains(keyword));
                        break;
                    default:
                        break;
                }
            return query.ToPage(page, limit);
        }

        public PageData<OperationLog> GetOperationLog(DateTime beginTime, DateTime endTime, int page, int limit, OperationLogSearchType searchType = OperationLogSearchType.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information)
        {
            var query = _repository.Select.Where(a => a.CreateTime >= beginTime && a.CreateTime <= endTime && a.LogLevel == logRecordLevel);
            if (!string.IsNullOrWhiteSpace(keyword))
                switch (searchType)
                {
                    case OperationLogSearchType.Default:
                        break;
                    case OperationLogSearchType.Ip:
                        query = query.Where(a => a.OperatorIP == keyword);
                        break;
                    case OperationLogSearchType.UserId:
                        query = query.Where(a => a.OperateUserId == keyword);
                        break;
                    case OperationLogSearchType.OperatorNickname:
                        query = query.Where(a => a.Operator == keyword);
                        break;
                    case OperationLogSearchType.OperationType:
                        query = query.Where(a => a.OperationType == keyword);
                        break;
                    case OperationLogSearchType.ObjectId:
                        query = query.Where(a => a.ObjectId == keyword);
                        break;
                    case OperationLogSearchType.ObjectName:
                        query = query.Where(a => a.ObjectName.Contains(keyword));
                        break;
                    case OperationLogSearchType.Description:
                        query = query.Where(a => a.Description.Contains(keyword));
                        break;
                    default:
                        break;
                }
            return query.OrderByDescending(a => a.CreateTime).ToPage(page, limit);
        }


        public PageData<OperationLog> GetMerchantOperationLog(DateTime beginTime, DateTime endTime, int page, int limit, string merchantId = "", OperationLogSearchType searchType = OperationLogSearchType.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information)
        {
            var query = _repository.Select.Where(a => a.MerchantId == merchantId && a.CreateTime >= beginTime && a.CreateTime <= endTime && a.LogLevel == logRecordLevel);
            if (!string.IsNullOrWhiteSpace(keyword))
                switch (searchType)
                {
                    case OperationLogSearchType.Default:
                        break;
                    case OperationLogSearchType.Ip:
                        query = query.Where(a => a.OperatorIP == keyword);
                        break;
                    case OperationLogSearchType.UserId:
                        query = query.Where(a => a.OperateUserId == keyword);
                        break;
                    case OperationLogSearchType.OperatorNickname:
                        query = query.Where(a => a.Operator == keyword);
                        break;
                    case OperationLogSearchType.OperationType:
                        query = query.Where(a => a.OperationType == keyword);
                        break;
                    case OperationLogSearchType.ObjectId:
                        query = query.Where(a => a.ObjectId == keyword);
                        break;
                    case OperationLogSearchType.ObjectName:
                        query = query.Where(a => a.ObjectName.Contains(keyword));
                        break;
                    case OperationLogSearchType.Description:
                        query = query.Where(a => a.Description.Contains(keyword));
                        break;
                    default:
                        break;
                }
            return query.OrderByDescending(a => a.CreateTime).ToPage(page, limit);
        }
    }
}
