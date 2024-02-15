using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region 변수
    private InventoryController inventoryController;
    private ItemGrid itemGrid;
    #endregion // 변수

    #region 함수 
    /** 초기화 */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }

    /** 마우스 커서가 충돌 영역 안으로 들어 올때 */
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;   
    }

    /** 마우스 커서가 충돌 영역 밖으로 나갈 때 */
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }
    #endregion // 함수
}
