using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;
    public int maxNPCs = 5;

    private float spawnTimer = 0f;
    private int currentNPCCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNPCCount >= maxNPCs) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnNPC();
            spawnTimer = 0f;
        }
    }

    void SpawnNPC()
    {
        if (npcPrefab == null || spawnPoint == null) return;

        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        npc.tag = "NPC";
        currentNPCCount++;

        // Bestelling aanmaken voor deze NPC
        NPCOrder npcOrder = npc.GetComponent<NPCOrder>();
        if (npcOrder != null)
        {
            npcOrder.npcName = "Klant " + currentNPCCount;
            npcOrder.CreateOrderOnSpawn();
        }
        

        NpcMovement movement = npc.GetComponent<NpcMovement>();
        if (movement != null)
        {
            movement.spawnPoint = spawnPoint;
        }

        NPCInteractable interactable = npc.GetComponent<NPCInteractable>();
        if (interactable != null)
        {
            npc.GetComponent<NPCInteractable>().onDespawn += OnNPCDespawned;
        }
    }

    void OnNPCDespawned()
    {
        currentNPCCount--;
    }
}
