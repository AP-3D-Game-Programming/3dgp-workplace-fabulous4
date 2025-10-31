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
        bool ingredientFound = false;
        foreach (string ingredient in allIngredients)
        {
            if (other.name.StartsWith(ingredient))
            {
                ingredientFound = true;
            }
        }
        if (ingredientFound && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log($"{objectInSphereColliderName} transferred to mixer");
            int indexOfSpace = other.name.IndexOf("(");
            string correctIngredientName = other.name.Substring(0, indexOfSpace);
            Debug.Log($"correct name: {correctIngredientName}");
            currentOvenIngredients.Add(correctIngredientName);
            Debug.Log($"Ingredient added name: {correctIngredientName}");
            Destroy(other.gameObject);
        }
        checkRecipesRequiredForProduct();
    }
    private void checkRecipesRequiredForProduct()
    {
        Debug.Log("checkRecipesRequiredForProduct reached");
        List<ProductIngredientMapping> allProducts = productData.Mappings;
        foreach (ProductIngredientMapping product in allProducts)
        {

            if (product.Ingredients.All(needed => currentOvenIngredients.Contains(needed)))
            {
                Debug.Log("Ingredients match found for product: " + product.ProductName);
                Vector3 localOffset = new Vector3(-0.5f, 1f, 1f);
                Vector3 worldSpawnPosition = parentForProductSpawn.transform.position + localOffset;
                    GameObject newProduct = Instantiate(
                    product.ProductPrefab,
                    worldSpawnPosition, // Gebruik de berekende wereldpositie
                    Quaternion.identity,
                    parentForProductSpawn.transform // De parent is optioneel, maar kan behouden blijven als je dat wilt.
                );
                    currentOvenIngredients.Clear(); // leeg maken van de currentIngredients in oven List
                    return;
                }

            }
        }
    }

    /*private bool CheckIfListsMatch(List<string> currentIngredients, List<string> neededIngredients)
    {
        if (currentIngredients == null || neededIngredients == null)
        {
            return false;
        }

        var sortedCurrent = currentIngredients.Select(n => n.ToLower()).OrderBy(n => n);
        var sortedNeeded = neededIngredients.Select(n => n.ToLower()).OrderBy(n => n);

        return sortedCurrent.SequenceEqual(sortedNeeded);
    
        return neededIngredients.All(needed => currentIngredients.Contains(needed));
        
    }
    */
    
