public class RegExp
{
    public static string AlphaNumericValidation(bool IsMandatory, int MaxLength)
    {
        return "^[a-zA-Z0-9]{" + (IsMandatory ? "1" : "0") + "," + MaxLength.ToString() + "}$";
    }

    public static string EmailValidation(bool IsMandatory, int MaxLength)
    {
        return "^[a-zA-Z0-9 /.,\\\\\\-_@]{" + (IsMandatory ? "1" : "0") + "," + MaxLength.ToString() + "}$";
    }

    public static string TextValidation(bool IsMandatory, int MaxLength)
    {
        return "^[a-zA-Z0-9 &%$#!*()+=\\[\\]:;<>?/.,\\\\\\-_@']{" + (IsMandatory ? "1" : "0") + "," + MaxLength.ToString() + "}$";
    }
}
