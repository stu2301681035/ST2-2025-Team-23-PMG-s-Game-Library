using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PMG_s_Game_Repo.Data;
using PMG_s_Game_Repo.Models;
using System.Text;

namespace PMG_s_Game_Repo.Services
{
    public class GameRepository
    {
        private readonly ApplicationDbContext _ctx;
        public GameRepository(ApplicationDbContext ctx) => _ctx = ctx;

        private static string EscapeLike(string s)
        {
            if (s == null) return s ?? "";
            return s.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");
        }

        public List<Game> SelectGamesAdvanced(QueryPlan plan)
        {
            var allowedCols = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "Id", "RawgId", "Name", "Description", "Released", "Rating" };

            var allowedOps = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "equals", "contains", "starts_with", "ends_with" };

            var sql = new StringBuilder("SELECT * FROM Games");
            var where = new List<string>();
            var parameters = new List<SqlParameter>();
            int p = 0;

            foreach (var f in plan.Filters ?? Enumerable.Empty<QueryPlan.Filter>())
            {
                if (string.IsNullOrWhiteSpace(f.Column) || !allowedCols.Contains(f.Column)) continue;
                if (string.IsNullOrWhiteSpace(f.Op) || !allowedOps.Contains(f.Op)) continue;

                var paramName = "@p" + (p++);
                string expr;
                switch (f.Op.ToLowerInvariant())
                {
                    case "equals":
                        expr = $"{f.Column} = {paramName}";
                        parameters.Add(new SqlParameter(paramName, f.Value ?? ""));
                        break;
                    case "contains":
                        expr = $"{f.Column} LIKE {paramName} ESCAPE '\\'";
                        parameters.Add(new SqlParameter(paramName, $"%{EscapeLike(f.Value ?? "")}%"));
                        break;
                    case "starts_with":
                        expr = $"{f.Column} LIKE {paramName} ESCAPE '\\'";
                        parameters.Add(new SqlParameter(paramName, $"{EscapeLike(f.Value ?? "")}%"));
                        break;
                    case "ends_with":
                        expr = $"{f.Column} LIKE {paramName} ESCAPE '\\'";
                        parameters.Add(new SqlParameter(paramName, $"%{EscapeLike(f.Value ?? "")}"));
                        break;
                    default:
                        continue;
                }
                where.Add(expr);
            }

            if (where.Count > 0)
                sql.Append(" WHERE " + string.Join(" AND ", where));

            // ORDER BY
            if (plan.Order_By != null && plan.Order_By.Count > 0)
            {
                var orders = new List<string>();
                foreach (var ob in plan.Order_By)
                {
                    if (allowedCols.Contains(ob.Column))
                    {
                        var dir = (ob.Direction ?? "asc").Equals("desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
                        orders.Add(ob.Column.Equals("Id", StringComparison.OrdinalIgnoreCase) ? $"{ob.Column} {dir}" : $"{ob.Column} {dir}");
                    }
                }
                if (orders.Count > 0)
                    sql.Append(" ORDER BY " + string.Join(", ", orders));
            }
            else
            {
                sql.Append(" ORDER BY Name ASC, Id ASC");
            }

            int limit = Math.Clamp(plan.Limit ?? 50, 1, 200);
            int offset = Math.Max(plan.Offset ?? 0, 0);
            sql.Append($" OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY");

            return _ctx.Games.FromSqlRaw(sql.ToString(), parameters.ToArray()).AsNoTracking().ToList();
        }
    }
}
