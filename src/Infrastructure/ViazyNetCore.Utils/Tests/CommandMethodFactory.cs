using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Tests
{
    /// <summary>
    /// 表示一个测试工厂。
    /// </summary>
    public class CommandMethodFactory : CommandMethodBase
    {
        private readonly Dictionary<int, CommandMethod> _methods = new Dictionary<int, CommandMethod>();
        class ExitException : Exception { }

        internal static Exception ExitError = new ExitException();
        internal const string EXIT_COMMAND = "exit";

        /// <summary>
        /// 初始化一个 <see cref="CommandMethodFactory"/> 类的新实例。
        /// </summary>
        public CommandMethodFactory() { }

        /// <summary>
        /// 获取或设置一个值，表示下一次执行是否为退出命令。
        /// </summary>
        public bool IsExitCommand { get; set; }

        /// <summary>
        /// 添加测试方法。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <returns>测试工厂。</returns>
        public static CommandMethodFactory Create(string title)
        {
            var factory = new CommandMethodFactory();
            using (factory.Color(ConsoleColor.Green)) Console.WriteLine("The {0} test", title);
            return factory;
        }

        /// <summary>
        /// 添加测试方法。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="action">参数方法。</param>
        /// <returns>测试工厂。</returns>
        public CommandMethodFactory Add(string title, Action<CommandMethod> action)
        {
            var code = this._methods.Count + 1;
            this._methods.Add(code, new CommandMethod(this, code, title, action));
            return this;
        }

        /// <summary>
        /// 添加异步测试方法。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="action">参数方法。</param>
        /// <returns>测试工厂。</returns>
        public CommandMethodFactory AddAsync(string title, Func<CommandMethod, Task> action)
        {
            return this.Add(title, command => action(command).GetAwaiter().GetResult());
        }

        /// <summary>
        /// 执行指定代码。
        /// </summary>
        public void Run()
        {
            foreach (var item in this._methods)
            {
                using (this.Color(ConsoleColor.Magenta, ConsoleColor.White)) Console.Write(" " + item.Key + " ");
                Console.Write("：");
                Console.WriteLine(item.Value.Title);
            }

            using (this.Color(ConsoleColor.Green)) Console.Write("> ");

            var code = Console.ReadLine().ToLower().Trim();

            if (code == EXIT_COMMAND) return;
            if (int.TryParse(code, out var result) && this._methods.TryGetValue(result, out var command))
            {
                try
                {
                    command.Run();
                    if (this.IsExitCommand) return;
                }
                catch (ExitException)
                {
                    goto END;
                }
                catch (Exception ex)
                {
                    this.WriteError(ex.ToString());
                }
            }
            else
            {
                this.WriteError($"Invalid code {code}, please try again.");
            }

END:
            Console.WriteLine();
            this.Run();
        }
    }
}
