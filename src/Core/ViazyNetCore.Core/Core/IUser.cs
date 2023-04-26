namespace ViazyNetCore
{
    public interface IUser<TKey>
    {
        public TKey Id { get; }

        public string Username { get; }
        public string Nickname { get; }

        public int IdentityType { get; }
        /// <summary>
        /// 是否被管制
        /// </summary>
        bool IsModerated { get; }
    }

    public interface IUser : IUser<string>
    { }
}
