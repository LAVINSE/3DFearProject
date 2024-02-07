using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    #region 변수
    [SerializeField] private int gridSizeWidth = 20;
    [SerializeField] private int gridSizeHeight = 10;
    [SerializeField] private GameObject inventoryItemPrefab;

    private const float tileSizeWidth = 32;
    private const float tileSizeHeight = 32;

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

        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 3, 2);
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

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        inventoryItemSlots[posX, posY] = inventoryItem;

        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight / 2);

        rectTransform.localPosition = position;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlots[x, y];
        inventoryItemSlots[x, y] = null;

        return toReturn;
    }
    #endregion // 함수
}
