using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewProductIngredientData", menuName = "Product Data/Product Mapping")] 
public class ProductDataSO : ScriptableObject
{
    // Let op: Verwijst nu naar de externe klasse
    public List<ProductIngredientMapping> Mappings = new List<ProductIngredientMapping>();
}