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
    public class DeleteDeliveryCommand : Command
    {
        public Guid Id { get; set; }
        public override bool Valid()
        {
            validationResult = new DeleteDeliveryValidation().Validate(this);
            return validationResult.IsValid;
        }
    }

    public class DeleteDeliveryValidation : AbstractValidator<DeleteDeliveryCommand>
    {
        public DeleteDeliveryValidation()
        {
            RuleFor(c => c.Id)
                  .NotEqual(Guid.Empty)
                  .WithMessage("Invalid id");
        }
    }
    public class DeleteDeliveryCommandHandler : CommandHandler,
         IRequestHandler<DeleteDeliveryCommand, ValidationResult>
    {
        private readonly IDeliveryRepository _DeliveryRepository;

        public DeleteDeliveryCommandHandler(IDeliveryRepository DeliveryRepository)
        {
            _DeliveryRepository = DeliveryRepository;
        }

        public async Task<ValidationResult> Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken)
        {
            if (!request.Valid()) return request.validationResult;
           await _DeliveryRepository.DeleteById(request.Id);
            return await PersistData(_DeliveryRepository.UnitOfWork);
        }
    }
}
