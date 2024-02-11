using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
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

    /** 마우스 위치에 따라 좌표를 반환한다 */
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

    /** 아이템을 특정 좌표에 배치한다 */
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        if(BoundryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.heigth) == false)
        {
            return false;
        }

        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for(int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for(int y = 0; y < inventoryItem.itemData.heigth; y++)
            {
                inventoryItemSlots[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        // 중심 계산
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.heigth / 2);

        // 위치 변경
        rectTransform.localPosition = position;

        return true;
    }


    /** 특정 좌표에 있는 아이템을 선택한다 */
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlots[x, y];

        if(toReturn == null) { return null; }

        for(int ix = 0; ix < toReturn.itemData.width; ix++)
        {
            for(int iy = 0; iy < toReturn.itemData.heigth; iy++)
            {
                inventoryItemSlots[toReturn.onGridPositionX + ix, toReturn.onGridPositionY + iy] = null;
            }
        }
        
        return toReturn;
    }

    private bool PositionCheck(int posX, int posY)
    {
        if(posX < 0 || posY < 0)
        {
            return false;
        }

        if(posX >= gridSizeWidth || posY >= gridSizeHeight) 
        {
            return false;
        }

        return true;
    }

    private bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if(PositionCheck(posX, posY) == false) { return false; }

        posX += width - 1;
        posY += height - 1;

        if(PositionCheck(posX, posY) == false) { return false; }
        return true;
    }
    #endregion // 함수
}
