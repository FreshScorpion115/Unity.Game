using ECM.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject player;
    public GameObject objectInventory;
    public GameObject gunHolder;
    public KeyCode invKey;
    public bool isOpen = false;

    public Slot[] slots;

    public Slot[] equipSlot;

    public Slot[] Hotbarslots;

    private void Start()
    {
        foreach (Slot i in slots)
        {
            i.CustomStart();
        }
        foreach (Slot i in equipSlot)
        {
            i.CustomStart();
        }
    }

    void Update()
    {
         if (Input.GetKeyDown(invKey))
        {
            objectInventory.SetActive(!objectInventory.activeInHierarchy);
            player.GetComponent<MouseLook>().OpenIventory();
            gunHolder.SetActive(!gunHolder.activeInHierarchy);
            if(isOpen == false)
            {
                isOpen = true;
            }
            else
            {
                isOpen = false;
            }
        }

         foreach(Slot i in slots)
        {
            i.CheckForUpdate();
        }
        foreach (Slot i in equipSlot)
        {
            i.CheckForUpdate();
        }
    }

    public int GetItemAmount(int id)
    {
        int numb = 0;
        foreach(Slot i in slots)
        {
            if (i.slotItem)
            {
                Item z = i.slotItem;
                if(z.itemID == id)
                {
                    numb += z.amountStack;
                }
            }
        }
        return numb;
    }

    public void RemoveItemAmount(int id, int amountToRemove)
    {
        foreach(Slot i in slots)
        {
            if(amountToRemove <= 0)
            {
                return;
            }
            if (i.slotItem)
            {
                Item z = i.slotItem;
                if(z.itemID == id)
                {
                    int amountThatCanBeRemoved = z.amountStack;
                    if(amountThatCanBeRemoved <= amountToRemove)
                    {
                        Destroy(z.gameObject);
                        amountToRemove -= amountThatCanBeRemoved;
                    }
                    else
                    {
                        z.amountStack -= amountToRemove;
                        amountToRemove = 0;
                    }
                }
            }
        }
    }
    
    public void AddItem(Item itemToBeAdded, Item startingItem = null)
    {
        int amountStack = itemToBeAdded.amountStack;
        List<Item> stackableItems = new List<Item>();
        List<Slot> emptySlots = new List<Slot>();

        if (startingItem && startingItem.itemID == itemToBeAdded.itemID && startingItem.amountStack < startingItem.maxAmountStack)
            stackableItems.Add(startingItem);

            foreach(Slot i in slots) 
            {
                if (i.slotItem)
                {
                    Item z = i.slotItem;
                    if(z.itemID == itemToBeAdded.itemID && z.amountStack < z.maxAmountStack && z != startingItem)
                    {
                        stackableItems.Add(z);
                    }

                }
                else
                {
                    emptySlots.Add(i);
                }
            }
        foreach(Item i in stackableItems)
        {
            int amountThatCanBeAdded = i.maxAmountStack - i.amountStack;
            if(amountStack <= amountThatCanBeAdded)
            {
                i.amountStack += amountStack;
                Destroy(itemToBeAdded.gameObject);
                return;
            }
            else
            {
                i.amountStack = i.maxAmountStack;
                amountStack -= amountThatCanBeAdded;
            }
        }
        itemToBeAdded.amountStack = amountStack;
        if(emptySlots.Count > 0)
        {
            itemToBeAdded.transform.parent = emptySlots[0].transform;
            itemToBeAdded.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Item>())
        {
            Debug.Log(col);
            Debug.Log(col.gameObject.name);
            if (col.gameObject.CompareTag("Weapon"))
            {
                GunController gun = col.GetComponent<Item>().gameOB.GetComponent<GunController>();
                if (col.GetComponent<Item>().droppedItem == true)
                {
                    Debug.Log("israel gay");
                    AddItem(col.GetComponent<Item>());
                }
                else if ((col.GetComponent<Item>().droppedItem == false))
                {
                    Debug.Log("trevor gay");
                    gun.DeleteFile();
                    AddItem(col.GetComponent<Item>());
                }
                else
                {
                    Debug.Log("how are you here");
                }

            }
            AddItem(col.GetComponent<Item>());

        }
    }
}
