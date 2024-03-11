using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBow : Item, ItemUseInterface
{
    private void Awake()
    {
        ItemData.itemUse = ItemUse;
    }

    public void ItemUse()
    {
        Debug.Log("이것은 활 입니다.");
    }
}
