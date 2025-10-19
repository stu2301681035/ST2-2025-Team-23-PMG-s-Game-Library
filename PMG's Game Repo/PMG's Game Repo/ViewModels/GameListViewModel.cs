namespace PMG_s_Game_Repo.ViewModels
{
    public class GameListViewModel
    {
        public List<RawgGameDto> Games { get; set; }
        public GameFilterViewModel Filters { get; set; }
        public int TotalGames { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalGames / Filters.PageSize);
    }

}
