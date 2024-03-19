namespace be_project_swp.Core.Dtos.User;

public class ResetPasswordRequestModel
{
    public string Email { get; set; }
}

public class ResetPasswordModel
{
    public string Email { get; set; }
    public string Code { get; set; }
    public string NewPassword { get; set; }
}

