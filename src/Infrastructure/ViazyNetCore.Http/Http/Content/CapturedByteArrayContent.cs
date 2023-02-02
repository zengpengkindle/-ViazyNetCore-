using System.Net.Http;

namespace ViazyNetCore.Http
{
    public class CapturedByteArrayContent : ByteArrayContent
    {

        public CapturedByteArrayContent(byte[] content, string contentType) : base(content)
        {
            Headers.Remove("Content-Type");
            if (contentType != null)
                Headers.TryAddWithoutValidation("Content-Type", contentType);
        }

    }
}