using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Inventory))]
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region ����
    private InventoryController inventoryController;
    private Inventory selectedItemInventory;
    private Inventory baseItemInventory;
    #endregion // ����

    #region �Լ� 
    /** �ʱ�ȭ */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        selectedItemInventory = GetComponent<Inventory>();
        baseItemInventory = GetComponent<Inventory>();

        Debug.Log(" �κ��丮 ���� ");
        inventoryController.SelectedInventory = baseItemInventory;
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ��� �ö� */
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedInventory = selectedItemInventory;   
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ���� �� */
    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO : �����ؾߵ�
        //inventoryController.SelectedInventory = null;
        inventoryController.SelectedInventory = baseItemInventory;
    }
    #endregion // �Լ�
}
