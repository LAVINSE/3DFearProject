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
    [SerializeField] private Transform canvasTransform;

    private ItemGrid selectedItemGrid; // 아이템 인벤토리 변수
    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
    private RectTransform selectedItemRectTransform;
    private InventoryHighlight inventoryHighlight;
    private InventoryItem itemToHighlight;
    private Vector2Int oldPosition;
    #endregion // 변수

    #region 프로퍼티
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        inventoryHighlight = this.GetComponent<InventoryHighlight>();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 아이템 아이콘 드래그
        ItemIconDrag();

        // Q를 눌렀을 경우
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 아이템 생성
            CreateRandomItem();
        }

        // 인벤토리가 없을 경우
        if(selectedItemGrid == null)
        {
            // 아이템 강조표시 비활성화
            inventoryHighlight.Show(false);
            return;
        }

        // 아이템 강조 표시 동작
        HandleHighlight();

        // 마우스 왼쪽 클릭을 했을경우
        if (Input.GetMouseButtonDown(0))
        {
            // 왼쪽 클릭을 했을때 작동
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
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            // 인벤토리에서 아이템을 찾았다면
            if(itemToHighlight != null)
            {
                // 강조표시를 활성화 한다
                inventoryHighlight.Show(true);
                // 강조의 크기를 설정한다
                inventoryHighlight.SetSize(itemToHighlight);
                // 강조의 위치를 설정한다
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
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
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y,
                    selectedItem.itemData.width, selectedItem.itemData.heigth));

            // 강조의 크기를 설정한다
            inventoryHighlight.SetSize(selectedItem);
            // 강조의 크기를 설정한다
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
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
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.itemData.heigth - 1) * ItemGrid.tileSizeHeight / 2;
        }

        // 타일 좌표를 가져온다
        return selectedItemGrid.GetTileGridPosition(position);
    }

    /** 아이템을 선택한다 */
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        // 좌표에 있는 아이템을 선택한다
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

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
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

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
            }
        }
    }
    #endregion // 함수
}
