using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Inventory))]
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region ����
    private InventoryController inventoryController;
    private Inventory itemGrid;
    #endregion // ����

    #region �Լ� 
    /** �ʱ�ȭ */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<Inventory>();

        Debug.Log(" �κ��丮 ���� ");
        //inventoryController.SelectedInventory = itemGrid;
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ��� �ö� */
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedInventory = itemGrid;   
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ���� �� */
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedInventory = null;
    }
    #endregion // �Լ�
}
