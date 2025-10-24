using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    [MaxLength(200)]
    public string? ProfileDescription { get; set; }

    [MaxLength(300)]
    public string? ProfilePictureUrl { get; set; } = "https://i.ibb.co/2Wj9WzN/default-avatar.png";

    public bool IsBanned { get; set; } = false;

    public bool IsAdmin { get; set; } = false;

    public ICollection<UserGame> Library { get; set; }
}