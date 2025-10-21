
using UnityEngine;

public class NPCOrder : MonoBehaviour
{
    public string npcName;

    public void CreateOrderOnSpawn()
    {
        OrderManager.Instance.CreateNewOrder(npcName);
    }
}


