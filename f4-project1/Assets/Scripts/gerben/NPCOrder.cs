using UnityEngine;

// public class NPCOrder : MonoBehaviour
// {
//     public string npcName;
//     private Order currentOrder;

//     // void Start()
//     // {
//     //     currentOrder = OrderManager.Instance.CreateNewOrder(npcName);
//     // }
// }
public class NPCOrder : MonoBehaviour
{
    public string npcName;
    private Order currentOrder;

    public void CreateOrderOnSpawn()
    {
        currentOrder = OrderManager.Instance.CreateNewOrder(npcName);
    }

    public void RemoveOrder()
    {
        if (currentOrder != null)
        {
            OrderManager.Instance.RemoveOrder(currentOrder);
            currentOrder = null;
        }
    }
}


