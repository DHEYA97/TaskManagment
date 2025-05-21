using TaskManagment.Core.Abstractions.Const;
using System.ComponentModel.DataAnnotations;

namespace TaskManagment.Core.ViewModel.User
{
    public class ResetPasswordFormViewModel
    {
        public string Id { get; set; }
        [DataType(DataType.Password),
            StringLength(100, ErrorMessage = Errors.MaxMinLength, MinimumLength = 8), Required,
            RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
        public string? Password { get; set; } = null!;

        [DataType(DataType.Password), Display(Name = "Confirm password"),
            Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch), Required]
        public string? ConfirmPassword { get; set; } = null!;
    }
}
