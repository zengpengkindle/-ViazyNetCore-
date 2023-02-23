namespace ViazyNetCore.CommondTest;

[Message(Id = nameof(NotifyBody))]
public class NotifyBody
{
    public string Id { get; set; } = null!;
    public decimal Amount { get; set; }
    public int Status { get; set; }
    public DateTime ModifyTime { get; set; }
}