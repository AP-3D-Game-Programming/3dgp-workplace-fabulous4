using Unity.VisualScripting;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Ingredient prefab to spawn")]
    public GameObject prefabToSpawn;

    [Header("Options")]
    public float respawnDelay = 0.5f;
    public float rotationSpeed = 50f;
    public string spawnTag = "canPickUp";

    private BoxCollider boxCollider;
    private GameObject currentIngredient;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("IngredientSpawner needs a boxcollider on the same GameObject to work!");
            return;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError("No prefab assigned to spawn!");
            return;
        }

        SpawnIngredient();
    }

    void SpawnIngredient()
    {
        if (currentIngredient != null) return;

        Vector3 spawnPos = transform.TransformPoint(boxCollider.center);

        currentIngredient = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, transform);
        currentIngredient.tag = spawnTag;

        var notifier = currentIngredient.AddComponent<DestructionNotifier>();
        notifier.onDestroyed += OnIngredientDestroyed;

        var rotator = currentIngredient.AddComponent<YRotator>();
        rotator.rotationSpeed = rotationSpeed;
    }

    void OnIngredientDestroyed()
    {
        currentIngredient = null;
        Invoke(nameof(SpawnIngredient), respawnDelay);
    }
}