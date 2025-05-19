using System;
using System.Collections.Generic;

public static class StringReplacer
{
    private static Dictionary<string, Func<string>> replacements = new Dictionary<string, Func<string>>
    {
        { "enemy_count", GetEnemyCount }
    };

    public static string ReplaceKeywords(string template)
    {
        foreach (var pair in replacements)
        {
            template = template.Replace($"{{{pair.Key}}}", pair.Value?.Invoke());
        }

        return template;
    }

    private static string GetEnemyCount()
    {
        return BattleData.Instance.LivingEnemies.Count.ToString();
    }
}