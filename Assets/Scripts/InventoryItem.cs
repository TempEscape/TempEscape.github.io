using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName =  "New Item", menuName = "Inventory/Item/DefaultItem")]
public class InventoryItem : ScriptableObject
{
    public bool stackable;
    public int maxCount;
    private int stackID;

    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void AddedToPlayer()
    {
        //call this when item is added to player inventory
    }

    //Use this item
    public virtual void Use()
    {
        //this is to check chest, probably don't need it
        //if (ChestInventory.instance.Chest != null)
        //{
        //    inChest = ChestInventory.instance.Chest.chestIsOpen;
        //}
        //else
        //{
        //    inChest = false;
        //}

        //use the item
        //if the player is in a chest (this can also probably be deleted)
        //if (inChest)
        //{
        //    //if item is in chest, move to inventory
        //    if (isInChest())
        //    {
        //        //TODO: check that items aren't removed when other inventory is full

        //        //remove item from one inventory and add to the other
        //        Item removedItem = ChestInventory.instance.Remove(this);
        //        PlayerInventory.instance.Add(removedItem);
        //    }
        //    else if (isInPlayer())   //if item is in inventory, move to chest
        //    {
        //        Item removedItem = PlayerInventory.instance.Remove(this);
        //        ChestInventory.instance.Add(removedItem);
        //    }
        //}else

        //Note: this is virtual so it is okay
        Debug.Log("Using " + name);

    }

    public void RemoveFromInventory()
    {
        PlayerInventory.instance.Remove(this);
    }


    //check if item is in chest (this is likely unneccesary)
    //public bool isInChest()
    //{
    //    //set items and itemNames to Chest Inventory values
    //    Dictionary<int, List<Item>> items = new Dictionary<int, List<Item>>();
    //    Dictionary<string, List<Item>> itemNames = new Dictionary<string, List<Item>>();

    //    items = ChestInventory.instance.items;
    //    itemNames = ChestInventory.instance.itemNames;

    //    //check that an item of this type exists in chest
    //    if (itemNames.ContainsKey(name))
    //    {
    //        //search stacks for stacks of this item type
    //        foreach (List<Item> itemList in items.Values)
    //        {
    //            Item firstItem = itemList[0];

    //            //search stacks for this specific item
    //            if (firstItem.name == name)
    //            {
    //                foreach (Item item in itemList)
    //                {
    //                    if (item == this)
    //                    {
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    //check if item is in Player inventory
    public bool isInPlayer()
    {
        //set items and itemNames to Chest Inventory values
        Dictionary<int, List<InventoryItem>> items = new Dictionary<int, List<InventoryItem>>();
        Dictionary<string, List<InventoryItem>> itemNames = new Dictionary<string, List<InventoryItem>>();

        items = PlayerInventory.instance.items;
        itemNames = PlayerInventory.instance.itemNames;

        //check that an item of this type exists in chest
        if (itemNames.ContainsKey(name))
        {
            //search stacks for stacks of this item type
            foreach (List<InventoryItem> itemList in items.Values)
            {
                InventoryItem firstItem = itemList[0];

                //search stacks for this specific item
                if (firstItem.name == name)
                {
                    foreach (InventoryItem item in itemList)
                    {
                        if (item == this)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    //determine whether or not the item is stackable
    public bool IsStackable(Inventory inventory)
    {
        //TODO: try to make this more efficient
        //check each stakc of the same type of item

        Dictionary<int, List<InventoryItem>> items = new Dictionary<int, List<InventoryItem>>();
        Dictionary<string, List<InventoryItem>> itemNames = new Dictionary<string, List<InventoryItem>>();

        //check if instance is check inventory or player inventory (this can probably be deleted)
        //if (inventory == ChestInventory.instance)
        //{
        //    items = ChestInventory.instance.items;
        //    itemNames = ChestInventory.instance.itemNames;
        //    //Debug.Log("chest inventory");
        //}
        //else
        //{
        //    //Debug.Log("player inventory");
        //    items = PlayerInventory.instance.items;
        //    itemNames = PlayerInventory.instance.itemNames;
        //}

        //check that the inventory conatains item of this type
        if (itemNames.ContainsKey(name))
        {
            //check each stack
            foreach(List<InventoryItem> itemList in inventory.items.Values)
            {
                InventoryItem firstItem = itemList[0];

                //check stacks that are of this item type
                if(firstItem.name == name)
                {
                    //check to see if there is room in this stack
                    if(items[firstItem.GetInstanceID()].Count < maxCount)
                    {
                        stackID = firstItem.GetInstanceID();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    #region DebugDictionary
    private void PrintItems(Dictionary<int, List<Item>> items)
    {
        string output = "";
        foreach (List<Item> itemList in items.Values)
        {
            output += itemList[0].name + ": ";
            foreach (Item item in itemList)
            {
                output += item.GetInstanceID() + ", ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }


    private void PrintKeys(Dictionary<int, List<Item>> items)
    {
        string output = "";
        foreach (int key in items.Keys)
        {
            output += key + ", ";
        }
        Debug.Log(output);
    }

    #endregion

    public int StackID
    {
        get
        {
            return stackID;
        }
    }

}
