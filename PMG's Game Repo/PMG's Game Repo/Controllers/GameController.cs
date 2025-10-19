using Microsoft.AspNetCore.Mvc;
using PMG_s_Game_Repo.ViewModels;
using System.Net.Http.Json;

public class GamesController : Controller
{
    private readonly HttpClient _http;

    public GamesController(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient();
        _http.BaseAddress = new Uri("https://api.rawg.io/api/");
    }

    public async Task<IActionResult> Index(string searchQuery, string selectedGenre, string selectedPlatform, string sortOrder, int pageNumber = 1)
    {
        // build RAWG API request
        string apiKey = "YOUR_API_KEY"; // put in config later
        string url = $"games?key={apiKey}&page={pageNumber}&page_size=12";

        if (!string.IsNullOrEmpty(searchQuery))
            url += $"&search={searchQuery}";
        if (!string.IsNullOrEmpty(selectedGenre))
            url += $"&genres={selectedGenre}";
        if (!string.IsNullOrEmpty(selectedPlatform))
            url += $"&platforms={selectedPlatform}";
        if (!string.IsNullOrEmpty(sortOrder))
            url += $"&ordering={(sortOrder == "rating" ? "-rating" : sortOrder)}";

        var response = await _http.GetFromJsonAsync<RawgApiResponseDto>(url);

        var viewModel = new GameListViewModel
        {
            Games = response.Results,
            TotalGames = response.Count,
            Filters = new GameFilterViewModel
            {
                SearchQuery = searchQuery,
                SelectedGenre = selectedGenre,
                SelectedPlatform = selectedPlatform,
                SortOrder = sortOrder,
                PageNumber = pageNumber,
                PageSize = 12,
                Genres = await GetGenres(),
                Platforms = await GetPlatforms()
            }
        };

        return View(viewModel);
    }

    private async Task<List<string>> GetGenres()
    {
        var genres = await _http.GetFromJsonAsync<RawgGenreResponseDto>("genres?key=YOUR_API_KEY");
        return genres.Results.Select(g => g.Name).ToList();
    }

    private async Task<List<string>> GetPlatforms()
    {
        var platforms = await _http.GetFromJsonAsync<RawgPlatformResponseDto>("platforms?key=YOUR_API_KEY");
        return platforms.Results.Select(p => p.Platform.Name).ToList();
    }

    public async Task<IActionResult> Details(int id)
    {
        string apiKey = "YOUR_API_KEY"; // later move to config
        var game = await _http.GetFromJsonAsync<RawgGameDetailsDto>($"games/{id}?key={apiKey}");
        return View(game);
    }
}

public class RawgApiResponseDto
{
    public int Count { get; set; }
    public List<RawgGameDto> Results { get; set; }
}

public class RawgGenreResponseDto
{
    public List<RawgGenreDto> Results { get; set; }
}

public class RawgPlatformResponseDto
{
    public List<RawgPlatformDto> Results { get; set; }
}
