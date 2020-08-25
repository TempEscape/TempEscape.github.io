using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public Transform itemsParent;

    PlayerInventory inventory;

    InventorySlot[] slots;

    private void Start()
    {
        inventory = PlayerInventory.instance;
        if(inventory != null)
        {
            inventory.onItemChangedCallback += UpdateUI;
        }
        else
        {
            Debug.LogWarning("there is no callback");
        }

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        //set as Player slots
        foreach (InventorySlot slot in slots)
        {
            slot.isPlayerSlot = true;
        }
        PlayerInventory.instance.AddDefault();
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        int i = 0;
        //add items to slots
        foreach (List<InventoryItem> itemList in inventory.items.Values)
        {
            if(i < slots.Length)
            {
                slots[i].AddItem(itemList);
            }
            i++;
        }
        //clear any remaining slots
        for(; i<slots.Length; i++)
        {
            slots[i].ClearSlot();
        }
    }
}
