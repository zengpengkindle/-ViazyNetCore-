using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.Aop;
using FreeSql;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection;
using ViazyNetCore.Annotations;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FreeMySqlSetup
    {
        public static void AddFreeMySql(this IServiceCollection services, IConfiguration configuration)
        {
#pragma warning disable IDE0039 // 使用本地函数
            Func<IServiceProvider, IFreeSql> fsqlFunc = r =>
            {
                var fsql = new FreeSqlBuilder().UseConnectionString(DataType.MySql, configuration.GetConnectionString("master"))
#if DEBUG
              //监听SQL语句
              .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))
              //.UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
#endif
              .Build();

                fsql.Aop.AuditValue += AopAuditValue;

                fsql.Aop.ConfigEntityProperty += (s, e) =>
                {
                    if (e.Property.PropertyType.IsEnum)
                        e.ModifyResult.MapType = typeof(int);
                };

                fsql.Aop.CurdAfter += (s, e) =>
                {
#if DEBUG
                    Debug.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}; ElapsedMilliseconds:{e.ElapsedMilliseconds}ms,\r\n [SQL:] {e.Sql}");
#endif
                    if (e.ElapsedMilliseconds > 500)
                    {
                        //记录日志
                        // LogHelper.WriteSqlWarning($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}; FullName:{e.EntityType.FullName} ElapsedMilliseconds:{e.ElapsedMilliseconds}ms,\r\n [SQL:] {e.Sql}", e.ElapsedMilliseconds).GetAwaiter();
                    }
                };
                fsql.Aop.CommandAfter += Aop_CommandAfter;

                return fsql;
            };
#pragma warning restore IDE0039 // 使用本地函数

            // FreeSql 必须使用单例注入
            services.AddSingleton(fsqlFunc);

            services.AddScoped(typeof(IBaseRepository<>), typeof(GuidRepository<>));
            services.AddScoped(typeof(BaseRepository<>), typeof(GuidRepository<>));

            services.AddScoped(typeof(IBaseRepository<,>), typeof(DefaultRepository<,>));
            services.AddScoped(typeof(BaseRepository<,>), typeof(DefaultRepository<,>));
        }

        public static IApplicationBuilder UseFreeSql(this IApplicationBuilder app)
        {
            var fsql = app.ApplicationServices.GetService<IFreeSql>();
            if (fsql == null)
                throw new ArgumentNullException(nameof(IFreeSql));
            fsql.Aop.CurdAfter += Aop_CurdAfter; ;
            return app;
        }

        #region EventHanlders
        private static void Aop_CommandAfter(object? sender, CommandAfterEventArgs e)
        {
            if (e.Exception != null)
            {
                //做一些日志记录的操作。以下为示例。
                //LogHelper.WriteSqlWarning($"Message:{e.Exception.Message}\r\nStackTrace:{e.Exception.StackTrace}\r\nCommandText:{e.Command.CommandText}", e.ElapsedMilliseconds).GetAwaiter();
            }
        }

        private static void Aop_CurdAfter(object? sender, CurdAfterEventArgs e)
        {
#if DEBUG
            Debug.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}; FullName:{e.EntityType.FullName} ElapsedMilliseconds:{e.ElapsedMilliseconds}ms,\r\n [SQL:] {e.Sql}");
#endif
            if (e.ElapsedMilliseconds > 1000)
            {
                //LogHelper.WriteSqlWarning(e.Sql, e.ElapsedMilliseconds).GetAwaiter();
                //Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}; FullName:{e.EntityType.FullName} ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}");
                //记录日志
            }
        }

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

        #endregion
    }
}
