using FluentValidation;
using OnlineStore.BLL.AccountService.Model;
using System.Text.RegularExpressions;

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

        public bool IsPhoneValid(string phone)=>
            Regex.IsMatch(phone, @"(?=.{10,12}$)(375[0-9]{9}$)|(7[0-9]{9}$)");

        public bool IsLoginValid(string login)=>
             Regex.IsMatch(login, @"[0-9a-zA-Z_\-]{8,20}");

        public bool IsPasswordValid(string password)=>
             Regex.IsMatch(password, @"(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])[0-9a-zA-Z_\-]{8,20}");
    }
}
