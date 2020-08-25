using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public Dictionary<int, List<InventoryItem>> items;
    public Dictionary<string, List<InventoryItem>> itemNames;

    public Inventory()
    {
        //Instantiate everything
        items = new Dictionary<int, List<InventoryItem>>();
        itemNames = new Dictionary<string, List<InventoryItem>>();
    }

    public bool Add(InventoryItem item)
    {
        if (!item.isDefaultItem)
        {
            //if inventory contains this type of itme and the item is stackable (stackable checks that there is space)
            if(itemNames.ContainsKey(item.name) && item.IsStackable(this))
            {
                //add item to stack
                items[item.StackID].Add(item);

                //update UI
                if(onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }
            }
            //else if there is space add new stack for this item
            else if (items.Count < space)
            {
                //create nfew stack for this item
                List<InventoryItem> list = new List<InventoryItem>();
                list.Add(item);
                items.Add(item.GetInstanceID(), list);

                //if there is not already a stack of this item, add to types in inventory
                if (!itemNames.ContainsKey(item.name))
                {
                    itemNames.Add(item.name, list);
                }

                //update UI
                if(onItemChangedCallback != null)
                {
                    onItemChangedCallback.Invoke();
                }
            }
            //if item can't be added, inventory is full, so do nothing
            else
            {
                Debug.Log("Inventory full, cannot add item.");
                return false;
            }
        }
        return true;
    }

    //Remove item from inventory
    public InventoryItem Remove(InventoryItem item)
    {
        int iD;

        //get id of stack
        if (item.IsStackable(this))
        {
            iD = item.StackID;
        }
        else
        {
            iD = item.GetInstanceID();
        }

        //remove last element of stack and store it to be returned
        InventoryItem removedItem = items[iD][items[iD].Count - 1];
        items[iD].RemoveAt(items[iD].Count - 1);

        //if stack is empty remove it
        if(items[iD].Count == 0)
        {
            items.Remove(iD);
            //if there are no more items of this name, remove name
            if (!ContainsDuplicate(item))
            {
                Debug.Log("Removed item name from items");
                itemNames.Remove(item.name);
            }
        }

        //update UI
        if(onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        //return removed item
        return removedItem;
    }

    //check if there is a duplicate of this item (ie. two stacks of the same item type)
    public bool ContainsDuplicate(InventoryItem item)
    {
        foreach (List<InventoryItem> itemList in items.Values)
        {
            if(itemList[0].name == item.name && item.StackID != itemList[0].GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    //clear inventory
    public void Clear()
    {
        //clear items and names
        items.Clear();
        itemNames.Clear();

        //Update UI
        if(onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

}
