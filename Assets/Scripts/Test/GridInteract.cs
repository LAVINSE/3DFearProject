using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region ����
    private InventoryController inventoryController;
    private ItemGrid itemGird;
    #endregion // ����

    #region �Լ� 
    /** �ʱ�ȭ */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGird = GetComponent<ItemGrid>();
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ��� �ö� */
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = itemGird;   
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ���� �� */
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.selectedItemGrid = null;
    }
    #endregion // �Լ�
}
