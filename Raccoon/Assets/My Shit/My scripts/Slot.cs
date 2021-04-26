using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Item slotItem;

    Sprite defaultSprite;
    Text amountText;
    public GameObject itemHolder;
    public GameObject player;
    public int push = 5;
    float spawnDistance = 2f;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void CustomStart()
    {
        defaultSprite = GetComponent<Image>().sprite;
        amountText = transform.GetChild(0).GetComponent<Text>();

    }

    public void DropItem()
    {
        if (slotItem)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 playerDirection = player.transform.forward;
            Quaternion playerRotation = player.transform.rotation;
            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

            slotItem.transform.parent = null;
            slotItem.gameObject.SetActive(true);
            slotItem.transform.position = spawnPos;
            slotItem.GetComponent<Rigidbody>().AddForce(player.transform.forward * 500);
            slotItem.GetComponent<Item>().droppedItem = true;
            itemHolder.GetComponent<UnEquip>().Unequipitem();
        }
    }


    public void CheckForUpdate()
    {
        if (transform.childCount > 1)
        {
            slotItem = transform.GetChild(1).GetComponent<Item>();
            GetComponent<Image>().sprite = slotItem.itemSprite;
            if(slotItem.amountStack > 1)
            {
                amountText.text = slotItem.amountStack.ToString();
            }
        }
        else
        {
            slotItem = null;
            GetComponent<Image>().sprite = defaultSprite;
            amountText.text = "";
        }
    }
}
