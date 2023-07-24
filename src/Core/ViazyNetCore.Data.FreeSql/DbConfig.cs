﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.Data.FreeSql
{
    public class DbConfig
    {
        /// <summary>
        /// 数据库注册键
        /// </summary>
        public string Key { get; set; } = "master";

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType Type { get; set; } = DataType.MySql;
        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// 指定程序集
        /// </summary>
        public string? ProviderType { get; set; }

        /// <summary>
        /// 生成数据
        /// </summary>
        public bool GenerateData { get; set; } = false;

        /// <summary>
        /// 同步结构
        /// </summary>
        public bool SyncStructure { get; set; } = false;

        /// <summary>
        /// 同步结构脚本
        /// </summary>
        public bool SyncStructureSql { get; set; } = false;

        /// <summary>
        /// 建库
        /// </summary>
        public bool CreateDb { get; set; } = false;

        /// <summary>
        /// 建库连接字符串
        /// </summary>
        public string CreateDbConnectionString { get; set; }

        /// <summary>
        /// 建库脚本
        /// </summary>
        public string CreateDbSql { get; set; }

        /// <summary>
        /// 监听所有操作
        /// </summary>
        public bool MonitorCommand { get; set; } = false;

        /// <summary>
        /// 监听Curd操作
        /// </summary>
        public bool Curd { get; set; } = false;

        /// <summary>
        /// 多数据库
        /// </summary>
        public DbConfig[] Dbs { get; set; }

        /// <summary>
        /// 读写分离从库列表
        /// </summary>
        public SlaveDb[] SlaveList { get; set; }

        /// <summary>
        /// 启用枚举映射为Int类型
        /// </summary>
        public bool UseEnumInt { get; set; }

        /// <summary>
        /// 是否自动同步结构
        /// </summary>
        public bool AutoSyncStructure { get; set; }
        /// <summary>
        /// 是否启用租户
        /// </summary>
        public bool Tenant { get; set; }
    }

    /// <summary>
    /// 读写分离从库
    /// </summary>
    public class SlaveDb
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public int Weight { get; set; } = 1;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
