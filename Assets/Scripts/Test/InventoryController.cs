using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<ItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvasTransform;
    [HideInInspector] public ItemGrid selectedItemGrid;

    private InventoryItem selectedItem;
    private InventoryItem overlapItem;
    private RectTransform selectedItemRectTransform;
    #endregion // 변수

    #region 함수
    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if(selectedItemGrid == null) { return; }

        // 마우스 왼쪽 클릭을 했을경우
        if (Input.GetMouseButtonDown(0))
        {
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
        // 마우스 위치
        Vector2 position = Input.mousePosition;

        // 선택된 아이템이 있을 경우
        if(selectedItem != null)
        {
            // 마우스 위치를 아이템의 중심으로 변경
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.itemData.heigth - 1) * ItemGrid.tileSizeHeight / 2;
        }

        // 타일 좌표를 가져온다
        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(position);

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
