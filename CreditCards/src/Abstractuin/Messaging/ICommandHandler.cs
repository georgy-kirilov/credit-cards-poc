//using SharedKernel;

namespace Application.Abstraction.Messaging;
public interface ICommandHandler<in TCommad> where TCommad : ICommand
{
    Task<Result> Handle(TCommad commad,CancellationToken cancellationToken);
}


public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command,CancellationToken cancellationToken);
}
