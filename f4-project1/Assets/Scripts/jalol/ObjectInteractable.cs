using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{
    public string itemId = "loaf";
    
    public void Interact()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        
        var inv = player.GetComponent<PlayerInventory>();
        if (inv == null) return;

        inv.AddItem(itemId);
        
        var pickup = player.GetComponent<PickUpScript>();
        if (pickup != null)
        {
            pickup.SpawnItemInHand(itemId);
        }
        
        Destroy(gameObject);
    }
}