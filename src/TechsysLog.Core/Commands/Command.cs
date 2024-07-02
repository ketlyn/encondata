using FluentValidation.Results;
using MediatR;
using System;

namespace TechsysLog.Core.Commands
{
    public abstract class Command : IRequest<ValidationResult>
    {
        public DateTime TimeStamp { get; private set; }
        public ValidationResult validationResult { get; set; }
        public Command()
        {
            TimeStamp = DateTime.Now;
        }

        public virtual bool Valid()
        {
            throw new NotImplementedException();
        }
    }
}
