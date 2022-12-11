namespace Demo.Document
{
    public class ServiceException : RuntimeException
    {
        public ServiceException(string? message) : base(message)
        {
        }

        public ServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}