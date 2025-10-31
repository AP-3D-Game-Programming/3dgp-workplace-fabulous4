using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;
    public int maxNPCs = 5;

    private float spawnTimer = 0f;
    private int currentNPCCount = 0;
    public GameObject waypoint;
    public GameObject player;

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
            spawnTimer = 0f;
            SpawnNPC();
        }
    }

    void SpawnNPC()
    {
        // VOEG DEZE REGEL TOE
        Debug.Log($"SpawnNPC() is aangeroepen door '{this.gameObject.name}' op tijdstip {Time.time}", this.gameObject);

        if (npcPrefab == null || spawnPoint == null) return;

        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        NpcMovement npcControls = npc.GetComponent<NpcMovement>();
        npcControls.waypoint = waypoint;
        npcControls.player = player;
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
            movement.waypoint = waypoint;
            movement.player = player;
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
