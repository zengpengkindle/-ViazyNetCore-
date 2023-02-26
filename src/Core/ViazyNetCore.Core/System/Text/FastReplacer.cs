using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 表示一个快速替换库。
    /// </summary>
    public class FastReplacer
    {
        private class TokenOccurrence
        {
            public readonly FastReplacerSnippet Snippet;
            public readonly int Start; // Position of a token in the snippet.
            public readonly int End; // Position of a token in the snippet.

            public TokenOccurrence(FastReplacerSnippet snippet, int start, int end)
            {
                this.Snippet = snippet;
                this.Start = start;
                this.End = end;
            }
        }

        private readonly Dictionary<string, List<TokenOccurrence>> _occurrencesOfToken;
        private readonly FastReplacerSnippet _rootSnippet = new(string.Empty);

        /// <summary>
        /// 获取所有标识。
        /// </summary>
        public IEnumerable<string> Tokens => this._occurrencesOfToken.Keys;

        /// <summary>
        /// 获取所有标识（不含开合和闭合标识）。
        /// </summary>
        public IEnumerable<string> TokenKeys
        {
            get
            {
                foreach(var item in this.Tokens)
                {
                    yield return item.Substring(this.TokenOpen.Length, item.Length - this.TokenOpen.Length - this.TokenClose.Length);
                }
            }
        }

        /// <summary>
        /// 获取开合标识。
        /// </summary>
        public string TokenOpen { get; }

        /// <summary>
        /// 获取闭合标识。
        /// </summary>
        public string TokenClose { get; }

        /// <summary>
        /// 初始化一个 <see cref="FastReplacer"/> 类的新实例。
        /// </summary>
        /// <param name="tokenOpen">开合标识。</param>
        /// <param name="tokenClose">闭合标识。</param>
        /// <param name="comparer">字符串比较器。</param>
        public FastReplacer(string tokenOpen = "{", string tokenClose = "}", StringComparer? comparer = null)
        {
            if(string.IsNullOrEmpty(tokenOpen))
                throw new ArgumentNullException(nameof(tokenOpen));
            if(string.IsNullOrEmpty(tokenClose))
                throw new ArgumentNullException(nameof(tokenClose));

            this.TokenOpen = tokenOpen;
            this.TokenClose = tokenClose;
            this._occurrencesOfToken = new Dictionary<string, List<TokenOccurrence>>(comparer?? StringComparer.Ordinal);
        }

        /// <summary>
        /// 添加一个字符串。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <returns>当前对象。</returns>
        public FastReplacer Append(string text)
        {
            var snippet = new FastReplacerSnippet(text);
            this._rootSnippet.Append(snippet);
            this.ExtractTokens(snippet);
            return this;
        }

        /// <summary>
        /// 替换指定的标识为新内容。
        /// </summary>
        /// <param name="token">标识（没有开闭合标签）。</param>
        /// <param name="text">新内容。</param>
        /// <returns>当前对象。</returns>
        public FastReplacer ReplaceToken(string token,string text)
        {
            this.Replace(this.TokenOpen + token + this.TokenClose, text);
            return this;
        }

        /// <summary>
        /// 替换指定的标识为新内容。
        /// </summary>
        /// <param name="token">标识。</param>
        /// <param name="text">新内容。</param>
        /// <returns>替换的结果，若为 <see langword="true"/> 值表示替换成功，否则表示替换失败。</returns>
        public bool Replace(string token, string text)
        {
            this.ValidateToken(token, text, false);
            if(this._occurrencesOfToken.TryGetValue(token, out var occurrences) && occurrences.Count > 0)
            {
                this._occurrencesOfToken.Remove(token);
                var snippet = new FastReplacerSnippet(text);
                foreach(var occurrence in occurrences)
                    occurrence.Snippet.Replace(occurrence.Start, occurrence.End, snippet);
                this.ExtractTokens(snippet);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在指定的标识前添加字符串。
        /// </summary>
        /// <param name="token">标识。</param>
        /// <param name="text">新内容。</param>
        /// <returns>若为 <see langword="true"/> 值表示插入成功，否则表示插入失败。</returns>
        public bool InsertBefore(string token, string text)
        {
            this.ValidateToken(token, text, false);
            if(this._occurrencesOfToken.TryGetValue(token, out var occurrences) && occurrences.Count > 0)
            {
                var snippet = new FastReplacerSnippet(text);
                foreach(var occurrence in occurrences)
                    occurrence.Snippet.InsertBefore(occurrence.Start, snippet);
                this.ExtractTokens(snippet);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在指定的标识后添加字符串。
        /// </summary>
        /// <param name="token">标识。</param>
        /// <param name="text">新内容。</param>
        /// <returns>若为 <see langword="true"/> 值 表示插入成功，否则表示插入失败。</returns>
        public bool InsertAfter(string token, string text)
        {
            this.ValidateToken(token, text, false);
            if(this._occurrencesOfToken.TryGetValue(token, out var occurrences) && occurrences.Count > 0)
            {
                var snippet = new FastReplacerSnippet(text);
                foreach(var occurrence in occurrences)
                    occurrence.Snippet.InsertAfter(occurrence.End, snippet);
                this.ExtractTokens(snippet);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判定指定的标识是否存在。
        /// </summary>
        /// <param name="token">标识。</param>
        /// <returns>若为 <see langword="true"/> 值表示存在，否则表示不存在。</returns>
        public bool Contains(string token)
        {
            this.ValidateToken(token, token, false);
            if(this._occurrencesOfToken.TryGetValue(token, out var occurrences))
                return occurrences.Count > 0;
            return false;
        }

        private void ExtractTokens(FastReplacerSnippet snippet)
        {
            var last = 0;
            while(last < snippet.Text.Length)
            {
                // Find next token position in snippet.Text:
                var start = snippet.Text.IndexOf(this.TokenOpen, last, StringComparison.InvariantCultureIgnoreCase);
                if(start == -1)
                    return;
                var end = snippet.Text.IndexOf(this.TokenClose, start + this.TokenOpen.Length, StringComparison.InvariantCultureIgnoreCase);
                if(end == -1)
                    throw new ArgumentException(string.Format("Token is opened but not closed in text \"{0}\".", snippet.Text));
                var eol = snippet.Text.IndexOf('\n', start + this.TokenOpen.Length);
                if(eol != -1 && eol < end)
                {
                    last = eol + 1;
                    continue;
                }

                // Take the token from snippet.Text:
                end += this.TokenClose.Length;
                var token = snippet.Text[start..end];
                var context = snippet.Text;
                this.ValidateToken(token, context, true);

                // Add the token to the dictionary:
                var tokenOccurrence = new TokenOccurrence(snippet, start, end);
                if(this._occurrencesOfToken.TryGetValue(token, out var occurrences))
                    occurrences.Add(tokenOccurrence);
                else
                    this._occurrencesOfToken.Add(token, new List<TokenOccurrence> { tokenOccurrence });

                last = end;
            }
        }

        private void ValidateToken(string token, string context, bool alreadyValidatedStartAndEnd)
        {
            if(!alreadyValidatedStartAndEnd)
            {
                if(!token.StartsWith(this.TokenOpen, StringComparison.InvariantCultureIgnoreCase))
                    throw new ArgumentException(string.Format("The token '{0}' is not at the start of '{1}'. The string is '{2}'.", token, this.TokenOpen, context));
                int closePosition = token.IndexOf(this.TokenClose, StringComparison.InvariantCultureIgnoreCase);
                if(closePosition == -1)
                    throw new ArgumentException(string.Format("The token '{0}' is not at the end of '{1}'. The string is '{2}'.", token, this.TokenClose, context));
                if(closePosition != token.Length - this.TokenClose.Length)
                    throw new ArgumentException(string.Format("The start and end positions of the token '{0}' are opposite. The string is '{1}'.", token, context));
            }

            if(token.Length == this.TokenOpen.Length + this.TokenClose.Length)
                throw new ArgumentException(string.Format("The token is empty. The string is '{0}'.", context));
            if(token.Contains("\n"))
                throw new ArgumentException(string.Format("Token contains new line. The string is '{0}'.", context));
            if(token.IndexOf(this.TokenOpen, this.TokenOpen.Length, StringComparison.InvariantCultureIgnoreCase) != -1)
                throw new ArgumentException(string.Format("Invalid identity '{0}'. The string is '{1}'.", token, context));
        }
    }

    class FastReplacerSnippet
    {
        private class InnerSnippet
        {
            public FastReplacerSnippet Snippet;
            public int Start; // Position of the snippet in parent snippet's Text.
            public int End; // Position of the snippet in parent snippet's Text.
            public int Order1; // Order of snippets with a same Start position in their parent.
            public int Order2; // Order of snippets with a same Start position and Order1 in their parent.

            public InnerSnippet(FastReplacerSnippet snippet, int start, int end, int order1, int order2)
            {
                this.Snippet = snippet;
                this.Start = start;
                this.End = end;
                this.Order1 = order1;
                this.Order2 = order2;
            }
        }

        public readonly string Text;
        private readonly List<InnerSnippet> _innerSnippets;

        public FastReplacerSnippet(string text)
        {
            this.Text = text;
            this._innerSnippets = new List<InnerSnippet>();
        }

        public void Append(FastReplacerSnippet snippet)
        {
            this._innerSnippets.Add(new InnerSnippet(snippet, this.Text.Length, this.Text.Length, 1, this._innerSnippets.Count));
        }

        public void Replace(int start, int end, FastReplacerSnippet snippet)
        {
            this._innerSnippets.Add(new InnerSnippet(snippet, start, end, 0, 0));
        }

        public void InsertBefore(int start, FastReplacerSnippet snippet)
        {
            this._innerSnippets.Add(new InnerSnippet(snippet, start, start, 2, this._innerSnippets.Count));
        }

        public void InsertAfter(int end, FastReplacerSnippet snippet)
        {
            this._innerSnippets.Add(new InnerSnippet(snippet, end, end, 1, this._innerSnippets.Count));
        }

        public void ToString(StringBuilder sb)
        {
            this._innerSnippets.Sort((a, b) =>
            {
                if(a.Start != b.Start) return a.Start - b.Start;
                if(a.End != b.End) return a.End - b.End; // Disambiguation if there are inner snippets inserted before a token (they have End==Start) go before inner snippets inserted instead of a token (End>Start).
                if(a.Order1 != b.Order1) return a.Order1 - b.Order1;
                return a.Order2 - b.Order2;
            });
            var lastPosition = 0;
            foreach(var innerSnippet in this._innerSnippets)
            {
                sb.Append(this.Text, lastPosition, innerSnippet.Start - lastPosition);
                innerSnippet.Snippet.ToString(sb);
                lastPosition = innerSnippet.End;
            }
            sb.Append(this.Text, lastPosition, this.Text.Length - lastPosition);
        }

        public int GetLength()
        {
            int len = this.Text.Length;
            foreach(var innerSnippet in this._innerSnippets)
            {
                len -= innerSnippet.End - innerSnippet.Start;
                len += innerSnippet.Snippet.GetLength();
            }
            return len;
        }
    }
}
