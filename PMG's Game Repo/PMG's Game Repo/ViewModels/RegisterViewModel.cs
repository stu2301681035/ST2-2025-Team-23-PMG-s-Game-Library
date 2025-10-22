using System;
using System.ComponentModel.DataAnnotations;

namespace PMG_s_Game_Repo.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}