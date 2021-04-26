using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HotbarSlots : MonoBehaviour
{
    public KeyCode key;
    public GameObject itemHolder;
    public GameObject objectInHand;
    private GameObject emptyHand;
    public Inventory inv;
    public bool hasItemInHand;
    private bool updated;
    public KeyCode itemDropKey;
    public int hotBarSlotID;

    private void Start()
    {
        emptyHand = objectInHand;
       
    }
    void Update()
    {
        if(updated == true)
        {
            UnEquip();
            updated = false;
        }
        if (Input.GetKeyDown(key))
        {
            // for when not holding an item + you press a slot with an item
            if(itemHolder.transform.childCount <= 2)
            {
                if (itemHolder.transform.childCount == 1 && transform.childCount == 2)
                {
                    Debug.Log("1");
                    Equip();
                }
                else if (transform.childCount == 2 && itemHolder.transform.GetChild(1).GetComponent<Item>().itemID == transform.GetChild(1).GetComponent<Item>().itemID)
                {
                    Debug.Log("3");
                    UnEquip();
                }
                //for when slot has an item + 
                else if (transform.childCount == 2)
                {
                    if (transform.GetChild(1).GetComponent<Item>().isArmor == true)
                    {
                        Debug.Log("kyle");
                    }
                    Debug.Log("2");
                    UnEquip();

                    Equip();
                }
                //for whne hand is empty
                else if (itemHolder.transform.childCount > 1)
                {
                    Debug.Log("4");
                    UnEquip();
                }
            }
            else
            {
                Debug.Log("tomanyitemsbitchass");
                return;
            }
        }
        if(Input.GetKeyDown(itemDropKey) && inv.isOpen == false && hasItemInHand == false)
        {
            DropHandItem();
            UnEquip();
        }
    }
    void Equip()
    {
            Item item = transform.GetChild(1).GetComponent<Item>();
            hasItemInHand = true;
            item.inHotBar = hotBarSlotID;
        Debug.Log(item.inHotBar + "id");
            if (item != objectInHand)
            {
                objectInHand = Instantiate(item.GetComponent<Item>().gameOB, itemHolder.transform);
                if (objectInHand.CompareTag("Weapon"))
                {
                Debug.Log("ffff");
                objectInHand.GetComponent<GunController>().Load();
                }
            }
            else if (objectInHand == item)
            {
                Debug.Log("bitch");
                UnEquip();
            }


    }
    public void UnEquip()
    {
        if (itemHolder.transform.childCount == 2)
        {
            Debug.Log("slit");
            objectInHand = itemHolder.transform.GetChild(1).gameObject;
            if (objectInHand.CompareTag("Weapon"))
            {
                objectInHand.GetComponent<GunController>().SaveInfo();
            }
            Destroy(objectInHand);
            objectInHand = emptyHand;
            hasItemInHand = false;
        }
    }

    public void DropHandItem()
    {
        if (itemHolder.transform.childCount == 2)
        {

            objectInHand = itemHolder.transform.GetChild(1).gameObject;
            if (objectInHand.CompareTag("Weapon"))
            {
                objectInHand.GetComponent<GunController>().SaveInfo();
            }
            Debug.Log("penis");
            Item slotItem1 = objectInHand.GetComponent<Item>();
            GameObject slotItem = transform.GetChild(1).GetComponent<Item>().gameOB;
            Vector3 playerPos = itemHolder.transform.position;
                Vector3 playerDirection = itemHolder.transform.forward;
                Quaternion playerRotation = itemHolder.transform.rotation;
                Vector3 spawnPos = playerPos + playerDirection * 2;

                slotItem.transform.parent = null;
                slotItem.gameObject.SetActive(true);
                slotItem.transform.position = spawnPos;
                slotItem.GetComponent<Rigidbody>().AddForce(itemHolder.transform.forward * 500);
                slotItem.GetComponent<Item>().droppedItem = true;
                itemHolder.GetComponent<UnEquip>().Unequipitem();
            }
    }

    public void UpdateItem(bool bitch)
    {
        updated = bitch;
        return;
    }
}
