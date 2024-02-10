using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region ����
    [SerializeField] private List<ItemData> items;
    [HideInInspector] public ItemGrid selectedItemGrid;

    private InventoryItem selectedItem;
    private RectTransform selectedItemRectTransform;
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if(selectedItemGrid == null) { return; }

        // ���콺 ���� Ŭ���� �������
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void CreateRandomItem()
    {
        
    }

    /** ������ �������� �巡�� �Ѵ� */
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            selectedItemRectTransform.position = Input.mousePosition;
        }
    }

    /** ���� ���콺 ��ư�� ������ ���*/
    private void LeftMouseButtonPress()
    {
        // ��ǥ�� �����´�
        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    /** �������� �����Ѵ� */
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        // ��ǥ�� �ִ� �������� �����Ѵ�
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        // ������ �������� �������
        if (selectedItem != null)
        {
            selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    /** �������� ��ġ�Ѵ� */
    private void PlaceItem(Vector2Int tileGridPosition)
    {
        // ������ ��ǥ�� ���õ� �������� ��ġ�Ѵ�
        selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
        selectedItem = null;
    }
    #endregion // �Լ�
}
