using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    /// <summary>
    /// 定义一个路径格式化程序。
    /// </summary>
    public interface IPathFormatter
    {
        /// <summary>
        /// 格式化指定路径。
        /// </summary>
        /// <param name="originFileName">路径。</param>
        /// <param name="pathFormat">格式化。</param>
        /// <returns>格式化后的路径。</returns>
        string Format(string originFileName, string pathFormat);
        /// <summary>
        /// 格式化指定路径。
        /// </summary>
        /// <param name="pathFormat">格式化。</param>
        /// <returns>格式化后的路径。</returns>
        string Format(string pathFormat);
    }

    /// <summary>
    /// 表示一个路径格式化程序。
    /// </summary>
    public class PathFormatter : IPathFormatter
    {
        /// <summary>
        /// 格式化指定路径。
        /// </summary>
        /// <param name="originFileName">路径。</param>
        /// <param name="pathFormat">格式化。</param>
        /// <returns>格式化后的路径。</returns>
        public virtual string Format(string originFileName, string pathFormat)
        {

            var invalidPattern = new Regex(@"[\\\/\:\*\?\042\<\>\|]");
            originFileName = invalidPattern.Replace(originFileName, "");

            string extension = Path.GetExtension(originFileName);
            string filename = Path.GetFileNameWithoutExtension(originFileName);

            pathFormat = pathFormat.Replace("{filename}", filename);

            return this.Format(pathFormat) + extension;
        }

        /// <summary>
        /// 格式化指定路径。
        /// </summary>
        /// <param name="pathFormat">格式化。</param>
        /// <returns>格式化后的路径。</returns>
        public virtual string Format(string pathFormat)
        {
            if(String.IsNullOrWhiteSpace(pathFormat))
            {
                pathFormat = "{filename}{rand:6}";
            }
            if(pathFormat.Contains("{rand"))
                pathFormat = new Regex(@"\{rand(\:?)(\d+)\}", RegexOptions.Compiled).Replace(pathFormat, new MatchEvaluator(delegate (Match match)
                {
                    var digit = 6;
                    if(match.Groups.Count > 2)
                    {
                        digit = Convert.ToInt32(match.Groups[2].Value);
                    }
                    var rand = new Random();
                    return rand.Next((int)Math.Pow(10, digit), (int)Math.Pow(10, digit + 1)).ToString();
                }));

            pathFormat = pathFormat.Replace("{time}", DateTime.Now.Ticks.ToString());
            pathFormat = pathFormat.Replace("{yyyy}", DateTime.Now.Year.ToString());
            pathFormat = pathFormat.Replace("{yy}", (DateTime.Now.Year % 100).ToString("D2"));
            pathFormat = pathFormat.Replace("{mm}", DateTime.Now.Month.ToString("D2"));
            pathFormat = pathFormat.Replace("{dd}", DateTime.Now.Day.ToString("D2"));
            pathFormat = pathFormat.Replace("{hh}", DateTime.Now.Hour.ToString("D2"));
            pathFormat = pathFormat.Replace("{ii}", DateTime.Now.Minute.ToString("D2"));
            pathFormat = pathFormat.Replace("{ss}", DateTime.Now.Second.ToString("D2"));
            return pathFormat;
        }
    }

    /// <summary>
    /// 表示一个基于授权的路径格式化程序。
    /// </summary>
    public class DefaultPathFormatter : PathFormatter
    {
        //private readonly IAppStorage _appStorage;

        /// <summary>
        /// 初始化一个 <see cref="DefaultPathFormatter"/> 类的新实例。
        /// </summary>
        public DefaultPathFormatter(/*IAppStorage appStorage = null*/)
        {
            //this._appStorage = appStorage;
        }

        /// <summary>
        /// 格式化指定路径。
        /// </summary>
        /// <param name="pathFormat">格式化。</param>
        /// <returns>格式化后的路径。</returns>
        public override string Format(string pathFormat)
        {
            var path = base.Format(pathFormat);

            //- 授权改为 header 方式，没有从 cookie 中获取认证，所以先去掉

            //var area = identity is null
            //    ? "public"
            //    : (identity.IdentityType + "/" + new string(identity.Id.Reverse().ToArray()));

            return path.Replace("{area}", "public");
        }
    }
}
