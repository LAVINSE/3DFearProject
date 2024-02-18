using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class InventoryController : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<ItemData> items;
    [SerializeField] private GameObject itemPrefab; 

    private Transform canvasTransform;
    private RectTransform selectedItemRectTransform;

    private PlayerKeyCode playerKeyCode;
    private Inventory selectedInventory; // 아이템 인벤토리 변수
    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
    private InventoryItem itemToHighlight;
    private InventoryHighlight inventoryHighlight;
    
    private Vector2Int oldPosition;
    #endregion // 변수

    #region 프로퍼티
    public Inventory SelectedInventory
    {
        get => selectedInventory;
        set
        {
            selectedInventory = value;
            inventoryHighlight.SetParent(value);
        }
    }
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        playerKeyCode = this.GetComponentInParent<PlayerKeyCode>();
        inventoryHighlight = this.GetComponent<InventoryHighlight>();
        canvasTransform = GameObject.FindWithTag("Canvas").transform;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 아이템 아이콘 드래그
        ItemIconDrag();

        // 입력 키
        InputKey();

        // 인벤토리 아이템 하이라이트
        InventoryHighlight();

        // 마우스 왼쪽 클릭을 했을경우
        if (Input.GetMouseButtonDown(0))
        {
            // 왼쪽 클릭을 했을때 작동
            LeftMouseButtonPress();
        }
    } 

    /** 인벤토리에 아이템을 추가한다 */
    public void AddItem(ItemData itemdata)
    {
        // 인벤토리가 없을 경우
        if(selectedInventory == null)
        {
            Debug.Log(" 인벤토리가 없습니다 ");
            return;
        }

        // 인벤토리 아이템 생성
        CreateRandomItem(itemdata);

        InventoryItem addItem = selectedItem;
        selectedItem = null;

        // 인벤토리에 아이템 추가
        InsertItem(addItem);
    }

    /** 입력 키 */
    private void InputKey()
    {
        // Q를 눌렀을 경우
        if (Input.GetKeyDown(playerKeyCode.testCreateItemKey))
        {
            // 아이템 생성
            CreateRandomItem();
        }

        if (Input.GetKeyDown(playerKeyCode.testAddItemKey))
        {
            // 인벤토리에 아이템 추가
            InsertRandomItem();
        }

        // 아이템 회전 키
        if (Input.GetKeyDown(playerKeyCode.RotateItemKeyCode))
        {
            // 아이템 회전
            RotateItem();
        }
    }

    /** 인벤토리에 추가할 아이템을 생성한다 */
    private void CreateRandomItem(ItemData itemdata = null)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        selectedItemRectTransform = inventoryItem.GetComponent<RectTransform>();
        selectedItemRectTransform.SetParent(canvasTransform);
        selectedItemRectTransform.SetAsLastSibling();

        // TODO : 수정해야됨
        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
        //inventoryItem.Set(itemdata);
    }

    /** 인벤토리에 아이템을 추가하고, 선택된 아이템을 초기화 한다 */
    private void InsertRandomItem()
    {
        // 선택된 인벤토리가 없다면 종료
        if (selectedInventory == null) { return; }

        // 선택 아이템 랜덤 생성
        CreateRandomItem();

        // 생성된 선택 아이템 저장
        InventoryItem itemToInsert = selectedItem;

        // 선택 아이템 초기화
        selectedItem = null;

        // 인벤토리에 아이템 추가
        InsertItem(itemToInsert);
    }

    /** 인벤토리에 아이템을 추가한다 */
    private void InsertItem(InventoryItem itemToInsert)
    {
        // 인벤토리에 아이템이 들어갈 위치를 반환한다
        Vector2Int? posOnGrid = selectedInventory.FindSpaceForObject(itemToInsert);

        // 아이템이 들어갈 위치가 없을 경우 종료
        if (posOnGrid == null) { return; }

        // 선택된 인벤토리에 들어갈 위치에 아이템 배치
        selectedInventory.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    /** 인벤토리 아이템 하이라이트 */
    private void InventoryHighlight()
    {
        // 인벤토리가 없을 경우
        if (selectedInventory == null)
        {
            // 아이템 강조표시 비활성화
            inventoryHighlight.Show(false);
            return;
        }

        // 아이템 강조 표시 동작
        HandleHighlight();
    }

    /** 아이템 강조 표시를 동작한다 */
    private void HandleHighlight()
    {
        // 마우스 위치에 따라 타일 좌표를 가져온다
        Vector2Int positionOnGrid = GetTileGridPosition();

        // 이전 위치와 현재 위치가 같다면 종료
        if(oldPosition == positionOnGrid) { return; }

        // 이전 위치를 현재 위치로 업데이트
        oldPosition = positionOnGrid;

        // 선택된 아이템이 없을 경우
        if (selectedItem == null)
        {
            // 아이템 인벤토리에서 입력한 좌표에 있는 아이템을 가져온다
            itemToHighlight = selectedInventory.GetItem(positionOnGrid.x, positionOnGrid.y);

            // 인벤토리에서 아이템을 찾았다면
            if(itemToHighlight != null)
            {
                // 강조표시를 활성화 한다
                inventoryHighlight.Show(true);
                // 강조의 크기를 설정한다
                inventoryHighlight.SetSize(itemToHighlight);
                // 강조의 위치를 설정한다
                inventoryHighlight.SetPosition(selectedInventory, itemToHighlight);
            }
            // 아이템을 못 찾았다면
            else
            {
                // 강조표시를 비활성화 한다
                inventoryHighlight.Show(false);
            }
        }
        // 선택된 아이템이 있을 경우
        else
        {
            // 현재 위치에 아이템을 배치할 수 있는지 여부에 따라 인벤토리 강조를 표시한다
            inventoryHighlight.Show(selectedInventory.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.Width,
                selectedItem.Height));

            // 강조의 크기를 설정한다
            inventoryHighlight.SetSize(selectedItem);
            // 강조의 크기를 설정한다
            inventoryHighlight.SetPosition(selectedInventory, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    /** 아이템 아이콘을 드래그 한다 */
    private void ItemIconDrag()
    {
        // 선택된 아이템이 있을 경우
        if (selectedItem != null)
        {
            // 선택된 아이템의 위치를 마우스 위치로
            selectedItemRectTransform.position = Input.mousePosition;
        }
    }

    /** 왼쪽 마우스 버튼을 눌렀을 경우*/
    private void LeftMouseButtonPress()
    {
        if(selectedInventory == null) { return; }

        // 마우스 위치에 따라 타일 좌표를 가져온다
        Vector2Int tileGridPosition = GetTileGridPosition();

        // 선택된 아이템이 없을 경우
        if (selectedItem == null)
        {
            // 해당 위치에 아이템을 선택한다
            PickUpItem(tileGridPosition);
        }
        // 선택된 아이템이 있을 경우
        else
        {
            // 해당 위치에 배치
            PlaceItem(tileGridPosition);
        }
    }

    /** 마우스 위치에 따라 타일 좌표를 가져온다 */
    private Vector2Int GetTileGridPosition()
    {
        // 마우스 위치
        Vector2 position = Input.mousePosition;

        // 선택된 아이템이 있을 경우
        if (selectedItem != null)
        {
            // 마우스 위치를 아이템의 중심으로 변경
            position.x -= (selectedItem.Width - 1) * Inventory.tileSizeWidth / 2;
            position.y += (selectedItem.Height - 1) * Inventory.tileSizeHeight / 2;
        }

        // 타일 좌표를 가져온다
        return selectedInventory.GetTileGridPosition(position);
    }

    /** 아이템을 선택한다 */
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        // 좌표에 있는 아이템을 선택한다
        selectedItem = selectedInventory.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        // 선택한 아이템이 있을경우
        if (selectedItem != null)
        {
            selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    /** 아이템을 배치한다 */
    private void PlaceItem(Vector2Int tileGridPosition)
    {
        // 가져온 좌표에 선택된 아이템을 배치한다
        bool complete = selectedInventory.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        // 아이템 배치에 성공했을 경우
        if (complete)
        {
            // 선택된 아이템 초기화
            selectedItem = null;

            // 오버랩된 아이템이 있을 경우
            if(overlapItem != null)
            {
                // 오버랩된 아이템을 선택중인 아이템으로 변경
                selectedItem = overlapItem;
                // 오버랩 아이템 초기화
                overlapItem = null;
                selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
                selectedItemRectTransform.SetAsLastSibling();
            }
        }
    }

    /** 아이템을 회전 시킨다 */
    private void RotateItem()
    {
        if(selectedItem == null) { return; }

        selectedItem.Rotate();
    }
#endregion // 함수
}
