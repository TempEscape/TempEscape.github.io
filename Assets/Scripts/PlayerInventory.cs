using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    #region Singleton

    public static PlayerInventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("there is already an instance of inventory system, buy you are trying to create another");
            return;
        }

        instance = this;
    }

    #endregion

    public InventoryItem[] defaultItems;

    public void AddDefault()
    {
        foreach(InventoryItem item in defaultItems)
        {
            instance.Add(item);
            item.AddedToPlayer();
        }
    }
}
