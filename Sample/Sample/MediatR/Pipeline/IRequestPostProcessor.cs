namespace MediatR.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a request post-processor for a request
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequestPostProcessor<in TRequest, in TResponse>
    {
        /// <summary>
        /// Process method executes after the Handle method on your handler
        /// </summary>
        /// <param name="request">Request instance</param>
        /// <param name="response">Response instance</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
    }
}