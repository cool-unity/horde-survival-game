using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private int maxSlots = 10; // Maximum number of inventory slots

    private List<Item> items = new List<Item>();

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        items.Add(item);
        Debug.Log($"Added {item.name} to inventory.");
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Removed {item.name} from inventory.");
        }
    }

    public void UseItem(Item item)
    {
        if (items.Contains(item))
        {
            item.Use();
            RemoveItem(item);
        }
    }

    public List<Item> GetItems()
    {
        return items;
    }
}

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public abstract void Use();
}
