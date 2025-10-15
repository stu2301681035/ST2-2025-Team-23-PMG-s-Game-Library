using System;

public class UserGame
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; }

    public bool IsFavorite { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime AddedAt { get; set; }
}