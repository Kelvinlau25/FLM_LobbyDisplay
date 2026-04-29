namespace FLM_LobbyDisplay.Models;

public class AuthenticatorModel
{
    public int    ID_ACL_USER     { get; set; }
    public int    ID_ACL_ROLE     { get; set; }
    public int    ID_ACL_RESOURCE { get; set; }
    public string USER_ID         { get; set; } = string.Empty;
    public string USR_EMAIL       { get; set; } = string.Empty;
    public string COMPANY         { get; set; } = string.Empty;
    public string EMP_NO          { get; set; } = string.Empty;
    public string EMP_NAME        { get; set; } = string.Empty;
    public string ROLE_NAME       { get; set; } = string.Empty;
    public string ROLE_DESC       { get; set; } = string.Empty;
    public string RESOURCE_NAME   { get; set; } = string.Empty;
    public string RESOURCE_DESC   { get; set; } = string.Empty;
    public bool   VALID_USER      { get; set; }
    public string LOGIN_ID        { get; set; } = string.Empty;
    public string PASSWORD        { get; set; } = string.Empty;
}
