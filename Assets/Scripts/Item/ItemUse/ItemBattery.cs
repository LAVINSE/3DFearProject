using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBattery : Item, ItemUseInterface
{
    #region
    /** �ʱ�ȭ */
    private void Awake()
    {
        ItemData.itemUse = ItemUse;
    }

    /** ������ ��� */
    public bool ItemUse()
    {
        
        return true;
    }
    #endregion
}
