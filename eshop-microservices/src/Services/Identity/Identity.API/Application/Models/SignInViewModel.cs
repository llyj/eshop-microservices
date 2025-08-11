namespace Identity.API.Application.Models;

public class SignInViewModel
{
    public string AccessToken { get; set; }

    public int ExpireAt { get; set; }

    public string RefreshToken { get; set; }
}