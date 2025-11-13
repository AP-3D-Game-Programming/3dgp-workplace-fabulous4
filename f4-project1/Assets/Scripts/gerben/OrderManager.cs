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
        // Map display-naam naar interne itemId (gebruik voor inventory checks)
        string itemId = MapDisplayToItemId(bestelling);
        var newText = Instantiate(orderTemplate, orderListParent);
        newText.text = klantNaam + ": " + bestelling;
        newText.gameObject.SetActive(true);

        var order = new Order(klantNaam, bestelling);
        order.uiElement = newText;
        order.itemId = itemId;
        currentOrders.Add(order);
        return order;
    }
    
    string MapDisplayToItemId(string displayName)
    {
        // eenvoudige mapping; pas aan als je andere namen gebruikt
        switch (displayName)
        {
            case "Brood":
                return "loaf_prefab(Clone)";
            case "Croissant":
                return "croissant_prefab(Clone)";
            case "Taartje":
                return "cake_prefab(Clone)";
            default:
                return displayName.ToLower();
        }
    }
    public void RemoveOrder(Order order)
    {
        if (order == null) return;

        if (currentOrders.Contains(order))
            currentOrders.Remove(order);

        if (order.uiElement != null)
            Destroy(order.uiElement.gameObject);
    }

    public Order GetOrderForNPC(string npcName)
    {
        return currentOrders.Find(o => o.npcName == npcName);
    }
}

[System.Serializable]
public class Order
{
    public string npcName;
    public string itemName;
    public string itemId;
    public TextMeshProUGUI uiElement;

    public Order(string npcName, string itemName)
    {
        this.npcName = npcName;
        this.itemName = itemName;
    }
}

