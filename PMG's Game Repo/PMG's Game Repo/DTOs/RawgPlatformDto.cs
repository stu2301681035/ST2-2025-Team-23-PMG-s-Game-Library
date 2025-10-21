using System.Collections.Generic;

public class RawgPlatformResponseDto
{
    public int Count { get; set; }
    public List<RawgPlatformDto> Results { get; set; }
}

public class RawgPlatformDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public int Games_Count { get; set; }
    public string Image_Background { get; set; }
    public List<RawgPlatformGameDto> Games { get; set; }
}

public class RawgPlatformGameDto
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public int Added { get; set; }
}

public class RawgGamePlatformWrapperDto
{
    public RawgPlatformDto Platform { get; set; }
    public string Released_At { get; set; }
    public object Requirements { get; set; }
}
