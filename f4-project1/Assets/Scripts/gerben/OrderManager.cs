using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI orderTemplate;
    public Transform orderListParent;

    [Header("Game Data")]
    public List<string> possibleOrders = new List<string> { "Brood", "Croissant", "Taartje" };

    private List<Order> currentOrders = new List<Order>();

    void Awake()
    {
        Instance = this;
    }

    public Order CreateNewOrder(string klantNaam)
    {
        // Kies random bestelling
        string bestelling = possibleOrders[Random.Range(0, possibleOrders.Count)];
        var newText = Instantiate(orderTemplate, orderListParent);
        newText.text = klantNaam + ": " + bestelling;
        newText.gameObject.SetActive(true);

        var order = new Order(klantNaam, bestelling);
        order.uiElement = newText;
        currentOrders.Add(order);
        return order;
    }
    public void RemoveOrder(Order order)
    {
        if (order == null) return;

        if (currentOrders.Contains(order))
            currentOrders.Remove(order);

        if (order.uiElement != null)
            Destroy(order.uiElement.gameObject);
    }

    public void CompleteOrder(Order order)
    {
        if (currentOrders.Contains(order))
        {
            currentOrders.Remove(order);

            // UI verwijderen
            if (order.uiElement != null)
                Destroy(order.uiElement.gameObject);

            // Score verhogen
            ScoreManager.Instance.AddScore(10);
        }
    }

    public Order GetOrderForNPC(string npcName)
    {
        return currentOrders.Find(o => o.npcName == npcName);
    }
    // void Start()
    // {
    //     CreateNewOrder("TestKlant");
    // }

}

[System.Serializable]
public class Order
{
    public string npcName;
    public string itemName;
    public TextMeshProUGUI uiElement;

    public Order(string npcName, string itemName)
    {
        this.npcName = npcName;
        this.itemName = itemName;
    }
}

