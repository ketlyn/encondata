using FluentValidation.Results;
using System.Threading.Tasks;
using TechsysLog.Core.Data;

namespace TechsysLog.Core.Commands
{
    public abstract class CommandHandler
    {
        protected ValidationResult _validationResult;
        protected CommandHandler()
        {
            _validationResult = new ValidationResult();
        }

        protected void AddError(string mensagem)
        {
            _validationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddError("There was an error persisting the data");

            return _validationResult;
        }
    }
}
