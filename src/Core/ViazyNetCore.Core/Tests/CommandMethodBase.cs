using System;
using System.Collections.Generic;
using System.Linq;
using System.Tests;
using System.Text;
using System.Threading.Tasks;

namespace System.Tests
{
    /// <summary>
    /// 表示一个测试方法的基类。
    /// </summary>
    public abstract class CommandMethodBase
    {
        class ConsoleColorWapper : IDisposable
        {
            private readonly ConsoleColor _oldColor;
            private readonly ConsoleColor? _oldBackgrouColor;
            private readonly IDisposable _disposable;

            public ConsoleColorWapper(ConsoleColor color, ConsoleColor? backgroundColor)
            {
                this._disposable = GA.Lock(typeof(ConsoleColorWapper).FullName);
                this._oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                if (backgroundColor.HasValue)
                {
                    this._oldBackgrouColor = Console.BackgroundColor;
                    Console.BackgroundColor = backgroundColor.Value;
                }
            }
            public void Dispose()
            {
                Console.ForegroundColor = this._oldColor;
                if (this._oldBackgrouColor.HasValue) Console.BackgroundColor = this._oldBackgrouColor.Value;
                this._disposable.Dispose();
            }
        }


        public class ConsoleSpinner
        {
            int _counter;

            public void Turn()
            {
                this._counter++;
                switch (this._counter % 4)
                {
                    case 0: Console.Write("/"); this._counter = 0; break;
                    case 1: Console.Write("-"); break;
                    case 2:
                        Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                Thread.Sleep(100);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        public void WriteTurn(TimeSpan timeSpan)
        {
            var spin = new ConsoleSpinner();
            var start = TimeSpan.Zero;
            while (timeSpan > start)
            {
                spin.Turn();
                Thread.Sleep(100);
                start.Add(new TimeSpan(100));
            }
        }

        public void ReplaceWriteLine(string value)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(value);
        }

        /// <summary>
        /// 使用控制台颜色。
        /// </summary>
        /// <param name="color">颜色。</param>
        /// <param name="backgroundColor">背景色。</param>
        /// <returns>可释放对象。</returns>
        public IDisposable Color(ConsoleColor color, ConsoleColor? backgroundColor = null)
        {
            return new ConsoleColorWapper(color, backgroundColor);
        }

        private void WriteTime()
        {
            Console.Write("[{0}] >> ", DateTime.Now.ToLongTimeString());
        }

        /// <summary>
        /// 输出错误信息。
        /// </summary>
        /// <param name="value">字符串。</param>
        public void WriteError(string value)
        {
            using (this.Color(ConsoleColor.White, ConsoleColor.Red))
            {
                this.WriteTime();
                Console.WriteLine(value);
            }
        }

        /// <summary>
        /// 输出正常消息。
        /// </summary>
        /// <param name="value">字符串。</param>
        public void WriteInfo(string value)
        {
            using (this.Color(ConsoleColor.Blue))
            {
                this.WriteTime();
                Console.WriteLine(value);
            }
        }

        /// <summary>
        /// 输出内容。
        /// </summary>
        /// <param name="value">字符串。</param>
        public void Write(string value)
        {
            using (this.Color(Console.ForegroundColor))
            {
                this.WriteTime();
                Console.WriteLine(value);
            }
        }

        /// <summary>
        /// 输出表格。
        /// </summary>
        /// <typeparam name="T">对象的数据类型。</typeparam>
        /// <param name="values">行数据。</param>
        public void WriteTable<T>(IEnumerable<T> values)
        {
            var tableText = ConsoleTable.From(values).ToString();

            using var reader = new System.IO.StringReader(tableText);
            string line;
            IDisposable disposable = null;
            var index = 0;
            do
            {
                line = reader.ReadLine();
                if (line == null) break;
                if (index == 0) disposable = this.Color(ConsoleColor.White, ConsoleColor.DarkMagenta);
                Console.WriteLine(line);
                index++;
                if (index == 3)
                {
                    disposable.TryDispose();
                    disposable = null;
                }
            } while (true);

            disposable.TryDispose();
        }

        /// <summary>
        /// 输出断行。
        /// </summary>
        /// <param name="br">是否使用横线断行。</param>
        public void NewLine(bool br = true)
        {
            if (br)
            {
                Console.WriteLine();
                Console.WriteLine(new string('-', Console.BufferWidth));
                Console.WriteLine();
            }
            else Console.WriteLine();
        }
    }
}
