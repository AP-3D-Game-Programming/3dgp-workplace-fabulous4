using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // eenvoudige inventaris: vul via code of inspector in editor (lijst van string ID's)
    public List<string> items = new List<string>();

    public bool HasItem(string itemId)
    {
        return items != null && items.Contains(itemId);
    }

    // helper om in Play mode items te geven
    public void AddItem(string itemId)
    {
        if (items == null) items = new List<string>();
        int before = items.Count;
        if (!items.Contains(itemId))
        {
            items.Add(itemId);
            Debug.Log($"PlayerInventory.AddItem: added '{itemId}' to {gameObject.name}. before={before} after={items.Count} contents={string.Join(", ", items)}", this);
        }
        else
        {
            Debug.Log($"PlayerInventory.AddItem: already has '{itemId}' on {gameObject.name}. count={items.Count}", this);
        }
    }

    public void RemoveItem(string itemId)
    {
        if (items == null) return;
        if (items.Remove(itemId))
            Debug.Log($"PlayerInventory.RemoveItem: removed '{itemId}' from {gameObject.name}. Count={items.Count}", this);
        else
            Debug.Log($"PlayerInventory.RemoveItem: '{itemId}' not found on {gameObject.name}", this);
    }

    // handige test-knop in Inspector (kies AddTestItem in context menu)
    [ContextMenu("AddTestItem")]
    void AddTestItemContext() => AddItem("loaf");
}
