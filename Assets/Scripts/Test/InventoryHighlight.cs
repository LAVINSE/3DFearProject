using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    #region ����
    [SerializeField] private RectTransform highlighter;
    #endregion // ����

    #region �Լ�
    /** ���̶���Ʈ �̹����� Ȱ��ȭ/��Ȱ��ȭ �Ѵ� */
    public void Show(bool isShow)
    {
        highlighter.gameObject.SetActive(isShow);
    }

    /** ���̶���Ʈ �̹��� ����� �����Ѵ� */
    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * ItemGrid.tileSizeWidth;
        size.y = targetItem.itemData.heigth * ItemGrid.tileSizeHeight;

        highlighter.sizeDelta = size;
    }
    
    /** */
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        // �������� ��ġ�� �׸����� ��ǥ�� �޾Ƽ� �߽��� ����Ѵ�
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);

        highlighter.localPosition = pos;
    }

    /** ���̶���Ʈ �̹����� �θ� ��ü�� �����Ѵ� */
    public void SetParent(ItemGrid targetGrid)
    {
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }
    #endregion // �Լ�
}
