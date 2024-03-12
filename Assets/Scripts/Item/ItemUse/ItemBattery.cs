using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBattery : Item, ItemUseInterface
{
    #region
    /** 초기화 */
    private void Awake()
    {
        ItemData.itemUse = ItemUse;
    }

    /** 아이템 사용 */
    public bool ItemUse()
    {
        
        return true;
    }
    #endregion
}
