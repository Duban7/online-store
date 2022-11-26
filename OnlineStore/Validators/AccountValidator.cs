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

        public bool IsPhoneValid(string phone)
        {
            int phoneLength = -1;
            if (phone.StartsWith("375")) phoneLength = 12;
            if (phone.StartsWith("7")) phoneLength = 10;

            if (phone.Length != phoneLength) return false;

            foreach (char c in phone)
                if (!Char.IsDigit(c)) return false;

            return true;
        }

        public bool IsLoginValid(string login)
        {
            char[] validSymbols = { '-', '_' };

            foreach (Char c in login)
                if (!(Char.IsLetter(c) || Char.IsDigit(c) || validSymbols.Contains(c))) return false;

            return true;
        }

        public bool IsPasswordValid(string password)
        {
            bool hasUpperCaseLetter = false, hasLowerCaseLetter = false, hasDigit = false;

            char[] validSymbols = { '-', '_' };

            foreach (Char c in password)
            {
                if (!(Char.IsLetter(c) || Char.IsDigit(c) || validSymbols.Contains(c))) return false;
                if (Char.IsUpper(c)) hasUpperCaseLetter = true;
                if (Char.IsLower(c)) hasLowerCaseLetter = true;
                if (Char.IsDigit(c)) hasDigit = true;
            }

            return hasUpperCaseLetter && hasLowerCaseLetter && hasDigit;
        }
    }
}
