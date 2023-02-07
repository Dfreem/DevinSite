namespace DevinSite.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Username { get; set; } = default!;
        public string OldPassword { get; internal set; } = default!;
        public string NewPassword { get; internal set; } = default!;
    }
}