﻿using System;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个用户的查询模型。
    /// </summary>
    public class UserDto : UserWithGoogleKeyDto
    {
        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}