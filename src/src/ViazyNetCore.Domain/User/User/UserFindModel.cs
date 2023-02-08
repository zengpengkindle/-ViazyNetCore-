﻿using System.Text.Json.Serialization;

namespace ViazyNetCore.Domain
{
    /// <summary>
    /// 表示一个用户的单项查询模型。
    /// </summary>
    public class UserFindModel : UserModel
    {
        /// <summary>
        /// 获取或设置一个值，表示角色名称。
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示谷歌验证器密钥。
        /// </summary>
        [JsonIgnore]
        public string GoogleKey { get; set; }
    }
}