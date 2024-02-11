using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    #region ����
    public ItemData itemData;

    public int onGridPositionX;
    public int onGridPositionY;
    #endregion // ����

    #region �Լ�
    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.heigth * ItemGrid.tileSizeHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }
    #endregion // �Լ�
}
