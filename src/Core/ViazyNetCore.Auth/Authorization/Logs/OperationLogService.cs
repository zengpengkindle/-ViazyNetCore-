using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Authorization.Modules.Repository;

namespace ViazyNetCore.Authorization.Modules
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
        public bool AddOperationLog(string operatorIp, string operateUserId, string operatorNickname, OperatorTypeEnum operatorType, string operationType, string objectId, string objectName, string descripation)
        {
            OperationLog log = new OperationLog(operatorIp, operateUserId, operatorNickname, operatorType, operationType, objectId, objectName, descripation);
            return AddOperationLog(log);
        }

        public bool AddOperationLog(OperationLog operationLog)
        {
            //截取长度，防止异常
            if (operationLog.Description.IsNotNull() && operationLog.Description.Length > 1000)
                operationLog.Description = operationLog.Description.Substring(0, 1000);

            if (CheckArgs(operationLog))
            {
                _repository.Insert(operationLog);
                return true;
            }
            else
                return false;
        }

        public PageData<OperationLog> GetOperationLog(int page, int limit, OperationLogSearchTypeEnum searchType = OperationLogSearchTypeEnum.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information)
        {
            var query = _repository.Select.Where(a => a.LogLevel == logRecordLevel).OrderByDescending(a => a.CreateTime);
            if (!string.IsNullOrWhiteSpace(keyword))
                switch (searchType)
                {
                    case OperationLogSearchTypeEnum.Default:
                        break;
                    case OperationLogSearchTypeEnum.Ip:
                        query = query.Where(a => a.OperatorIP == keyword);
                        break;
                    case OperationLogSearchTypeEnum.UserId:
                        query = query.Where(a => a.OperateUserId == keyword);
                        break;
                    case OperationLogSearchTypeEnum.OperatorNickname:
                        query = query.Where(a => a.Operator == keyword);
                        break;
                    case OperationLogSearchTypeEnum.OperationType:
                        query = query.Where(a => a.OperationType == keyword);
                        break;
                    case OperationLogSearchTypeEnum.ObjectId:
                        query = query.Where(a => a.ObjectId == keyword);
                        break;
                    case OperationLogSearchTypeEnum.ObjectName:
                        query = query.Where(a => a.ObjectName.Contains(keyword));
                        break;
                    case OperationLogSearchTypeEnum.Description:
                        query = query.Where(a => a.Description.Contains(keyword));
                        break;
                    default:
                        break;
                }
            return query.ToPage(page, limit);
        }

        public PageData<OperationLog> GetOperationLog(DateTime beginTime, DateTime endTime, int page, int limit, OperationLogSearchTypeEnum searchType = OperationLogSearchTypeEnum.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information)
        {
            var query = _repository.Select.Where(a => a.CreateTime >= beginTime && a.CreateTime <= endTime && a.LogLevel == logRecordLevel).OrderByDescending(a => a.CreateTime);
            if (!string.IsNullOrWhiteSpace(keyword))
                switch (searchType)
                {
                    case OperationLogSearchTypeEnum.Default:
                        break;
                    case OperationLogSearchTypeEnum.Ip:
                        query = query.Where(a => a.OperatorIP == keyword);
                        break;
                    case OperationLogSearchTypeEnum.UserId:
                        query = query.Where(a => a.OperateUserId == keyword);
                        break;
                    case OperationLogSearchTypeEnum.OperatorNickname:
                        query = query.Where(a => a.Operator == keyword);
                        break;
                    case OperationLogSearchTypeEnum.OperationType:
                        query = query.Where(a => a.OperationType == keyword);
                        break;
                    case OperationLogSearchTypeEnum.ObjectId:
                        query = query.Where(a => a.ObjectId == keyword);
                        break;
                    case OperationLogSearchTypeEnum.ObjectName:
                        query = query.Where(a => a.ObjectName.Contains(keyword));
                        break;
                    case OperationLogSearchTypeEnum.Description:
                        query = query.Where(a => a.Description.Contains(keyword));
                        break;
                    default:
                        break;
                }
            return query.ToPage(page, limit);
        }


        public PageData<OperationLog> GetMerchantOperationLog(DateTime beginTime, DateTime endTime, int page, int limit, string merchantId = "", OperationLogSearchTypeEnum searchType = OperationLogSearchTypeEnum.Default, string keyword = "", LogRecordLevel logRecordLevel = LogRecordLevel.Information)
        {
            var query = _repository.Select.Where(a => a.MerchantId == merchantId && a.CreateTime >= beginTime && a.CreateTime <= endTime && a.LogLevel == logRecordLevel).OrderByDescending(a => a.CreateTime);
            if (!string.IsNullOrWhiteSpace(keyword))
                switch (searchType)
                {
                    case OperationLogSearchTypeEnum.Default:
                        break;
                    case OperationLogSearchTypeEnum.Ip:
                        query = query.Where(a => a.OperatorIP == keyword);
                        break;
                    case OperationLogSearchTypeEnum.UserId:
                        query = query.Where(a => a.OperateUserId == keyword);
                        break;
                    case OperationLogSearchTypeEnum.OperatorNickname:
                        query = query.Where(a => a.Operator == keyword);
                        break;
                    case OperationLogSearchTypeEnum.OperationType:
                        query = query.Where(a => a.OperationType == keyword);
                        break;
                    case OperationLogSearchTypeEnum.ObjectId:
                        query = query.Where(a => a.ObjectId == keyword);
                        break;
                    case OperationLogSearchTypeEnum.ObjectName:
                        query = query.Where(a => a.ObjectName.Contains(keyword));
                        break;
                    case OperationLogSearchTypeEnum.Description:
                        query = query.Where(a => a.Description.Contains(keyword));
                        break;
                    default:
                        break;
                }
            return query.ToPage(page, limit);
        }
    }
}
