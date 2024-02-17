using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Inventory))]
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region 변수
    private InventoryController inventoryController;
    private Inventory itemGrid;
    #endregion // 변수

    #region 함수 
    /** 초기화 */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<Inventory>();

        Debug.Log(" 인벤토리 참조 ");
        //inventoryController.SelectedInventory = itemGrid;
    }

    /** 마우스 커서가 충돌 영역 안으로 들어 올때 */
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedInventory = itemGrid;   
    }

    /** 마우스 커서가 충돌 영역 밖으로 나갈 때 */
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedInventory = null;
    }
    #endregion // 함수
}
