using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlashLight : Item, ItemUseInterface
{
    #region 함수
    /** 초기화 */
    private void Awake()
    {
        ItemData.itemUse = ItemUse;
    }

    /** 아이템 사용 */
    public bool ItemUse()
    {
        PlayerAction.instance.FlashLightActive(true);
        return true;
    }
    #endregion // 함수
}
