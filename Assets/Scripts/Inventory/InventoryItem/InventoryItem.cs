using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region 변수
    public ItemDataSO itemDataSO;

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotate = false;

    public Image image;
    #endregion // 변수

    #region 프로퍼티
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
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        image = GetComponent<Image>();

        
    }

    private void Start()
    {
        
    }

    /** 아이템 정보를 받아와 설정한다 */
    public void Set(ItemDataSO itemData)
    {
        this.itemDataSO = itemData;

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

    /** 클릭했을때 */
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            inventoryController.itemUseShowTextUI.gameObject.SetActive(true);
        }
    }

    /** 마우스를 올렸을때 */
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    /** 마우스가 나갔을때 */
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void ShowText(bool isOK)
    {
        // TODO : 아이템 사용
        // true일 경우 파괴

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
    #endregion // 함수
}
