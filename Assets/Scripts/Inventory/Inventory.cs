using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region 변수
    [SerializeField] private int gridSizeWidth = 20;
    [SerializeField] private int gridSizeHeight = 10;

    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    private InventoryItem[,] inventoryItemSlots;

    private RectTransform rectTransform;

    // 그리드 위치를 저장하는 변수
    private Vector2 positionOnTheGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    /** 인벤토리 타일 크기 설정 및 아이템 크기 이차원 배열 초기화 */
    private void Init(int width, int height)
    {
        inventoryItemSlots = new InventoryItem[width, height];

        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    /** 마우스 위치에 따라 타일의 좌표를 반환한다 */
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        // 마우스 위치를 UI 좌표로 변환하여 저장
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        // 타일의 그리드 위치 계산, 정수형으로 변환하여 저장
        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    /** 아이템을 특정 좌표에 배치한다 (마우스로 배치할때) */
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        // 아이템을 배치할 위치가 유효한지 검사한다
        if (BoundryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height) == false)
        {
            // 배치 실패
            return false;
        }

        // 오버랩이 발생했을 경우
        if (OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height, ref overlapItem) == false)
        {
            // 오버랩된 아이템을 비운다.
            overlapItem = null;
            return false;
        }

        // 오버랩된 아이템이 존재할 경우
        if (overlapItem != null)
        {
            // 인벤토리 그리드에서 해당 아이템을 제거합니다.
            CleanGridReference(overlapItem);
        }

        // 아이템을 특정 좌표에 배치한다
        PlaceItem(inventoryItem, posX, posY);

        // 배치 성공
        return true;
    }

    /** 아이템을 특정 좌표에 배치한다 (마우스 사용 X, 인벤토리에 배치될때) */
    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        // 아이템 RectTransform 가져오기
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();

        // 인벤토리에 배치
        rectTransform.SetParent(this.rectTransform);

        // 아이템 크기 만큼 위치한 좌표 더하기, 어디에 위치하는지 추적할때 사용
        for (int x = 0; x < inventoryItem.Width; x++)
        {
            for (int y = 0; y < inventoryItem.Height; y++)
            {
                inventoryItemSlots[posX + x, posY + y] = inventoryItem;
            }
        }

        // 아이템의 현재 그리드 위치 업데이트
        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        // 아이템이 위치할 그리드의 좌표를 받아서 중심을 계산한다
        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        // 위치 변경
        rectTransform.localPosition = position;
    }

    /** 아이템이 위치할 그리드의 좌표를 받아서 중심을 계산한다 */
    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        // 중심 계산
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.Width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.Height / 2);
        return position;
    }

    /** 특정 좌표에 있는 아이템을 선택한다 */
    public InventoryItem PickUpItem(int x, int y)
    {
        // 해당 좌표에 있는 아이템을 가져온다
        InventoryItem toReturn = inventoryItemSlots[x, y];

        // 아이템이 없을 경우 null 반환
        if (toReturn == null) { return null; }

        // 아이템 공간을 비운다
        CleanGridReference(toReturn);

        // 가져온 아이템 반환
        return toReturn;
    }

    /** 인벤토리 그리드에서 아이템이 차지하고 있는 공간을 비운다 */
    private void CleanGridReference(InventoryItem item)
    {
        // 가져온 아이템의 크기를 계산해 공간을 비운다
        for (int ix = 0; ix < item.Width; ix++)
        {
            for (int iy = 0; iy < item.Height; iy++)
            {
                inventoryItemSlots[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    /** 입력한 좌표가 그리드 내에 있는지 확인 */
    private bool PositionCheck(int posX, int posY)
    {
        // 좌표가 음수인 경우
        if(posX < 0 || posY < 0)
        {
            // X
            return false;
        }

        // 좌표가 그리드 크기를 초과하는 경우
        if(posX >= gridSizeWidth || posY >= gridSizeHeight) 
        {
            // X
            return false;
        }

        // 그 외 O
        return true;
    }

    /** 좌표와 크기에 대한 경계를 검사한다 */
    public bool BoundryCheck(int posX, int posY, int width, int height)
    {
        // 입력한 좌표가 그리드 내에 있는지 확인
        if(PositionCheck(posX, posY) == false) { return false; }

        // 아이템의 너비와 높이를 고려하여 좌표 계산
        posX += width - 1;
        posY += height - 1;

        // 입력한 좌표가 그리드 내에 있는지 확인
        if(PositionCheck(posX, posY) == false) { return false; }

        // 그 외 O
        return true;
    }

    /** 오버랩이 발생했는지 확인한다 */
    private bool OverlapCheck(int posX, int posY, int width, int heigth, ref InventoryItem overlapItem)
    {
        // 주어진 영역에 대해 반복하여 아이템이 있는지 확인
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < heigth; y++)
            {
                // 현재 위치에 아이템이 있을 경우
                if (inventoryItemSlots[posX + x, posY + y] != null)
                {
                    // overlapItem이 null 일 경우
                    if(overlapItem == null)
                    {
                        // 현재 아이템으로 설정
                        overlapItem = inventoryItemSlots[posX + x, posY + y];
                    }
                    else
                    {
                        // overlapItem이 이미 다른 아이템으로 설정되어 있을 경우
                        if(overlapItem != inventoryItemSlots[posX + x, posY + y])
                        {
                            // 오버랩 발생
                            return false;
                        }
                    }
                }
            }
        }

        // 오버랩이 발생하지 않았다
        return true;
    }

    /** 입력한 좌표에 있는 아이템을 가져온다 */
    public InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlots[x, y];
    }

    /** 주어진 위치에 아이템이 들어갈 공간이 있는지 확인한다 */
    private bool CheckAvailableSpace(int posX, int posY, int itemWidth, int itemHeigth)
    {
        // 주어진 영역에 대해 반복하여 아이템이 있는지 확인
        for (int x = 0; x < itemWidth; x++)
        {
            for (int y = 0; y < itemHeigth; y++)
            {
                // 인벤토리 슬롯 배열에서 해당 위치에 아이템이 존재할 경우
                if (inventoryItemSlots[posX + x, posY + y] != null)
                {
                    // 공간 사용 불가능
                    return false;
                }
            }
        }

        // 공간 사용 가능
        return true;
    }

    /** 아이템이 들어갈 공간이 있는지 확인하고 좌표를 반환한다 */
    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        // 인벤토리 크기와 아이템의 크기를 생각해 계산
        int height = gridSizeHeight - itemToInsert.Height + 1;
        int width = gridSizeWidth - itemToInsert.Width + 1;

        // 인벤토리에 아이템이 들어갈 시작 위치를 찾는다
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // 해당 위치에 아이템을 넣을 수 있는 공간이 있을 경우
                if(CheckAvailableSpace(x, y, itemToInsert.Width, itemToInsert.Height) == true)
                {
                    // 해당 위치를 반환
                    return new Vector2Int(x, y);
                }
            }
        }

        // 공간이 없을 경우 null 반환
        return null;
    }
    #endregion // 함수
}
