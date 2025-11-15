using UnityEngine;

public class ovenBakeScript : MonoBehaviour
{
    public GameObject parentForProductSpawn;
    public RawCookedProductDataSO rawCookedProductData; //product mapping in de unity editor waar producten en ingredienten kunnen toegevoegd worden.


    private void OnTriggerStay(Collider other) //checkt of er een ingredient in de sphere collider van de oven zit, E klik voegt dit dan toe aan de huidige items in de mixer. 
    {

        string objectInSphereColliderName = other.name;
        foreach (RawCookedProductMapping mapping in rawCookedProductData.Mappings)
        {
            if (other.name.StartsWith(mapping.RawProductName) && Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log($"{objectInSphereColliderName} baked to cooked product");
                Destroy(other.gameObject);
                Vector3 localOffset = new Vector3(-0.5f, 1f, 1f);
                Vector3 worldSpawnPosition = parentForProductSpawn.transform.position + localOffset;
                GameObject newProduct = Instantiate(
                    mapping.CookedProductPrefab,
                    worldSpawnPosition, // Gebruik de berekende wereldpositie
                    Quaternion.identity,
                    parentForProductSpawn.transform // De parent is optioneel, maar kan behouden blijven als je dat wilt.
                );
                return;
            }
        }
    }
}

