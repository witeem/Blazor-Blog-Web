namespace BlazorServerApp.HttpServers
{
    public class CustomerHttpException : Exception
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public CustomerHttpException() : base() { }

        public CustomerHttpException(string errorCode, string errorMessage) : base()
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = ErrorMessage;
        }
    }
}
