using Microsoft.AspNetCore.Mvc;
using PMG_s_Game_Repo.ViewModels;
using System.Net.Http.Json;

public class GamesController : Controller
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public GamesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _http = httpClientFactory.CreateClient();
        _http.BaseAddress = new Uri("https://api.rawg.io/api/");
        _apiKey = configuration["RawgApi:ApiKey"] ?? throw new InvalidOperationException("RAWG API key not configured");
    }

    public async Task<IActionResult> Index(string searchQuery, string selectedGenre, int? selectedPlatform, string sortOrder, int pageNumber = 1)
    {
        string url = $"games?key={_apiKey}&page={pageNumber}&page_size=12";

        if (!string.IsNullOrEmpty(searchQuery))
            url += $"&search={searchQuery}";
        if (!string.IsNullOrEmpty(selectedGenre))
            url += $"&genres={selectedGenre}";
        if (selectedPlatform.HasValue)
            url += $"&platforms={selectedPlatform.Value}";
        if (!string.IsNullOrEmpty(sortOrder))
            url += $"&ordering={(sortOrder == "rating" ? "-rating" : sortOrder)}";

        var response = await _http.GetFromJsonAsync<RawgApiResponseDto>(url);

        if (response == null || response.Results == null)
        {
            return View(new GameListViewModel
            {
                Games = new List<RawgGameDto>(),
                TotalGames = 0,
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
            });
        }
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

    private async Task<List<RawgGenreDto>> GetGenres()
    {
        var genres = await _http.GetFromJsonAsync<RawgGenreResponseDto>($"genres?key={_apiKey}");
        return genres?.Results ?? new List<RawgGenreDto>();
    }

    private async Task<List<RawgPlatformDto>> GetPlatforms()
    {
        var url = $"platforms?key={_apiKey}";
        var response = await _http.GetFromJsonAsync<RawgPlatformResponseDto>(url);

        return response?.Results ?? new List<RawgPlatformDto>();


    }

    public async Task<IActionResult> Details(int id)
    {
        var game = await _http.GetFromJsonAsync<RawgGameDetailsDto>($"games/{id}?key={_apiKey}");
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


