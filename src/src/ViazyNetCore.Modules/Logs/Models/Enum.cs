using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    public enum OperationLogSearchTypeEnum
    {
        /// <summary>
        /// 默认的无意义的
        /// </summary>
        Default = 0,
        /// <summary>
        /// 操作人ip
        /// </summary>
        Ip = 1,
        /// <summary>
        /// 操作人操作人用户编号
        /// </summary>
        UserId = 2,
        /// <summary>
        /// 操作人昵称
        /// </summary>
        OperatorNickname = 3,
        /// <summary>
        /// 操作类型(一般直接写事件，一个单词，如新增，删除，上分，下分，字符型)
        /// </summary>
        OperationType = 4,
        /// <summary>
        /// 操作对象ID
        /// </summary>
        ObjectId = 5,
        /// <summary>
        /// 操作对象名称
        /// </summary>
        ObjectName = 6,
        /// <summary>
        /// 描述
        /// </summary>
        Description = 7
    }
}
