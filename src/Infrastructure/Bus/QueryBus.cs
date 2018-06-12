using Common.Enums;
using Common.Interfaces;
using Infrastructure.CommunicationModels;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infrastructure.Bus
{
    public class QueryBus : IQuerySender
    {
        private readonly IQueryHandlerFactory queryHandlerFactory;
        private readonly IExceptionTypeResolver exceptionTypeResolver;

        public QueryBus(IQueryHandlerFactory queryHandlerFactory,
                        IExceptionTypeResolver exceptionTypeResolver)
        {
            this.queryHandlerFactory = queryHandlerFactory;
            this.exceptionTypeResolver = exceptionTypeResolver;
        }

        public IQueryResult<T> InvokeQuery<T>(IQuery query)
        {
            try
            {
                var handler = queryHandlerFactory.GetHandler(query);
                var sendingMethod = handler.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Handle");
                T result;

                if (sendingMethod != null)
                {
                    result = (T)sendingMethod.Invoke(handler, new object[] { query });
                    return new QueryResult<T>() { Data = result, QueryStatus = ActionStatus.Success };
                }

                throw new Exception($"Handle method not found on {handler.ToString()} object");
            }
            catch (Exception ex)
            {
                var message = new StringBuilder();

                message.Append(ex.Message);
                
                if (ex.InnerException != null)
                {
                    message.Append($" Inner: -> {ex.InnerException.Message}");
                }

                return new QueryResult<T>() { Data = default(T), QueryStatus = exceptionTypeResolver.ReturnCommandStatusForException(ex), ExceptionMessage = message.ToString()};
            }
        }
    }
}
