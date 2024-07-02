using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechsysLog.Api.Repositories;
using TechsysLog.Core.Commands;

namespace TechsysLog.Api.Commands
{
    public class DeleteOrderCommand : Command
    {
        public Guid Id { get; set; }
        public override bool Valid()
        {
            validationResult = new DeleteOrderValidation().Validate(this);
            return validationResult.IsValid;
        }
    }

    public class DeleteOrderValidation : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderValidation()
        {
            RuleFor(c => c.Id)
                  .NotEqual(Guid.Empty)
                  .WithMessage("Invalid id");
        }
    }
    public class DeleteOrderCommandHandler : CommandHandler,
         IRequestHandler<DeleteOrderCommand, ValidationResult>
    {
        private readonly IOrderRepository _OrderRepository;

        public DeleteOrderCommandHandler(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<ValidationResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            if (!request.Valid()) return request.validationResult;
           await _OrderRepository.DeleteById(request.Id);
            return await PersistData(_OrderRepository.UnitOfWork);
        }
    }
}
