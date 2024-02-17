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

    public bool rotate = false;
    #endregion // 변수

    #region 프로퍼티
    public int Width
    {
        get
        {
            if(rotate == false)
            {
                return itemData.width;
            }
            return itemData.heigth;
        }
    }

    public int Height
    {
        get
        {
            if (rotate == false)
            {
                return itemData.heigth;
            }
            return itemData.width;
        }
    }
    #endregion // 프로퍼티

    #region 함수
    /** 아이템 정보를 받아와 설정한다 */
    public void Set(ItemData itemData)
    {
        this.itemData = itemData;

        this.GetComponent<Image>().sprite = itemData.itemIcon;

        // 아이템 크기 계산
        Vector2 size = new Vector2();
        size.x = itemData.width * Inventory.tileSizeWidth;
        size.y = itemData.heigth * Inventory.tileSizeHeight;

        this.GetComponent<RectTransform>().sizeDelta = size;
    }

    /** 아이템을 회전 시킨다 */
    public void Rotate()
    {
        rotate = !rotate;

        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotate == true ? 90f : 0f);
    }
    #endregion // 함수
}
