namespace ViazyNetCore.Ddd;

public interface IDto
{

}

public interface IDto<TKey> : IDto
{
    TKey Id { get; set; }
}
