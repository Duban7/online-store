namespace OnlineStore.BLL.AccountService.Exceptions
{
    public class UserFoundException: Exception
    {
        public UserFoundException() : base()
        {

        }
        public UserFoundException(string message) : base(message)
        {

        }
        public UserFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
