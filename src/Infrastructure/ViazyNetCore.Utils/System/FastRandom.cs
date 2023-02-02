using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// 表示一个快速的随机数实现。
    /// </summary>
    public sealed class FastRandom
    {
        // The +1 ensures NextDouble doesn't generate 1.0
        private const double REAL_UNIT_INT = 1.0 / ((double)int.MaxValue + 1.0);
        private const uint Y = 842502087, Z = 3579807591, W = 273326509;

        private static readonly Local<FastRandom> LocalInstance = new();

        /// <summary>
        /// 获取随机数的唯一实例。
        /// </summary>
        public static FastRandom Instance => LocalInstance.Value;

        private uint _x, _y, _z, _w;

        #region Constructors

        /// <summary>
        /// 使用与时间相关的默认种子值，初始化一个 <see cref="FastRandom"/> 类的新实例。
        /// </summary>
        public FastRandom() : this((int)DateTime.Now.Ticks)
        {
        }

        /// <summary>
        /// 使用指定的种子值初始化一个 <see cref="FastRandom"/> 类的新实例。
        /// </summary>
        /// <param name="seed">用来计算伪随机数序列起始值的数字。如果指定的是负数，则使用其绝对值。</param>
        public FastRandom(int seed)
        {
            this.Reinitialise(seed);
        }

        #endregion

        #region Public Methods [Reinitialisation]

        /// <summary>
        /// 重新加载种子值。
        /// </summary>
        /// <param name="seed">用来计算伪随机数序列起始值的数字。如果指定的是负数，则使用其绝对值。</param>
        public void Reinitialise(int seed)
        {
            // The only stipulation stated for the xorshift RNG is that at least one of
            // the seeds x,y,z,w is non-zero. We fulfill that requirement by only allowing
            // resetting of the x seed
            this._x = (uint)seed;
            this._y = Y;
            this._z = Z;
            this._w = W;
        }

        #endregion

        #region Public Methods [System.Random functionally equivalent methods]

        /// <summary>
        /// 返回非负随机数。 
        /// </summary>
        /// <returns>大于等于零且小于 <see cref="int.MaxValue"/> 的 32 位带符号整数。</returns>
        public int Next()
        {
            uint t = this._x ^ (this._x << 11);
            this._x = this._y; this._y = this._z; this._z = this._w;
            this._w = this._w ^ (this._w >> 19) ^ t ^ (t >> 8);

            // Handle the special case where the value int.MaxValue is generated. This is outside of 
            // the range of permitted values, so we therefore call Next() to try again.
            uint rtn = this._w & 0x7FFFFFFE;
            return (int)rtn;
        }

        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数。
        /// </summary>
        /// <param name="maxValue">要生成的随机数的上限（随机数不能取该上限值）。<paramref name="maxValue"/> 必须大于或等于零。</param>
        /// <returns>大于等于零且小于 <paramref name="maxValue"/> 的 32 位带符号整数，即：返回值的范围通常包括零但不包括 <paramref name="maxValue"/>。不过，如果 <paramref name="maxValue"/> 等于零，则返回 <paramref name="maxValue"/>。</returns>
        public int Next(int maxValue)
        {
            if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue), maxValue, $"The {nameof(maxValue)} less than 0 value.");

            uint t = this._x ^ (this._x << 11);
            this._x = this._y; this._y = this._z; this._z = this._w;

            // The explicit int cast before the first multiplication gives better performance.
            // See comments in NextDouble.
            return (int)(REAL_UNIT_INT * (int)(0x7FFFFFFF & (this._w = this._w ^ (this._w >> 19) ^ t ^ (t >> 8))) * maxValue);
        }

        /// <summary>
        /// 返回一个指定范围内的随机数。
        /// </summary>
        /// <param name="minValue">返回的随机数的下界（随机数可取该下界值）。</param>
        /// <param name="maxValue">返回的随机数的上界（随机数不能取该上界值）。<paramref name="maxValue"/> 必须大于或等于 <paramref name="minValue"/>。</param>
        /// <returns>大于等于 <paramref name="minValue"/> 且小于 <paramref name="maxValue"/> 的 32 位带符号整数，即：返回的值范围包括 <paramref name="minValue"/> 但不包括 <paramref name="maxValue"/>。如果 <paramref name="minValue"/> 等于 <paramref name="maxValue"/>，则返回 <paramref name="minValue"/>。</returns>
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException(nameof(maxValue), maxValue, $"The {nameof(minValue)} value greater than ${nameof(maxValue)} value.");

            uint t = this._x ^ (this._x << 11);
            this._x = this._y; this._y = this._z; this._z = this._w;

            int range = maxValue - minValue;

            return minValue + (int)(REAL_UNIT_INT * (double)(int)(0x7FFFFFFF & (this._w = this._w ^ (this._w >> 19) ^ t ^ (t >> 8))) * range);
        }

        internal readonly static char[] UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        internal readonly static char[] LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        internal readonly static char[] NumericCharacters = "0123456789".ToCharArray();
        internal readonly static char[] SpecialCharacters = ",.;:?!/@#$%^&()=+*-_{}[]<>|~".ToCharArray();

        /// <summary>
        /// 返回一个固定长度随机字符串。
        /// </summary>
        /// <param name="length">随机字符串的长度。</param>
        /// <param name="type">随机字符串的类型。</param>
        /// <returns>固定长度的随机字符串。</returns>
        public string NextString(int length, CharacterType type = CharacterType.Default)
        {
            if (length < 1) throw new ArgumentOutOfRangeException(nameof(length));
            var chars = new char[length];
            var charsArray = new List<char>(40);
            if ((type & CharacterType.UpperCase) == CharacterType.UpperCase) charsArray.AddRange(UpperCaseCharacters);
            if ((type & CharacterType.LowerCase) == CharacterType.LowerCase) charsArray.AddRange(LowerCaseCharacters);
            if ((type & CharacterType.Numeric) == CharacterType.Numeric) charsArray.AddRange(NumericCharacters);
            if ((type & CharacterType.Special) == CharacterType.Special) charsArray.AddRange(SpecialCharacters);

            for (int i = 0; i < length; i++)
            {
                chars[i] = charsArray[this.Next(0, charsArray.Count)];
            }
            return new string(chars);
        }

        /// <summary>
        /// 返回一个指定范围内长度的随机字符串。
        /// </summary>
        /// <param name="minLength">返回的随机字符串长度的下界（可取该下界值）。</param>
        /// <param name="maxLength">返回的随机字符串长度的上界（不能取该上界值）。<paramref name="maxLength"/> 必须大于或等于 <paramref name="minLength"/>。</param>
        /// <param name="type">随机字符串的类型。</param>
        /// <returns>字符串长度大于等于 <paramref name="minLength"/> 且小于 <paramref name="maxLength"/> 的字符串。</returns>
        public string NextString(int minLength, int maxLength, CharacterType type = CharacterType.Default)
        {
            if (minLength < 1) throw new ArgumentOutOfRangeException(nameof(minLength));
            if (maxLength < 1) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (minLength > maxLength) throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, $"The {nameof(minLength)} value greater than ${nameof(maxLength)} value.");
            return this.NextString(this.Next(minLength, maxLength), type);
        }

        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数。
        /// </summary>
        /// <returns>大于等于 0.0 并且小于 1.0 的双精度浮点数。</returns>
        public double NextDouble()
        {
            uint t = this._x ^ (this._x << 11);
            this._x = this._y; this._y = this._z; this._z = this._w;

            // Here we can gain a 2x speed improvement by generating a value that can be cast to 
            // an int instead of the more easily available uint. If we then explicitly cast to an 
            // int the compiler will then cast the int to a double to perform the multiplication, 
            // this final cast is a lot faster than casting from a uint to a double. The extra cast
            // to an int is very fast (the allocated bits remain the same) and so the overall effect 
            // of the extra cast is a significant performance improvement.
            //
            // Also note that the loss of one bit of precision is equivalent to what occurs within 
            // System.Random.
            return REAL_UNIT_INT * (int)(0x7FFFFFFF & (this._w = this._w ^ (this._w >> 19) ^ t ^ (t >> 8)));
        }

        /// <summary>
        /// 用随机数填充指定字节数组的元素。
        /// </summary>
        /// <param name="buffer">包含随机数的字节数组。</param>
        public void NextBytes(byte[] buffer)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));

            // Fill up the bulk of the buffer in chunks of 4 bytes at a time.
            uint x = this._x, y = this._y, z = this._z, w = this._w;
            var i = 0;
            uint t;
            int bound;
            for (bound = buffer.Length - 3; i < bound;)
            {
                // Generate 4 bytes. 
                // Increased performance is achieved by generating 4 random bytes per loop.
                // Also note that no mask needs to be applied to zero out the higher order bytes before
                // casting because the cast ignores thos bytes. Thanks to Stefan Troschz for pointing this out.
                t = x ^ (x << 11);
                x = y; y = z; z = w;
                w = w ^ (w >> 19) ^ t ^ (t >> 8);

                buffer[i++] = (byte)w;
                buffer[i++] = (byte)(w >> 8);
                buffer[i++] = (byte)(w >> 16);
                buffer[i++] = (byte)(w >> 24);
            }

            // Fill up any remaining bytes in the buffer.
            if (i < buffer.Length)
            {
                // Generate 4 bytes.
                t = x ^ (x << 11);
                x = y; y = z; z = w;
                w = w ^ (w >> 19) ^ t ^ (t >> 8);

                buffer[i++] = (byte)w;
                var s = 0;
                while (i < buffer.Length)
                {
                    buffer[i++] = (byte)(w >> ((s + 8) % 24));
                }
            }
            this._x = x; this._y = y; this._z = z; this._w = w;
        }

        #endregion

        #region Public Methods [Methods not present on System.Random]

        /// <summary>
        /// 返回非负的 <see cref="uint.MaxValue"/> 随机数。 
        /// </summary>
        /// <returns>大于等于零且小于 <see cref="uint.MaxValue"/> 的 32 位无符号整数。</returns>
        public uint NextUInt()
        {
            uint t = this._x ^ (this._x << 11);
            this._x = this._y; this._y = this._z; this._z = this._w;
            return this._w = this._w ^ (this._w >> 19) ^ t ^ (t >> 8);
        }

        /// <summary>
        /// 返回非负的 <see cref="int.MaxValue"/> 随机数。 
        /// </summary>        
        /// <returns>大于等于零且小于 <see cref="int.MaxValue"/> 的 32 位有符号整数。</returns>  
        public int NextInt()
        {
            uint t = this._x ^ (this._x << 11);
            this._x = this._y; this._y = this._z; this._z = this._w;
            return (int)(0x7FFFFFFF & (this._w = this._w ^ (this._w >> 19) ^ t ^ (t >> 8)));
        }

        // Buffer 32 bits in bitBuffer, return 1 at a time, keep track of how many have been returned
        // with bitBufferIdx.
        private uint _bitBuffer;
        private uint _bitMask = 1;

        /// <summary>
        /// 返回一个随机的布尔值。
        /// </summary>
        /// <returns><see cref="bool"/> 的随机值。</returns>
        public bool NextBool()
        {
            if (this._bitMask == 1)
            {
                // Generate 32 more bits.
                uint t = this._x ^ (this._x << 11);
                this._x = this._y; this._y = this._z; this._z = this._w;
                this._bitBuffer = this._w = this._w ^ (this._w >> 19) ^ t ^ (t >> 8);

                // Reset the bitMask that tells us which bit to read next.
                this._bitMask = 0x80000000;
                return (this._bitBuffer & this._bitMask) == 0;
            }

            return (this._bitBuffer & (this._bitMask >>= 1)) == 0;
        }

        #endregion

        /// <summary>
        /// 创建一个加权平均算法的提供程序。
        /// </summary>
        /// <typeparam name="TKey">元素的数据类型。</typeparam>
        /// <param name="enumerable">枚举集合。</param>
        /// <param name="weightSelector">权重取值选择器。</param>
        /// <returns>一个加权平均算法的提供程序。</returns>
        public static IWeightedRandomProvider<TKey> CreateWeighted<TKey>(IEnumerable<TKey> enumerable, Func<TKey, int> weightSelector) where TKey : notnull
        {
            var weightSum = enumerable.Sum(weightSelector) * 1.0M;
            var items = new Dictionary<TKey, decimal>();

            var total = 1M;
            var last = default(TKey);
            foreach (var item in enumerable)
            {
                last = item;
                var weight = weightSelector(item);
                if (weight < 1 || weight > 1000) throw new NotSupportedException("The weight value shall not be less than 1 or more than 1000.");

                var fw = Math.Round(weight * 1M / weightSum, 4);
                total -= fw;

                items.Add(item, fw);
            }

            if(last == null) throw new ArgumentException("Invalid Operation",nameof(enumerable));
            
            if (total == 1M) throw new ArgumentException("Invalid iteratable collection.", nameof(enumerable));

            if (total != 0m) items[last] += total;

            var ran = new WeightedRandomProvider<TKey>(Instance, items.Count);
            foreach (var item in items)
            {
                ran.AddOrUpdate(item.Key, item.Value);
            }
            return ran;
        }
    }
    /// <summary>
    /// 表示一个字符串的类型。
    /// </summary>
    [Flags]
    public enum CharacterType
    {
        /// <summary>
        /// 默认类型。表示 A-Z 和 0-9。
        /// </summary>
        Default = UpperCase | Numeric,
        /// <summary>
        /// 大写字符。表示 A-Z。
        /// </summary>
        UpperCase = 1,
        /// <summary>
        /// 小写字符。表示 a-z。
        /// </summary>
        LowerCase = 2,
        /// <summary>
        /// 数字字符。表示 0-9。
        /// </summary>
        Numeric = 4,
        /// <summary>
        /// 特殊字符。表示“,.;:?!/@#$%^&amp;()=+*-_{}[]&lt;&gt;>|~”之一。
        /// </summary>
        Special = 8,
        /// <summary>
        /// 所有字符。
        /// </summary>
        All = UpperCase | LowerCase | Numeric | Special
    }
}
