using System;

public class RawgGameDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Released { get; set; }
    public double Rating { get; set; }
    public string Background_Image { get; set; }
    public List<RawgGenreDto> Genres { get; set; }
    public List<RawgPlatformDto> Platforms { get; set; }
}