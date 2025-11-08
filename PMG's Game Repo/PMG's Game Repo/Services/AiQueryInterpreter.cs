using System.Text.Json;
using Microsoft.Extensions.Logging;
using PMG_s_Game_Repo.Models;

namespace PMG_s_Game_Repo.Services
{
    // Wraps LlmClient -> deserialize -> validate whitelist
    public class AiQueryInterpreter
    {
        private readonly LlmClient _llm;
        private readonly ILogger<AiQueryInterpreter> _logger;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // allowed columns/operators — adjust if needed
        private static readonly HashSet<string> allowedCols = new(StringComparer.OrdinalIgnoreCase) { "Id", "RawgId", "Name", "Description", "Released", "Rating" };
        private static readonly HashSet<string> allowedOps = new(StringComparer.OrdinalIgnoreCase) { "equals", "contains", "starts_with", "ends_with" };

        public AiQueryInterpreter(LlmClient llm, ILogger<AiQueryInterpreter> logger)
        {
            _llm = llm;
            _logger = logger;
        }

        public async Task<(QueryPlan? Plan, string? RawJson, string? Error)> GetPlanFromNaturalQueryAsync(string userQuery)
        {
            try
            {
                var rawJson = await _llm.GetJsonPlanAsync(userQuery, Prompts.IntelligentSearchSystem);

                var plan = JsonSerializer.Deserialize<QueryPlan>(rawJson, _jsonOptions);
                if (plan == null) return (null, rawJson, "LLM returned invalid JSON.");

                // validate filters
                var validFilters = new List<QueryPlan.Filter>();
                foreach (var f in plan.Filters ?? Enumerable.Empty<QueryPlan.Filter>())
                {
                    if (string.IsNullOrWhiteSpace(f.Column) || !allowedCols.Contains(f.Column)) continue;
                    if (string.IsNullOrWhiteSpace(f.Op) || !allowedOps.Contains(f.Op)) continue;
                    validFilters.Add(f);
                }
                plan.Filters = validFilters;

                // validate order_by
                if (plan.Order_By != null)
                {
                    plan.Order_By = plan.Order_By.Where(o => !string.IsNullOrWhiteSpace(o.Column) && allowedCols.Contains(o.Column))
                                                 .ToList();
                }

                // limit/offset safety
                plan.Limit = Math.Clamp(plan.Limit ?? 50, 1, 200);
                plan.Offset = Math.Max(plan.Offset ?? 0, 0);

                // If after validation no filters remain — return error
                if (plan.Filters == null || plan.Filters.Count == 0)
                {
                    return (null, rawJson, "After validation the plan contains no valid filters.");
                }

                return (plan, rawJson, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI query failed");
                return (null, null, $"LLM error: {ex.Message}");
            }
        }
    }
}
