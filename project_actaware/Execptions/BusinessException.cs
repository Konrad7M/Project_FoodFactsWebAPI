namespace project_actaware.Execptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message): base(message) { 
        }
    }
}
