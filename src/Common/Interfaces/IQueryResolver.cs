namespace Common.Interfaces
{
    public interface IQuerySender
    {
        IQueryResult<T> InvokeQuery<T>(IQuery query);
    }
}
