using System.Linq;
using UnityEngine;
using TMPro;

public class PickUpV2 : MonoBehaviour
{
    private int scoreCounter = 0;
    public TextMeshProUGUI scoreText;
    private GameObject heldObject; // Track the currently held object

    void Start()
    {

    }
    private bool hold = false;
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.gameObject != gameObject) // skip jezelf
                {
                    Debug.Log($"[DEBUG] Collided with: {collider.gameObject.name}");
                    if (collider.CompareTag("canPickUp"))
                    {
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        BoxCollider box = collider.GetComponent<BoxCollider>();
                        PlayerInventory inv = gameObject.GetComponent<PlayerInventory>();
                        if (!hold)
                        {
                            if (rb != null)
                            {
                                Destroy(rb);
                                rb.isKinematic = true;
                                rb.useGravity = false;
                            }

                            collider.transform.SetParent(this.transform);
                            inv.AddItem(collider.gameObject.name);
                            heldObject = collider.gameObject; // Set the held object
                            hold = !hold;
                        }
                        else
                        {
                            var npc = colliderArray.FirstOrDefault(x => x.CompareTag("NPC"));
                            if (npc != null)
                            {
                                NPCInteractable npcInteract = npc.GetComponent<NPCInteractable>();
                                npcInteract.Interact();
                                Destroy(collider.gameObject);
                                scoreText.text = $"Score: {++scoreCounter}";

                            }
                            else
                            {
                                Rigidbody newRb = collider.gameObject.AddComponent<Rigidbody>();
                                collider.transform.SetParent(null);
                                newRb.isKinematic = false;
                                newRb.useGravity = true;
                            }
                            hold = !hold;
                        }
                    }
                }

            }
        }

        // Throw logic
        if (hold && Input.GetKeyDown(KeyCode.F) && heldObject != null)
        {
            // Detach from player
            heldObject.transform.SetParent(null);

            // Enable collider
            if (heldObject.TryGetComponent(out Collider col))
            {
                col.enabled = true;
            }

            // Add Rigidbody if not present
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = heldObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false;
            rb.useGravity = true;

            // Apply forward force
            float throwForce = 500f;
            rb.AddForce(transform.forward * throwForce);

            heldObject = null;
            hold = false;
        }
    }
}
