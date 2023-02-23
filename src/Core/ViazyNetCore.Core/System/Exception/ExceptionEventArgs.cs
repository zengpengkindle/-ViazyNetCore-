namespace System
{
    public class ExceptionEventArgs
    {
        public ExceptionEventArgs(Exception ex)
        {
            this.Exception = ex;
        }

        public Exception Exception { get; }
    }
}
