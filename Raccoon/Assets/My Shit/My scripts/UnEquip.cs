using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnEquip : MonoBehaviour
{
    public Inventory inv;
    public void Unequipitem()
    {
        if(transform.childCount == 2)
        {
            Debug.Log("you did something right");                
            foreach (Slot i in inv.Hotbarslots)
                {
                    if (transform.GetChild(1).CompareTag("Weapon"))
                    {
                        transform.GetChild(1).GetComponent<GunController>().SaveInfo();
                    }
                    i.GetComponent<HotbarSlots>().UnEquip();
                }
        }
    }
}
