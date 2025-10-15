using System;
using System.Collections.Generic;

public class RawgGameDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Released { get; set; }
    public double Rating { get; set; }
    public string Background_Image { get; set; }
    public List<RawgGenreDto> Genres { get; set; }
    public List<RawgPlatformDto> Platforms { get; set; }
    public List<RawgScreenshotDto> Screenshots { get; set; }
    public List<RawgDeveloperDto> Developers { get; set; }
}
