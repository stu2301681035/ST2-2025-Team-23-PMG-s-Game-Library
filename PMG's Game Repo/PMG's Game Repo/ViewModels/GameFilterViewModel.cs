namespace PMG_s_Game_Repo.ViewModels
{
    public class GameFilterViewModel
    {
        public string SearchQuery { get; set; }
        public string SelectedGenre { get; set; }
        public int? SelectedPlatform { get; set; }
        public string SortOrder { get; set; } // "name", "rating", "released"
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;

        public List<RawgGenreDto>? Genres { get; set; }
        public List<RawgPlatformDto>? Platforms { get; set; }
    }

}
