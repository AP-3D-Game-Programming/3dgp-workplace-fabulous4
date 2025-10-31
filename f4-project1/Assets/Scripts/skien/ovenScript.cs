using System;using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ovenScript : MonoBehaviour
{
    public ProductDataSO productData; //product mapping in de unity editor waar producten en ingredienten kunnen toegevoegd worden.
    public GameObject parentForProductSpawn;
    List<string> currentOvenIngredients = new List<string>();
    List<string> allIngredients = new List<string>() { "butter", "milk", "water", "sugar", "egg", "flour" };

    private void OnTriggerStay(Collider other) //checkt of er een ingredient in de sphere collider van de mixer zit, E klik voegt dit dan toe aan de huidige items in de mixer. 
    {
        string objectInSphereColliderName = other.name;
        if (allIngredients.Contains(objectInSphereColliderName) && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"{objectInSphereColliderName} transferred to oven");
            currentOvenIngredients.Add(other.name);
            currentOvenIngredients.ForEach(ingredient => Debug.Log(ingredient));
            Destroy(other.gameObject);
            checkRecipesRequiredForProduct();
        }
    }
    private void checkRecipesRequiredForProduct()
    {
        List<ProductIngredientMapping> allProducts = productData.Mappings;
        foreach (ProductIngredientMapping productAndRecipe in allProducts)
        {
            if (CheckIfListsMatch(currentOvenIngredients, productAndRecipe.Ingredients) )
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Vector3 localSpawnPosition = new Vector3(-0.3f, 0.5f, 0f); 
                    GameObject newProduct = Instantiate(
                        productAndRecipe.ProductPrefab,
                        localSpawnPosition,
                        Quaternion.identity,
                        parentForProductSpawn.transform
                        );
                    currentOvenIngredients.Clear(); // leeg maken van de currentIngredients in oven List
                }
            }
        }
    }

    private bool CheckIfListsMatch(List<string> currentIngredients, List<string> neededIngredients)
    {
    if (currentIngredients == null || neededIngredients == null)
    {
        return false;
    }

    var sortedCurrent = currentIngredients.Select(n => n.ToLower()).OrderBy(n => n);
    var sortedNeeded = neededIngredients.Select(n => n.ToLower()).OrderBy(n => n);

    return sortedCurrent.SequenceEqual(sortedNeeded);

    }
    
}
