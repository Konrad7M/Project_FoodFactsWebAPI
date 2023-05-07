namespace project_actaware.Execptions
{
    public class BusinessException : Exception
    {
        public ErrorTypeEnum ErrorType { get; }
        public BusinessException(ErrorTypeEnum errorType,string message): base(message) 
        { 
            ErrorType = errorType;
        }
    }
}
