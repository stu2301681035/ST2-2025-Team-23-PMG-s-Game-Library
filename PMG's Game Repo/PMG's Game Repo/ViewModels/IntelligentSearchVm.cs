using PMG_s_Game_Repo.Models;

namespace PMG_s_Game_Repo.ViewModels
{
    public class IntelligentSearchVm
    {
        public string? NaturalQuery { get; set; }
        public string? JsonPlan { get; set; }
        public List<Game>? Results { get; set; }
        public string? Error { get; set; }
    }
}
