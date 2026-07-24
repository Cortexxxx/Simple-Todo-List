namespace ToDoList.Shared.Constants;

public static class ApiErrors
{
    public const string IncorrectEmailOrPassword = "Incorrect email or password";
    public const string UserIsAlreadyLoggedIn = "User is already logged in";
    public const string InternalServerError = "Internal server error occurred";

    public const string EmailRequired = "Email must not be empty";
    public const string EmailInvalid = "Enter a valid email";
    public const string EmailAlreadyRegistered = "A user with this email is already registered";
    public const string PasswordRequired = "Password must not be empty";
    public const string PasswordMinLength = "Minimum password length is 6 characters";

    public const string TodoTitleRequired = "Title must not be empty";
    public const string TodoTitleMaxLength = "Title length must not exceed 100 characters";
    public const string TodoDescriptionMaxLength = "Description length must not exceed 2000 characters";

    public const string TagNameRequired = "Tag name must not be empty";
    public const string TagNameMaxLength = "Tag name length must not exceed 20 characters";
    public const string TagColorRequired = "Tag color is required";
    public const string TagColorInvalidHex = "Color must be a valid HEX code (e.g. #FF5733)";

    public const string JwtOptionsSectionMissing = "JwtOptions section is missing in configuration!";
    public const string JwtSecretKeyNotConfigured = "JWT SecretKey is not configured!";
}
