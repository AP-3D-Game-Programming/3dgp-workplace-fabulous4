using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public bool HasItem(string itemId)
    {
        Debug.Log($"HasItem: {items != null && items.Contains(itemId)}");
        Debug.Log($"Items: {string.Join(", ", items)}, itemID: {itemId}");
        return items != null && items.Contains(itemId);
    }

    public void AddItem(string itemId)
    {
        Debug.Log("Test");
        if (items == null) items = new List<string>();
        Debug.Log($"Items: {string.Join(", ", items)}");
        Debug.Log($"[DEBUG] {itemId} added to inventory");
        items.Add(itemId);

    }

    public void RemoveItem(string itemId)
    {
        if (items == null) return;
        items.Remove(itemId);
    }
}