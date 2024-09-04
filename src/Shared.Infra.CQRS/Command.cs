namespace Shared.Infra.CQRS;

public abstract class Command : UseCase<EmptyCommandResponse> { }

public abstract class Command<TResponse> : UseCase<TResponse> { }