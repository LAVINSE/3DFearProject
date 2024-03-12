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

    public Image image;
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

    public InventoryController inventoryController { get; set; }
    #endregion // ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        image = GetComponent<Image>();

        
    }

    private void Start()
    {
        
    }

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
            inventoryController.itemUseShowTextUI.gameObject.SetActive(true);
        }
    }

    /** ���콺�� �÷����� */
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    /** ���콺�� �������� */
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void ShowText(bool isOK)
    {
        // TODO : ������ ���
        // true�� ��� �ı�

        if (isOK)
        {
            if (itemDataSO.itemUse())
            {
                Destroy(this.gameObject);
                inventoryController.itemUseShowTextUI.gameObject.SetActive(false);
            }
        }
        else
        {
            inventoryController.itemUseShowTextUI.gameObject.SetActive(false);
        }
    }
    #endregion // �Լ�
}
