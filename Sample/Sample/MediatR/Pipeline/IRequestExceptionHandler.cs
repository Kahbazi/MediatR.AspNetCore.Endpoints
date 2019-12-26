namespace MediatR.Pipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an exception handler for a request
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <typeparam name="TException">Exception type</typeparam>
    public interface IRequestExceptionHandler<in TRequest, TResponse, in TException>
        where TException : Exception
    {
        /// <summary>
        /// Called when the request handler throws an exception
        /// </summary>
        /// <param name="request">Request instance</param>
        /// <param name="exception">The thrown exception</param>
        /// <param name="state">The current state of handling the exception</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Defines an exception handler for a request
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequestExceptionHandler<in TRequest, TResponse> : IRequestExceptionHandler<TRequest, TResponse, Exception>
    {

    }
}
