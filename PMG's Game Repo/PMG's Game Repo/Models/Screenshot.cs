using System;

public class Screenshot
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
}
