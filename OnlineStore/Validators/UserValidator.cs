using FluentValidation;
using OnlineStore.BLL.AccountService.Model;

namespace OnlineStore.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            string msg = "Ошибка в поле {PropertyName}: значение {PropertyValue}";

            RuleFor(acc => acc.User.Name)
                .NotEmpty().WithMessage(msg);

            RuleFor(acc => acc.User.Email)
                .EmailAddress().WithMessage(msg);

            RuleFor(acc => acc.User.Phone)
                .Length(12).WithMessage(msg)
                .Must(IsPhoneValid).WithMessage(msg);

            RuleFor(acc=>acc.RegUser.Login)
                .Length(8,30).WithMessage(msg)
                .Must(IsLoginValid).WithMessage(msg);

            RuleFor(acc=>acc.RegUser.Password)
                .Length(8,30).WithMessage(msg)
                .Must(IsPasswordValid).WithMessage(msg);
        }

        public bool IsPhoneValid(string phone) => true;//not finished

        public bool IsLoginValid(string login) => true;//not finished

        public bool IsPasswordValid(string password) => true;//not finished
    }
}
