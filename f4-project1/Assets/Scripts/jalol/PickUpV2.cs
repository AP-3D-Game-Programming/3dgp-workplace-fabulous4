using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PickUpV2 : MonoBehaviour
{
    private int scoreCounter = 0;
    public TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    // Update is called once per frame
    public bool hold = false;
    void Update()
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
    }
}
