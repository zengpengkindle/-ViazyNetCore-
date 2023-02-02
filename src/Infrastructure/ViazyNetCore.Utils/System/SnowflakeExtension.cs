using System.Net;
using System.Net.Sockets;

namespace System
{
    /// <summary>
    /// 雪花扩展方法
    /// </summary>
    public static class SnowflakeExtension
    {
        /// <summary>
        /// 雪花静态类
        /// </summary>
        static class SnowflakeStatic
        {
            /// <summary>
            /// 表示 UTC 的开始纪元。
            /// </summary>
            public static readonly DateTime UTC_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            /// <summary>
            /// 表示标准字符，包括 0-9 和 A-Z 的字符。
            /// </summary>
            public static readonly char[] CHARACTERS ={ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'I', 'J', 'K', 'L', 'M', 'N',
            'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z' };

            /// <summary>
            /// 表示时间戳开始纪元。现为2020.09.01
            /// </summary>
            public const long TW_EPOCH = 1598889600000L;

            /// <summary>
            /// 表示工作机器编号的位数。
            /// </summary>
            public const int WORKER_ID_BITS = 5;
            /// <summary>
            /// 表示数据中心编号的位数。
            /// </summary>
            public const int DATACENTER_ID_BITS = 5;
            /// <summary>
            /// 表示序列的字节数。
            /// </summary>
            public const int SEQUENCE_BITS = 6; //12

            /// <summary>
            /// 表示最大的工作机器编号。
            /// </summary>
            public const long MAX_WORKER_ID = -1L ^ (-1L << WORKER_ID_BITS);
            /// <summary>
            /// 表示最大的数据中心编号。
            /// </summary>
            public const long MAX_DATACENTER_ID = -1L ^ (-1L << DATACENTER_ID_BITS);

            /// <summary>
            /// 表示工作机器编号的位移量。
            /// </summary>
            public const int WORKER_ID_SHIFT = SEQUENCE_BITS;
            /// <summary>
            /// 表示数据中心编号的位移量。
            /// </summary>
            public const int DATACENTER_ID_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS;
            /// <summary>
            /// 表示时间戳的位移量。
            /// </summary>
            public const int TIMESTAMP_LEFT_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS + DATACENTER_ID_BITS;

            /// <summary>
            /// 表示序列掩码。
            /// </summary>
            public const long SEQUENCE_MASK = -1L ^ (-1L << SEQUENCE_BITS);
        }

        /// <summary>
        /// 雪花
        /// </summary>
        public class Snowflake
        {
            private readonly object _syncRoot = new object();
            private long _sequence = 0L;
            private long _lastTimestamp = -1L;

            /// <summary>
            /// 获取工作机器的编号。
            /// </summary>
            public long WorkerId { get; }
            /// <summary>
            /// 获取数据中心的编号。
            /// </summary>
            public long DatacenterId { get; }

            /// <summary>
            /// 初始化一个 <see cref="Snowflake"/> 类的新实例。
            /// </summary>
            /// <param name="workerId">工作机器的编号。</param>
            /// <param name="datacenterId">数据中心的编号。</param>
            /// <param name="sequence">起始序列。</param>
            public Snowflake(long workerId, long datacenterId, long sequence = 0)
            {
                //- 工作机器的编号超过最大数 { SnowflakeStatic.MAX_WORKER_ID } 或者小于 0。
                if(workerId > SnowflakeStatic.MAX_WORKER_ID || workerId < 0)
                    throw new ArgumentException($"The worker id exceeds the maximum number {SnowflakeStatic.MAX_WORKER_ID} or less than 0 number.", nameof(workerId));
                if(datacenterId > SnowflakeStatic.MAX_DATACENTER_ID || datacenterId < 0)
                    throw new ArgumentException($"The datacenter id exceeds the maximum number  {SnowflakeStatic.MAX_DATACENTER_ID} or less than 0 number.", nameof(datacenterId));
                if(sequence < 0) throw new ArgumentOutOfRangeException(nameof(sequence));

                this.WorkerId = workerId;
                this.DatacenterId = datacenterId;
                this._sequence = sequence;
            }

            /// <summary>
            /// 获取下一个唯一编号。
            /// </summary>
            /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
            public long NextId()
            {
                lock(this._syncRoot)
                {
                    var timestamp = this.GenerateTimestamp();

                    if(timestamp < this._lastTimestamp)
                    {
                        System.Threading.Thread.SpinWait((int)Math.Abs((this._lastTimestamp - timestamp) % 20));
                        return this.NextId();
                    }

                    if(this._lastTimestamp == timestamp)
                    {
                        this._sequence = (this._sequence + 1) & SnowflakeStatic.SEQUENCE_MASK;
                        if(this._sequence == 0)
                        {
                            timestamp = this.GetNextTimestamp(this._lastTimestamp);
                        }
                    }
                    else
                    {
                        this._sequence = 0L;
                    }
                    this._lastTimestamp = timestamp;

                    long newId = ((timestamp - SnowflakeStatic.TW_EPOCH) << SnowflakeStatic.TIMESTAMP_LEFT_SHIFT) | (this.DatacenterId << SnowflakeStatic.DATACENTER_ID_SHIFT) | (this.WorkerId << SnowflakeStatic.WORKER_ID_SHIFT) | this._sequence;
                    return newId;
                }
            }

            /// <summary>
            /// 获取下一个唯一字符串。
            /// </summary>
            /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
            public string NextIdString() => ToBase(this.NextId());
            /// <summary>
            /// 获取下一个时间戳
            /// </summary>
            /// <param name="lastTimestamp"></param>
            /// <returns></returns>
            private long GetNextTimestamp(long lastTimestamp)
            {
                long timestamp;
                while((timestamp = this.GenerateTimestamp()) <= lastTimestamp) { }
                return timestamp;
            }

            /// <summary>
            /// 生成时间戳
            /// </summary>
            /// <returns></returns>
            private long GenerateTimestamp() => (long)(DateTime.UtcNow - SnowflakeStatic.UTC_EPOCH).TotalMilliseconds;

            private static IEnumerable<char> GetBaseCharacters(long value)
            {
                var radix = SnowflakeStatic.CHARACTERS.Length;
                if(value >= radix)
                {
                    foreach(var item in GetBaseCharacters(value / radix))
                    {
                        yield return item;
                    }
                    value %= radix;
                }
                yield return SnowflakeStatic.CHARACTERS[(int)value];
            }

            private static long GetCharacterIndex(char c)
            {
                if(c > 'Z') c = (char)(c - 32);
                if(c < ':') return c - 48;
                return c - 65 + 10;
            }

            /// <summary>
            /// 将指定的 <see cref="long"/>  整数转换为其用 Base36 数字编码的等效字符串表示形式。
            /// </summary>
            /// <param name="value">要转换的 <see cref="long"/>  整数。</param>
            /// <returns>等效字符串表示形式。</returns>
            public static string ToBase(long value)
            {
                if(value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                return new string(GetBaseCharacters(value).ToArray());
            }

            /// <summary>
            /// 将指定的字符串（它将二进制数据编码为 Base36 数字）转换为等效的 <see cref="long"/> 整数。
            /// </summary>
            /// <param name="value">要转换的字符串。</param>
            /// <returns>等效的 <see cref="long"/> 整数。</returns>
            public static long FromBase(string value)
            {
                if(string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

                var radix = SnowflakeStatic.CHARACTERS.Length;
                long result = GetCharacterIndex(value[^1]);

                for(int i = 1; i < value.Length; i++)
                {
                    result += (long)Math.Pow(radix, i) * GetCharacterIndex(value[^(i + 1)]);
                }

                return result;
            }

            /// <summary>
            /// 获取本地 IP 地址。
            /// </summary>
            /// <returns>一个 <see cref="IPAddress"/> 实例。</returns>
            public static IPAddress GetLocalIPAddress()
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach(var ip in host.AddressList)
                {
                    if(ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip;
                    }
                }
                return IPAddress.Parse("127.0.0.1");
            }

            /// <summary>
            /// 提供 <see cref="IPAddress"/> 实例，创建一个分布式编号生成器。
            /// </summary>
            /// <param name="address">一个 <see cref="IPAddress"/> 实例。</param>
            /// <param name="sequence">起始序列。</param>
            /// <returns>一个 <see cref="Snowflake"/> 实例。</returns>
            public static Snowflake Create(IPAddress address, long sequence = 0)
            {
                try
                {
                    if(address is null) throw new ArgumentNullException(nameof(address));

                    var addressBytes = address.GetAddressBytes();
                    var value = addressBytes.Sum(n => n);
                    if(value > 1024) throw new NotSupportedException();
                    var workerId = value % 32;
                    var datacenterId = (value - workerId) / 32;
                    return new Snowflake(workerId, datacenterId, sequence);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("aaa=" + ex.Message);
                    return null;
                }
            }
        }
    }
}