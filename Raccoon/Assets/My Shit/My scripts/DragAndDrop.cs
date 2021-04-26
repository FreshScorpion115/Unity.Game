using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    public Inventory inv;

    GameObject curSlot;
    Item curSlotItem;
    public Image followMouseImage;
    public GameObject itemHolder;
    public KeyCode itemDrop;


    private void Update()
    {
        followMouseImage.transform.position = Input.mousePosition;

        if (Input.GetKeyDown(itemDrop))
        {
            GameObject obj = GameObjectUnderMouse();
            if (obj)
                obj.GetComponent<Slot>().DropItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            curSlot = GameObjectUnderMouse();
            Debug.Log("raccoon");

        }
        else if (Input.GetMouseButton(0))
        {
            if (curSlot && curSlot.GetComponent<Slot>().slotItem)
            {
                followMouseImage.color = new Color(255, 255, 255, 255);
                followMouseImage.sprite = curSlot.GetComponent<Image>().sprite;

            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (curSlot && curSlot.GetComponent<Slot>().slotItem)
            {
                curSlotItem = curSlot.GetComponent<Slot>().slotItem;

                GameObject newObj = GameObjectUnderMouse();
                if (newObj && newObj != curSlot)
                {
                    if (newObj.GetComponent<EquipmentSlot>() && newObj.GetComponent<EquipmentSlot>().equipmentType != curSlotItem.equipmentType)
                        return;
                    if (newObj.GetComponent<Slot>().slotItem)
                    {
                        Item objectsItem = newObj.GetComponent<Slot>().slotItem;
                        if (objectsItem.itemID == curSlotItem.itemID && objectsItem.amountStack != objectsItem.maxAmountStack && !newObj.GetComponent<EquipmentSlot>())
                        {
                            Debug.Log("lqawhefoewh");
                            inv.AddItem(curSlotItem, objectsItem);
                        }
                        else
                        {
                            Debug.Log("ththththththt");
                            objectsItem.transform.parent = curSlot.transform;
                            curSlotItem.transform.parent = newObj.transform;

                        }

                    }
                    else
                    {
                        curSlotItem.transform.parent = newObj.transform;
                        Debug.Log("bitty");
                        if(itemHolder.transform.childCount == 2)
                        {
                            Debug.Log("you got this far");
                            itemHolder.GetComponent<UnEquip>().Unequipitem();
                        }

                    }

                }
            }
            foreach(Slot i in inv.equipSlot)
            {
                i.GetComponent<EquipmentSlot>().Equip();
            }
        }
            else
            {
                followMouseImage.sprite = null;
                followMouseImage.color = new Color(0, 0, 0, 0);
            }
    }
    

    GameObject GameObjectUnderMouse()
    {
        GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(eventData, results);

        foreach (RaycastResult i in results)
        {
            if (i.gameObject.GetComponent<Slot>())
            {
                return i.gameObject;

            }

        }
        return null;

    }

}