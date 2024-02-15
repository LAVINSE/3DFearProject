using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class InventoryController : MonoBehaviour
{
    #region ����
    [SerializeField] private List<ItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvasTransform;

    private ItemGrid selectedItemGrid; // ������ �κ��丮 ����
    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
    private RectTransform selectedItemRectTransform;
    private InventoryHighlight inventoryHighlight;
    private InventoryItem itemToHighlight;
    private Vector2Int oldPosition;
    #endregion // ����

    #region ������Ƽ
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }
    #endregion // ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        inventoryHighlight = this.GetComponent<InventoryHighlight>();
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        // ������ ������ �巡��
        ItemIconDrag();

        // Q�� ������ ���
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // ������ ����
            CreateRandomItem();
        }

        // �κ��丮�� ���� ���
        if(selectedItemGrid == null)
        {
            // ������ ����ǥ�� ��Ȱ��ȭ
            inventoryHighlight.Show(false);
            return;
        }

        // ������ ���� ǥ�� ����
        HandleHighlight();

        // ���콺 ���� Ŭ���� �������
        if (Input.GetMouseButtonDown(0))
        {
            // ���� Ŭ���� ������ �۵�
            LeftMouseButtonPress();
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        selectedItemRectTransform = inventoryItem.GetComponent<RectTransform>();
        selectedItemRectTransform.SetParent(canvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    /** ������ ���� ǥ�ø� �����Ѵ� */
    private void HandleHighlight()
    {
        // ���콺 ��ġ�� ���� Ÿ�� ��ǥ�� �����´�
        Vector2Int positionOnGrid = GetTileGridPosition();

        // ���� ��ġ�� ���� ��ġ�� ���ٸ� ����
        if(oldPosition == positionOnGrid) { return; }

        // ���� ��ġ�� ���� ��ġ�� ������Ʈ
        oldPosition = positionOnGrid;

        // ���õ� �������� ���� ���
        if (selectedItem == null)
        {
            // ������ �κ��丮���� �Է��� ��ǥ�� �ִ� �������� �����´�
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            // �κ��丮���� �������� ã�Ҵٸ�
            if(itemToHighlight != null)
            {
                // ����ǥ�ø� Ȱ��ȭ �Ѵ�
                inventoryHighlight.Show(true);
                // ������ ũ�⸦ �����Ѵ�
                inventoryHighlight.SetSize(itemToHighlight);
                // ������ ��ġ�� �����Ѵ�
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            // �������� �� ã�Ҵٸ�
            else
            {
                // ����ǥ�ø� ��Ȱ��ȭ �Ѵ�
                inventoryHighlight.Show(false);
            }
        }
        // ���õ� �������� ���� ���
        else
        {
            // ���� ��ġ�� �������� ��ġ�� �� �ִ��� ���ο� ���� �κ��丮 ������ ǥ���Ѵ�
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y,
                    selectedItem.itemData.width, selectedItem.itemData.heigth));

            // ������ ũ�⸦ �����Ѵ�
            inventoryHighlight.SetSize(selectedItem);
            // ������ ũ�⸦ �����Ѵ�
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    /** ������ �������� �巡�� �Ѵ� */
    private void ItemIconDrag()
    {
        // ���õ� �������� ���� ���
        if (selectedItem != null)
        {
            // ���õ� �������� ��ġ�� ���콺 ��ġ��
            selectedItemRectTransform.position = Input.mousePosition;
        }
    }

    /** ���� ���콺 ��ư�� ������ ���*/
    private void LeftMouseButtonPress()
    {
        // ���콺 ��ġ�� ���� Ÿ�� ��ǥ�� �����´�
        Vector2Int tileGridPosition = GetTileGridPosition();

        // ���õ� �������� ���� ���
        if (selectedItem == null)
        {
            // �ش� ��ġ�� �������� �����Ѵ�
            PickUpItem(tileGridPosition);
        }
        // ���õ� �������� ���� ���
        else
        {
            // �ش� ��ġ�� ��ġ
            PlaceItem(tileGridPosition);
        }
    }

    /** ���콺 ��ġ�� ���� Ÿ�� ��ǥ�� �����´� */
    private Vector2Int GetTileGridPosition()
    {
        // ���콺 ��ġ
        Vector2 position = Input.mousePosition;

        // ���õ� �������� ���� ���
        if (selectedItem != null)
        {
            // ���콺 ��ġ�� �������� �߽����� ����
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.itemData.heigth - 1) * ItemGrid.tileSizeHeight / 2;
        }

        // Ÿ�� ��ǥ�� �����´�
        return selectedItemGrid.GetTileGridPosition(position);
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
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        // ������ ��ġ�� �������� ���
        if (complete)
        {
            // ���õ� ������ �ʱ�ȭ
            selectedItem = null;

            // �������� �������� ���� ���
            if(overlapItem != null)
            {
                // �������� �������� �������� ���������� ����
                selectedItem = overlapItem;
                // ������ ������ �ʱ�ȭ
                overlapItem = null;
                selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }
    #endregion // �Լ�
}
