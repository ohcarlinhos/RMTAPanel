using Shared.User;
using System.ComponentModel.DataAnnotations;

namespace App.Pages.Users.Components.Util;

public class UpdatePasswordForm : UpdatePasswordDto
{
    [MinLength(8), MaxLength(48), Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = "";
}