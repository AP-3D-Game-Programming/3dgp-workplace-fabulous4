using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPrefabMapping
{
    public string itemId;
    public GameObject prefab;
}

public class PickUpScript : MonoBehaviour
{
    public Transform holdPos;
    public Vector3 holdOffset = new Vector3(0.3f, 0, 0.2f);
    public List<ItemPrefabMapping> itemPrefabs = new List<ItemPrefabMapping>();
    
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private string currentItemId;
    public string CurrentItemId => currentItemId;


    void Update()
    {
        if (heldObj != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropObject();
            }
        }
    }

    public void SpawnItemInHand(string itemId)
    {
        if (holdPos == null)
        {
            Debug.LogError("holdPos is NULL!", this);
            return;
        }
        
        ItemPrefabMapping mapping = itemPrefabs.Find(m => m.itemId == itemId);
        if (mapping == null || mapping.prefab == null)
        {
            Debug.LogWarning($"No prefab mapping found for itemId: {itemId}");
            return;
        }
        
        if (heldObj != null)
        {
            Destroy(heldObj);
        }
        Debug.Log($"Trying to spawn {itemId} in hand");
        Debug.Log($"Spawn mapping: {mapping?.prefab?.name}");

        heldObj = Instantiate(mapping.prefab, holdPos.position, holdPos.rotation, holdPos);
        heldObj.transform.localPosition = holdOffset;
        heldObj.transform.localRotation = Quaternion.identity;
        currentItemId = itemId;
        
        heldObjRb = heldObj.GetComponent<Rigidbody>();
        if (heldObjRb != null)
        {
            heldObjRb.isKinematic = true;
            heldObjRb.useGravity = false;
        }
        
        Collider objCollider = heldObj.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = false;
        }
        
        ObjectInteractable interactable = heldObj.GetComponent<ObjectInteractable>();
        if (interactable != null)
        {
            Destroy(interactable);
        }
    }

    public void RemoveItemFromHand(string itemId)
    {
        if (heldObj != null && currentItemId == itemId)
        {
            Destroy(heldObj);
            heldObj = null;
            heldObjRb = null;
            currentItemId = null;
        }
    }

    void DropObject()
    {
        if (heldObj == null) return;
        
        Destroy(heldObj);
        // heldObj.transform.parent = null;
        // heldObjRb.useGravity = true;
        // heldObjRb.isKinematic = false;
        heldObj = null;
        heldObjRb = null;
        currentItemId = null;

    }

    void OnDrawGizmosSelected()
    {
        if (holdPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(holdPos.position + holdPos.TransformDirection(holdOffset), 0.15f);
        }
    }
}