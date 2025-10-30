using System;

public class Game
{
    public int Id { get; set; } 
    public int RawgId { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public string Released { get; set; }
    public double Rating { get; set; }
    public string BackgroundImage { get; set; }

    public ICollection<Genre> Genres { get; set; }
    public ICollection<Platform> Platforms { get; set; }
    public ICollection<Screenshot> Screenshots { get; set; }
}