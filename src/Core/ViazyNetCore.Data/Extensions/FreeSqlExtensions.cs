﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FreeSql.Aop;
using ViazyNetCore.Annotations;
using ViazyNetCore.Data.FreeSql;

namespace FreeSql
{
    public static class FreeSqlExtensions
    {
        public static FreeSqlCloud<string> AsClound(this IFreeSql freeSql)
        {
            if (freeSql is FreeSqlCloud<string> freeSqlChoud)
            {
                return freeSqlChoud;
            }
            else
            {
                throw new ArgumentException($"当前FreeSql对象不是{nameof(FreeSqlCloud<string>)}");
            }
        }

        public static IFreeSql UseDb(this IFreeSql freeSql, string dbKey)
        {
            if (freeSql is FreeSqlCloud<string> freeSqlChoud)
            {
                return freeSqlChoud.Use(dbKey);
            }
            else
            {
                throw new ArgumentException($"当前FreeSql对象不是{nameof(FreeSqlCloud<string>)}");
            }
        }

        public static void RegisterDb(FreeSqlCloud<string> freeSqlCloud, DbConfig dbConfig)
        {
            freeSqlCloud.Register(dbConfig.Key, () =>
            {
                //创建数据库
                if (dbConfig.CreateDb)
                {
                    CreateDatabaseAsync(dbConfig).Wait();
                }

                var providerType = dbConfig.ProviderType.IsNotNull() ? Type.GetType(dbConfig.ProviderType!) : null;
                var freeSqlBuilder = new FreeSqlBuilder()
                        .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString, providerType)
                        .UseAutoSyncStructure(false)
                        .UseLazyLoading(false)
                        .UseNoneCommandParameter(true);

                if (dbConfig.SlaveList?.Length > 0)
                {
                    var slaveList = dbConfig.SlaveList.Select(a => a.ConnectionString).ToArray();
                    var slaveWeightList = dbConfig.SlaveList.Select(a => a.Weight).ToArray();
                    freeSqlBuilder.UseSlave(slaveList).UseSlaveWeight(slaveWeightList);
                }


                #region 监听所有命令

                if (dbConfig.MonitorCommand)
                {
                    freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                    {
                        //Console.WriteLine($"{cmd.CommandText}\n{traceLog}{Environment.NewLine}");
                        Console.WriteLine($"{cmd.CommandText}{Environment.NewLine}");
                    });
                }

                #endregion 监听所有命令

                var fsql = freeSqlBuilder.Build();

                //软删除过滤器
                fsql.GlobalFilter.ApplyOnly<IDelete>(FilterNames.Delete, a => a.IsDeleted == false);

                #region 初始化数据库

                //同步结构
                if (dbConfig.SyncStructure)
                {
                    SyncStructure(fsql, dbConfig: dbConfig);
                }

                #region 审计数据

                //计算服务器时间
                var serverTime = fsql.Ado.QuerySingle(() => DateTime.UtcNow);
                var timeOffset = DateTime.UtcNow.Subtract(serverTime);
                fsql.Aop.AuditValue += AopAuditValue;

                #endregion 审计数据


                #endregion 初始化数据库

                #region 监听Curd操作

                if (dbConfig.Curd)
                {
                    fsql.Aop.CurdBefore += (s, e) =>
                    {
                        Console.WriteLine($"{e.Sql}{Environment.NewLine}");
                    };
                    fsql.Aop.CurdAfter += Aop_CurdAfter;
                }

                #endregion 监听Curd操作

                return fsql;
            });

            //执行注册数据库
            var fsql = freeSqlCloud.Use(dbConfig.Key);
        }
        #region Aop
        /// <summary>
        /// Aop审计(新增或修改)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AopAuditValue(object? sender, AuditValueEventArgs e)
        {
            switch (e.AuditValueType)
            {
                case AuditValueType.Update:
                    if (e.AuditValueType == AuditValueType.InsertOrUpdate && e.Property.Name.ToLower() == "updatetime"
                        && e.Property.PropertyType == typeof(long)
                        && e.Value == default)
                        e.Value = DateTime.Now.ToUniversalTime();
                    break;
                case AuditValueType.Insert:
                    if (e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null && e.Property.PropertyType == typeof(long))
                    {
                        if (e.Value is long v)
                        {
                            if (v == 0)
                            {
                                e.Value = Snowflake.NextId();
                            }
                        }
                    }
                    else if (e.AuditValueType == AuditValueType.InsertOrUpdate && e.Property.Name.ToLower() == "createtime" && e.Property.PropertyType == typeof(long))
                        e.Value = DateTime.Now.ToUniversalTime();
                    //e.Value = SnowflakeHelper.NextId();
                    break;
                case AuditValueType.InsertOrUpdate:
                    if (e.AuditValueType == AuditValueType.InsertOrUpdate && e.Property.Name.ToLower() == "updatetime"
                        && e.Property.PropertyType == typeof(long)
                        && e.Value == default)
                        e.Value = DateTime.Now.ToUniversalTime();


                    break;
                default:
                    break;
            }
        }

        private static void Aop_CurdAfter(object? sender, CurdAfterEventArgs e)
        {
#if DEBUG
            Debug.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}; ElapsedMilliseconds:{e.ElapsedMilliseconds}ms,\r\n [SQL:] {e.Sql}");
#endif
            if (e.ElapsedMilliseconds > 1000)
            {
                //记录日志
                //Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}; FullName:{e.EntityType.FullName} ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}");
            }
        }
        #endregion

        /// <summary>
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string? msg = null, DbConfig? dbConfig = null)
        {
            //打印结构同步脚本
            if (dbConfig.SyncStructureSql)
            {
                db.Aop.SyncStructureAfter += SyncStructureAfter;
            }

            // 同步结构
            var dbType = dbConfig.Type.ToString();
            Console.WriteLine($"{Environment.NewLine} {(msg.IsNotNull() ? msg : $"sync {dbType} structure")} started");

            if (dbConfig.Type == DataType.Oracle)
            {
                db.CodeFirst.IsSyncStructureToUpper = true;
            }

            //获得指定程序集表实体
            //var entityTypes = GetEntityTypes(dbConfig.AssemblyNames);
            //db.CodeFirst.SyncStructure(entityTypes);

            if (dbConfig.SyncStructureSql)
            {
                db.Aop.SyncStructureAfter -= SyncStructureAfter;
            }

            Console.WriteLine($" {(msg.IsNotNull() ? msg : $"sync {dbType} structure")} succeed");
        }

        private static void SyncStructureAfter(object? s, SyncStructureAfterEventArgs e)
        {
            if (e.Sql.IsNotNull())
            {
                Console.WriteLine(" sync structure sql:\n" + e.Sql);
            }
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        public async static Task CreateDatabaseAsync(DbConfig dbConfig)
        {
            if (!dbConfig.CreateDb || dbConfig.Type == DataType.Sqlite)
            {
                return;
            }

            var db = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.CreateDbConnectionString)
                    .Build();

            try
            {
                await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
                Console.WriteLine(" create database succeed");
            }
            catch (Exception e)
            {
                Console.WriteLine($" create database failed.\n {e.Message}");
            }
        }
    }
}