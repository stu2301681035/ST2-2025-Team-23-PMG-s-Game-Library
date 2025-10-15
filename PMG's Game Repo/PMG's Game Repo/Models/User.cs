using Microsoft.AspNetCore.Identity;
using System;

public class User : IdentityUser
{
    public ICollection<UserGame> Library { get; set; }
}