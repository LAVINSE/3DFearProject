using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlashLight : Item, ItemUseInterface
{
    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        ItemData.itemUse = ItemUse;
    }

    /** ������ ��� */
    public bool ItemUse()
    {
        PlayerAction.instance.FlashLightActive(true);
        return true;
    }
    #endregion // �Լ�
}
