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
    public class AddOrderCommand : Command
    {
        public string Number { get; private set; }
        public string Description { get; private set; }
        public double Value { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public Address Address { get; private set; }

        public override bool Valid()
        {
            validationResult = new AddOrderValidation().Validate(this);
            return validationResult.IsValid;
        }
        public class AddOrderValidation : AbstractValidator<AddOrderCommand>
        {
            public AddOrderValidation()
            {
                RuleFor(c => c.Number)
                    .NotEmpty()
                    .WithMessage("Invalid value Number")
                    .NotNull()
                    .WithMessage("Invalid value Number");

                RuleFor(c => c.Description)
                    .NotEmpty()
                    .WithMessage("Invalid value Description")
                    .NotNull()
                    .WithMessage("Invalid value  Description");

                RuleFor(c => c.Value)
                    .GreaterThan(0)
                    .WithMessage("Invalid price value");

                RuleFor(c => c.Address)
                 .NotEmpty()
                    .WithMessage("Invalid value Address")
                    .NotNull()
                    .WithMessage("Invalid value Address");

            }
        }
    }


    public class AddOrderCommandHandler : CommandHandler,
        IRequestHandler<AddOrderCommand, ValidationResult>
    {
        private readonly IOrderRepository _OrderRepository;
        public AddOrderCommandHandler(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        public async Task<ValidationResult> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            if (!request.Valid()) return request.validationResult;
            _OrderRepository.Add(MappOrder(request));
            return await PersistData(_OrderRepository.UnitOfWork);
        }

        private Order MappOrder(AddOrderCommand request)
        {
            return new Order(request.Number, request.Description, request.Value, request.UpdateDate, request.Address);
        }
    }
}
