
using UnityEngine;

public class NPCOrder : MonoBehaviour
{
    public string npcName;
    private Order myOrder;

    public void CreateOrderOnSpawn()
    {
        if (OrderManager.Instance != null)
        myOrder = OrderManager.Instance.CreateNewOrder(npcName);
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



