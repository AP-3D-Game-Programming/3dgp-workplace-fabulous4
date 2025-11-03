
using UnityEngine;

public class NPCOrder : MonoBehaviour
{
    public string npcName;
    private Order myOrder;

    public void CreateOrderOnSpawn()
    {
        if (OrderManager.Instance != null)
        {
            myOrder = OrderManager.Instance.CreateNewOrder(npcName);

            // Stel requiredItem op NPCInteractable in op de interne itemId zodat
            // NPCInteractable.Interact() weet welk item geaccepteerd wordt
            var interactable = GetComponent<NPCInteractable>();
            if (interactable != null && myOrder != null)
            {
                interactable.requiredItem = myOrder.itemId;
            }
        }
    }
    public void DeleteOrderOnDespawn()
    {
        if (myOrder != null && OrderManager.Instance != null)
        {
            OrderManager.Instance.RemoveOrder(myOrder);
            myOrder = null;
        }
            
    }
}



