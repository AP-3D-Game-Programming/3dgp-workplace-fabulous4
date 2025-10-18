using System;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        
        var inv = player.GetComponent<PlayerInventory>();
        if (inv == null) return;

        inv.AddItem("loaf");
    }
}
