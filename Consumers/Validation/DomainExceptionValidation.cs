using Domain.Entities.Base;
using System.Text.RegularExpressions;

namespace Domain.Validation;

public class DomainExceptionValidation : Exception
{
    public DomainExceptionValidation(string error) : base(error)
    { }

    public static void When(bool hasError, string error)
    {
        if (hasError)
        {
            throw new DomainExceptionValidation(error);
        }
    }

    public static ResultResponse<bool> When(bool hasError, string error, string type)
    {
        return new ResultResponse<bool> { success = hasError, type = type, message = hasError ? error : string.Empty, Data = hasError };
    }

    public static void IsValidPassword(string password, string error)
    {
        if (String.IsNullOrEmpty(password) || password.Length < 8)
        {
            throw new DomainExceptionValidation(error);
        }

        bool hasUpperCase = false;
        bool hasLowerCase = false;
        bool hasSpecialChar = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUpperCase = true;
            else if (char.IsLower(c)) hasLowerCase = true;
            else if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
        }

        if (!hasUpperCase || !hasLowerCase || !hasSpecialChar)
            throw new DomainExceptionValidation(error);
    }

    public static void IsValidFullName(string fullName, string error)
    {
        if (String.IsNullOrEmpty(fullName))
            throw new DomainExceptionValidation(error);

        if (Regex.IsMatch(fullName, @"^[A-Z][a-z]+$"))
            throw new DomainExceptionValidation(error);
    }

    public static void IsValidCPF(string cpf, string error)
    {
        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (cpf.Length != 11)
        {
            throw new DomainExceptionValidation(error);
        }

        for (int i = 0; i < 10; i++)
        {
            if (cpf == new string(i.ToString()[0], 11))
            {
                throw new DomainExceptionValidation(error);
            }
        }

        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * multiplier1[i];
        }

        int remaining = sum % 11;

        if (remaining < 2)
        {
            remaining = 0;
        }
        else
        {
            remaining = 11 - remaining;
        }

        string digit = remaining.ToString();

        sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(cpf[i].ToString()) * multiplier2[i];
        }

        remaining = sum % 11;

        if (remaining < 2)
        {
            remaining = 0;
        }
        else
        {
            remaining = 11 - remaining;
        }

        digit += remaining.ToString();

        if (!cpf.EndsWith(digit))
            throw new DomainExceptionValidation(error);
    }

    public static void IsValidEmail(string email, string error)
    {
        string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
        if (!Regex.IsMatch(email, pattern))
            throw new DomainExceptionValidation(error);
    }
}
