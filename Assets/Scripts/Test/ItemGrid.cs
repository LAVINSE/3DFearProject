using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        // �������� ��ġ�� ��ġ�� ��ȿ���� �˻��Ѵ�
        if(BoundryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.heigth) == false)
        {
            // ��ġ ����
            return false;
        }

        // �������� �߻����� ���
        if(OverlapCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.heigth, ref overlapItem) == false)
        {
            // �������� �������� ����.
            overlapItem = null;
            return false;
        }

        // �������� �������� ������ ���
        if(overlapItem != null)
        {
            // �κ��丮 �׸��忡�� �ش� �������� �����մϴ�.
            CleanGridReference(overlapItem);
        }

        // ������ RectTransform ��������
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();

        // �κ��丮�� ��ġ
        rectTransform.SetParent(this.rectTransform);

        // ������ ũ�� ��ŭ ��ġ�� ��ǥ ���ϱ�, ��� ��ġ�ϴ��� �����Ҷ� ���
        for(int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for(int y = 0; y < inventoryItem.itemData.heigth; y++)
            {
                inventoryItemSlots[posX + x, posY + y] = inventoryItem;
            }
        }

        // �������� ���� �׸��� ��ġ ������Ʈ
        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        // �߽� ���
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.heigth / 2);

        // ��ġ ����
        rectTransform.localPosition = position;

        // ��ġ ����
        return true;
    }


    /** Ư�� ��ǥ�� �ִ� �������� �����Ѵ� */
    public InventoryItem PickUpItem(int x, int y)
    {
        // �ش� ��ǥ�� �ִ� �������� �����´�
        InventoryItem toReturn = inventoryItemSlots[x, y];

        // �������� ���� ��� null ��ȯ
        if (toReturn == null) { return null; }

        // ������ ������ ����
        CleanGridReference(toReturn);

        // ������ ������ ��ȯ
        return toReturn;
    }

    /** �κ��丮 �׸��忡�� �������� �����ϰ� �ִ� ������ ���� */
    private void CleanGridReference(InventoryItem item)
    {
        // ������ �������� ũ�⸦ ����� ������ ����
        for (int ix = 0; ix < item.itemData.width; ix++)
        {
            for (int iy = 0; iy < item.itemData.heigth; iy++)
            {
                inventoryItemSlots[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    /** �Է��� ��ǥ�� �׸��� ���� �ִ��� Ȯ�� */
    private bool PositionCheck(int posX, int posY)
    {
        // ��ǥ�� ������ ���
        if(posX < 0 || posY < 0)
        {
            // X
            return false;
        }

        // ��ǥ�� �׸��� ũ�⸦ �ʰ��ϴ� ���
        if(posX >= gridSizeWidth || posY >= gridSizeHeight) 
        {
            // X
            return false;
        }

        // �� �� O
        return true;
    }

    /** ��ǥ�� ũ�⿡ ���� ��踦 �˻��Ѵ� */
    private bool BoundryCheck(int posX, int posY, int width, int height)
    {
        // �Է��� ��ǥ�� �׸��� ���� �ִ��� Ȯ��
        if(PositionCheck(posX, posY) == false) { return false; }

        // �������� �ʺ�� ���̸� ����Ͽ� ��ǥ ���
        posX += width - 1;
        posY += height - 1;

        // �Է��� ��ǥ�� �׸��� ���� �ִ��� Ȯ��
        if(PositionCheck(posX, posY) == false) { return false; }

        // �� �� O
        return true;
    }

    /** �������� �߻��ߴ��� Ȯ���Ѵ� */
    private bool OverlapCheck(int posX, int posY, int width, int heigth, ref InventoryItem overlapItem)
    {
        // �־��� ������ ���� �ݺ��Ͽ� �������� �ִ��� Ȯ��
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < heigth; y++)
            {
                // ���� ��ġ�� �������� ���� ���
                if (inventoryItemSlots[posX + x, posY + y] != null)
                {
                    // overlapItem�� null �� ���
                    if(overlapItem == null)
                    {
                        // ���� ���������� ����
                        overlapItem = inventoryItemSlots[posX + x, posY + y];
                    }
                    else
                    {
                        // overlapItem�� �̹� �ٸ� ���������� �����Ǿ� ���� ���
                        if(overlapItem != inventoryItemSlots[posX + x, posY + y])
                        {
                            // ������ �߻�
                            return false;
                        }
                    }
                }
            }
        }

        // �������� �߻����� �ʾҴ�
        return true;
    }
    #endregion // �Լ�
}
