namespace Tsukinia.Core
{
    public class DataOperationResult<T> : OperationResult<T>
    {
        public DataErrorType ErrorType {get;set;}
    }
}