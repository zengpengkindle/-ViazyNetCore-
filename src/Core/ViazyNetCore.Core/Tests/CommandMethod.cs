using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Tests
{
    public class CommandMethod : CommandMethodBase
    {
        private readonly Action<CommandMethod> _action;

        internal CommandMethod(CommandMethodFactory factory, int code, string title, Action<CommandMethod> action)
        {
            this.Factory = factory;
            this.Code = code;
            this.Title = title;
            this._action = action;
        }

        /// <summary>
        /// 获取测试工厂。
        /// </summary>
        public CommandMethodFactory Factory { get; }

        /// <summary>
        /// 获取代码。
        /// </summary>
        public int Code { get; }
        /// <summary>
        /// 获取标题。
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 执行参数方法。
        /// </summary>
        public void Run() => this._action(this);

        /// <summary>
        /// 获取指定标题的参数。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>参数。</returns>
        public string Get(string title, string defaultValue = null)
        {
            return this.Get<string>(title, defaultValue);
        }

        /// <summary>
        /// 获取指定标题的参数，并将值转换为 <typeparamref name="T"/> 类型。
        /// </summary>
        /// <typeparam name="T">值的数据类型。</typeparam>
        /// <param name="title">标题。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>类型为  <typeparamref name="T"/> 的参数。</returns>
        public T Get<T>(string title, T defaultValue = default)
        {
            try
            {
                var type = typeof(T);
                Console.Write("Please input");
                using (this.Factory.Color(ConsoleColor.Magenta))
                {
                    Console.Write(" ");
                    Console.Write(title);
                    Console.Write(" ");
                }
                using (this.Factory.Color(ConsoleColor.Yellow))
                {
                    Console.Write("<");
                    Console.Write(type.Name);
                    Console.Write(">");
                }

                Console.Write("：");
                var isFalseDefaultValue = defaultValue is bool b && !b;
                var isNonDefaultValue = !EqualityComparer<T>.Default.Equals(defaultValue, default);
                if (isNonDefaultValue || isFalseDefaultValue)
                {
                    Console.Write("Default");
                    using (this.Factory.Color(ConsoleColor.Yellow))
                    {
                        Console.Write("[");
                        Console.Write(defaultValue);
                        Console.Write("]");
                    }
                }

                var r = Console.ReadLine().Trim();
                if (r.Length == 0)
                {
                    if (isNonDefaultValue || isFalseDefaultValue) return defaultValue;
                    return this.Get(title, defaultValue);
                }

                if (r == CommandMethodFactory.EXIT_COMMAND) throw CommandMethodFactory.ExitError;

                if (r is T v) return v;

                return r.ParseTo<T>();
            }
            catch (Exception ex)
            {
                if (ex == CommandMethodFactory.ExitError) throw;
#pragma warning disable IDE0067 // 丢失范围之前释放对象
                using (this.Factory.Color(ConsoleColor.Red))
#pragma warning restore IDE0067 // 丢失范围之前释放对象
                {
                    Console.WriteLine(ex);
                }

                return this.Get<T>(title);
            }
        }
    }
}
