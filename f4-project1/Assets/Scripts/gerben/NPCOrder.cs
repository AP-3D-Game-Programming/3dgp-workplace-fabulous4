
using UnityEngine;

public class NPCOrder : MonoBehaviour
{
    public string npcName;
    private Order myOrder;

    public void CreateOrderOnSpawn()
    {
        OrderManager.Instance.CreateNewOrder(npcName);
    }
    public void DeleteOrderOnDespawn()
    {
        if (myOrder != null)
            OrderManager.Instance.RemoveOrder(myOrder);
    }
}


