using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region ����
    public ItemDataSO itemDataSO;

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotate = false;
    #endregion // ����

    #region ������Ƽ
    public int Width
    {
        get
        {
            if(rotate == false)
            {
                return itemDataSO.width;
            }
            return itemDataSO.heigth;
        }
    }
    public int Height
    {
        get
        {
            if (rotate == false)
            {
                return itemDataSO.heigth;
            }
            return itemDataSO.width;
        }
    }
    #endregion // ������Ƽ

    #region �Լ�
    /** ������ ������ �޾ƿ� �����Ѵ� */
    public void Set(ItemDataSO itemData)
    {
        this.itemDataSO = itemData;

        this.GetComponent<Image>().sprite = itemData.itemIcon;

        // ������ ũ�� ���
        Vector2 size = new Vector2();
        size.x = itemData.width * Inventory.tileSizeWidth;
        size.y = itemData.heigth * Inventory.tileSizeHeight;

        this.GetComponent<RectTransform>().sizeDelta = size;
    }

    /** �������� ȸ�� ��Ų�� */
    public void Rotate()
    {
        rotate = !rotate;

        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotate == true ? 90f : 0f);
    }

    /** Ŭ�������� */
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            // TODO : ������ ���
            itemDataSO.itemUse();
            Destroy(this.gameObject);
        }
    }

    /** ���콺�� �÷����� */
    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    /** ���콺�� �������� */
    public void OnPointerExit(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
    #endregion // �Լ�
}
