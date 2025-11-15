using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ovenBakeScript : MonoBehaviour
{

    public Image progressBarFillImage;
    public GameObject progressBarCanvas;
    private float currentBakeTime = 0f;
    public GameObject parentForProductSpawn;
    public RawCookedProductDataSO rawCookedProductData;
    private bool isBaking = false; 
    private GameObject currentRawProduct = null; 

    private void OnTriggerStay(Collider other)
    {
        if (isBaking) 
            return; 

        string objectInSphereColliderName = other.name;

        foreach (RawCookedProductMapping mapping in rawCookedProductData.Mappings)
        {
            if (other.name.StartsWith(mapping.RawProductName) && Input.GetKeyDown(KeyCode.T))
            {
                StartBaking(other.gameObject, mapping); 
                return;
            }
        }
    }
    
    public void StartBaking(GameObject rawProduct, RawCookedProductMapping mapping)
    {
        currentRawProduct = rawProduct;
        rawProduct.SetActive(false);
        isBaking = true;
        
        if (progressBarCanvas != null)
        {
            progressBarCanvas.SetActive(true);
        }

        if (progressBarFillImage != null)
        {
            progressBarFillImage.fillAmount = 1f;
        }

        Debug.Log($"Start bakken van {rawProduct.name}. Duur: {mapping.BakeTime} seconden.");

        StartCoroutine(BakeTimer(mapping));
    }

    IEnumerator BakeTimer(RawCookedProductMapping mapping)
    {

    float elapsedTime = 0f;
    float totalBakeTime = mapping.BakeTime;

    while (elapsedTime < totalBakeTime)
    {
        elapsedTime += Time.deltaTime;
        
        float progress = 1f - (elapsedTime / totalBakeTime);
        
        if (progressBarFillImage != null)
        {
            progressBarFillImage.fillAmount = progress;
        }

        yield return null;
    }
    
    if (progressBarCanvas != null)
    {
        progressBarCanvas.SetActive(false); 
    }


    Debug.Log($"Bakken voltooid! Spawnen van {mapping.CookedProductPrefab.name}");
    Destroy(currentRawProduct);
    Vector3 localOffset = new Vector3(0f, 1f, -1f);
    Vector3 worldSpawnPosition = parentForProductSpawn.transform.position + localOffset;
    Instantiate(
        mapping.CookedProductPrefab,
        worldSpawnPosition,
        Quaternion.identity,
        parentForProductSpawn.transform
    );

    currentRawProduct = null;
    isBaking = false;
    }
}