using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public GameObject counterImage;
    public TextMeshProUGUI counter;
    public Button removeBtn;

    public bool isPlayerSlot = false;

    InventoryItem item;

    public void AddItem(List<InventoryItem> itemList)
    {
        item = itemList[0];

        icon.sprite = item.icon;
        icon.enabled = true;
        removeBtn.interactable = true;

        //counter value = itemList.count
        counter.text = itemList.Count.ToString();
        if(itemList.Count <= 1)
        {
            //make sure to change this back to false
            counterImage.SetActive(false);
        }
        else
        {
            counterImage.SetActive(true);
        }
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeBtn.interactable = false;
        counterImage.SetActive(false);
    }

    public void OnRemoveButton()
    {
        if (isPlayerSlot)
        {
            PlayerInventory.instance.Remove(item);
        }
        //else
        //{
        //    ChestInventory.instance.Remove(item);
        //}
    }

    public void UsedItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }
}
