using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechsysLog.Api.Entities;
using TechsysLog.Api.Repositories;
using TechsysLog.Core.Commands;

namespace TechsysLog.Api.Commands
{
    public class AddDeliveryCommand : Command
    {
        public string Number { get; private set; }
        public DateTime DeliveryDate { get; private set; }
        public Order Order { get; private set; }

        public override bool Valid()
        {
            validationResult = new AddDeliveryValidation().Validate(this);
            return validationResult.IsValid;
        }
        public class AddDeliveryValidation : AbstractValidator<AddDeliveryCommand>
        {
            public AddDeliveryValidation()
            {
                RuleFor(c => c.Number)
                    .NotEmpty()
                    .WithMessage("Invalid value Number")
                    .NotNull()
                    .WithMessage("Invalid value Number");

                RuleFor(c => c.DeliveryDate)
                    .NotEmpty()
                    .WithMessage("Invalid value DeliveryDate")
                    .NotNull()
                    .WithMessage("Invalid value  DeliveryDate");

                RuleFor(c => c.Order)
                    .NotEmpty()
                    .WithMessage("Invalid value Order")
                    .NotNull()
                    .WithMessage("Invalid value  Order");

            }
        }
    }


    public class AddDeliveryCommandHandler : CommandHandler,
        IRequestHandler<AddDeliveryCommand, ValidationResult>
    {
        private readonly IDeliveryRepository _DeliveryRepository;
        public AddDeliveryCommandHandler(IDeliveryRepository DeliveryRepository)
        {
            _DeliveryRepository = DeliveryRepository;
        }

        public async Task<ValidationResult> Handle(AddDeliveryCommand request, CancellationToken cancellationToken)
        {
            if (!request.Valid()) return request.validationResult;
            _DeliveryRepository.Add(MappDelivery(request));
            return await PersistData(_DeliveryRepository.UnitOfWork);
        }

        private Delivery MappDelivery(AddDeliveryCommand request)
        {
            return new Delivery(request.Number, request.DeliveryDate, request.Order);
        }
    }
}
