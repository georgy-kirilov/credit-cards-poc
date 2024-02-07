using SharedKernel;

namespace Application.Abstraction.Messaging;
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQueary<TResponse>
{
    Task<Result> Handle(TQuery query,CancellationToken cancellationToken);
}
