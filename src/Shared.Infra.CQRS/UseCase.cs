using MediatR;

namespace Shared.Infra.CQRS;

public abstract class UseCase<TResponse> : IRequest<TResponse> { }