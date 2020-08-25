using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pickup : MonoBehaviour
{

    public float PickupRadius;
    public InventoryItem inventoryItem;
    public GameObject itemPickupPrompt;

    CircleCollider2D _coll;
    bool _inRange;
    PlayerInventory _inventory;

    // Start is called before the first frame update
    void Start()
    {
        _coll = GetComponent<CircleCollider2D>();
        _coll.radius = PickupRadius;
        _inventory = PlayerInventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inRange)
        {
            Debug.Log("_inRange = true");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Pickup();
            }
        }
    }

    void HidePromt()
    {
        itemPickupPrompt.SetActive(false);
    }

    void ShowPrompt()
    {
        itemPickupPrompt.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //player is in range
            _inRange = true;

            //show prompt
            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //player is out of range
            _inRange = false;

            //hide prompt
            HidePromt();
        }
    }


    void Pickup()
    {

        //add the inventory item to the players inventory
        if (_inventory.Add(inventoryItem))
        {
            //hide the item because it has been added to the inventory successfully
            gameObject.SetActive(false);
        }
        else
        {
            //don't pickup the item, because the inventory is full
            Debug.Log("Inventory is full");
        }
    }
}
