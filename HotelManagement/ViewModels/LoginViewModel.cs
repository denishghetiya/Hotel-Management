using System.ComponentModel.DataAnnotations;

namespace HotelManagement.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
    }
    public class RegisterUserViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IFormFile Image { get; set; }

    }
    public class ResetPasswordViewModel
    {
        public string? Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class UserInfoViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
    public class EditProfileViewModel
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public IFormFile? Image { get; set; }
        public string? ExistingImagePath { get; set; }
    }
}
