using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OverBaked/RecipeDatabase", fileName = "RecipeDatabase")]
public class RecipeDatabaseSO : ScriptableObject
{
    public List<RecipeSO> recipes = new();

    // Vergelijk twee multisets (itemId -> count), volgorde-onafhankelijk
    public RecipeSO FindMatch(Dictionary<string, int> currentContents, string applianceTag = "Oven")
    {
        foreach (var r in recipes)
        {
            if (!string.IsNullOrEmpty(applianceTag) && r.applianceTag != applianceTag) continue;

            if (Matches(currentContents, r.inputs))
                return r;
        }
        return null;
    }

    private bool Matches(Dictionary<string, int> contents, List<ItemAmount> required)
    {
        // exact match: zelfde items met zelfde aantallen
        // (Desgewenst kan je >= toestaan; nu is het exact)
        if (contents.Count != required.Count) return false;

        foreach (var req in required)
        {
            if (!contents.TryGetValue(req.itemId, out int have)) return false;
            if (have != req.amount) return false;
        }
        return true;
    }
}
