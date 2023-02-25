namespace ViazyNetCore.Authorization.Models
{
    public class OperationLog : EntityBase
    {
        public OperationLog()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorIp"></param>
        /// <param name="operateUserId">操作人用户编号</param>
        /// <param name="operatorNickname">操作人名（昵称）</param>
        /// <param name="operatorType">用户类型（BMS用户，Merchant用户，子商户）</param>
        /// <param name="operationType">操作类型(一般直接写事件，一个单词，如新增，删除字符型)</param>
        public OperationLog(string operatorIp, string operateUserId, string operatorNickname, OperatorType operatorType) : this()
        {
            this.LogLevel = LogRecordLevel.Information;
            this.OperatorIP = operatorIp;
            this.OperateUserId = operateUserId;
            this.Operator = operatorNickname;
            this.OperatorType = operatorType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorIp"></param>
        /// <param name="operateUserId">操作人用户编号</param>
        /// <param name="operatorNickname">操作人名（昵称）</param>
        /// <param name="operatorType">用户类型（BMS用户，Merchant用户，子商户）</param>
        /// <param name="operationType">操作类型(一般直接写事件，一个单词，如新增，删除，字符型)</param>
        /// <param name="objectId">操作对象ID</param>
        /// <param name="objectName">操作对象名称</param>
        public OperationLog(string operatorIp, string operateUserId, string operatorNickname, OperatorType operatorType, string operationType, string objectId, string objectName) : this(operatorIp, operateUserId, operatorNickname, operatorType)
        {
            this.LogLevel = LogRecordLevel.Information;
            this.OperationType = operationType;
            this.ObjectId = objectId;
            this.ObjectName = objectName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorIp"></param>
        /// <param name="operateUserId">操作人用户编号</param>
        /// <param name="operatorNickname">操作人名（昵称）</param>
        /// <param name="operatorType">用户类型（BMS用户，Merchant用户，子商户）</param>
        /// <param name="operationType">操作类型(一般直接写事件，一个单词，如新增，删除，字符型)</param>
        /// <param name="objectId">操作对象ID</param>
        /// <param name="objectName">操作对象名称</param>
        /// <param name="descripation">描述</param>
        public OperationLog(string operatorIp, string operateUserId, string operatorNickname, OperatorType operatorType, string operationType, string objectId, string objectName, string description) : this(operatorIp, operateUserId, operatorNickname, operatorType, operationType, objectId, objectName)
        {
            this.LogLevel = LogRecordLevel.Information;
            this.Description = description;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorIp"></param>
        /// <param name="operateUserId">操作人用户编号</param>
        /// <param name="operatorNickname">操作人名（昵称）</param>
        /// <param name="operatorType">用户类型（BMS用户，Merchant用户，子商户）</param>
        /// <param name="operationType">操作类型(一般直接写事件，一个单词，如新增，删除，字符型)</param>
        /// <param name="objectId">操作对象ID</param>
        /// <param name="objectName">操作对象名称</param>
        /// <param name="descripation">描述</param>
        /// <param name="createTime">日志产生时间</param>
        public OperationLog(string operatorIp, string operateUserId, string operatorNickname, OperatorType operatorType, string operationType, string objectId, string objectName, string description, DateTime createTime) : this(operatorIp, operateUserId, operatorNickname, operatorType, operationType, objectId, objectName, description)
        {
            this.LogLevel = LogRecordLevel.Information;
            this.CreateTime = createTime;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorType">用户类型（BMS用户，Merchant用户，子商户）</param>
        /// <param name="operationType">操作类型(一般直接写事件，一个单词，如新增，删除，字符型)</param>
        /// <param name="objectId">操作对象ID</param>
        /// <param name="objectName">操作对象名称</param>
        /// <param name="descripation">描述</param>
        /// <param name="createTime">日志产生时间</param>
        public OperationLog(OperatorType operatorType, string operationType, string objectId, string objectName, string description, DateTime createTime) : this("", "", "", operatorType, operationType, objectId, objectName, description)
        {
            this.LogLevel = LogRecordLevel.Information;
            this.CreateTime = createTime;
        }

        /// <summary>
        /// 快捷创建日志对象,不传取默认值
        /// </summary>
        /// <param name="objectId">操作对象ID,必传</param>
        /// <param name="objectName">操作对象名</param>
        /// <param name="merchantId">商家端/渠道端 id</param>
        /// <param name="operatorType">用户类型</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="logLevel">日志等级</param>
        /// <param name="description">操作详细描述</param>
        /// <returns></returns>
        public static OperationLog CreateLog(string objectId = null, string objectName = null, string merchantId = null, OperatorType operatorType = OperatorType.Bms, string operationType = null, LogRecordLevel logLevel = LogRecordLevel.Information, string description = null)
        {
            return new OperationLog()
            {
                MerchantId = merchantId,
                OperatorType = operatorType,
                OperationType = operationType,

                ObjectId = objectId,
                ObjectName = objectName,

                LogLevel = logLevel,

                Description = description,
            };
        }

        /// <summary>
        ///    操作IP
        /// </summary>
        public string? OperatorIP { get; set; }
        /// <summary>
        /// 操作人用户编号
        /// </summary>
        public string OperateUserId { get; set; }
        /// <summary>
        /// 操作人名（昵称）
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 用户类型（BMS用户，Merchant用户，子商户）
        /// </summary>
        public OperatorType OperatorType { get; set; }
        /// <summary>
        /// 操作类型(一般直接写事件，一个单词，如新增，删除，字符型)
        /// </summary>
        public string OperationType { get; set; }
        /// <summary>
        /// 操作对象ID
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// 操作对象名称
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        ///  描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogRecordLevel LogLevel { get; set; }
    }

    public enum OperatorType
    {
        Bms = 1
    }

    /// <summary>
    /// 表示日志级别。
    /// </summary>
    public enum LogRecordLevel
    {
        /// <summary>
        /// 表示跟踪级别。
        /// </summary>
        Trace = 0,
        /// <summary>
        /// 表示调试级别。
        /// </summary>
        Debug = 1,
        /// <summary>
        /// 表示信息级别。
        /// </summary>
        Information = 2,
        /// <summary>
        /// 表示警告级别。
        /// </summary>
        Warning = 3,
        /// <summary>
        /// 表示错误级别。
        /// </summary>
        Error = 4,
        /// <summary>
        /// 表示严重错误级别。
        /// </summary>
        Critical = 5,
        /// <summary>
        /// 表示无级别。
        /// </summary>
        None = 6
    }
}
