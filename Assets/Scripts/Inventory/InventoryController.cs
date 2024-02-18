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

    private Transform canvasTransform;
    private RectTransform selectedItemRectTransform;

    private PlayerKeyCode playerKeyCode;
    private Inventory selectedInventory; // ������ �κ��丮 ����
    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
    private InventoryItem itemToHighlight;
    private InventoryHighlight inventoryHighlight;
    
    private Vector2Int oldPosition;
    #endregion // ����

    #region ������Ƽ
    public Inventory SelectedInventory
    {
        get => selectedInventory;
        set
        {
            selectedInventory = value;
            inventoryHighlight.SetParent(value);
        }
    }
    #endregion // ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        playerKeyCode = this.GetComponentInParent<PlayerKeyCode>();
        inventoryHighlight = this.GetComponent<InventoryHighlight>();
        canvasTransform = GameObject.FindWithTag("Canvas").transform;
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        // ������ ������ �巡��
        ItemIconDrag();

        // �Է� Ű
        InputKey();

        // �κ��丮 ������ ���̶���Ʈ
        InventoryHighlight();

        // ���콺 ���� Ŭ���� �������
        if (Input.GetMouseButtonDown(0))
        {
            // ���� Ŭ���� ������ �۵�
            LeftMouseButtonPress();
        }
    } 

    /** �κ��丮�� �������� �߰��Ѵ� */
    public void AddItem(ItemData itemdata)
    {
        // �κ��丮�� ���� ���
        if(selectedInventory == null)
        {
            Debug.Log(" �κ��丮�� �����ϴ� ");
            return;
        }

        // �κ��丮 ������ ����
        CreateRandomItem(itemdata);

        InventoryItem addItem = selectedItem;
        selectedItem = null;

        // �κ��丮�� ������ �߰�
        InsertItem(addItem);
    }

    /** �Է� Ű */
    private void InputKey()
    {
        // Q�� ������ ���
        if (Input.GetKeyDown(playerKeyCode.testCreateItemKey))
        {
            // ������ ����
            CreateRandomItem();
        }

        if (Input.GetKeyDown(playerKeyCode.testAddItemKey))
        {
            // �κ��丮�� ������ �߰�
            InsertRandomItem();
        }

        // ������ ȸ�� Ű
        if (Input.GetKeyDown(playerKeyCode.RotateItemKeyCode))
        {
            // ������ ȸ��
            RotateItem();
        }
    }

    /** �κ��丮�� �߰��� �������� �����Ѵ� */
    private void CreateRandomItem(ItemData itemdata = null)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        selectedItemRectTransform = inventoryItem.GetComponent<RectTransform>();
        selectedItemRectTransform.SetParent(canvasTransform);
        selectedItemRectTransform.SetAsLastSibling();

        // TODO : �����ؾߵ�
        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
        //inventoryItem.Set(itemdata);
    }

    /** �κ��丮�� �������� �߰��ϰ�, ���õ� �������� �ʱ�ȭ �Ѵ� */
    private void InsertRandomItem()
    {
        // ���õ� �κ��丮�� ���ٸ� ����
        if (selectedInventory == null) { return; }

        // ���� ������ ���� ����
        CreateRandomItem();

        // ������ ���� ������ ����
        InventoryItem itemToInsert = selectedItem;

        // ���� ������ �ʱ�ȭ
        selectedItem = null;

        // �κ��丮�� ������ �߰�
        InsertItem(itemToInsert);
    }

    /** �κ��丮�� �������� �߰��Ѵ� */
    private void InsertItem(InventoryItem itemToInsert)
    {
        // �κ��丮�� �������� �� ��ġ�� ��ȯ�Ѵ�
        Vector2Int? posOnGrid = selectedInventory.FindSpaceForObject(itemToInsert);

        // �������� �� ��ġ�� ���� ��� ����
        if (posOnGrid == null) { return; }

        // ���õ� �κ��丮�� �� ��ġ�� ������ ��ġ
        selectedInventory.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    /** �κ��丮 ������ ���̶���Ʈ */
    private void InventoryHighlight()
    {
        // �κ��丮�� ���� ���
        if (selectedInventory == null)
        {
            // ������ ����ǥ�� ��Ȱ��ȭ
            inventoryHighlight.Show(false);
            return;
        }

        // ������ ���� ǥ�� ����
        HandleHighlight();
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
            itemToHighlight = selectedInventory.GetItem(positionOnGrid.x, positionOnGrid.y);

            // �κ��丮���� �������� ã�Ҵٸ�
            if(itemToHighlight != null)
            {
                // ����ǥ�ø� Ȱ��ȭ �Ѵ�
                inventoryHighlight.Show(true);
                // ������ ũ�⸦ �����Ѵ�
                inventoryHighlight.SetSize(itemToHighlight);
                // ������ ��ġ�� �����Ѵ�
                inventoryHighlight.SetPosition(selectedInventory, itemToHighlight);
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
            inventoryHighlight.Show(selectedInventory.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.Width,
                selectedItem.Height));

            // ������ ũ�⸦ �����Ѵ�
            inventoryHighlight.SetSize(selectedItem);
            // ������ ũ�⸦ �����Ѵ�
            inventoryHighlight.SetPosition(selectedInventory, selectedItem, positionOnGrid.x, positionOnGrid.y);
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
        if(selectedInventory == null) { return; }

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
            position.x -= (selectedItem.Width - 1) * Inventory.tileSizeWidth / 2;
            position.y += (selectedItem.Height - 1) * Inventory.tileSizeHeight / 2;
        }

        // Ÿ�� ��ǥ�� �����´�
        return selectedInventory.GetTileGridPosition(position);
    }

    /** �������� �����Ѵ� */
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        // ��ǥ�� �ִ� �������� �����Ѵ�
        selectedItem = selectedInventory.PickUpItem(tileGridPosition.x, tileGridPosition.y);

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
        bool complete = selectedInventory.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

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
                selectedItemRectTransform.SetAsLastSibling();
            }
        }
    }

    /** �������� ȸ�� ��Ų�� */
    private void RotateItem()
    {
        if(selectedItem == null) { return; }

        selectedItem.Rotate();
    }
#endregion // �Լ�
}
