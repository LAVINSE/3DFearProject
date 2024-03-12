using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Inventory))]
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler
{
    #region 변수
    [SerializeField] private bool isLock = true;
    [SerializeField] private GameObject lockInvenotoryImg;

    private InventoryController inventoryController;
    private Inventory selectedItemInventory;
    #endregion // 변수

    #region 함수 
    /** 초기화 */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        selectedItemInventory = GetComponent<Inventory>();

        // 인벤토리가 잠금상태가 아니고, 선택된 인벤토리가 없을 경우
        if (!isLock && inventoryController.SelectedInventory == null)
        {
            inventoryController.SelectedInventory = selectedItemInventory;
        }

        // 인벤토리가 잠금상태이고, 잠금 이미지가 있을 경우
        if(isLock && lockInvenotoryImg != null)
        {
            UnLockInventory(true);
        }
    }

    /** 마우스 커서가 충돌 영역 안으로 들어 올때 */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLock)
        {
            inventoryController.SelectedInventory = selectedItemInventory;
        }  
    }

    /** 인벤토리를 잠금해제한다 */
    public void UnLockInventory(bool isLock)
    {
        // 잠금 이미지가 없을 경우
        if(lockInvenotoryImg == null) { return; }

        this.isLock = isLock;
        lockInvenotoryImg.SetActive(isLock);
    }
    #endregion // 함수
}
