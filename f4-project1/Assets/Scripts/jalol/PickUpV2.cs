using System.Linq;
using UnityEngine;
using TMPro;

public class PickUpV2 : MonoBehaviour
{
    public float interactRange = 2f;
    public TextMeshProUGUI scoreText;
    
    [Header("Hold Settings")]
    public Transform holdParent; // Sleep hier de 'arm-right' transform naartoe
    public Vector3 holdOffset;   // Gebruik dit om de positie in de hand te finetunen

    // Maak de array public om hem in de Inspector te zien
    public Collider[] colliderArray;

    [Header("Throw Settings")]
    public float throwForce = 6f;
    public float throwUpwardForce = 2f;

    private int scoreCounter = 0;
    private GameObject heldObject = null; // Houdt bij welk object we vasthouden

    void Start()
    {
        // Zoek de arm automatisch als deze niet is toegewezen
        if (holdParent == null)
        {
            holdParent = transform.Find("root/torso/arm-right");
            if (holdParent == null)
            {
                Debug.LogError("Kon 'arm-right' niet automatisch vinden. Wijs de 'holdParent' handmatig toe in de Inspector.", this);
            }
        }
    }

    // Hernoemd naar Update zodat Unity de methode aanroept
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Als we niks vasthouden, probeer iets op te pakken
            if (heldObject == null)
            {
                TryPickUp();
            }
            // Als we wel iets vasthouden, probeer het weg te geven of te laten vallen
            else
            {
                TryDropOrGive();
            }
        }

        // Throw logic: als je iets vast hebt en op F drukt, gooi het weg
        if (Input.GetKeyDown(KeyCode.F) && heldObject != null)
        {
            ThrowHeldObject();
        }
    }

    void TryPickUp()
    {
        // Wijs de resultaten toe aan de public array
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        
        // Gebruik FirstOrDefault om het eerste opraapbare item te vinden
        Collider itemCollider = colliderArray.FirstOrDefault(c => c.CompareTag("canPickUp"));

        // Als er een item is gevonden
        if (itemCollider != null)
        {
            if (holdParent == null)
            {
                Debug.LogError("Kan item niet oppakken: 'holdParent' is niet ingesteld!", this);
                return;
            }

            heldObject = itemCollider.gameObject;

            // Zet de Rigidbody "uit" door hem kinematic te maken
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // *** NIEUW: Zet de collider van het item uit om wegvliegen te voorkomen ***
            Collider heldObjectCollider = heldObject.GetComponent<Collider>();
            if (heldObjectCollider != null)
            {
                heldObjectCollider.enabled = false;
            }

            // Maak het object een child van de rechterarm
            heldObject.transform.SetParent(holdParent);
            // Reset de positie en rotatie relatief aan de arm
            heldObject.transform.localPosition = holdOffset;
            heldObject.transform.localRotation = Quaternion.identity;
            
            PlayerInventory inv = GetComponent<PlayerInventory>();
            if (inv != null) inv.AddItem(heldObject.name);

            Debug.Log($"Opgepakt: {heldObject.name}");
            Debug.Log($"Inventory: {string.Join(", ", inv.items)}");
        }
    }

    void TryDropOrGive()
    {
        // Wijs de resultaten toe aan de public array
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        
        // Gebruik FirstOrDefault om een NPC te vinden
        Collider npcCollider = colliderArray.FirstOrDefault(c => c.CompareTag("NPC"));
        PlayerInventory inv = GetComponent<PlayerInventory>();

        // Als er een NPC is, geef het item
        if (npcCollider != null)
        {
            NPCInteractable npcInteract = npcCollider.GetComponent<NPCInteractable>();
            if (npcInteract != null)
            {
                npcInteract.Interact();
                Destroy(heldObject); // Vernietig het object dat we vasthielden

                if (scoreText != null) scoreText.text = $"Score: {++scoreCounter}";
                if (inv != null)
                inv.RemoveItem(heldObject.name);
                Debug.Log($"Item: {heldObject.name} gegeven aan {npcCollider.name}");
            }
        }
        // Als er geen NPC is, laat het item vallen
        else
        {
            heldObject.transform.SetParent(null);

            // *** NIEUW: Zet de collider weer aan VOORDAT je physics activeert ***
            Collider heldObjectCollider = heldObject.GetComponent<Collider>();
            if (heldObjectCollider != null)
            {
                heldObjectCollider.enabled = true;
            }

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = heldObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false; // Zet physics weer aan

            if (inv != null)
            inv.RemoveItem(heldObject.name);
            
            Debug.Log($"Laten vallen: {heldObject.name}");
        }

        // We houden nu niks meer vast
        heldObject = null;
    }

    void ThrowHeldObject()
    {
        if (heldObject == null) return;

        // Maak los en zet collider aan
        heldObject.transform.SetParent(null);
        Collider heldObjectCollider = heldObject.GetComponent<Collider>();
        if (heldObjectCollider != null)
        {
            heldObjectCollider.enabled = true;
        }

        // Rigidbody toevoegen of hergebruiken
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = heldObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
        rb.useGravity = true;

        // Kleine offset zodat het niet in de speler/hand blijft steken
        heldObject.transform.position = holdParent != null
            ? holdParent.position + transform.forward * 0.5f
            : transform.position + transform.forward * 0.5f;

        // Pas de kracht aan naar smaak (impulse)
        Vector3 throwVector = transform.forward * throwForce + Vector3.up * throwUpwardForce;
        rb.AddForce(throwVector, ForceMode.Impulse);

        Debug.Log($"Gegooid: {heldObject.name} met kracht {throwVector}");

        heldObject = null;
    }
}
