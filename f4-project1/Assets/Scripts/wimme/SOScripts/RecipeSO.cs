using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OverBaked/Recipe", fileName = "NewRecipe")]
public class RecipeSO : ScriptableObject
{
    public List<ItemAmount> inputs = new();
   
    public string outputItemId;
    public int outputAmount = 1;

    
    public float cookTimeSeconds = 5f;

    public string applianceTag = "Oven";
}
