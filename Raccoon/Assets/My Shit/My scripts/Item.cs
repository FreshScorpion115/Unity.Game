using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite itemSprite;
    public int amountStack = 1;
    public int maxAmountStack = 1;
    public GameObject gameOB;
    public bool isArmor;
    public bool droppedItem = false;

    public string equipmentType;
    public int equipmentIndex;
    public int itemID;
    public bool isItem;
    public int inHotBar;
}
