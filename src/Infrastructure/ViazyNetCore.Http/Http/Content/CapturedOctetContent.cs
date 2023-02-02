namespace ViazyNetCore.Http
{
    public class CapturedOctetContent : CapturedByteArrayContent
    {
        public CapturedOctetContent(byte[] content) : base(content, "application/octet-stream;tt-data=a")
        {
        }
    }
}