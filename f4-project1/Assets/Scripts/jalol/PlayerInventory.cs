using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public bool HasItem(string itemId)
    {
        return items != null && items.Contains(itemId);
    }

    public void AddItem(string itemId)
    {
        Debug.Log("Test");
        if (items == null) items = new List<string>();
        Debug.Log($"[DEBUG] {itemId} added to inventory");
        items.Add(itemId);

    }

    public void RemoveItem(string itemId)
    {
        if (items == null) return;
        if (items.Remove(itemId))
        {
            var pickup = GetComponent<PickUpScript>();
            if (pickup != null)
            {
                pickup.RemoveItemFromHand(itemId);
            }
        }
    }

    [ContextMenu("AddTestItem")]
    void AddTestItemContext() => AddItem("loaf");
}