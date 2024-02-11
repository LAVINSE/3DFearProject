using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    #region ����
    [SerializeField] private int gridSizeWidth = 20;
    [SerializeField] private int gridSizeHeight = 10;

    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    private InventoryItem[,] inventoryItemSlots;

    private RectTransform rectTransform;

    // �׸��� ��ġ�� �����ϴ� ����
    private Vector2 positionOnTheGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();
    #endregion // ����

    #region �Լ�
    /** �ʱ�ȭ */
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    /** �κ��丮 Ÿ�� ũ�� ���� �� ������ ũ�� ������ �迭 �ʱ�ȭ */
    private void Init(int width, int height)
    {
        inventoryItemSlots = new InventoryItem[width, height];

        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    /** ���콺 ��ġ�� ���� ��ǥ�� ��ȯ�Ѵ� */
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        // ���콺 ��ġ�� UI ��ǥ�� ��ȯ�Ͽ� ����
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        // Ÿ���� �׸��� ��ġ ���, ���������� ��ȯ�Ͽ� ����
        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    /** �������� Ư�� ��ǥ�� ��ġ�Ѵ� */
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

        // �߽� ���
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.heigth / 2);

        // ��ġ ����
        rectTransform.localPosition = position;

        return true;
    }


    /** Ư�� ��ǥ�� �ִ� �������� �����Ѵ� */
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
    #endregion // �Լ�
}
