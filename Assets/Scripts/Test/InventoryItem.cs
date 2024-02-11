using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    #region 변수
    public ItemData itemData;

    public int onGridPositionX;
    public int onGridPositionY;
    #endregion // 변수

    #region 함수
    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        this.GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.heigth * ItemGrid.tileSizeHeight;

        this.GetComponent<RectTransform>().sizeDelta = size;
    }
    #endregion // 함수
}
