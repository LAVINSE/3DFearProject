using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region ����
    private InventoryController inventoryController;
    private ItemGrid itemGrid;
    #endregion // ����

    #region �Լ� 
    /** �ʱ�ȭ */
    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ��� �ö� */
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;   
    }

    /** ���콺 Ŀ���� �浹 ���� ������ ���� �� */
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }
    #endregion // �Լ�
}
